import { Component, ViewChild, ElementRef, inject, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { timeout, catchError, of, Subscription } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { LanguageService } from '../../services/language.service';
import { AuthService } from '../../services/auth.service';

const BRANCH_STRINGS = {
  ar: {
    description: 'تتحكم في جميع فروعك، أضف فروعًا جديدة وقم بالتحكم في كل فرع على حده من مكان واحد.',
    addBranch: 'إضافة فرع جديد',
    loadingBranches: 'جاري تحميل الفروع...',
    loadingTimeoutMsg: 'الاتصال بالخادم يستغرق وقتاً أطول من المعتاد...',
    retry: 'إعادة المحاولة',
    noBranches: 'لا توجد فروع',
    noBranchesDesc: 'لم يتم العثور على أي فروع. ابدأ بإضافة فرع جديد.',
    active: 'نشط',
    inactive: 'غير نشط',
    revenue: 'الإيرادات',
    currentSurplus: 'الفائض الحالي',
    currency: 'ر.س',
    edit: 'تعديل',
    deleteConfirmTitle: 'تأكيد حذف الفرع',
    deleteConfirmDesc: 'هل أنت متأكد من حذف الفرع "{{name}}"؟ لا يمكن التراجع عن هذا الإجراء.',
    cancel: 'إلغاء',
    confirmDelete: 'تأكيد الحذف',
    deleting: 'جاري الحذف...',
    addBranchTitle: 'إضافة فرع جديد',
    branchImage: 'صورة الفرع',
    changeImage: 'تغيير الصورة',
    dragOrClickUpload: 'اسحب الصورة أو اقر للرفع',
    branchDetails: 'بيانات الفرع',
    branchName: 'اسم الفرع',
    branchNamePlaceholder: 'أدخل اسم الفرع',
    neighborhood: 'اسم الحي',
    neighborhoodPlaceholder: 'أدخل اسم الحي',
    branchNameEn: 'اسم الفرع بالإنجليزية',
    branchNameEnPlaceholder: 'ادخل اسم الفرع بالإنجليزية',
    neighborhoodEn: 'اسم الحي بالإنجليزية',
    neighborhoodEnPlaceholder: 'ادخل اسم الحي بالإنجليزية',
    location: 'الموقع',
    manualLocationPlaceholder: 'أدخل الموقع يدويًا',
    locationPlaceholder: 'سيتم تحديد الموقع تلقائياً',
    detectLocation: 'اكتشاف موقعي الحالي',
    openMap: 'فتح الخريطة',
    latitude: 'خط العرض',
    longitude: 'خط الطول',
    workingHours: 'ساعات العمل',
    from: 'من',
    to: 'إلى',
    workingHoursPlaceholder: 'من 9 ص إلى 12 م',
    branchStatus: 'حالة الفرع',
    deleteBranch: 'حذف الفرع',
    saveChanges: 'حفظ التعديلات',
    saving: 'جاري الحفظ...',
    adding: 'جاري الإضافة...',
    pleaseLogin: 'يرجى تسجيل الدخول مرة أخرى',
    requestTimeout: 'انتهت مهلة الطلب. يرجى التحقق من الاتصال بالإنترنت وإعادة المحاولة',
    connectionFailed: 'تعذر الاتصال بالخادم. يرجى التحقق من الاتصال بالإنترنت',
    loadBranchesError: 'حدث خطأ أثناء تحميل الفروع',
    geolocationNotSupported: 'المتصفح لا يدعم تحديد الموقع الجغرافي',
    locationDenied: 'تم رفض طلب الوصول إلى الموقع الجغرافي',
    locationUnavailable: 'معلومات الموقع غير متاحة',
    locationTimeout: 'انتهت مهلة طلب الموقع',
    locationError: 'حدث خطأ غير معروف أثناء الحصول على الموقع',
    cannotDeleteBranchTitle: 'لا يمكن حذف الفرع',
    cannotDeleteBranchDesc:
      'لا يمكن حذف هذا الفرع لأنه مرتبط بمنتجات. أزل أو انقل المنتجات أولاً ثم حاول مرة أخرى.',
    gotIt: 'حسناً',
    branchDeletedSuccess: 'تم حذف الفرع بنجاح',
    toastClose: 'إغلاق'
  },
  en: {
    description: 'Manage all your branches, add new branches and control each branch from one place.',
    addBranch: 'Add new branch',
    loadingBranches: 'Loading branches...',
    loadingTimeoutMsg: 'Connection is taking longer than usual...',
    retry: 'Retry',
    noBranches: 'No branches',
    noBranchesDesc: 'No branches found. Start by adding a new branch.',
    active: 'Active',
    inactive: 'Inactive',
    revenue: 'Revenue',
    currentSurplus: 'Current surplus',
    currency: 'SAR',
    edit: 'Edit',
    deleteConfirmTitle: 'Confirm branch deletion',
    deleteConfirmDesc: 'Are you sure you want to delete branch "{{name}}"? This action cannot be undone.',
    cancel: 'Cancel',
    confirmDelete: 'Confirm delete',
    deleting: 'Deleting...',
    addBranchTitle: 'Add new branch',
    branchImage: 'Branch image',
    changeImage: 'Change image',
    dragOrClickUpload: 'Drag image or click to upload',
    branchDetails: 'Branch details',
    branchName: 'Branch name',
    branchNamePlaceholder: 'Enter branch name',
    neighborhood: 'Neighborhood',
    neighborhoodPlaceholder: 'Enter neighborhood name',
    branchNameEn: 'Branch name (English)',
    branchNameEnPlaceholder: 'Enter Branch name (English)',
    neighborhoodEn: 'Neighborhood (English)',
    neighborhoodEnPlaceholder: 'Enter Neighborhood (English)',
    location: 'Location',
    manualLocationPlaceholder: 'Enter location manually',
    locationPlaceholder: 'Location will be set automatically',
    detectLocation: 'Detect current location',
    openMap: 'Open map',
    latitude: 'Latitude',
    longitude: 'Longitude',
    workingHours: 'Working hours',
    from: 'From',
    to: 'To',
    workingHoursPlaceholder: 'e.g. 9 AM to 12 PM',
    branchStatus: 'Branch status',
    deleteBranch: 'Delete branch',
    saveChanges: 'Save changes',
    saving: 'Saving...',
    adding: 'Adding...',
    pleaseLogin: 'Please log in again',
    requestTimeout: 'Request timed out. Please check your connection and try again',
    connectionFailed: 'Unable to connect to server. Please check your connection',
    loadBranchesError: 'An error occurred while loading branches',
    geolocationNotSupported: 'Browser does not support geolocation',
    locationDenied: 'Location access was denied',
    locationUnavailable: 'Location information is unavailable',
    locationTimeout: 'Location request timed out',
    locationError: 'An unknown error occurred while getting location',
    cannotDeleteBranchTitle: 'Cannot delete this branch',
    cannotDeleteBranchDesc:
      'This branch cannot be deleted because it still has products linked to it. Remove or move those products first, then try again.',
    gotIt: 'Got it',
    branchDeletedSuccess: 'Branch deleted successfully',
    toastClose: 'Close'
  }
};

interface Branch {
  id?: number;
  name?: string;
  nameAr?: string;
  nameEn?: string;
  location?: string;
  latitude?: number;
  longitude?: number;
  operatingHours?: string;
  revenue?: string;
  currentSurplus?: string;
  status?: string;
  image?: string;
  neighborhoodAr?: string;
  neighborhoodEn?: string;
  area?: string;
  areaId?: number;
  merchant?: string;
  openingTime?: string;
  closingTime?: string;
}

interface AreaOption {
  id: number;
  name: string;
  nameEn: string;
}

@Component({
  selector: 'app-branch-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './branch-management.component.html',
  styleUrls: ['./branch-management.component.scss']
})
export class BranchManagementComponent implements OnInit, OnDestroy {
  @ViewChild('imageInput', { static: false }) imageInput!: ElementRef<HTMLInputElement>;

  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private cdr = inject(ChangeDetectorRef);
  private authService = inject(AuthService);

  t = BRANCH_STRINGS.ar;
  isMerchant = false;
  merchantDisplayName = '';

  isModalOpen = false;
  isEditMode = false;
  editingBranch: any = null;
  showDeleteConfirmationModal = false;
  /** Shown when delete fails because the branch still has products (replaces browser alert). */
  showCannotDeleteBranchModal = false;
  /** Success toast after branch is deleted. */
  branchDeletedToastVisible = false;
  private branchDeletedToastTimer?: ReturnType<typeof setTimeout>;
  branchToDelete: Branch | null = null;
  isDeleting = false;
  isLoading = false;
  isLoadingBranches = false;
  errorMessage = '';

  newBranch = {
    id: 0,
    nameAr: '',
    nameEn: '',
    neighborhoodAr: '',
    neighborhoodEn: '',
    location: '',
    latitude: '',
    longitude: '',
    openingTime: '',
    closingTime: '',
    status: 'active',
    image: null as File | null,
    imageUrl: ''
  };

  showMapPickerModal = false;
  mapPickerLat: number = 24.7136;
  mapPickerLng: number = 46.6753;
  private mapPickerInstance: any = null;
  private mapPickerMarker: any = null;
  isGettingLocation = false;
  mapUrl: SafeResourceUrl | null = null;
  private sanitizer = inject(DomSanitizer);
  private languageService = inject(LanguageService);
  private languageSubscription?: Subscription;
  branches: Branch[] = [];
  areas: AreaOption[] = [];
  loadingTimeout = false;
  private readonly REQUEST_TIMEOUT = 15000; // 15 seconds for slow APIs
  private static branchesCache: { data: any[]; timestamp: number } | null = null;
  private static readonly CACHE_TTL_MS = 2 * 60 * 1000; // 2 minutes

  ngOnInit() {
    this.isMerchant = this.authService.isMerchant();
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      try {
        const user = JSON.parse(savedUser);
        this.merchantDisplayName = String(
          user?.name ??
          user?.nameAr ??
          user?.nameEn ??
          user?.userName ??
          user?.username ??
          user?.id ??
          ''
        ).trim();
      } catch {
        this.merchantDisplayName = '';
      }
    }
    this.updateLanguage();
    this.languageSubscription = this.languageService.getCurrentLanguage().subscribe(() => {
      this.updateLanguage();
      if (this.branches.length > 0) {
        this.mapBranchesWithLanguage(this.branches);
      }
    });
    this.loadBranches();
  }

  private updateLanguage(): void {
    const isEn = this.languageService.getCurrentLanguageValue() === 'en';
    this.t = isEn ? BRANCH_STRINGS.en : BRANCH_STRINGS.ar;
  }

  getDeleteConfirmMessage(): string {
    const name = this.branchToDelete?.name ?? this.branchToDelete?.nameAr ?? '';
    return this.t.deleteConfirmDesc.replace('{{name}}', name);
  }

  ngOnDestroy() {
    this.languageSubscription?.unsubscribe();
    if (this.branchDeletedToastTimer !== undefined) {
      clearTimeout(this.branchDeletedToastTimer);
    }
  }

  private showBranchDeletedToast(): void {
    if (this.branchDeletedToastTimer !== undefined) {
      clearTimeout(this.branchDeletedToastTimer);
    }
    this.branchDeletedToastVisible = true;
    this.cdr.detectChanges();
    this.branchDeletedToastTimer = setTimeout(() => {
      this.branchDeletedToastVisible = false;
      this.branchDeletedToastTimer = undefined;
      this.cdr.detectChanges();
    }, 4200);
  }

  dismissBranchDeletedToast(): void {
    if (this.branchDeletedToastTimer !== undefined) {
      clearTimeout(this.branchDeletedToastTimer);
      this.branchDeletedToastTimer = undefined;
    }
    this.branchDeletedToastVisible = false;
    this.cdr.detectChanges();
  }

  /**
   * Map branches array with language-aware fields
   */
  private mapBranchesWithLanguage(branchesData: any[]): void {
    this.branches = branchesData.map((branch: any) => this.mapBranchWithLanguage(branch));
  }

  /**
   * Map single branch with language-aware fields
   */
  private mapBranchWithLanguage(branch: any): Branch {
    return {
      id: branch.id,
      name: this.languageService.getLocalizedName(branch),
      nameAr: branch.nameAr || branch.name,
      nameEn: branch.nameEn || branch.name,
      location: this.languageService.getLocalizedField(branch, 'location') ||
        (this.languageService.isArabic()
          ? `${branch.neighborhood || ''}، ${branch.area || ''}`.trim()
          : `${branch.neighborhoodEn || ''}, ${branch.area || ''}`.trim()),
      latitude: branch.latitude,
      longitude: branch.longitude,
      operatingHours: this.formatOperatingHours(branch.openingTime, branch.closingTime),
      revenue: branch.revenue || '0',
      currentSurplus: branch.currentSurplus || '0',
      status: branch.status || 'active',
      image: branch.imagePath || branch.image || 'assets/images/Image (فرع الرياض - النخيل).png',
      neighborhoodAr: branch.neighborhood || branch.neighborhoodAr,
      neighborhoodEn: branch.neighborhoodEn,
      area: branch.area,
      areaId: branch.areaId,
      merchant: branch.merchant,
      openingTime: branch.openingTime,
      closingTime: branch.closingTime
    };
  }

  loadBranches() {
    this.errorMessage = '';
    this.loadingTimeout = false;

    const token = localStorage.getItem('token');
    if (!token) {
      console.error('No token found in localStorage. Please login again.');
      this.errorMessage = this.t.pleaseLogin;
      this.isLoadingBranches = false;
      return;
    }

    // Show cached data immediately when available and not stale (faster perceived load)
    const cached = BranchManagementComponent.branchesCache;
    if (cached && (Date.now() - cached.timestamp) < BranchManagementComponent.CACHE_TTL_MS && cached.data.length > 0) {
      this.mapBranchesWithLanguage(cached.data);
      this.isLoadingBranches = false;
      this.cdr.detectChanges();
    } else {
      this.isLoadingBranches = true;
    }

    const timeoutIndicator = setTimeout(() => {
      if (this.isLoadingBranches) {
        this.loadingTimeout = true;
        this.cdr.detectChanges();
      }
    }, 3000);

    this.http.get<any>(this.apiService.getUrl('Branches/GetAll'))
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          clearTimeout(timeoutIndicator);
          this.isLoadingBranches = false;
          this.loadingTimeout = false;
          if (error.name === 'TimeoutError') {
            this.errorMessage = this.t.requestTimeout;
          } else {
            throw error; // Re-throw to handle in subscribe
          }
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (response) => {
          clearTimeout(timeoutIndicator);
          this.isLoadingBranches = false;
          this.loadingTimeout = false;
          if (!response) {
            this.cdr.detectChanges();
            return;
          }
          const resultData = response.resultData !== undefined ? response.resultData : (response.data !== undefined ? response.data : response);
          if (resultData === null || resultData === undefined || (Array.isArray(resultData) && resultData.length === 0)) {
            this.branches = [];
            this.cdr.detectChanges();
            return;
          }
          const branchesArray = Array.isArray(resultData) ? resultData : [resultData];
          BranchManagementComponent.branchesCache = { data: branchesArray, timestamp: Date.now() };
          this.mapBranchesWithLanguage(branchesArray);
          this.cdr.detectChanges();
        },
        error: (error: HttpErrorResponse) => {
          clearTimeout(timeoutIndicator);
          this.isLoadingBranches = false;
          this.loadingTimeout = false;
          console.error('Error loading branches:', error);
          if (error.status === 0) {
            this.errorMessage = this.t.connectionFailed;
          } else if (error.status === 408 || error.status === 504) {
            this.errorMessage = this.t.requestTimeout;
          } else {
            this.errorMessage = error.error?.message || this.t.loadBranchesError;
          }
          this.branches = [];
          this.cdr.detectChanges();
        }
      });
  }

  formatOperatingHours(openingTime?: string, closingTime?: string): string {
    if (!openingTime || !closingTime) {
      return '8:00 ص - 11:00 م';
    }

    // Convert 24-hour format to 12-hour format with Arabic indicators
    const formatTime = (timeStr: string): string => {
      if (!timeStr) return '';

      // Handle format like "08:00:00" or "08:00"
      const timeMatch = timeStr.match(/(\d{1,2}):(\d{2})/);
      if (!timeMatch) return timeStr;

      let hours = parseInt(timeMatch[1]);
      const minutes = timeMatch[2];
      const period = hours >= 12 ? 'م' : 'ص';

      if (hours > 12) {
        hours -= 12;
      } else if (hours === 0) {
        hours = 12;
      }

      return `${hours}:${minutes} ${period}`;
    };

    return `${formatTime(openingTime)} - ${formatTime(closingTime)}`;
  }

  addNewBranch() {
    this.isEditMode = false;
    this.editingBranch = null;
    this.resetForm();
    this.updateMapUrl();
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.isEditMode = false;
    this.editingBranch = null;
    this.resetForm();
  }

  resetForm() {
    this.newBranch = {
      id: 0,
      nameAr: '',
      nameEn: '',
      neighborhoodAr: '',
      neighborhoodEn: '',
      location: '',
      latitude: '',
      longitude: '',
      openingTime: '',
      closingTime: '',
      status: 'active',
      image: null,
      imageUrl: ''
    };
    this.errorMessage = '';
    this.mapUrl = null;
  }

  useCurrentLocation() {
    if (!navigator.geolocation) {
      this.errorMessage = this.t.geolocationNotSupported;
      return;
    }

    this.isGettingLocation = true;
    this.errorMessage = '';

    navigator.geolocation.getCurrentPosition(
      (position) => {
        this.newBranch.latitude = position.coords.latitude.toString();
        this.newBranch.longitude = position.coords.longitude.toString();
        this.newBranch.location = `${position.coords.latitude.toFixed(6)}, ${position.coords.longitude.toFixed(6)}`;
        this.isGettingLocation = false;
        this.updateMapUrl();
        this.cdr.detectChanges();
      },
      (error) => {
        this.isGettingLocation = false;
        switch (error.code) {
          case error.PERMISSION_DENIED:
            this.errorMessage = this.t.locationDenied;
            break;
          case error.POSITION_UNAVAILABLE:
            this.errorMessage = this.t.locationUnavailable;
            break;
          case error.TIMEOUT:
            this.errorMessage = this.t.locationTimeout;
            break;
          default:
            this.errorMessage = this.t.locationError;
            break;
        }
      },
      {
        enableHighAccuracy: true,
        timeout: 10000,
        maximumAge: 0
      }
    );
  }

  updateMapUrl() {
    let url: string;
    if (this.newBranch.latitude && this.newBranch.longitude) {
      const lat = parseFloat(this.newBranch.latitude);
      const lng = parseFloat(this.newBranch.longitude);
      // Using OpenStreetMap embed (no API key required)
      url = `https://www.openstreetmap.org/export/embed.html?bbox=${lng - 0.01},${lat - 0.01},${lng + 0.01},${lat + 0.01}&layer=mapnik&marker=${lat},${lng}`;
    } else {
      // Default to Riyadh, Saudi Arabia
      url = `https://www.openstreetmap.org/export/embed.html?bbox=46.6,24.6,46.8,24.8&layer=mapnik&marker=24.7136,46.6753`;
    }
    this.mapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

  onMapClick(event: MouseEvent) {
    this.mapPickerLat = parseFloat(this.newBranch.latitude) || 24.7136;
    this.mapPickerLng = parseFloat(this.newBranch.longitude) || 46.6753;
    this.showMapPickerModal = true;

    setTimeout(() => this.initMapPicker(), 100);
  }

  private initMapPicker() {
    if ((window as any).L) {
      this.setupLeafletMap();
      return;
    }

    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
    document.head.appendChild(link);

    const script = document.createElement('script');
    script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
    script.onload = () => this.setupLeafletMap();
    document.head.appendChild(script);
  }

  private setupLeafletMap() {
    const L = (window as any).L;
    const container = document.getElementById('map-picker-container');
    if (!container) return;

    // Destroy previous instance if any
    if (this.mapPickerInstance) {
      this.mapPickerInstance.remove();
      this.mapPickerInstance = null;
      this.mapPickerMarker = null;
    }

    this.mapPickerInstance = L.map('map-picker-container').setView(
      [this.mapPickerLat, this.mapPickerLng], 13
    );

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.mapPickerInstance);

    if (this.newBranch.latitude && this.newBranch.longitude) {
      this.mapPickerMarker = L.marker([this.mapPickerLat, this.mapPickerLng])
        .addTo(this.mapPickerInstance);
    }

    this.mapPickerInstance.on('click', (e: any) => {
      this.mapPickerLat = e.latlng.lat;
      this.mapPickerLng = e.latlng.lng;

      if (this.mapPickerMarker) {
        this.mapPickerMarker.setLatLng(e.latlng);
      } else {
        this.mapPickerMarker = L.marker(e.latlng).addTo(this.mapPickerInstance);
      }
      this.cdr.detectChanges();
    });
  }

  confirmMapLocation() {
    this.newBranch.latitude = this.mapPickerLat.toFixed(6);
    this.newBranch.longitude = this.mapPickerLng.toFixed(6);
    this.newBranch.location = `${this.newBranch.latitude}, ${this.newBranch.longitude}`;
    this.closeMapPicker();
    this.updateMapUrl();
  }

  closeMapPicker() {
    if (this.mapPickerInstance) {
      this.mapPickerInstance.remove();
      this.mapPickerInstance = null;
      this.mapPickerMarker = null;
    }
    this.showMapPickerModal = false;
  }

  onCoordinatesChange() {
    this.updateMapUrl();
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.newBranch.image = file;
      // Create preview URL for edit mode
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.newBranch.imageUrl = e.target.result;
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmitBranch() {
    const nameAr = String(this.newBranch.nameAr ?? '').trim();
    const nameEn = String(this.newBranch.nameEn ?? '').trim();
    const neighborhoodAr = String(this.newBranch.neighborhoodAr ?? '').trim();
    const neighborhoodEn = String(this.newBranch.neighborhoodEn ?? '').trim();
    const latitude = String(this.newBranch.latitude ?? '').trim();
    const longitude = String(this.newBranch.longitude ?? '').trim();
    const openingTime = String(this.newBranch.openingTime ?? '').trim();
    const closingTime = String(this.newBranch.closingTime ?? '').trim();
    const locationValue = String(this.newBranch.location ?? '').trim();

    // Backend requires both Arabic + English values.
    // If the user filled only one side, mirror it to the other to avoid 400 validation.
    this.newBranch.nameAr = nameAr || nameEn;
    this.newBranch.nameEn = nameEn || nameAr;
    this.newBranch.neighborhoodAr = neighborhoodAr || neighborhoodEn;
    this.newBranch.neighborhoodEn = neighborhoodEn || neighborhoodAr;
    this.newBranch.openingTime = openingTime;
    this.newBranch.closingTime = closingTime;

    // If location text is empty but coordinates exist, auto-generate location.
    if (!locationValue && latitude && longitude) {
      this.newBranch.location = `${latitude}, ${longitude}`;
    } else {
      this.newBranch.location = locationValue;
    }

    // Validate required fields
    // Frontend required-fields blocking removed by request.

    this.submitBranch();
  }

  isValidForm(): boolean {
    return !this.newBranch.nameAr || !this.newBranch.nameEn ||
      !this.newBranch.neighborhoodAr || !this.newBranch.neighborhoodEn ||
      !this.newBranch.latitude || !this.newBranch.longitude ||
      !this.newBranch.openingTime || !this.newBranch.closingTime ||
      (!this.newBranch.image && !this.isEditMode);
  }

  submitBranch() {
    this.isLoading = true;
    this.errorMessage = '';

    const nameAr = String(this.newBranch.nameAr ?? '').trim();
    const nameEn = String(this.newBranch.nameEn ?? '').trim();
    const neighborhoodAr = String(this.newBranch.neighborhoodAr ?? '').trim();
    const neighborhoodEn = String(this.newBranch.neighborhoodEn ?? '').trim();
    const safeName = nameAr || nameEn || 'Branch';
    const safeNameEn = nameEn || nameAr || 'Branch';
    const safeNeighborhood = neighborhoodAr || neighborhoodEn || safeName;
    const safeNeighborhoodEn = neighborhoodEn || neighborhoodAr || safeNameEn;

    // Send as multipart/form-data to match backend [FromForm] binding.
    const openingTime24 = this.convertTo24HourFormat(this.newBranch.openingTime) || '09:00:00';
    const closingTime24 = this.convertTo24HourFormat(this.newBranch.closingTime) || '22:00:00';
    let latitudeValue = Number.parseFloat(String(this.newBranch.latitude ?? '').trim());
    let longitudeValue = Number.parseFloat(String(this.newBranch.longitude ?? '').trim());
    if ((!Number.isFinite(latitudeValue) || !Number.isFinite(longitudeValue)) && this.newBranch.location) {
      const parts = String(this.newBranch.location).split(',');
      if (parts.length >= 2) {
        const parsedLat = Number.parseFloat(parts[0].trim());
        const parsedLng = Number.parseFloat(parts[1].trim());
        if (Number.isFinite(parsedLat) && Number.isFinite(parsedLng)) {
          latitudeValue = parsedLat;
          longitudeValue = parsedLng;
        }
      }
    }
    if (!Number.isFinite(latitudeValue)) latitudeValue = 0;
    if (!Number.isFinite(longitudeValue)) longitudeValue = 0;

    const merchantId = this.authService.getMerchantId();
    const handleResponse = (response: any) => {
      this.isLoading = false;

      const failed = response?.succeeded === false ||
        response?.success === false ||
        response?.isSuccess === false ||
        response?.failed === true;

      if (failed) {
        const errorMessages: string[] = [];
        if (typeof response?.message === 'string' && response.message.trim()) {
          errorMessages.push(response.message);
        }
        if (typeof response?.error === 'string' && response.error.trim()) {
          errorMessages.push(response.error);
        }
        const validationErrors = response?.errors ?? response?.error ?? response?.message;
        if (validationErrors && typeof validationErrors === 'object') {
          Object.keys(validationErrors).forEach(key => {
            const value = validationErrors[key];
            if (Array.isArray(value)) {
              errorMessages.push(...value.map((v: any) => String(v)));
            } else if (typeof value === 'string') {
              errorMessages.push(value);
            }
          });
        }

        this.errorMessage = errorMessages.filter(Boolean).join('\n') ||
          response?.message ||
          response?.error ||
          'بيانات غير صحيحة. يرجى التحقق من الحقول';
        this.cdr.detectChanges();
        return;
      }

      this.closeModal();
      BranchManagementComponent.branchesCache = null;
      this.loadBranches();
    };

    const handleError = (error: HttpErrorResponse) => {
      this.isLoading = false;
      console.error('Branch submit error:', error);

      if (error.status === 400 && error.error) {
        const validationErrors = error.error;
        const errorMessages: string[] = [];
        Object.keys(validationErrors).forEach(key => {
          if (Array.isArray(validationErrors[key])) {
            errorMessages.push(...validationErrors[key]);
          } else {
            errorMessages.push(validationErrors[key]);
          }
        });
        this.errorMessage = errorMessages.join('\n') || 'بيانات غير صحيحة. يرجى التحقق من الحقول';
      } else if (error.status === 0) {
        this.errorMessage = 'تعذر الاتصال بالخادم. يرجى التحقق من الاتصال بالإنترنت';
      } else {
        this.errorMessage = error.error?.message || 'حدث خطأ أثناء حفظ الفرع. يرجى المحاولة مرة أخرى';
      }
    };

    const formData = new FormData();
    formData.append('Id', String(this.newBranch.id));
    formData.append('Name', safeName);
    formData.append('NameEn', safeNameEn);
    formData.append('Neighborhood', safeNeighborhood);
    formData.append('NeighborhoodEn', safeNeighborhoodEn);
    if (merchantId != null) {
      formData.append('MerchantId', String(merchantId));
    }
    formData.append('Latitude', String(latitudeValue));
    formData.append('Longitude', String(longitudeValue));
    formData.append('OpeningTime', openingTime24);
    formData.append('ClosingTime', closingTime24);
    formData.append('Status', this.newBranch.status || 'active');
    if (this.newBranch.image) {
      formData.append('ImageFile', this.newBranch.image, this.newBranch.image.name);
    }

    const request$ = this.isEditMode
      ? this.http.put<any>(this.apiService.getUrl('Branches/Update'), formData)
      : this.http.post<any>(this.apiService.getUrl('Branches/Create'), formData);

    request$.subscribe({ next: handleResponse, error: handleError });
  }

  updateBranch() {
    this.isLoading = true;
    this.errorMessage = '';

    const id = String(this.newBranch.id ?? '').trim();
    const nameAr = String(this.newBranch.nameAr ?? '').trim();
    const nameEn = String(this.newBranch.nameEn ?? '').trim();
    const neighborhoodAr = String(this.newBranch.neighborhoodAr ?? '').trim();
    const neighborhoodEn = String(this.newBranch.neighborhoodEn ?? '').trim();
    const safeName = nameAr || nameEn || 'Branch';
    const safeNameEn = nameEn || nameAr || 'Branch';
    const safeNeighborhood = neighborhoodAr || neighborhoodEn || safeName;
    const safeNeighborhoodEn = neighborhoodEn || neighborhoodAr || safeNameEn;

    // Send as multipart/form-data to match backend [FromForm] binding.
    const openingTime24 = this.convertTo24HourFormat(this.newBranch.openingTime) || '09:00:00';
    const closingTime24 = this.convertTo24HourFormat(this.newBranch.closingTime) || '22:00:00';
    let latitudeValue = Number.parseFloat(String(this.newBranch.latitude ?? '').trim());
    let longitudeValue = Number.parseFloat(String(this.newBranch.longitude ?? '').trim());
    if ((!Number.isFinite(latitudeValue) || !Number.isFinite(longitudeValue)) && this.newBranch.location) {
      const parts = String(this.newBranch.location).split(',');
      if (parts.length >= 2) {
        const parsedLat = Number.parseFloat(parts[0].trim());
        const parsedLng = Number.parseFloat(parts[1].trim());
        if (Number.isFinite(parsedLat) && Number.isFinite(parsedLng)) {
          latitudeValue = parsedLat;
          longitudeValue = parsedLng;
        }
      }
    }
    if (!Number.isFinite(latitudeValue)) latitudeValue = 0;
    if (!Number.isFinite(longitudeValue)) longitudeValue = 0;

    const body = {
      name: safeName,
      nameEn: safeNameEn,
      neighborhood: safeNeighborhood,
      neighborhoodEn: safeNeighborhoodEn,
      merchantId: this.authService.getMerchantId(),
      latitude: latitudeValue,
      longitude: longitudeValue,
      openingTime: openingTime24,
      closingTime: closingTime24
    }

    // Send POST request to create branch
    this.http.put<any>(this.apiService.getUrl('Branches/Update'), body)
      .subscribe({
        next: (response) => {
          this.isLoading = false;
          console.log('Branch created successfully:', response);

          // Close modal and reset form
          this.closeModal();
          BranchManagementComponent.branchesCache = null;
          this.loadBranches();
        },
        error: (error: HttpErrorResponse) => {
          this.isLoading = false;
          console.error('Create branch error:', error);

          // Handle validation errors
          if (error.status === 400 && error.error) {
            const validationErrors = error.error;
            const errorMessages: string[] = [];

            // Collect all validation error messages
            Object.keys(validationErrors).forEach(key => {
              if (Array.isArray(validationErrors[key])) {
                errorMessages.push(...validationErrors[key]);
              } else {
                errorMessages.push(validationErrors[key]);
              }
            });

            this.errorMessage = errorMessages.join('\n') || 'بيانات غير صحيحة. يرجى التحقق من الحقول';
          } else if (error.status === 0) {
            this.errorMessage = 'تعذر الاتصال بالخادم. يرجى التحقق من الاتصال بالإنترنت';
          } else {
            this.errorMessage = error.error?.message || 'حدث خطأ أثناء إنشاء الفرع. يرجى المحاولة مرة أخرى';
          }
        }
      });
  }

  convertTo24HourFormat(timeString: string): string {
    // Convert time from format like "8:00 ص" or "11:00 م" to 24-hour format "HH:mm:ss"
    // Expected input: "8:00 ص" or "11:00 م"
    // Expected output: "08:00:00" or "23:00:00"

    if (!timeString) return '';

    const trimmed = timeString.trim();
    const isPM = trimmed.includes('م') || trimmed.includes('PM') || trimmed.includes('pm');
    const isAM = trimmed.includes('ص') || trimmed.includes('AM') || trimmed.includes('am');

    // Extract time numbers
    const timeMatch = trimmed.match(/(\d{1,2}):(\d{2})/);
    if (!timeMatch) return '';

    let hours = parseInt(timeMatch[1]);
    const minutes = timeMatch[2];

    if (isPM && hours !== 12) {
      hours += 12;
    } else if (isAM && hours === 12) {
      hours = 0;
    }

    return `${hours.toString().padStart(2, '0')}:${minutes}:00`;
  }

  onOverlayClick(event: Event) {
    if (event.target === event.currentTarget) {
      this.closeModal();
    }
  }

  triggerImageInput() {
    this.imageInput?.nativeElement.click();
  }

  getWorkingHours(): string {
    if (this.newBranch.openingTime && this.newBranch.closingTime) {
      return `من ${this.newBranch.openingTime} إلى ${this.newBranch.closingTime}`;
    }
    return '';
  }

  setWorkingHours(event: any) {
    const value = event.target.value;
    // Parse "من 9 ص إلى 12 م" format
    const match = value.match(/من\s+(.+?)\s+إلى\s+(.+)/);
    if (match) {
      this.newBranch.openingTime = match[1].trim();
      this.newBranch.closingTime = match[2].trim();
    }
  }

  viewDetails(branch: any) {
    // TODO: Implement view details functionality
    console.log('View details for:', branch);
  }

  editBranch(branch: any) {
    this.isEditMode = true;
    this.editingBranch = branch;

    // Convert numeric status to string ('active' or 'inactive')
    let statusStr = 'active';
    if (typeof branch.status === 'number') {
      statusStr = branch.status === 0 ? 'inactive' : 'active';
    } else if (typeof branch.status === 'string') {
      statusStr = branch.status.toLowerCase() === 'inactive' ? 'inactive' : 'active';
    }

    this.newBranch = {
      id: branch.id,
      nameAr: branch.nameAr || branch.name || '',
      nameEn: branch.nameEn || '',
      neighborhoodAr: branch.neighborhoodAr || branch.neighborhood || '',
      neighborhoodEn: branch.neighborhoodEn || '',
      location: branch.location || this.formatLocation(branch),
      latitude: branch.latitude?.toString() || '',
      longitude: branch.longitude?.toString() || '',
      openingTime: this.normalizeTimeForInput(branch.openingTime || ''),
      closingTime: this.normalizeTimeForInput(branch.closingTime || ''),
      status: statusStr,
      image: null,
      imageUrl: branch.imagePath || branch.image || ''
    };

    this.updateMapUrl();
    this.isModalOpen = true;
  }

  private normalizeTimeForInput(value: string): string {
    if (!value) return '';
    const match = value.match(/(\d{1,2}):(\d{2})/);
    if (!match) return value;
    const h = match[1].padStart(2, '0');
    const m = match[2];
    return `${h}:${m}`;
  }

  formatLocation(branch: any): string {
    const parts: string[] = [];
    if (branch.neighborhoodAr || branch.neighborhood) {
      parts.push(branch.neighborhoodAr || branch.neighborhood);
    }
    if (branch.area) {
      parts.push(branch.area);
    }
    if (branch.location) {
      return branch.location;
    }
    return parts.join('، ') || '';
  }

  deleteBranch(branch: any) {
    this.branchToDelete = branch;
    this.showDeleteConfirmationModal = true;
    // If in edit modal, close it when showing delete confirmation
    if (this.isModalOpen && this.editingBranch?.id === branch?.id) {
      this.closeModal();
    }
  }

  closeDeleteConfirmationModal() {
    this.showDeleteConfirmationModal = false;
    this.branchToDelete = null;
    this.errorMessage = '';
  }

  closeCannotDeleteBranchModal() {
    this.showCannotDeleteBranchModal = false;
    this.branchToDelete = null;
    this.errorMessage = '';
  }

  confirmDeleteBranch() {
    if (!this.branchToDelete?.id) {
      this.errorMessage = 'معرف الفرع غير متوفر';
      return;
    }

    this.isDeleting = true;
    this.errorMessage = '';

    this.http.delete(this.apiService.getUrl(`Branches/Delete?id=${this.branchToDelete.id}`))
      .subscribe({
        next: (response: any) => {
          const isDeleteFailed = response?.isSuccess === false || response?.succeeded === false || response?.success === false;
          if (isDeleteFailed) {
            this.isDeleting = false;
            this.showDeleteConfirmationModal = false;
            this.showCannotDeleteBranchModal = true;
            this.cdr.detectChanges();
            return;
          }
          this.isDeleting = false;
          this.closeDeleteConfirmationModal();
          BranchManagementComponent.branchesCache = null;
          this.loadBranches();
          this.showBranchDeletedToast();
        },
        error: (error: HttpErrorResponse) => {
          this.isDeleting = false;
          console.error('Delete branch error:', error);
          this.showDeleteConfirmationModal = false;
          this.showCannotDeleteBranchModal = true;
          this.cdr.detectChanges();
        }
      });
  }
}

