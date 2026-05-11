import { Component, OnInit, OnDestroy, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { timeout, catchError, of, Subscription } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { LanguageService } from '../../services/language.service';
import { TranslationsService } from '../../services/translations.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AddProductPanelComponent } from './add-product-panel.component';
import { EditProductPanelComponent } from './edit-product-panel.component';

interface Product {
  id: number;
  code: string;
  name: string;
  price: number;
  quantity: number;
  views: number;
  salesPercentage: number;
  status: 'active' | 'sold' | 'expired';
  statusLabel: string;
}

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe, AddProductPanelComponent, EditProductPanelComponent],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit, OnDestroy {
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  private languageService = inject(LanguageService);
  private translationsService = inject(TranslationsService);
  private cdr = inject(ChangeDetectorRef);
  private subscription?: Subscription;
  private languageSubscription?: Subscription;

  translations: any = {};
  currentLanguage: 'ar' | 'en' = 'ar';

  isAdmin = false;
  searchQuery = '';
  selectedStatus = '';
  selectedPriceRange = '';
  dateInputRef?: HTMLInputElement;
  showAddPanel = false;
  showEditPanel = false;
  selectedProduct: any = null;
  isLoadingProductDetails = false;
  isLoadingProducts = false;
  errorMessage = '';

  products: Product[] = [];
  filteredProducts: Product[] = [];

  currentPage = 1;
  itemsPerPage = 12;
  totalPages = 1;

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin();
    this.currentLanguage = this.languageService.getCurrentLanguageValue();
    this.translations = this.translationsService.getTranslationsSync();
    
    // Subscribe to language changes
    this.languageSubscription = this.languageService.getCurrentLanguage().subscribe(lang => {
      this.currentLanguage = lang;
      this.translations = this.translationsService.getTranslationsSync();
      // Update status labels when language changes
      this.products.forEach(product => {
        product.statusLabel = this.getStatusLabel(product.status);
      });
      this.cdr.detectChanges();
    });
    
    this.loadProducts();
  }

  loadProducts() {
    // Check if token exists
    const token = localStorage.getItem('token');
    if (!token || token.trim() === '') {
      this.isLoadingProducts = false;
      this.errorMessage = this.translationsService.getSync('noAccess');
      this.cdr.detectChanges();
      console.warn('No token found in localStorage');
      return;
    }

    this.isLoadingProducts = true;
    this.errorMessage = '';

    console.log('Loading products from:', this.apiService.getUrl('Products/GetAll'));
    console.log('Token exists:', !!token);

    // Unsubscribe from previous subscription if exists
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.http.get<any>(this.apiService.getUrl('Products/GetAll'))
      .pipe(
        timeout(10000),
        catchError((error: HttpErrorResponse) => {
          console.error('Error loading products:', error);
          this.isLoadingProducts = false;
          
          // Handle 401 Unauthorized specifically
          if (error.status === 401) {
            this.errorMessage = this.translationsService.getSync('sessionExpired');
          } else if (error.status === 0) {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else {
            this.errorMessage = error.error?.message || error.message || this.translationsService.getSync('loadProductsError');
          }
          
          this.cdr.detectChanges();
          return of({ succeeded: false, resultData: null, message: this.errorMessage });
        })
      )
      .subscribe({
        next: (response) => {
          console.log('Products API response:', response);
          this.isLoadingProducts = false;
          this.cdr.detectChanges();

          if (response.succeeded === false) {
            this.errorMessage = response.message || this.translationsService.getSync('loadProductsFailed');
            this.products = [];
            this.filteredProducts = [];
            this.calculateTotalPages();
            this.cdr.detectChanges();
            return;
          }

          // Handle different response structures
          let resultData = null;
          
          // Check if response is already an array
          if (Array.isArray(response)) {
            resultData = response;
          } 
          // Check for resultData property
          else if (response.resultData !== undefined && response.resultData !== null) {
            resultData = response.resultData;
          }
          // Check for data property
          else if (response.data !== undefined && response.data !== null) {
            resultData = response.data;
          }
          // Check if response itself is the data (and not a wrapper object)
          else if (response && typeof response === 'object' && !response.succeeded && !response.message) {
            resultData = response;
          }

          // Check if resultData is null, undefined, or empty array
          if (resultData === null || resultData === undefined || (Array.isArray(resultData) && resultData.length === 0)) {
            this.products = [];
            this.filteredProducts = [];
            this.calculateTotalPages();
            this.cdr.detectChanges();
            return;
          }
          
          // Ensure resultData is an array
          const productsArray = Array.isArray(resultData) ? resultData : [resultData];
          
          // Map API response to component format
          this.products = productsArray.map((product: any) => {
            // Format dates
            const formatDate = (dateValue: any): string => {
              if (!dateValue) return '';
              if (typeof dateValue === 'string') {
                // If it's already a formatted string, return it
                if (dateValue.includes('T')) {
                  // ISO format, extract date part
                  return dateValue.split('T')[0];
                }
                return dateValue;
              }
              if (dateValue instanceof Date) {
                return dateValue.toISOString().split('T')[0];
              }
              return '';
            };

            const rawStatus =
              product.status ?? product.statusLabel ?? product.statusId ?? product.isActive ?? 'active';
            const mappedStatus = this.mapStatus(rawStatus);

            return {
              id: product.id || product.productId || 0,
              code: product.code || product.productCode || `#${String(product.id || product.productId || 0).padStart(8, '0')}`,
              name: this.languageService.getLocalizedName(product) || product.name || product.nameAr || product.productName || '',
              price: product.price || product.productPrice || product.unitPrice || 0,
              quantity: product.quantity || product.stock || product.stockQuantity || 0,
              views: product.views || product.viewCount || product.viewsCount || Math.floor(Math.random() * 30) + 10, // Random for demo if not available
              salesPercentage: product.salesPercentage || product.salesPercent || product.soldPercentage || Math.floor(Math.random() * 100), // Random for demo if not available
              status: mappedStatus,
              statusLabel: this.getStatusLabel(mappedStatus)
            };
          });
          
          this.applyFilters();
          this.calculateTotalPages();
          this.cdr.detectChanges();
        }
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.languageSubscription) {
      this.languageSubscription.unsubscribe();
    }
  }

  /** API: 0 = active, 1 = sold, 2 = expired */
  mapStatus(status: string | number | boolean | null | undefined): Product['status'] {
    if (status === 0 || status === '0') {
      return 'active';
    }
    if (status === 1 || status === '1') {
      return 'sold';
    }
    if (status === 2 || status === '2') {
      return 'expired';
    }
    if (status === true) {
      return 'active';
    }
    if (status === false) {
      return 'sold';
    }
    if (typeof status === 'number' && Number.isFinite(status)) {
      return 'active';
    }
    const statusLower = String(status ?? '').toLowerCase();
    if (statusLower.includes('sold') || statusLower.includes('مباع')) {
      return 'sold';
    }
    if (statusLower.includes('expired') || statusLower.includes('منتهي')) {
      return 'expired';
    }
    if (statusLower.includes('active') || statusLower.includes('نشط')) {
      return 'active';
    }
    return 'active';
  }

  getStatusLabel(status: Product['status']): string {
    switch (status) {
      case 'sold':
        return this.translations.sold || 'Sold';
      case 'expired':
        return this.translations.expired || 'Expired';
      default:
        return this.translations.active || 'Active';
    }
  }

  applyFilters(): void {
    this.filteredProducts = this.products.filter(product => {
      const matchesSearch = !this.searchQuery || 
        product.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        product.code.toLowerCase().includes(this.searchQuery.toLowerCase());
      
      const matchesStatus = !this.selectedStatus || product.status === this.selectedStatus;
      
      const matchesPrice = !this.selectedPriceRange || 
        (this.selectedPriceRange === '0-25' && product.price >= 0 && product.price <= 25) ||
        (this.selectedPriceRange === '25-50' && product.price > 25 && product.price <= 50) ||
        (this.selectedPriceRange === '50-100' && product.price > 50 && product.price <= 100) ||
        (this.selectedPriceRange === '100+' && product.price > 100);
      
      return matchesSearch && matchesStatus && matchesPrice;
    });

    this.currentPage = 1;
    this.calculateTotalPages();
  }

  calculateTotalPages() {
    this.totalPages = Math.ceil(this.filteredProducts.length / this.itemsPerPage) || 1;
  }

  get paginatedProducts(): Product[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredProducts.slice(startIndex, endIndex);
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  editProduct(product: Product) {
    if (this.isAdmin) {
      return;
    }
    if (!product?.id) {
      this.errorMessage = this.translationsService.getSync('loadProductsError');
      this.cdr.detectChanges();
      return;
    }
    this.isLoadingProductDetails = true;
    this.errorMessage = '';
    this.cdr.detectChanges();

    const url = this.apiService.getUrl(`Products/Get?id=${product.id}`);
    this.http.get<any>(url)
      .pipe(
        timeout(10000),
        catchError((err: HttpErrorResponse) => {
          this.isLoadingProductDetails = false;
          if (err.status === 404) {
            this.errorMessage = this.translationsService.getSync('loadProductsError') || 'Product not found.';
          } else if (err.status === 0) {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else {
            this.errorMessage = err.error?.message || err.message || this.translationsService.getSync('loadProductsError');
          }
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe((response) => {
        this.isLoadingProductDetails = false;
        if (!response) {
          this.cdr.detectChanges();
          return;
        }
        let productDetails = null;
        if (response.resultData != null) {
          productDetails = response.resultData;
        } else if (response.data != null) {
          productDetails = response.data;
        } else if (Array.isArray(response) && response.length > 0) {
          productDetails = response[0];
        } else if (response && typeof response === 'object' && response.id != null) {
          productDetails = response;
        }
        this.selectedProduct = productDetails || response;
        this.showEditPanel = true;
        this.cdr.detectChanges();
      });
  }

  closeEditPanel() {
    this.showEditPanel = false;
    this.selectedProduct = null;
  }

  updateProduct(_response: any) {
    this.showEditPanel = false;
    this.selectedProduct = null;
    this.errorMessage = '';
    this.loadProducts();
    this.cdr.detectChanges();
  }

  deleteProduct(product: Product) {
    if (!product?.id) {
      this.errorMessage = this.translationsService.getSync('loadProductsError');
      this.cdr.detectChanges();
      return;
    }

    this.http.delete(this.apiService.getUrl(`Products/Delete/${product.id}`))
      .subscribe({
        next: () => {
          this.loadProducts();
        },
        error: (error: HttpErrorResponse) => {
          console.error('Delete product error:', error);
          if (error.status === 401) {
            this.errorMessage = this.translationsService.getSync('sessionExpired');
          } else if (error.status === 0) {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else {
            this.errorMessage = error.error?.message || error.message || this.translationsService.getSync('loadProductsError');
          }
          this.cdr.detectChanges();
        }
      });
  }

  addNewProduct() {
    if (this.isAdmin) {
      return;
    }
    this.showAddPanel = true;
  }

  closeAddPanel() {
    this.showAddPanel = false;
  }

  saveProduct(response: any) {
    console.log('Product created successfully:', response);
    this.showAddPanel = false;
    // Refresh products list after adding
    this.loadProducts();
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'active':
        return 'status-active';
      case 'sold':
        return 'status-sold';
      case 'expired':
        return 'status-expired';
      default:
        return '';
    }
  }

  duplicateProduct(product: Product) {
    if (!product?.id) {
      return;
    }

    const url = this.apiService.getUrl(`Products/Get?id=${product.id}`);
    this.http.get<any>(url)
      .pipe(
        timeout(10000),
        catchError((err: HttpErrorResponse) => {
          if (err.status === 0) {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else {
            this.errorMessage =
              err.error?.message || err.message || this.translationsService.getSync('loadProductsError');
          }
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe((response) => {
        if (!response) {
          return;
        }

        const details =
          response?.resultData ??
          response?.data ??
          (Array.isArray(response) && response.length > 0 ? response[0] : response);

        if (!details || typeof details !== 'object') {
          return;
        }

        const rawStatus = details.status ?? details.statusLabel ?? details.statusId ?? details.isActive ?? 'active';
        const mappedStatus = this.mapStatus(rawStatus);
        const updatedProduct: Product = {
          id: details.id || details.productId || product.id,
          code: details.code || details.productCode || product.code,
          name:
            this.languageService.getLocalizedName(details) ||
            details.name ||
            details.nameAr ||
            details.productName ||
            product.name,
          price: details.price || details.productPrice || details.unitPrice || 0,
          quantity: details.quantity || details.stock || details.stockQuantity || 0,
          views: details.views || details.viewCount || details.viewsCount || product.views,
          salesPercentage:
            details.salesPercentage || details.salesPercent || details.soldPercentage || product.salesPercentage,
          status: mappedStatus,
          statusLabel: this.getStatusLabel(mappedStatus)
        };

        this.products = this.products.map((item) => item.id === updatedProduct.id ? updatedProduct : item);
        this.filteredProducts = this.filteredProducts.map((item) => item.id === updatedProduct.id ? updatedProduct : item);
        this.errorMessage = '';
        this.calculateTotalPages();
        this.cdr.detectChanges();
      });
  }

  onFilterChange(): void {
    this.applyFilters();
  }
}
