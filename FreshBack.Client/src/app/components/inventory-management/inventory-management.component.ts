import { ChangeDetectorRef, Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { LanguageService } from '../../services/language.service';

const INVENTORY_STRINGS = {
  ar: {
    pageTitle: 'إدارة المخزون الفائض',
    today: 'اليوم',
    moreThan7Days: 'أكثر من 7 أيام',
    dailyForecast: 'توقعات الفائض اليومي',
    basedOnPos: 'بناء على نقاط البيع (POS)',
    productName: 'اسم المنتج',
    branchesAndQuantities: 'الفروع والكميات',
    branchName: 'اسم الفرع',
    currentInventory: 'المخزون الحالي',
    expectedSurplus: 'الفائض المتوقع',
    expiryTime: 'وقت الإنتهاء',
    suggestedAction: 'الإجراء المقترح',
    addToSurplus: 'إضافة للفائض',
    loading: 'جاري التحميل...',
    loadForecastFailed: 'فشل تحميل التوقعات',
    updateFailed: 'فشل في التحديث',
    panelTitle: 'إضافة للفائض',
    close: 'إغلاق',
    quantity: 'الكمية',
    discount: 'الخصم',
    views: 'المشاهدات',
    status: 'الحالة',
    creationDate: 'تاريخ الإنشاء',
    expiryDate: 'تاريخ الانتهاء',
    startDeliveryDate: 'تاريخ بداية التوصيل',
    endDeliveryDate: 'تاريخ نهاية التوصيل',
    cancel: 'إلغاء',
    update: 'تحديث',
    saving: 'جاري الحفظ...',
    smartPricingTitle: 'محرك التسعير الذكي',
    smartPricingSubtitle: 'توصيات أسعار بناءً على الإنتهاء والطلب',
    originalPrice: 'السعر الأصلي',
    suggestedPrice: 'السعر المقترح',
    applySuggestedPrice: 'تطبيق السعر المقترح',
    currency: 'ريال',
    tip1Title: 'أفضل وقت للنشر',
    tip1Content: 'المنتجات المنشورة بين الساعة 2-4 مساءً تحقق أعلى نسبة مشاهدات.',
    tip2Title: 'نصيحة ذكية',
    tip2Content: 'المنتجات التي تنتهي خلال 2-3 ساعات تباع بشكل أفضل عند خصم 40-50%.',
    recommendationText: 'توصية: هذا السعر يزيد نسبة البيع بمعدل 75 % حسب البيانات التاريخية',
    noBranches: 'لا توجد فروع',
    productDetails: 'تفاصيل المنتج'
  },
  en: {
    pageTitle: 'Excess Inventory Management',
    today: 'Today',
    moreThan7Days: 'More than 7 days',
    dailyForecast: 'Daily Excess Forecast',
    basedOnPos: 'Based on Point of Sale (POS)',
    productName: 'Product Name',
    branchesAndQuantities: 'Branches & Quantities',
    branchName: 'Branch Name',
    currentInventory: 'Current Inventory',
    expectedSurplus: 'Expected Surplus',
    expiryTime: 'Expiry Time',
    suggestedAction: 'Suggested Action',
    addToSurplus: 'Add to Surplus',
    loading: 'Loading...',
    loadForecastFailed: 'Failed to load forecast',
    updateFailed: 'Update failed',
    panelTitle: 'Add to Surplus',
    close: 'Close',
    quantity: 'Quantity',
    discount: 'Discount',
    views: 'Views',
    status: 'Status',
    creationDate: 'Creation Date',
    expiryDate: 'Expiry Date',
    startDeliveryDate: 'Start Delivery Date',
    endDeliveryDate: 'End Delivery Date',
    cancel: 'Cancel',
    update: 'Update',
    saving: 'Saving...',
    smartPricingTitle: 'Smart Pricing Engine',
    smartPricingSubtitle: 'Price recommendations based on expiry and demand',
    originalPrice: 'Original Price',
    suggestedPrice: 'Suggested Price',
    applySuggestedPrice: 'Apply Suggested Price',
    currency: 'SAR',
    tip1Title: 'Best time to publish',
    tip1Content: 'Products published between 2-4 PM get the highest view rate.',
    tip2Title: 'Smart tip',
    tip2Content: 'Products expiring in 2-3 hours sell better with a 40-50% discount.',
    recommendationText: 'Recommendation: This price increases sales by 75% based on historical data.',
    noBranches: 'No branches available',
    productDetails: 'Product Details'
  }
};

interface ExcessInventoryItem {
  productId?: number;
  branchId?: number;
  productName: string;
  currentStock: number;
  expectedSurplus: number;
  expirationTime: string;
  isUrgent?: boolean;
}

interface BranchQuantityRow {
  branchId: number;
  branchName: string;
  quantity: number;
}

interface PricingRecommendation {
  productName: string;
  originalPrice: number;
  suggestedPrice: number;
  discount: string;
  recommendation: string;
}

interface SmartTip {
  titleKey: keyof typeof INVENTORY_STRINGS.ar;
  contentKey: keyof typeof INVENTORY_STRINGS.ar;
  iconPath: string;
}

@Component({
  selector: 'app-inventory-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventory-management.component.html',
  styleUrls: ['./inventory-management.component.scss']
})
export class InventoryManagementComponent implements OnInit, OnDestroy {
  /** API endpoint for branch products (paginated). */
  private readonly branchesProductsEndpoint = 'BranchesProducts/GetBranchesRemainingProductsPaginated';
  private readonly branchesProductsUpdateEndpoint = 'BranchesProducts/Update';
  private readonly branchesEndpoint = 'Branches/GetAll';

  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private languageService = inject(LanguageService);
  private cdr = inject(ChangeDetectorRef);
  private languageSubscription?: Subscription;

  t = INVENTORY_STRINGS.ar;
  selectedFilter: string = 'today';
  isLoadingForecast = false;
  forecastError = '';
  surplusPanelOpen = false;
  isSubmittingSurplus = false;
  surplusSubmitError = '';
  selectedProduct: ExcessInventoryItem | null = null;
  branchQuantities: BranchQuantityRow[] = [];
  isLoadingBranches = false;

  /** Form model for BranchesProducts update (add to surplus). */
  surplusForm = {
    branchId: 0,
    productId: 0,
    discount: 0,
    quantity: 0,
    creationDate: '',
    expiryDate: '',
    startDeliveryDate: '',
    endDeliveryDate: ''
  };

  excessInventoryItems: ExcessInventoryItem[] = [];
  pageSize = 10;
  pageNumber = 1;
  totalPages = 1;

  pricingRecommendations: PricingRecommendation[] = [
    { productName: 'كرواسوان', originalPrice: 8, suggestedPrice: 5, discount: '37%', recommendation: '' },
    { productName: 'خبز فرنسي', originalPrice: 12, suggestedPrice: 8, discount: '33%', recommendation: '' },
    { productName: 'كرواسوان', originalPrice: 8, suggestedPrice: 5, discount: '37%', recommendation: '' }
  ];

  ngOnInit(): void {
    this.updateLanguage();
    this.languageSubscription = this.languageService.getCurrentLanguage().subscribe(() => this.updateLanguage());
    this.loadGridData();
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private updateLanguage(): void {
    const isEn = this.languageService.getCurrentLanguageValue() === 'en';
    this.t = isEn ? INVENTORY_STRINGS.en : INVENTORY_STRINGS.ar;
  }

  loadGridData(): void {
    this.isLoadingForecast = true;
    this.forecastError = '';
    const url = this.apiService.getUrl(this.branchesProductsEndpoint);
    const body = { pageSize: this.pageSize, pageNumber: this.pageNumber };
    this.http.post<any>(url, body).subscribe({
      next: (res) => {
        this.isLoadingForecast = false;
        const data = res?.resultData ?? res?.data ?? res?.result ?? res;
        let list: any[] = [];
        if (Array.isArray(data)) {
          list = data;
        } else if (data && typeof data === 'object') {
          list = data.items ?? data.data ?? data.records ?? data.results ?? Object.values(data);
          if (!Array.isArray(list)) list = [];
        }
        this.excessInventoryItems = list.map((item: any) => this.mapToExcessItem(item));
        const total = data?.totalCount ?? data?.total ?? data?.totalRecords ?? this.excessInventoryItems.length;
        this.totalPages = this.pageSize > 0 ? Math.ceil(Number(total) / this.pageSize) || 1 : 1;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoadingForecast = false;
        this.forecastError = err?.error?.message ?? err?.message ?? this.t.loadForecastFailed;
        this.excessInventoryItems = [];
        this.cdr.detectChanges();
      }
    });
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadGridData();
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadGridData();
    }
  }

  private mapToExcessItem(item: any): ExcessInventoryItem {
    const product = item.product ?? item.Product ?? item.ProductDto ?? {};
    const name =
      item.productName ?? item.ProductName ?? item.name ?? item.Name ?? item.nameAr ?? item.NameAr
      ?? product.nameAr ?? product.NameAr ?? product.name ?? product.Name ?? product.nameEn ?? product.NameEn
      ?? item.productNameAr ?? '';
    const current = item.quantity ?? item.Quantity ?? item.currentStock ?? item.currentInventory ?? item.stock ?? item.Stock ?? 0;
    const expected = item.expectedSurplus ?? item.expectedExcess ?? item.surplus ?? item.ExpectedSurplus ?? 0;
    const expiry = item.expirationTime ?? item.expiryTime ?? item.expiry ?? item.timeToExpiry ?? item.ExpirationTime ?? '';
    const urgent = expiry && (String(expiry).includes('ساعتين') || String(expiry).includes('2 ساعات') || (item.isUrgent === true));
    return {
      productId: item.productId ?? item.ProductId ?? product?.id ?? product?.Id,
      branchId: item.branchId ?? item.BranchId,
      productName: String(name || '—'),
      currentStock: Number(current),
      expectedSurplus: Number(expected),
      expirationTime: String(expiry || '—'),
      isUrgent: Boolean(urgent)
    };
  }

  openAddToSurplus(item?: ExcessInventoryItem): void {
    this.surplusSubmitError = '';
    this.selectedProduct = item ?? null;
    this.branchQuantities = [];
    this.initSurplusForm();
    if (item) {
      this.loadBranchQuantitiesForProduct(item);
    }
    this.surplusPanelOpen = true;
  }

  private initSurplusForm(): void {
    this.surplusForm.discount = 0;
    const now = new Date().toISOString().slice(0, 16);
    this.surplusForm.creationDate = now;
    this.surplusForm.expiryDate = now;
    this.surplusForm.startDeliveryDate = now;
    this.surplusForm.endDeliveryDate = now;
  }

  private loadBranchQuantitiesForProduct(item: ExcessInventoryItem): void {
    this.isLoadingBranches = true;
    const productId = item.productId ?? 0;
    const urlBranches = this.apiService.getUrl(this.branchesEndpoint);
    this.http.get<any>(urlBranches).subscribe({
      next: (resBranches) => {
        const branchesData = resBranches?.resultData ?? resBranches?.data ?? resBranches;
        const branchesList = Array.isArray(branchesData) ? branchesData : (branchesData ? Object.values(branchesData) : []);
        const branches = branchesList.map((b: any) => ({
          id: b.id ?? b.Id,
          name: this.languageService.getLocalizedName(b) || (b.nameAr ?? b.name ?? b.nameEn ?? '')
        }));

        // Fetch product-branch quantities (all pages for this product)
        const urlProducts = this.apiService.getUrl(this.branchesProductsEndpoint);
        this.http.post<any>(urlProducts, { pageSize: 500, pageNumber: 1 }).subscribe({
          next: (resProducts) => {
            this.isLoadingBranches = false;
            const data = resProducts?.resultData ?? resProducts?.data ?? resProducts?.result ?? resProducts;
            const items = data?.items ?? data?.data ?? data?.records ?? (Array.isArray(data) ? data : []);
            const getProdId = (r: any) => r.productId ?? r.ProductId ?? r.product?.id ?? r.product?.Id ?? r.Product?.id ?? r.Product?.Id;
            const productRows = items.filter((r: any) => getProdId(r) == productId);

            const quantityByBranch = new Map<number, number>();
            productRows.forEach((r: any) => {
              const bid = r.branchId ?? r.BranchId ?? 0;
              const qty = r.quantity ?? r.Quantity ?? r.currentStock ?? r.stock ?? 0;
              quantityByBranch.set(bid, qty);
            });

            const firstRow = productRows[0];
            if (firstRow) {
              this.surplusForm.discount = firstRow.discount ?? firstRow.Discount ?? this.surplusForm.discount;
              this.surplusForm.creationDate = this.toDatetimeLocal(firstRow.creationDate ?? firstRow.CreationDate);
              this.surplusForm.expiryDate = this.toDatetimeLocal(firstRow.expiryDate ?? firstRow.ExpiryDate);
              this.surplusForm.startDeliveryDate = this.toDatetimeLocal(firstRow.startDeliveryDate ?? firstRow.StartDeliveryDate);
              this.surplusForm.endDeliveryDate = this.toDatetimeLocal(firstRow.endDeliveryDate ?? firstRow.EndDeliveryDate);
            }

            this.branchQuantities = branches
              .filter((b: { id: number }) => b.id != null)
              .map((b: { id: number; name: string }) => ({
                branchId: b.id,
                branchName: b.name,
                quantity: quantityByBranch.get(b.id) ?? (b.id === item.branchId ? (item.currentStock ?? 0) : 0)
              }));
            this.cdr.detectChanges();
          },
          error: () => {
            this.isLoadingBranches = false;
            this.branchQuantities = branches
              .filter((b: { id: number }) => b.id != null)
              .map((b: { id: number; name: string }) => ({
                branchId: b.id,
                branchName: b.name,
                quantity: b.id === item.branchId ? (item.currentStock ?? 0) : 0
              }));
            this.cdr.detectChanges();
          }
        });
      },
      error: () => {
        this.isLoadingBranches = false;
        this.branchQuantities = item.branchId != null ? [{
          branchId: item.branchId,
          branchName: '',
          quantity: item.currentStock ?? 0
        }] : [];
        this.cdr.detectChanges();
      }
    });
  }

  closeSurplusPanel(): void {
    this.surplusPanelOpen = false;
    this.surplusSubmitError = '';
    this.selectedProduct = null;
    this.branchQuantities = [];
  }

  submitSurplusUpdate(): void {
    if (!this.selectedProduct?.productId) return;
    this.isSubmittingSurplus = true;
    this.surplusSubmitError = '';
    const productId = this.selectedProduct.productId;
    const url = this.apiService.getUrl(this.branchesProductsUpdateEndpoint);
    const updates = this.branchQuantities.map(row => ({
      branchId: row.branchId,
      productId,
      discount: this.surplusForm.discount,
      creationDate: this.toIsoDate(this.surplusForm.creationDate),
      expiryDate: this.toIsoDate(this.surplusForm.expiryDate),
      quantity: row.quantity,
      startDeliveryDate: this.toIsoDate(this.surplusForm.startDeliveryDate),
      endDeliveryDate: this.toIsoDate(this.surplusForm.endDeliveryDate),
      views: 0,
      status: 0
    }));

    let completed = 0;
    let hasError = false;
    updates.forEach(body => {
      this.http.put<any>(url, body).subscribe({
        next: () => {
          completed++;
          if (completed === updates.length && !hasError) {
            this.isSubmittingSurplus = false;
            this.closeSurplusPanel();
            this.loadGridData();
          }
        },
        error: (err) => {
          hasError = true;
          this.isSubmittingSurplus = false;
          this.surplusSubmitError = err?.error?.message ?? err?.message ?? this.t.updateFailed;
        }
      });
    });
    if (updates.length === 0) {
      this.isSubmittingSurplus = false;
    }
  }

  private toIsoDate(value: string): string {
    if (!value) return new Date().toISOString();
    if (value.length <= 10) return new Date(value + 'T00:00:00.000Z').toISOString();
    return new Date(value).toISOString();
  }

  private toDatetimeLocal(value: string | Date | null | undefined): string {
    if (!value) return new Date().toISOString().slice(0, 16);
    const d = typeof value === 'string' ? new Date(value) : value;
    if (isNaN(d.getTime())) return new Date().toISOString().slice(0, 16);
    const pad = (n: number) => String(n).padStart(2, '0');
    return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
  }

  smartTips: SmartTip[] = [
    { titleKey: 'tip1Title', contentKey: 'tip1Content', iconPath: 'assets/icons/3.svg' },
    { titleKey: 'tip2Title', contentKey: 'tip2Content', iconPath: 'assets/icons/star.svg' }
  ];

  applySuggestedPrice(recommendation: PricingRecommendation): void {
    console.log('Applying suggested price for:', recommendation.productName);
    // Implement apply price logic
  }
}
