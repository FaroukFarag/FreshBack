import { Component, EventEmitter, Output, OnInit, OnDestroy, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { LanguageService } from '../../services/language.service';
import { BranchContextService } from '../../services/branch-context.service';
import { AuthService } from '../../services/auth.service';

interface Branch {
  id?: number;
  name?: string;
  nameAr?: string;
  nameEn?: string;
}

interface BranchQuantity {
  branchId: number;
  branchName: string;
  quantity: number;
  expiryDate: string;
  startDeliveryDate: string;
  endDeliveryDate: string;
  discount: number;
  views: number;
  status: number;
}

const ADD_PANEL_STRINGS = {
  ar: {
    title: 'إضافة منتج فائض جديد',
    productImages: 'صور المنتج',
    productData: 'بيانات المنتج',
    productCode: 'كود المنتج',
    productCodePlaceholder: 'كود المنتج',
    productNameAr: 'اسم المنتج',
    productNameEn: 'اسم المنتج باللغة الإنجليزية',
    productDescriptionAr: 'وصف المنتج',
    productDescriptionEn: 'وصف المنتج باللغة الإنجليزية',
    allergensAr: 'مسببات الحساسية',
    allergensEn: 'مسببات الحساسية باللغة الإنجليزية',
    price: 'السعر',
    weightInKg: 'الوزن (كجم)',
    branchesAndQuantities: 'الفروع والكميات',
    loadingBranches: 'جاري تحميل الفروع...',
    noBranches: 'لا توجد فروع متاحة',
    dragOrClick: 'اسحب الصورة أو انقر للرفع',
    submitAdd: 'إضافة منتج فائض جديد',
    submitting: 'جاري الإضافة...',
    quantity: 'الكمية',
    discountPercentage: 'نسبة الخصم (%)',
    expiryDate: 'تاريخ الانتهاء',
    startDelivery: 'بداية التوصيل',
    endDelivery: 'نهاية التوصيل',
    removeImage: 'إزالة الصورة',
    errorCodeRequired: 'يرجى إدخال كود المنتج',
    errorNameArRequired: 'يرجى إدخال اسم المنتج',
    errorNameEnRequired: 'يرجى إدخال اسم المنتج بالإنجليزية',
    errorDescriptionArRequired: 'يرجى إدخال وصف المنتج',
    errorDescriptionEnRequired: 'يرجى إدخال وصف المنتج بالإنجليزية',
    errorAllergensRequired: 'يرجى إدخال مسببات الحساسية',
    errorAllergensEnRequired: 'يرجى إدخال مسببات الحساسية بالإنجليزية',
    errorPriceRequired: 'يرجى إدخال سعر صحيح أكبر من صفر',
    errorWeightRequired: 'يرجى إدخال وزن صحيح أكبر من صفر',
    errorImageRequired: 'يرجى رفع صورة واحدة على الأقل',
    errorBranchRequired: 'يرجى اختيار فرع واحد على الأقل',
    errorBranchQuantity: 'يرجى إدخال رقم صحيح لكمية فرع',
    errorBranchDiscount: 'يرجى إدخال رقم صحيح لنسبة خصم فرع',
    errorBranchExpiry: 'يرجى إدخال تاريخ الإنتهاء لفرع',
    errorBranchStartDelivery: 'يرجى إدخال تاريخ بداية التوصيل لفرع',
    errorBranchEndDelivery: 'يرجى إدخال تاريخ نهاية التوصيل لفرع',
    errorBadData: 'بيانات غير صحيحة. يرجى التحقق من الحقول',
    errorNoConnection: 'تعذر الاتصال بالخادم. يرجى التحقق من الاتصال بالإنترنت',
    errorCreateFailed: 'حدث خطأ أثناء إنشاء المنتج. يرجى المحاولة مرة أخرى',
    errorProcessImages: 'حدث خطأ أثناء معالجة الصور'
  },
  en: {
    title: 'Add Surplus Product',
    productImages: 'Product Images',
    productData: 'Product Data',
    productCode: 'Product Code',
    productCodePlaceholder: 'Product Code',
    productNameAr: 'Product Name (Arabic)',
    productNameEn: 'Product Name (English)',
    productDescriptionAr: 'Product Description (Arabic)',
    productDescriptionEn: 'Product Description (English)',
    allergensAr: 'Allergens',
    allergensEn: 'Allergens (English)',
    price: 'Price',
    weightInKg: 'Weight (kg)',
    branchesAndQuantities: 'Branches and Quantities',
    loadingBranches: 'Loading branches...',
    noBranches: 'No branches available',
    dragOrClick: 'Drag image or click to upload',
    submitAdd: 'Add Surplus Product',
    submitting: 'Adding...',
    quantity: 'Quantity',
    discountPercentage: 'Discount Percentage (%)',
    expiryDate: 'Expiry Date',
    startDelivery: 'Start Delivery',
    endDelivery: 'End Delivery',
    removeImage: 'Remove image',
    errorCodeRequired: 'Please enter the product code',
    errorNameArRequired: 'Please enter the product name',
    errorNameEnRequired: 'Please enter the product name in English',
    errorDescriptionArRequired: 'Please enter the product description',
    errorDescriptionEnRequired: 'Please enter the product description in English',
    errorAllergensRequired: 'Please enter the allergens',
    errorAllergensEnRequired: 'Please enter the allergens in English',
    errorPriceRequired: 'Please enter a valid price greater than zero',
    errorWeightRequired: 'Please enter a valid weight greater than zero',
    errorImageRequired: 'Please upload at least one image',
    errorBranchRequired: 'Please select at least one branch',
    errorBranchQuantity: 'Please enter a valid quantity for branch',
    errorBranchDiscount: 'Please enter a valid discount for branch',
    errorBranchExpiry: 'Please enter an expiry date for branch',
    errorBranchStartDelivery: 'Please enter a start delivery date for branch',
    errorBranchEndDelivery: 'Please enter an end delivery date for branch',
    errorBadData: 'Invalid data. Please check the fields',
    errorNoConnection: 'Unable to connect to the server. Please check your internet connection',
    errorCreateFailed: 'An error occurred while creating the product. Please try again',
    errorProcessImages: 'An error occurred while processing images'
  }
};

@Component({
  selector: 'app-add-product-panel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-product-panel.component.html',
  styleUrls: ['./add-product-panel.component.scss']
})
export class AddProductPanelComponent implements OnInit, OnDestroy {
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  t = ADD_PANEL_STRINGS.ar;
  isEnglish = false;
  private languageSubscription?: Subscription;

  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private languageService = inject(LanguageService);
  private branchContext = inject(BranchContextService);
  private cdr = inject(ChangeDetectorRef);
  private authService = inject(AuthService);

  branches: Branch[] = [];
  isLoadingBranches = false;
  selectedBranches: BranchQuantity[] = [];

  // Removed: pickupTime, expiryDate, discount — now per-branch
  productData = {
    code: '',
    nameAr: '',
    nameEn: '',
    descriptionAr: '',
    descriptionEn: '',
    allergens: '',
    allergensEn: '',
    price: '',
    weightInKg: ''
  };

  primaryImage: File | null = null;
  secondaryImage: File | null = null;
  primaryImagePreview: string | null = null;
  secondaryImagePreview: string | null = null;
  isLoading = false;
  errorMessage = '';

  ngOnInit() {
    this.updateLanguage();
    this.languageSubscription = this.languageService
      .getCurrentLanguage()
      .subscribe(() => this.updateLanguage());

    setTimeout(() => this.loadBranches(), 0);
  }

  ngOnDestroy() {
    this.languageSubscription?.unsubscribe();
  }

  private updateLanguage() {
    this.isEnglish = this.languageService.getCurrentLanguageValue() === 'en';
    this.t = this.isEnglish ? ADD_PANEL_STRINGS.en : ADD_PANEL_STRINGS.ar;
  }

  loadBranches() {
    if (this.isLoadingBranches) return;
    this.isLoadingBranches = true;
    this.branches = [];
    this.cdr.detectChanges();

    this.http.get<any>(this.apiService.getUrl('Branches/GetAll')).subscribe({
      next: (response) => {
        this.isLoadingBranches = false;
        let resultData: any[] = [];
        if (Array.isArray(response)) {
          resultData = response;
        } else if (Array.isArray(response?.resultData)) {
          resultData = response.resultData;
        } else if (Array.isArray(response?.data)) {
          resultData = response.data;
        }
        this.branches = resultData.map((branch: any) => ({
          id: branch.id,
          name: this.languageService.getLocalizedName(branch),
          nameAr: branch.nameAr || branch.name,
          nameEn: branch.nameEn || branch.name
        }));
        this.cdr.detectChanges();
      },
      error: (error: HttpErrorResponse) => {
        this.isLoadingBranches = false;
        this.branches = [];
        console.error('Error loading branches:', error);
        this.cdr.detectChanges();
      }
    });
  }

  onClose() { this.close.emit(); }

  onOverlayClick(event: Event) {
    if (event.target === event.currentTarget) this.onClose();
  }

  onPrimaryImageSelect(event: any) {
    const file = event.target.files[0];
    if (!file) return;
    this.primaryImage = file;
    const reader = new FileReader();
    reader.onload = (e: any) => { this.primaryImagePreview = e.target.result; this.cdr.detectChanges(); };
    reader.readAsDataURL(file);
  }

  onSecondaryImageSelect(event: any) {
    const file = event.target.files[0];
    if (!file) return;
    this.secondaryImage = file;
    const reader = new FileReader();
    reader.onload = (e: any) => { this.secondaryImagePreview = e.target.result; this.cdr.detectChanges(); };
    reader.readAsDataURL(file);
  }

  removePrimaryImage() {
    this.primaryImage = null;
    this.primaryImagePreview = null;
    const input = document.getElementById('primary-image') as HTMLInputElement;
    if (input) input.value = '';
    this.cdr.detectChanges();
  }

  removeSecondaryImage() {
    this.secondaryImage = null;
    this.secondaryImagePreview = null;
    const input = document.getElementById('secondary-image') as HTMLInputElement;
    if (input) input.value = '';
    this.cdr.detectChanges();
  }

  // ── branch helpers ──────────────────────────────────────────────────────────

  private todayStr(): string {
    return new Date().toISOString().split('T')[0];
  }

  onBranchToggle(branch: Branch, event: any) {
    if (event.target.checked) {
      this.selectedBranches.push({
        branchId: branch.id!,
        branchName: branch.name || branch.nameAr || '',
        quantity: 0,
        discount: 0,
        expiryDate: '',
        startDeliveryDate: '',
        endDeliveryDate: '',
        views: 0,
        status: 0
      });
    } else {
      this.selectedBranches = this.selectedBranches.filter(b => b.branchId !== branch.id);
    }
  }

  isBranchSelected(branchId?: number): boolean {
    return this.selectedBranches.some(b => b.branchId === branchId);
  }

  getBranch(branchId?: number): BranchQuantity | undefined {
    return this.selectedBranches.find(b => b.branchId === branchId);
  }

  updateBranchField(branchId: number, field: keyof BranchQuantity, value: any) {
    const b = this.selectedBranches.find(b => b.branchId === branchId);
    if (b) (b as any)[field] = value;
  }

  // ── submit ──────────────────────────────────────────────────────────────────

  async onSubmit() {
    if (!this.productData.code?.trim()) { this.errorMessage = this.t.errorCodeRequired; return; }
    if (!this.productData.nameAr?.trim()) { this.errorMessage = this.t.errorNameArRequired; return; }
    if (!this.productData.nameEn?.trim()) { this.errorMessage = this.t.errorNameEnRequired; return; }
    if (!this.productData.descriptionAr?.trim()) { this.errorMessage = this.t.errorDescriptionArRequired; return; }
    if (!this.productData.descriptionEn?.trim()) { this.errorMessage = this.t.errorDescriptionEnRequired; return; }
    if (!this.productData.allergens?.trim()) { this.errorMessage = this.t.errorAllergensRequired; return; }
    if (!this.productData.allergensEn?.trim()) { this.errorMessage = this.t.errorAllergensEnRequired; return; }
    if (!this.productData.price || parseFloat(this.productData.price) <= 0) { this.errorMessage = this.t.errorPriceRequired; return; }
    if (!this.productData.weightInKg || parseFloat(this.productData.weightInKg) <= 0) { this.errorMessage = this.t.errorWeightRequired; return; }
    if (!this.primaryImage && !this.secondaryImage) { this.errorMessage = this.t.errorImageRequired; return; }
    if (this.selectedBranches.length === 0) { this.errorMessage = this.t.errorBranchRequired; return; }

    for (const b of this.selectedBranches) {

      if (b.quantity <= 0) {
        this.errorMessage =
          `يرجى إدخال رقم صحيح لكمية فرع ${b.branchName}`;
        return;
      }

      if (b.discount < 0) {
        this.errorMessage =
          `يرجى إدخال رقم صحيح لنسبة خصم فرع ${b.branchName}`;
        return;
      }

      if (!b.expiryDate) {
        this.errorMessage =
          `يرجى إدخال تاريخ الإنتهاء لفرع ${b.branchName}`;
        return;
      }

      if (!b.startDeliveryDate) {
        this.errorMessage =
          `يرجى إدخال تاريخ بداية التوصيل لفرع ${b.branchName}`;
        return;
      }

      if (!b.endDeliveryDate) {
        this.errorMessage =
          `يرجى إدخال تاريخ نهاية التوصيل لفرع ${b.branchName}`;
        return;
      }

    }

    this.isLoading = true;
    this.errorMessage = '';

    try {
      const productImages: File[] = [];
      if (this.primaryImage) productImages.push(this.primaryImage);
      if (this.secondaryImage) productImages.push(this.secondaryImage);

      const formData = new FormData();
      formData.append('Code', this.productData.code.trim());
      formData.append('Name', this.productData.nameAr.trim());
      formData.append('NameEn', this.productData.nameEn.trim());
      formData.append('Description', this.productData.descriptionAr.trim());
      formData.append('DescriptionEn', this.productData.descriptionEn.trim());
      formData.append('Allergens', this.productData.allergens.trim());
      formData.append('AllergensEn', this.productData.allergensEn.trim());
      formData.append('Price', parseFloat(this.productData.price).toString());
      formData.append('WeightInKg', parseFloat(this.productData.weightInKg).toString());

      const merchantId = this.authService.getMerchantId();
      if (merchantId != null) formData.append('MerchantId', String(merchantId));

      const contextBranchId = this.branchContext.getSelectedBranchId();
      const branchIdForProduct =
        contextBranchId != null && Number.isFinite(contextBranchId)
          ? contextBranchId
          : this.selectedBranches[0]?.branchId;
      if (branchIdForProduct != null && Number.isFinite(branchIdForProduct)) {
        formData.append('BranchId', String(branchIdForProduct));
      }

      // Images — new field-name format
      productImages.forEach((imageUrl, index) => {
        formData.append(`ProductImages[${index}].ImageFile`, imageUrl);
        formData.append(`ProductImages[${index}].ProductId`, '0');
      });

      // Branches — new indexed dot-notation format
      this.selectedBranches.forEach((branch, index) => {
        formData.append(`ProductsBranches[${index}].ProductId`, '0');
        formData.append(`ProductsBranches[${index}].BranchId`, String(branch.branchId));
        formData.append(`ProductsBranches[${index}].Discount`, String(branch.discount ?? 0));
        formData.append(`ProductsBranches[${index}].ExpiryDate`, branch.expiryDate ?? '');
        formData.append(`ProductsBranches[${index}].Quantity`, String(branch.quantity ?? 0));
        formData.append(`ProductsBranches[${index}].StartDeliveryDate`, branch.startDeliveryDate ?? '');
        formData.append(`ProductsBranches[${index}].EndDeliveryDate`, branch.endDeliveryDate ?? '');
        formData.append(`ProductsBranches[${index}].Views`, String(branch.views ?? 0));
        formData.append(`ProductsBranches[${index}].Status`, String(branch.status ?? 0));
      });

      this.http.post<any>(this.apiService.getUrl('Products/Create'), formData).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.resetForm();
          this.save.emit(response);
          this.onClose();
        },
        error: (error: HttpErrorResponse) => {
          this.isLoading = false;
          if (error.status === 400 && error.error) {
            const msgs: string[] = [];
            Object.keys(error.error).forEach(key => {
              if (Array.isArray(error.error[key])) msgs.push(...error.error[key]);
              else msgs.push(error.error[key]);
            });
            this.errorMessage = msgs.join('\n') || 'بيانات غير صحيحة. يرجى التحقق من الحقول';
          } else if (error.status === 0) {
            this.errorMessage = this.t.errorNoConnection;
          } else {
            this.errorMessage = error.error?.message || this.t.errorCreateFailed;
          }
          this.cdr.detectChanges();
        }
      });
    } catch (error) {
      this.isLoading = false;
      this.errorMessage = this.t.errorProcessImages;
      console.error('Error processing images:', error);
      this.cdr.detectChanges();
    }
  }

  private fileToBase64DataURL(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });
  }

  private resetForm() {
    this.productData = {
      code: '', nameAr: '', nameEn: '',
      descriptionAr: '', descriptionEn: '',
      allergens: '', allergensEn: '',
      price: '', weightInKg: ''
    };
    this.primaryImage = null;
    this.secondaryImage = null;
    this.primaryImagePreview = null;
    this.secondaryImagePreview = null;
    this.selectedBranches = [];
    this.errorMessage = '';
    const p = document.getElementById('primary-image') as HTMLInputElement;
    const s = document.getElementById('secondary-image') as HTMLInputElement;
    if (p) p.value = '';
    if (s) s.value = '';
  }
}