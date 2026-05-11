import { Component, OnInit, OnDestroy, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { timeout, catchError, of, Subscription } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { LanguageService } from '../../services/language.service';
import { TranslationsService } from '../../services/translations.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AddMerchantPanelComponent } from './add-merchant-panel.component';

interface Merchant {
  id: number;
  name: string;
  activityType: string;
  region: string;
  salesVolume: number;
  orderCount: number;
  rating: number;
  status: 'active' | 'suspended' | 'pending';
  /** Resolved URL for table / UI; empty when API sends no image. */
  imageUrl?: string;
}

interface MerchantDetails extends Merchant {
  description: string;
  story: string;
  phoneNumbers: string[];
  reviews: MerchantReview[];
}

interface MerchantReview {
  id: number;
  date: string;
  reviewerName: string;
  text: string;
  rating: number;
  imagePaths: string[];
}

@Component({
  selector: 'app-merchant-management',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe, AddMerchantPanelComponent],
  templateUrl: './merchant-management.component.html',
  styleUrls: ['./merchant-management.component.scss']
})
export class MerchantManagementComponent implements OnInit, OnDestroy {
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private languageService = inject(LanguageService);
  private translationsService = inject(TranslationsService);
  private cdr = inject(ChangeDetectorRef);
  private subscription?: Subscription;
  private merchantDetailsSubscription?: Subscription;
  private statusUpdateSubscription?: Subscription;

  merchantStats = {
    suspended: 0,
    pending: 0,
    active: 0
  };

  searchQuery = '';
  selectedActivityType = '';
  selectedSalesVolume = '';
  selectedRegion = '';

  activityTypes: string[] = [];
  salesVolumeKeys = ['salesVolumeLow', 'salesVolumeMedium', 'salesVolumeHigh'] as const;
  regions: string[] = [];

  merchants: Merchant[] = [];
  filteredMerchants: Merchant[] = [];
  currentPage = 1;
  totalPages = 1;
  isLoading = true; // Start with loading true
  errorMessage = '';
  
  // Merchant details panel
  selectedMerchant: MerchantDetails | null = null;
  showMerchantDetails = false;
  isLoadingMerchantDetails = false;
  merchantDetailsError = '';
  showAddMerchantPanel = false;

  // Deactivate confirmation modal
  showDeactivateModal = false;
  deactivateReason = '';
  deactivateReasonKeys = ['deactivateReason1', 'deactivateReason2', 'deactivateReason3', 'deactivateReason4', 'deactivateReason5'] as const;

  ngOnInit(): void {
    console.log('MerchantManagementComponent initialized');
    this.loadMerchants();
  }

