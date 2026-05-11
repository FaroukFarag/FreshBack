import { Component, OnInit, OnDestroy, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Subscription, combineLatest, timeout, catchError, of } from 'rxjs';
import { TranslationsService, Translations } from '../../services/translations.service';
import { LanguageService } from '../../services/language.service';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

interface OrderProduct {
  name: string;
  quantity: number;
  price: number;
}

interface Order {
  orderNumber: string;
  status: 'new' | 'confirmed' | 'delivered' | 'cancelled';
  statusLabel: string;
  statusKey?: 'received' | 'scheduled' | 'pending' | 'cancelled' | 'confirmed' | 'delivered';
  customerName: string;
  customerEmail?: string;
  customerPhone?: string;
  totalPrice: number;
  discount?: number;
  fees?: number;
  time: string;
  receiptTime?: string;
  receiptDate?: string;
  paymentMethod?: string;
  merchantName?: string;
  orderDate?: string;
  orderTime?: string;
  productImage?: string;
  productName?: string;
  vendorName?: string;
  deliveryType?: 'pickup' | 'delivery';
  products: OrderProduct[];
}

@Component({
  selector: 'app-order-management',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './order-management.component.html',
  styleUrls: ['./order-management.component.scss']
})
export class OrderManagementComponent implements OnInit, OnDestroy {
  activeTab: 'new' | 'confirmed' | 'delivered' = 'new';
  searchQuery: string = '';
  selectedOrder: Order | null = null;
  isModalOpen: boolean = false;
  isRejectModalOpen: boolean = false;
  showOrderDetailsPanel: boolean = false;
  isLoadingOrderDetails: boolean = false;
  orderDetailsError: string = '';
  showQRScanPanel: boolean = false;
  qrCodeManualInput: string = '';
  selectedRejectionReason: string = '';
  isLoadingOrders = false;
  errorMessage = '';
  isAdmin = false;
  isBranchManager = false;
  isMerchant = false;
  isLiveOrdersRoute = false;
  selectedOrderType = 'all';
  selectedOrderStatus = '';
  currentPage = 1;
  itemsPerPage = 12;
  totalPages = 1;
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private languageService = inject(LanguageService);
  private authService = inject(AuthService);
  private translationsService = inject(TranslationsService);
  private route = inject(ActivatedRoute);
  private cdr = inject(ChangeDetectorRef);
  private languageSubscription?: Subscription;
  private readonly REQUEST_TIMEOUT = 5000;
  
  rejectReasonKeys = ['rejectReason1', 'rejectReason2', 'rejectReason3', 'rejectReason4', 'rejectReason5'] as const;
  orderStatusFilters: Array<{ value: string; labelKey: string }> = [
    { value: '', labelKey: 'orderStatus' },
    { value: 'received', labelKey: 'orderStatusReceived' },
    { value: 'scheduled', labelKey: 'orderStatusScheduled' },
    { value: 'pending', labelKey: 'orderStatusPending' },
    { value: 'cancelled', labelKey: 'orderStatusCancelled' }
  ];

  orderTabs: Array<{ key: 'new' | 'confirmed' | 'delivered'; labelKey: string; count: number | null }> = [
    { key: 'new', labelKey: 'new', count: null },
    { key: 'confirmed', labelKey: 'confirmed', count: null },
    { key: 'delivered', labelKey: 'delivered', count: null }
  ];

  branchManagerLiveTab: 'all' | 'pending' | 'confirmed' | 'delivered' | 'canceled' = 'all';
  branchManagerLiveTabs: Array<{ key: 'all' | 'pending' | 'confirmed' | 'delivered' | 'canceled'; labelKey: string; count: number | null }> = [];
  sortOrderAsc = true;

  allOrders: Order[] = [];

  ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isBranchManager = this.authService.isMerchantAdmin();
    this.isMerchant = this.authService.isMerchant();
    this.isLiveOrdersRoute = this.route.snapshot.data['liveOrders'] === true;
    this.loadOrders();
    
    this.languageSubscription = this.languageService.getCurrentLanguage().subscribe(() => {
      if (this.allOrders.length > 0) {
        this.loadOrders();
      }
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadOrders() {
    this.isLoadingOrders = true;
    this.errorMessage = '';

    this.http.get<any>(this.apiService.getUrl('Orders/GetAll'))
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          this.isLoadingOrders = false;
          if (error.name === 'TimeoutError') {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else {
            console.error('Error loading orders:', error);
            this.errorMessage = error.error?.message || this.translationsService.getSync('loadingOrders').replace('...', '');
          }
          this.cdr.detectChanges();
          return of({ resultData: [] });
        })
      )
      .subscribe({
        next: (response) => {
          this.isLoadingOrders = false;
          console.log('Orders API response:', response);
          
          // Handle different response structures
          let resultData = this.extractOrdersFromResponse(response);

          if (resultData === null || resultData.length === 0) {
            this.allOrders = [];
            this.calculateTotalPages();
            this.cdr.detectChanges();
            return;
          }

          const ordersArray = resultData;
          
          // Map API response to Order interface
          this.allOrders = ordersArray.map((order: any) => this.mapOrderWithLanguage(order));
          
          // Update tab counts
          this.updateTabCounts();
          this.calculateTotalPages();
          this.cdr.detectChanges();
        },
        error: (error: HttpErrorResponse) => {
          this.isLoadingOrders = false;
          console.error('Error loading orders:', error);
          
          if (error.status === 0) {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else if (error.status === 408 || error.status === 504) {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else {
            this.errorMessage = error.error?.message || this.translationsService.getSync('loadOrdersError');
          }
          
          this.allOrders = [];
          this.cdr.detectChanges();
        }
      });
  }

  /**
   * Format payment method (numeric enum: 0 Cash, 1 Card, etc.)
   */
  private formatPaymentMethod(val: any): string {
    if (val == null || val === '') return this.translationsService.getSync('paymentMethod') || '—';
    if (typeof val === 'string') return val;
    const map: Record<number, string> = {
      0: 'Cash',
      1: 'Card',
      2: 'Wallet',
      3: 'Transfer'
    };
    return map[Number(val)] ?? `Payment ${val}`;
  }

  /**
   * Extract orders array from various backend response structures
   */
  private extractOrdersFromResponse(response: any): any[] | null {
    if (!response || typeof response !== 'object') return null;

    // Direct array
    if (Array.isArray(response)) return response;

    let data = response.resultData ?? response.data ?? null;

    // resultData/data could be the array
    if (Array.isArray(data)) return data;

    // resultData could be object with nested array (e.g. { orders: [...] })
    if (data && typeof data === 'object') {
      const arr = data.orders ?? data.items ?? data.list ?? data.data ?? data.records ?? null;
      if (Array.isArray(arr)) return arr;
    }

    // Response itself might have orders/items at top level
    const topArr = response.orders ?? response.items ?? response.list ?? null;
    if (Array.isArray(topArr)) return topArr;

    // Single order object
    if (data && typeof data === 'object' && !Array.isArray(data)) {
      return [data];
    }

    return null;
  }

  /**
   * Map API order response to Order interface with language support
   */
  private mapOrderWithLanguage(order: any): Order {
    // Normalize field names (camelCase + PascalCase)
    const o = (key: string) => order[key] ?? order[key.charAt(0).toUpperCase() + key.slice(1)] ?? order[key.toLowerCase()];

    // Map order status from API to component status (string or numeric)
    // Enum: 0 Pending, 1 Confirmed, 2 Delivered, 3 Canceled
    let status: 'new' | 'confirmed' | 'delivered' | 'cancelled' = 'new';
    let statusLabel = 'مستلم';
    let statusKey: 'received' | 'scheduled' | 'pending' | 'cancelled' | 'confirmed' | 'delivered' = 'received';

    const rawStatus = o('status');
    if (typeof rawStatus === 'number') {
      if (rawStatus === 0) { status = 'new'; statusKey = 'pending'; statusLabel = this.translationsService.getSync('orderStatusPending'); }       // Pending
      else if (rawStatus === 1) { status = 'confirmed'; statusKey = 'confirmed'; statusLabel = this.translationsService.getSync('confirmed'); }  // Confirmed
      else if (rawStatus === 2) { status = 'delivered'; statusKey = 'delivered'; statusLabel = this.translationsService.getSync('delivered'); }  // Delivered
      else if (rawStatus === 3) { status = 'cancelled'; statusKey = 'cancelled'; statusLabel = this.translationsService.getSync('orderStatusCancelled'); }  // Canceled
    } else if (rawStatus != null && rawStatus !== '') {
      const orderStatus = String(rawStatus).toLowerCase();
      if (orderStatus === 'confirmed' || orderStatus === 'مؤكد' || orderStatus === 'موعد') {
        status = 'confirmed';
        statusLabel = 'موعد';
        statusKey = 'scheduled';
      } else if (orderStatus === 'delivered' || orderStatus === 'مسلم' || orderStatus === 'مستلم') {
        status = 'delivered';
        statusLabel = 'مستلم';
        statusKey = 'received';
      } else if (orderStatus === 'new' || orderStatus === 'جديد') {
        status = 'new';
        statusLabel = 'جديد - يحتاج تأكيد';
      } else if (orderStatus === 'pending' || orderStatus === 'معلق') {
        statusLabel = 'معلق';
        statusKey = 'pending';
      } else if (orderStatus === 'cancelled' || orderStatus === 'canceled' || orderStatus === 'ملغي') {
        status = 'cancelled';
        statusLabel = this.translationsService.getSync('orderStatusCancelled');
        statusKey = 'cancelled';
      }
    }

    // Map products (productsOrders, products, items, orderItems, OrderItems)
    const products: OrderProduct[] = [];
    const productsRaw = order.productsOrders ?? order.products ?? order.items ?? order.orderItems ?? order.OrderItems;
    if (Array.isArray(productsRaw)) {
      products.push(...productsRaw.map((p: any) => {
        const prod = p.product ?? p.Product ?? p;
        const name = (this.languageService.getLocalizedName(prod) || prod.nameAr) ?? prod.name ?? prod.Name ?? prod.nameEn ?? prod.NameEn ?? '';
        const qty = p.quantity ?? p.Quantity ?? 1;
        const priceVal = p.price ?? p.Price ?? prod.price ?? prod.Price ?? p.unitPrice ?? prod.unitPrice ?? 0;
        return { name, quantity: qty, price: priceVal };
      }));
    }

    // Format dates and times (creationDate, receiptDate, orderDate, createdAt)
    const receiptDate = o('creationDate') ?? o('receiptDate') ?? o('orderDate') ?? o('createdAt') ?? '';
    const receiptTime = o('receiptTime') ?? o('orderTime') ?? o('createdTime') ?? '';
    const dateStr = String(receiptDate);
    let time = String(receiptTime || '');
    if (dateStr && dateStr.includes('T')) {
      const dateObj = new Date(dateStr);
      time = dateObj.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', hour12: false });
    }

    const firstProduct = products.length > 0 ? products[0] : null;
    const orderNum = o('id') ?? o('orderNumber') ?? o('orderId') ?? order.number ?? '';
    const customer = order.customer ?? order.Customer ?? order.customerDto ?? order.customerData;
    const custFirst = customer?.firstName ?? customer?.FirstName ?? '';
    const custLast = customer?.lastName ?? customer?.LastName ?? '';
    const custNameJoined = (custFirst || custLast) ? `${custFirst} ${custLast}`.trim() : null;
    const custName = customer?.name ?? customer?.Name ?? customer?.fullName ?? customer?.FullName ??
      customer?.displayName ?? customer?.DisplayName ?? custNameJoined ?? customer?.userName ?? customer?.UserName;
    const custNameFallback = (custName && String(custName).trim()) ? String(custName).trim() : null;
    const customerName = o('customerName') ?? o('CustomerName') ?? custNameFallback ?? o('userName') ?? (order.customerId != null ? `Customer #${order.customerId}` : '');
    const totalPrice = Number(o('totalAmount') ?? o('totalPrice') ?? o('total') ?? o('amount') ?? o('price') ?? 0);
    const deliveryRaw = o('deliveryType') ?? o('deliveryMethod');

    return {
      orderNumber: orderNum != null ? String(orderNum) : '',
      status,
      statusLabel: o('statusLabel') ?? statusLabel,
      statusKey: o('statusKey') ?? statusKey,
      customerName: String(customerName || ''),
      customerEmail: String(o('customerEmail') ?? o('CustomerEmail') ?? customer?.email ?? customer?.Email ?? ''),
      customerPhone: String(o('customerPhone') ?? o('CustomerPhone') ?? customer?.phone ?? customer?.Phone ?? customer?.mobile ?? customer?.Mobile ?? customer?.phoneNumber ?? customer?.PhoneNumber ?? o('mobileNumber') ?? o('phone') ?? ''),
      totalPrice: Number.isNaN(totalPrice) ? 0 : totalPrice,
      discount: (Number(o('discount') ?? o('discountAmount') ?? 0) || 0),
      fees: (Number(o('fees') ?? o('serviceFee') ?? 0) || 0),
      time: (time || String(o('time') ?? '')),
      receiptTime: (time || String(receiptTime || '')),
      receiptDate: dateStr ? (dateStr.includes('T') ? dateStr.split('T')[0] : dateStr) : '',
      paymentMethod: this.formatPaymentMethod(o('paymentMethod')),
      merchantName: String(o('merchantName') ?? order.merchant?.name ?? order.Merchant?.name ?? o('storeName') ?? (order.merchantId != null ? `Merchant #${order.merchantId}` : '')),
      orderDate: String((o('orderDate') ?? o('createdDate') ?? receiptDate) ? (String(receiptDate).includes('T') ? String(receiptDate).split('T')[0] : receiptDate) : ''),
      orderTime: (time || String(o('orderTime') ?? receiptTime ?? '')),
      productImage: String(firstProduct ? (o('productImage') ?? o('image') ?? '') : ''),
      productName: String(firstProduct?.name ?? o('productName') ?? ''),
      vendorName: String(o('vendorName') ?? o('merchantName') ?? order.merchant?.name ?? o('storeName') ?? ''),
      deliveryType: deliveryRaw === 'pickup' || deliveryRaw === 'استلام' || deliveryRaw === 'Pickup' ? 'pickup' : (deliveryRaw === 'delivery' || deliveryRaw === 'توصيل' || deliveryRaw === 'Delivery' ? 'delivery' : undefined),
      products
    };
  }

  /**
   * Update tab counts based on current orders
   */
  private updateTabCounts(): void {
    const newCount = this.allOrders.filter(o => o.status === 'new').length;
    const confirmedCount = this.allOrders.filter(o => o.status === 'confirmed').length;
    const deliveredCount = this.allOrders.filter(o => o.status === 'delivered').length;
    const pendingCount = this.allOrders.filter(o => o.statusKey === 'pending' || o.status === 'new').length;
    const canceledCount = this.allOrders.filter(o => o.status === 'cancelled').length;

    this.orderTabs = [
      { key: 'new', labelKey: 'new', count: newCount > 0 ? newCount : null },
      { key: 'confirmed', labelKey: 'confirmed', count: confirmedCount > 0 ? confirmedCount : null },
      { key: 'delivered', labelKey: 'delivered', count: deliveredCount > 0 ? deliveredCount : null }
    ];

    this.branchManagerLiveTabs = [
      { key: 'all', labelKey: 'all', count: this.allOrders.length > 0 ? this.allOrders.length : null },
      { key: 'pending', labelKey: 'orderStatusPending', count: pendingCount > 0 ? pendingCount : null },
      { key: 'confirmed', labelKey: 'confirmed', count: confirmedCount > 0 ? confirmedCount : null },
      { key: 'delivered', labelKey: 'delivered', count: deliveredCount > 0 ? deliveredCount : null },
      { key: 'canceled', labelKey: 'orderStatusCancelled', count: canceledCount > 0 ? canceledCount : null }
    ];
  }

  toggleSortByOrder(): void {
    this.sortOrderAsc = !this.sortOrderAsc;
    this.onFilterChange();
  }

  get filteredOrders(): Order[] {
    let filtered = this.allOrders;
    
    // Filter by order type (for admin view)
    if (this.isAdmin && this.selectedOrderType !== 'all') {
      if (this.selectedOrderType === 'new') {
        filtered = filtered.filter(order => order.status === 'new');
      } else if (this.selectedOrderType === 'confirmed') {
        filtered = filtered.filter(order => order.status === 'confirmed');
      } else if (this.selectedOrderType === 'delivered') {
        filtered = filtered.filter(order => order.status === 'delivered');
      }
    } else if (this.isMerchant) {
      filtered = filtered.filter(order => order.status === this.activeTab);
    }
    
    // Filter by status (statusKey)
    if (this.selectedOrderStatus) {
      filtered = filtered.filter(order => order.statusKey === this.selectedOrderStatus);
    }
    
    // Branch manager: live orders filters (All, Pending, Confirmed, Delivered, Canceled)
    if (this.isBranchManager && this.isLiveOrdersRoute) {
      if (this.branchManagerLiveTab === 'pending') {
        filtered = filtered.filter(o => o.statusKey === 'pending' || o.status === 'new');
      } else if (this.branchManagerLiveTab === 'confirmed') {
        filtered = filtered.filter(o => o.status === 'confirmed');
      } else if (this.branchManagerLiveTab === 'delivered') {
        filtered = filtered.filter(o => o.status === 'delivered');
      } else if (this.branchManagerLiveTab === 'canceled') {
        filtered = filtered.filter(o => o.status === 'cancelled');
      }
    }
    
    // Filter by search query
    if (this.searchQuery) {
      const query = this.searchQuery.toLowerCase();
      if (this.isBranchManager) {
        filtered = filtered.filter(order => (order.orderNumber || '').toLowerCase().includes(query));
      } else {
        filtered = filtered.filter(order => 
          order.orderNumber.toLowerCase().includes(query) ||
          order.customerName.toLowerCase().includes(query) ||
          (order.merchantName && order.merchantName.toLowerCase().includes(query)) ||
          order.statusLabel.toLowerCase().includes(query)
        );
      }
    }

    // Sort by order (Branch Manager Order History)
    if (this.isBranchManager && !this.isLiveOrdersRoute && filtered.length > 0) {
      filtered = [...filtered].sort((a, b) => {
        const cmp = (a.orderNumber || '').localeCompare(b.orderNumber || '');
        return this.sortOrderAsc ? cmp : -cmp;
      });
    }
    
    return filtered;
  }

  get paginatedOrders(): Order[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredOrders.slice(startIndex, endIndex);
  }

  calculateTotalPages(): void {
    this.totalPages = Math.ceil(this.filteredOrders.length / this.itemsPerPage) || 1;
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.calculateTotalPages();
  }

  clearAdminOrderFilters(): void {
    this.searchQuery = '';
    this.selectedOrderStatus = '';
    this.selectedOrderType = 'all';
    this.onFilterChange();
    this.cdr.detectChanges();
  }

  formatAdminOrderDateTime(order: Order): string {
    const dateRaw = String(order.orderDate || order.receiptDate || '').trim();
    const timeRaw = String(order.orderTime || order.time || order.receiptTime || '').trim();
    if (dateRaw.includes('T')) {
      const d = new Date(dateRaw);
      if (!Number.isNaN(d.getTime())) {
        const hh = String(d.getHours()).padStart(2, '0');
        const mm = String(d.getMinutes()).padStart(2, '0');
        const datePart = dateRaw.split('T')[0];
        return `${hh}:${mm} ${datePart}`;
      }
    }
    const datePart = dateRaw && !dateRaw.includes('T') ? dateRaw : (dateRaw ? dateRaw.split('T')[0] : '');
    const parts = [timeRaw, datePart].filter(Boolean);
    const joined = parts.join(' ').trim();
    return joined || '—';
  }

  copyOrderNumber(order: Order): void {
    const text = (order.orderNumber || '').trim();
    if (!text || typeof navigator === 'undefined') return;
    navigator.clipboard?.writeText(text).catch(() => {});
  }

  getStatusClass(statusKey: string | undefined): string {
    if (!statusKey) return 'status-received';
    if (statusKey === 'scheduled' || statusKey === 'confirmed') return 'status-confirmed';
    if (statusKey === 'pending') return 'status-pending';
    if (statusKey === 'cancelled') return 'status-cancelled';
    if (statusKey === 'delivered') return 'status-delivered';
    return 'status-received';
  }

  getStatusTranslationKey(statusKey: string | undefined): string {
    if (!statusKey || statusKey === 'received') return 'orderStatusReceived';
    if (statusKey === 'scheduled') return 'orderStatusScheduled';
    if (statusKey === 'pending') return 'orderStatusPending';
    if (statusKey === 'cancelled') return 'orderStatusCancelled';
    if (statusKey === 'confirmed') return 'confirmed';
    if (statusKey === 'delivered') return 'delivered';
    return 'orderStatusReceived';
  }

  switchTab(tab: 'new' | 'confirmed' | 'delivered'): void {
    this.activeTab = tab;
  }

  switchBranchManagerLiveTab(tab: 'all' | 'pending' | 'confirmed' | 'delivered' | 'canceled'): void {
    this.branchManagerLiveTab = tab;
    this.onFilterChange();
  }

  /**
   * Format order number for card display (e.g. FB-2026-010)
   */
  formatDisplayOrderNumber(order: Order): string {
    const num = order.orderNumber || '';
    if (num.includes('-') && num.startsWith('FB-')) return num;
    const dateStr = order.receiptDate ?? order.orderDate ?? '';
    const year = dateStr ? (dateStr.includes('T') ? dateStr.split('T')[0].slice(0, 4) : dateStr.slice(0, 4)) : new Date().getFullYear().toString();
    const seq = String(parseInt(num, 10) || num || '0').padStart(3, '0');
    return `FB-${year}-${seq}`;
  }

  getMerchantOrderStatusLabel(order: Order): string {
    if (order.status === 'new' || order.statusKey === 'pending') return this.translationsService.getSync('newOrderNeedsConfirmation') || 'جديد - يحتاج تأكيد';
    if (order.status === 'confirmed') return this.translationsService.getSync('confirmed') || 'مؤكد';
    if (order.status === 'delivered') return this.translationsService.getSync('delivered') || 'مسلم';
    if (order.status === 'cancelled') return this.translationsService.getSync('orderStatusCancelled') || 'ملغي';
    return order.statusLabel || '';
  }

  getMerchantOrderStatusPillClass(order: Order): string {
    if (order.status === 'new' || order.statusKey === 'pending') return 'pill-new';
    if (order.status === 'confirmed') return 'pill-confirmed';
    if (order.status === 'delivered') return 'pill-delivered';
    if (order.status === 'cancelled') return 'pill-cancelled';
    return 'pill-pending';
  }

  getLiveOrderStatusPills(order: Order): { labelKey: string; cssClass: string }[] {
    const pills: { labelKey: string; cssClass: string }[] = [];
    // Status pill: 0 Pending, 1 Confirmed, 2 Delivered, 3 Canceled
    if (order.status === 'new' || order.statusKey === 'pending') {
      pills.push({ labelKey: 'orderStatusPending', cssClass: 'status-pending' });
    } else if (order.status === 'confirmed') {
      pills.push({ labelKey: 'confirmed', cssClass: 'status-confirmed' });
    } else if (order.status === 'delivered') {
      pills.push({ labelKey: 'delivered', cssClass: 'status-delivered' });
    } else if (order.status === 'cancelled') {
      pills.push({ labelKey: 'orderStatusCancelled', cssClass: 'status-cancelled' });
    }
    return pills;
  }

  viewOrderDetails(order: Order): void {
    this.selectedOrder = order;
    this.showOrderDetailsPanel = true;
    this.orderDetailsError = '';
    this.loadOrderDetails(order.orderNumber);
  }

  loadOrderDetails(orderId: string): void {
    if (!orderId) return;
    this.isLoadingOrderDetails = true;
    this.orderDetailsError = '';

    this.http.get<any>(`${this.apiService.getUrl('Orders/Get')}?id=${encodeURIComponent(orderId)}`)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err) => {
          this.isLoadingOrderDetails = false;
          this.orderDetailsError = err.error?.message || this.translationsService.getSync('loadOrdersError');
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (response) => {
          this.isLoadingOrderDetails = false;
          if (response) {
            const raw = response.resultData ?? response.data ?? response;
            const order = raw ? this.mapOrderWithLanguage(typeof raw === 'object' ? raw : {}) : this.selectedOrder;
            if (order) this.selectedOrder = order;
          }
          this.cdr.detectChanges();
        },
        error: () => {
          this.isLoadingOrderDetails = false;
          this.cdr.detectChanges();
        }
      });
  }