  loadMerchants(): void {
    // Check if token exists
    const token = localStorage.getItem('token');
    if (!token || token.trim() === '') {
      this.isLoading = false;
      this.errorMessage = 'غير مصرح لك بالوصول. يرجى تسجيل الدخول مرة أخرى';
      this.cdr.detectChanges();
      console.warn('No token found in localStorage');
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    console.log('Loading merchants from:', this.apiService.getUrl('Merchants/GetAll'));
    console.log('Token exists:', !!token);

    // Unsubscribe from previous subscription if exists
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.http.get<any>(this.apiService.getUrl('Merchants/GetAll'))
      .pipe(
        timeout(10000),
        catchError((error: HttpErrorResponse) => {
          console.error('Error loading merchants:', error);
          this.isLoading = false;
          
          // Handle 401 Unauthorized specifically
          if (error.status === 401) {
            this.errorMessage = 'انتهت صلاحية الجلسة. يرجى تسجيل الدخول مرة أخرى';
            // Optionally redirect to login
            // this.router.navigate(['/login']);
          } else if (error.status === 0) {
            this.errorMessage = 'فشل الاتصال بالخادم. يرجى التحقق من الاتصال بالإنترنت';
          } else {
            this.errorMessage = error.error?.message || error.message || 'حدث خطأ أثناء تحميل التجار';
          }
          
          this.cdr.detectChanges();
          return of({ succeeded: false, resultData: null, message: this.errorMessage });
        })
      )
      .subscribe({
        next: (response) => {
          console.log('Merchants API response:', response);
          this.isLoading = false;
          this.cdr.detectChanges();

          if (response.succeeded === false) {
            this.errorMessage = response.message || this.translationsService.getSync('loadMerchantsFailed');
            this.merchants = [];
            this.filteredMerchants = [];
            this.updateStats();
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

          console.log('Result data:', resultData);

          if (resultData === null || resultData === undefined) {
            console.warn('No data in response');
            this.merchants = [];
            this.filteredMerchants = [];
            this.updateStats();
            this.cdr.detectChanges();
            return;
          }

          // Ensure resultData is an array
          let merchantsArray: any[] = [];
          if (Array.isArray(resultData)) {
            merchantsArray = resultData;
          } else if (typeof resultData === 'object') {
            // If it's a single object, wrap it in an array
            merchantsArray = [resultData];
          }

          if (merchantsArray.length === 0) {
            console.warn('Empty array in response');
            this.merchants = [];
            this.filteredMerchants = [];
            this.updateStats();
            this.cdr.detectChanges();
            return;
          }

          console.log('Merchants array length:', merchantsArray.length);

          // Map merchants with language support
          this.merchants = merchantsArray.map((merchant: any, index: number) => {
            // Map status from API to component status
            let status: 'active' | 'suspended' | 'pending' = 'pending';
            if (merchant.status === 1 || merchant.status === 'active' || merchant.isActive === true || merchant.statusId === 1) {
              status = 'active';
            } else if (merchant.status === 2 || merchant.status === 'suspended' || merchant.isSuspended === true || merchant.statusId === 2) {
              status = 'suspended';
            } else if (merchant.status === 0 || merchant.status === 'pending' || merchant.isPending === true || merchant.statusId === 0) {
              status = 'pending';
            }

            const imageRaw = this.pickMerchantImageRaw(merchant);
            const mappedMerchant = {
              id: merchant.id || merchant.merchantId || index,
              name: this.languageService.getLocalizedName(merchant) || merchant.name || merchant.nameAr || merchant.nameEn || `تاجر ${index + 1}`,
              activityType: this.languageService.getLocalizedField(merchant, 'activityType') || merchant.activityType || merchant.activityTypeAr || merchant.activityTypeEn || merchant.businessType || '',
              region:
                this.languageService.getLocalizedField(merchant, 'region') ||
                this.languageService.getLocalizedField(merchant, 'neighborhood') ||
                merchant.region ||
                merchant.regionAr ||
                merchant.regionEn ||
                merchant.neighborhood ||
                merchant.neighborhoodEn ||
                merchant.city ||
                merchant.location ||
                '',
              salesVolume: merchant.salesVolume || merchant.totalSales || merchant.sales || merchant.revenue || 0,
              orderCount: merchant.orderCount || merchant.totalOrders || merchant.orders || merchant.orderCount || 0,
              rating: merchant.rating || merchant.averageRating || merchant.rate || 0,
              status: status,
              imageUrl: imageRaw ? this.resolveMediaUrl(imageRaw) : ''
            };

            console.log(`Mapped merchant ${index + 1}:`, mappedMerchant);
            return mappedMerchant;
          });

          // Extract unique activity types and regions for filters
          this.activityTypes = [...new Set(this.merchants.map(m => m.activityType).filter(Boolean))];
          this.regions = [...new Set(this.merchants.map(m => m.region).filter(Boolean))];

          console.log('Total merchants loaded:', this.merchants.length);
          console.log('Activity types:', this.activityTypes);
          console.log('Regions:', this.regions);

          this.updateStats();
          this.applyFilters();

          console.log('Filtered merchants:', this.filteredMerchants.length);
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.error('Subscription error:', error);
          this.isLoading = false;
          this.errorMessage = this.translationsService.getSync('loadMerchantsError');
          this.cdr.detectChanges();
        }
      });
  }

  updateStats(): void {
    this.merchantStats = {
      active: this.merchants.filter(m => m.status === 'active').length,
      pending: this.merchants.filter(m => m.status === 'pending').length,
      suspended: this.merchants.filter(m => m.status === 'suspended').length
    };
  }

  private pickMerchantImageRaw(m: unknown): string {
    if (!m || typeof m !== 'object') return '';
    const o = m as Record<string, unknown>;
    const keys = [
      'imagePath',
      'imageUrl',
      'image',
      'logo',
      'merchantImage',
      'profileImage',
      'coverImage',
      'ImagePath',
      'ImageUrl',
      'Image',
      'Logo',
      'MerchantImage',
      'ProfileImage',
      'CoverImage'
    ];
    for (const key of keys) {
      const v = o[key];
      if (typeof v === 'string' && v.trim()) return v.trim();
    }
    return '';
  }

  /** Turn API paths into a usable `img` src (full URL or site-root path). */
  private resolveMediaUrl(raw: string): string {
    const s = raw.trim();
    if (!s) return '';
    if (/^https?:\/\//i.test(s) || s.startsWith('data:') || s.startsWith('blob:')) return s;
    const base = this.apiService.getBaseUrl();
    if (s.startsWith('/')) {
      const root = base.replace(/\/api\/?$/i, '').replace(/\/+$/, '');
      return `${root}${s}`;
    }
    const apiRoot = base.endsWith('/') ? base : `${base}/`;
    return `${apiRoot}${s.replace(/^\//, '')}`;
  }

  onSearch(): void {
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredMerchants = this.merchants.filter(merchant => {
      const matchesSearch = !this.searchQuery || 
        merchant.name.toLowerCase().includes(this.searchQuery.toLowerCase());
      const matchesActivity = !this.selectedActivityType || 
        merchant.activityType === this.selectedActivityType;
      const matchesRegion = !this.selectedRegion || 
        merchant.region === this.selectedRegion;
      
      return matchesSearch && matchesActivity && matchesRegion;
    });

    // Update pagination
    this.totalPages = Math.ceil(this.filteredMerchants.length / 10) || 1;
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }
  }

  getStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      'active': 'status-active',
      'suspended': 'status-suspended',
      'pending': 'status-pending'
    };
    return statusMap[status] || '';
  }

  toggleMerchantStatus(merchant: Merchant): void {
    const previousStatus = merchant.status;
    const nextStatus: Merchant['status'] = merchant.status === 'active' ? 'suspended' : 'active';

    // Optimistic UI update for quick feedback.
    merchant.status = nextStatus;
    this.updateStats();
    this.applyFilters();
    this.cdr.detectChanges();

    const statusCode = nextStatus === 'active' ? 1 : 2;
    const payload = {
      id: merchant.id,
      status: statusCode
    };

    if (this.statusUpdateSubscription) {
      this.statusUpdateSubscription.unsubscribe();
    }

    this.statusUpdateSubscription = this.http
      .patch<any>(this.apiService.getUrl('Merchants/UpdateStatus'), payload)
      .pipe(
        timeout(10000),
        catchError((error: HttpErrorResponse) => {
          // Revert on failure
          merchant.status = previousStatus;
          this.updateStats();
          this.applyFilters();
          this.errorMessage =
            error.error?.message ||
            error.message ||
            this.translationsService.getSync('loadMerchantsError');
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe((response) => {
        if (!response) return;
        if (response.succeeded === false || response.success === false) {
          merchant.status = previousStatus;
          this.updateStats();
          this.applyFilters();
          this.errorMessage =
            response.message || this.translationsService.getSync('loadMerchantsError');
        }
        this.cdr.detectChanges();
      });
  }

  viewMerchant(merchant: Merchant): void {
    console.log('View merchant:', merchant);
    this.showMerchantDetails = true;
    this.isLoadingMerchantDetails = true;
    this.merchantDetailsError = '';
    this.selectedMerchant = null;

    if (this.merchantDetailsSubscription) {
      this.merchantDetailsSubscription.unsubscribe();
    }

    this.merchantDetailsSubscription = this.http
      .get<any>(this.apiService.getUrl(`Merchants/Get?id=${merchant.id}`))
      .pipe(
        timeout(10000),
        catchError((err: HttpErrorResponse) => {
          this.isLoadingMerchantDetails = false;
          if (err.status === 404) {
            this.merchantDetailsError = this.translationsService.getSync('loadMerchantsError') || 'Merchant not found.';
          } else if (err.status === 0) {
            this.merchantDetailsError = this.translationsService.getSync('connectionFailed');
          } else {
            this.merchantDetailsError = err.error?.message || err.message || this.translationsService.getSync('loadMerchantsError');
          }
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe((response) => {
        this.isLoadingMerchantDetails = false;
        if (!response) {
          this.cdr.detectChanges();
          return;
        }

        let merchantData = null;
        if (response.resultData !== undefined && response.resultData !== null) {
          merchantData = response.resultData;
        } else if (response.data !== undefined && response.data !== null) {
          merchantData = response.data;
        } else if (Array.isArray(response) && response.length > 0) {
          merchantData = response[0];
        } else if (response && typeof response === 'object') {
          merchantData = response;
        }

        this.selectedMerchant = this.mapMerchantDetails(merchantData, merchant);
        this.cdr.detectChanges();
      });

    this.cdr.detectChanges();
  }

  private mapMerchantDetails(raw: any, fallback: Merchant): MerchantDetails {
    const data = raw && typeof raw === 'object' ? raw : {};

    let status: 'active' | 'suspended' | 'pending' = fallback.status;
    if (data.status === 1 || data.status === 'active' || data.isActive === true || data.statusId === 1) {
      status = 'active';
    } else if (data.status === 2 || data.status === 'suspended' || data.isSuspended === true || data.statusId === 2) {
      status = 'suspended';
    } else if (data.status === 0 || data.status === 'pending' || data.isPending === true || data.statusId === 0) {
      status = 'pending';
    }

    const primaryPhone = String(data.phoneNumber ?? data.mobileNumber ?? data.phone ?? '').trim();
    const secondaryPhone = String(data.phoneNumber2 ?? data.secondaryPhone ?? data.alternatePhone ?? '').trim();
    const phoneNumbers = [primaryPhone, secondaryPhone].filter(Boolean);
    const imageRaw = this.pickMerchantImageRaw(data);
    const imageUrl = imageRaw ? this.resolveMediaUrl(imageRaw) : (fallback.imageUrl?.trim() || '');

    const reviewsRaw = Array.isArray(data.reviews) ? data.reviews : [];
    const reviews: MerchantReview[] = reviewsRaw.map((review: any, index: number) => ({
      id: Number(review?.id ?? index + 1),
      date: String(review?.date ?? ''),
      reviewerName:
        this.languageService.getLocalizedName(review?.user ?? {}) ||
        String(review?.user?.name ?? review?.user?.nameAr ?? review?.user?.nameEn ?? 'User'),
      text: String(review?.comment ?? ''),
      rating: Number(review?.rating ?? 0),
      imagePaths: Array.isArray(review?.reviewImages)
        ? review.reviewImages
            .map((img: any) => String(img?.imagePath ?? '').trim())
            .filter(Boolean)
        : []
    }));

    return {
      id: Number(data.id ?? data.merchantId ?? fallback.id ?? 0),
      name:
        this.languageService.getLocalizedName(data) ||
        fallback.name ||
        String(data.name ?? data.nameAr ?? data.nameEn ?? ''),
      activityType:
        this.languageService.getLocalizedField(data, 'activityType') ||
        fallback.activityType ||
        String(data.activityType ?? data.businessType ?? ''),
      region:
        this.languageService.getLocalizedField(data, 'region') ||
        fallback.region ||
        String(data.region ?? data.city ?? data.location ?? ''),
      salesVolume: Number(data.salesVolume ?? data.totalSales ?? fallback.salesVolume ?? 0),
      orderCount: Number(data.orderCount ?? data.totalOrders ?? fallback.orderCount ?? 0),
      rating: Number(data.rating ?? data.averageRating ?? fallback.rating ?? 0),
      status,
      imageUrl,
      description:
        this.languageService.getCurrentLanguageValue() === 'en'
          ? String(data.descriptionEn ?? data.description ?? '')
          : String(data.description ?? data.descriptionEn ?? ''),
      story:
        this.languageService.getCurrentLanguageValue() === 'en'
          ? String(data.storyEn ?? data.story ?? '')
          : String(data.story ?? data.storyEn ?? ''),
      phoneNumbers,
      reviews,
    };
  }

  closeMerchantDetails(): void {
    this.showMerchantDetails = false;
    this.selectedMerchant = null;
    this.isLoadingMerchantDetails = false;
    this.merchantDetailsError = '';
    if (this.merchantDetailsSubscription) {
      this.merchantDetailsSubscription.unsubscribe();
      this.merchantDetailsSubscription = undefined;
    }
    this.cdr.detectChanges();
  }

  deactivateMerchant(): void {
    if (this.selectedMerchant) {
      this.showDeactivateModal = true;
      this.cdr.detectChanges();
    }
  }

  closeDeactivateModal(): void {
    this.showDeactivateModal = false;
    this.deactivateReason = '';
    this.cdr.detectChanges();
  }

  confirmDeactivate(): void {
    if (!this.deactivateReason) {
      // Show validation message
      return;
    }

    if (this.selectedMerchant) {
      this.selectedMerchant.status = 'suspended';
      this.updateStats();
      this.applyFilters();
      this.closeDeactivateModal();
      this.cdr.detectChanges();
      // TODO: Call API to deactivate merchant with reason
      console.log('Deactivating merchant:', this.selectedMerchant.id, 'Reason:', this.deactivateReason);
    }
  }

  deleteReview(reviewId: number): void {
    // TODO: Implement delete review functionality
    console.log('Delete review:', reviewId);
  }

  getStars(rating: number): number[] {
    const stars = Math.max(0, Math.min(5, Math.round(Number(rating) || 0)));
    return Array(stars).fill(0);
  }

  chatWithMerchant(merchant: Merchant): void {
    console.log('Chat with merchant:', merchant);
    // Implement chat/message logic
  }

  openAddMerchantPanel(): void {
    this.showAddMerchantPanel = true;
    this.cdr.detectChanges();
  }

  closeAddMerchantPanel(): void {
    this.showAddMerchantPanel = false;
    this.cdr.detectChanges();
  }

  onMerchantCreated(): void {
    this.loadMerchants();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.merchantDetailsSubscription) {
      this.merchantDetailsSubscription.unsubscribe();
    }
    if (this.statusUpdateSubscription) {
      this.statusUpdateSubscription.unsubscribe();
    }
  }
}