  openQRScanPanel(): void {
    this.showQRScanPanel = true;
    this.qrCodeManualInput = '';
  }

  closeQRScanPanel(): void {
    this.showQRScanPanel = false;
    this.qrCodeManualInput = '';
  }

  activateCamera(): void {
    // Placeholder for camera/QR scan integration - can use @zxing/ngx-scanner or similar
    console.log('Activate camera for QR scan');
  }

  confirmQRCode(): void {
    if (this.qrCodeManualInput.trim()) {
      console.log('QR code entered:', this.qrCodeManualInput.trim());
      // TODO: Use QR code to look up customer/order
      this.closeQRScanPanel();
    }
  }

  closeOrderDetailsPanel(): void {
    this.showOrderDetailsPanel = false;
    this.selectedOrder = null;
    this.orderDetailsError = '';
  }

  markOrderReadyAwaitingCustomer(): void {
    if (this.selectedOrder) {
      // TODO: Call API to update order status
      console.log('Order ready, awaiting customer:', this.selectedOrder.orderNumber);
      this.closeOrderDetailsPanel();
    }
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedOrder = null;
  }

  confirmOrder(): void {
    if (this.selectedOrder) {
      console.log('Confirming order:', this.selectedOrder.orderNumber);
      this.selectedOrder.status = 'confirmed';
      this.selectedOrder.statusLabel = 'موعد - جاري التوصيل';
      this.selectedOrder.statusKey = 'scheduled';
      this.closeModal();
    }
  }

  rejectOrder(): void {
    this.isRejectModalOpen = true;
  }

  confirmRejection(): void {
    if (this.selectedOrder && this.selectedRejectionReason) {
      console.log('Rejecting order:', this.selectedOrder.orderNumber, 'Reason:', this.selectedRejectionReason);
      // Implement reject order logic with reason
      this.isRejectModalOpen = false;
      this.selectedRejectionReason = '';
      this.closeModal();
    }
  }

  cancelRejection(): void {
    this.isRejectModalOpen = false;
    this.selectedRejectionReason = '';
  }

  confirmDelivery(): void {
    if (this.selectedOrder) {
      console.log('Confirming delivery for order:', this.selectedOrder.orderNumber);
      this.selectedOrder.status = 'delivered';
      this.selectedOrder.statusLabel = 'تم التسليم';
      this.selectedOrder.statusKey = 'received';
      this.closeModal();
    }
  }

  getProductTotal(): number {
    if (!this.selectedOrder) return 0;
    return this.selectedOrder.products.reduce((sum, product) => sum + (product.price * product.quantity), 0);
  }
}
