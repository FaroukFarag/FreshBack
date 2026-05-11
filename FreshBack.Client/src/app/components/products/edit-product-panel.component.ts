import {
  Component,
  EventEmitter,
  Output,
  Input,
  OnChanges,
  SimpleChanges,
  OnInit,
  OnDestroy,
  inject,
  ChangeDetectorRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { LanguageService } from '../../services/language.service';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { BranchContextService } from '../../services/branch-context.service';

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
  discount: number;
  expiryDate: string;
  startDeliveryDate: string;
  endDeliveryDate: string;
  views: number;
  status: number;
  rowId: number;
}

const EDIT_PANEL_STRINGS = {
  ar: {
    title: 'تعديل منتج',
    productImages: 'صورة المنتج',
    productData: 'بيانات المنتج',
    productCode: 'كود المنتج',
    productNameAr: 'اسم المنتج (عربي)',
    productNameEn: 'اسم المنتج (إنجليزي)',
    productDescriptionAr: 'وصف المنتج (عربي)',
    productDescriptionEn: 'وصف المنتج (إنجليزي)',
    allergensAr: 'مسببات الحساسية',
    allergensEn: 'مسببات الحساسية (إنجليزي)',
    categoriesPrice: 'تصنيفات المنتج / السعر',
    quantity: 'الكمية',
    price: 'السعر',
    weightInKg: 'الوزن (كجم)',
    pickupTime: 'وقت الاستلام',
    expiryDate: 'تاريخ الانتهاء',
    discountPercentage: 'نسبة الخصم',
    saveChanges: 'حفظ التعديلات',
    branchesAndQuantities: 'الفروع والكميات',
    loadingBranches: 'جاري تحميل الفروع...',
    noBranches: 'لا توجد فروع متاحة',
    quantityLabel: 'الكمية:',
    dragOrClick: 'اسحب الصورة أو انقر للرفع',
    saving: 'جاري الحفظ...',
    required: '*'
  },
  en: {
    title: 'Edit Product',
    productImages: 'Product Images',
    productData: 'Product Data',
    productCode: 'Product Code',
    productNameAr: 'Product Name (Arabic)',
    productNameEn: 'Product Name (English)',
    productDescriptionAr: 'Product Description (Arabic)',
    productDescriptionEn: 'Product Description (English)',
    allergensAr: 'Allergens',
    allergensEn: 'Allergens (English)',
    categoriesPrice: 'Product Categories / Price',
    quantity: 'Quantity',
    price: 'Price',
    weightInKg: 'Weight (kg)',
    pickupTime: 'Pickup Time',
    expiryDate: 'Expiry Date',
    discountPercentage: 'Discount Percentage',
    saveChanges: 'Save Changes',
    branchesAndQuantities: 'Branches and Quantities',
    loadingBranches: 'Loading branches...',
    noBranches: 'No branches available',
    quantityLabel: 'Quantity:',
    dragOrClick: 'Drag image or click to upload',
    saving: 'Saving...',
    required: '*'
  }
};

@Component({
  selector: 'app-edit-product-panel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-product-panel.component.html',
  styleUrls: ['./edit-product-panel.component.scss']
})
export class EditProductPanelComponent implements OnInit, OnDestroy, OnChanges {
  @Input() product: any;
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  t = EDIT_PANEL_STRINGS.ar;
  isEnglish = false;

  private languageService = inject(LanguageService);
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  private branchContext = inject(BranchContextService);
  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);
  private languageSubscription?: Subscription;

  // ── form fields ────────────────────────────────────────────────────────────
  productData = {
    id: '',
    code: '',
    nameAr: '',
    nameEn: '',
    descriptionAr: '',
    descriptionEn: '',
    allergens: '',
    allergensEn: '',
    price: '',
    weightInKg: '',
    pickupTime: '',
    expiryDate: '',
    discount: '',
    merchantId: ''
  };

  // ── images ─────────────────────────────────────────────────────────────────
  /**
   * Existing image URLs loaded from the server.
   * These are only used as a fallback when the user hasn't selected a new file
   * for that slot. We fetch them as File objects at submit time to avoid the
   * race condition that occurred when fetching eagerly in populateFromProduct.
   */
  existingImages: string[] = [];

  /**
   * New files chosen by the user for each slot.
   * null means "user hasn't picked anything new — keep the existing image".
   */
  primaryImage: File | null = null;
  secondaryImage: File | null = null;

  /** Preview URLs shown in the UI (either a blob URL from a new file, or the existing remote URL) */
  primaryImagePreview: string | null = null;
  secondaryImagePreview: string | null = null;

  // ── branches ───────────────────────────────────────────────────────────────
  branches: Branch[] = [];
  isLoadingBranches = false;
  selectedBranches: BranchQuantity[] = [];

  // ── state ──────────────────────────────────────────────────────────────────
  isLoading = false;
  errorMessage = '';

  // ── lifecycle ──────────────────────────────────────────────────────────────

  ngOnInit() {
    this.updateLanguage();
    this.languageSubscription = this.languageService
      .getCurrentLanguage()
      .subscribe(() => this.updateLanguage());
    this.loadBranches();
  }

  ngOnDestroy() {
    this.languageSubscription?.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['product'] && this.product) {
      this.populateFromProduct(this.product);
    }
  }

  // ── private helpers ────────────────────────────────────────────────────────

  private updateLanguage() {
    this.isEnglish = this.languageService.getCurrentLanguageValue() === 'en';
    this.t = this.isEnglish ? EDIT_PANEL_STRINGS.en : EDIT_PANEL_STRINGS.ar;
  }

  private populateFromProduct(p: any): void {
    const num = (v: any) => (v != null && v !== '') ? String(v) : '';
    const str = (v: any) => (v != null && v !== undefined) ? String(v) : '';

    this.productData = {
      id: str(p.id ?? ''),
      code: str(p.code ?? p.productCode ?? ''),
      nameAr: str(p.name ?? p.nameAr ?? p.productName ?? ''),
      nameEn: str(p.nameEn ?? ''),
      descriptionAr: str(p.descriptionAr ?? p.DescriptionAr ?? p.description ?? p.Description ?? ''),
      descriptionEn: str(p.descriptionEn ?? p.DescriptionEn ?? p.description ?? p.Description ?? ''),
      allergens: str(p.allergens ?? p.allergensAr ?? ''),
      allergensEn: str(p.allergensEn ?? ''),
      price: num(p.price ?? p.productPrice ?? p.unitPrice ?? ''),
      weightInKg: num(p.weightInKg ?? p.weight ?? ''),
      pickupTime: str(p.pickupTime ?? p.pickupTimeAr ?? ''),
      expiryDate: this.normalizeDate(p.expiryDate),
      discount: num(p.discount ?? '0'),
      merchantId: num(p.merchantId ?? p.merchant?.id ?? '')
    };

    // ── Collect existing image URLs from the product ─────────────────────────
    if (Array.isArray(p.productImages) && p.productImages.length > 0) {
      this.existingImages = p.productImages.map((img: any) =>
        typeof img === 'string' ? img : (img?.imagePath ?? img?.path ?? '')
      ).filter(Boolean);
    } else if (p.imageUrl) {
      this.existingImages = [p.imageUrl];
    } else if (p.image) {
      this.existingImages = [p.image];
    } else {
      this.existingImages = [];
    }

    // ── Reset user-selected files — the user hasn't picked anything yet ───────
    // Do NOT pre-convert existing URLs to File objects here.
    // Conversion happens at submit time so there is no async race condition:
    // a user picking a new file can never be overwritten by a later .then().
    this.primaryImage = null;
    this.secondaryImage = null;

    // ── Show existing images as previews in the UI ───────────────────────────
    this.primaryImagePreview = this.existingImages[0]
      ? this.getImageSrc(this.existingImages[0])
      : null;
    this.secondaryImagePreview = this.existingImages[1]
      ? this.getImageSrc(this.existingImages[1])
      : null;

    // ── Pre-select branches already assigned to the product ──────────────────
    this.selectedBranches = this.normalizeBranchQuantities(p);

    this.cdr.detectChanges();
  }

  private normalizeDate(raw: any): string {
    if (!raw) return '';
    if (typeof raw === 'string' && raw.includes('T')) return raw.split('T')[0];
    return String(raw);
  }

  private normalizeBranchQuantities(p: any): BranchQuantity[] {

    const raw =
      p.productsBranches ??
      p.productBranches ??
      p.branches ??
      p.branchQuantities ??
      [];

    if (!Array.isArray(raw) || raw.length === 0)
      return [];

    return raw.map((item: any) => {

      const branchId =
        item.branchId ??
        item.BranchId ??
        item.id ??
        0;

      const qty =
        item.quantity ??
        item.Quantity ??
        0;

      const rowId =
        item.id ??
        item.Id ??
        0;

      const name =
        item.branchName ??
        item.branch?.name ??
        item.branch?.nameAr ??
        item.branch?.nameEn ??
        item.name ??
        item.Name ??
        '';

      return {
        branchId: Number(branchId),
        branchName: String(name || 'Branch #' + branchId),
        quantity: Number(qty),
        discount: Number(item.discount ?? item.Discount ?? 0),
        expiryDate: item.expiryDate ?? item.ExpiryDate ?? '',
        startDeliveryDate: item.startDeliveryDate ?? item.StartDeliveryDate ?? '',
        endDeliveryDate: item.endDeliveryDate ?? item.EndDeliveryDate ?? '',
        views: Number(item.views ?? item.Views ?? 0),
        status: Number(item.status ?? item.Status ?? 0),
        rowId: Number(rowId)
      };

    });

  }

  loadBranches() {
    if (this.isLoadingBranches) return;
    this.isLoadingBranches = true;
    this.cdr.detectChanges();

    this.http.get<any>(this.apiService.getUrl('Branches/GetAll')).subscribe({
      next: (response) => {
        this.isLoadingBranches = false;
        let data: any[] = [];
        if (Array.isArray(response)) {
          data = response;
        } else if (Array.isArray(response?.resultData)) {
          data = response.resultData;
        } else if (Array.isArray(response?.data)) {
          data = response.data;
        }
        this.branches = data.map((b: any) => ({
          id: b.id,
          name: this.languageService.getLocalizedName(b),
          nameAr: b.nameAr ?? b.name,
          nameEn: b.nameEn ?? b.name
        }));
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoadingBranches = false;
        this.branches = [];
        this.cdr.detectChanges();
      }
    });
  }

  // ── image helpers ──────────────────────────────────────────────────────────

  getImageSrc(img: string): string {
    if (!img) return '';
    if (img.startsWith('data:') || img.startsWith('http')) return img;
    // Relative path — prepend the API base URL so the browser can display it
    return this.apiService.getBaseUrl().replace(/\/$/, '') + '/' + img.replace(/^\//, '');
  }

  /**
   * Fetches a remote image URL and returns it as a File.
   * Resolves relative paths against the API base URL before fetching.
   * Returns null if the fetch fails — callers must handle this gracefully.
   */
  private async urlToFile(
    url: string,
    filename: string,
    mimeType: string = 'image/jpeg'
  ): Promise<File | null> {
    try {
      const fullUrl = url.startsWith('http')
        ? url
        : this.apiService.getBaseUrl().replace(/\/$/, '') + '/' + url.replace(/^\//, '');
      const response = await fetch(fullUrl);
      if (!response.ok) return null;
      const blob = await response.blob();
      return new File([blob], filename, { type: blob.type || mimeType });
    } catch {
      return null;
    }
  }

  onPrimaryImageSelect(event: any) {
    const file: File = event.target.files[0];
    if (!file) return;
    // User picked a brand-new file — store it and show a local preview.
    // This intentionally replaces whatever was in existingImages[0].
    this.primaryImage = file;
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.primaryImagePreview = e.target.result;
      this.cdr.detectChanges();
    };
    reader.readAsDataURL(file);
  }

  onSecondaryImageSelect(event: any) {
    const file: File = event.target.files[0];
    if (!file) return;
    this.secondaryImage = file;
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.secondaryImagePreview = e.target.result;
      this.cdr.detectChanges();
    };
    reader.readAsDataURL(file);
  }

  removePrimaryImage() {
    this.primaryImage = null;
    this.primaryImagePreview = null;
    // Also clear the existing URL for this slot so it won't be re-sent
    this.existingImages = this.existingImages.slice(1);
    const el = document.getElementById('edit-primary-image') as HTMLInputElement;
    if (el) el.value = '';
    this.cdr.detectChanges();
  }

  removeSecondaryImage() {
    this.secondaryImage = null;
    this.secondaryImagePreview = null;
    // Clear the existing URL for slot 1
    if (this.existingImages.length > 1) {
      this.existingImages = [this.existingImages[0]];
    }
    const el = document.getElementById('edit-secondary-image') as HTMLInputElement;
    if (el) el.value = '';
    this.cdr.detectChanges();
  }

  // ── branch helpers ─────────────────────────────────────────────────────────

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

  onBranchToggle(branch: Branch, event: any) {
    if (event.target.checked) {
      this.selectedBranches.push({
        branchId: branch.id!,
        branchName: branch.name ?? branch.nameAr ?? '',
        quantity: 0,
        discount: 0,
        expiryDate: '',
        startDeliveryDate: '',
        endDeliveryDate: '',
        views: 0,
        status: 0,
        rowId: 0
      });
    } else {
      this.selectedBranches = this.selectedBranches.filter(b => b.branchId !== branch.id);
    }
  }

  updateBranchQuantity(branchId: number, quantity: number) {
    const b = this.selectedBranches.find(b => b.branchId === branchId);
    if (b) b.quantity = quantity;
  }

  // ── panel controls ─────────────────────────────────────────────────────────

  onClose() { this.close.emit(); }

  onOverlayClick(event: Event) {
    if (event.target === event.currentTarget) this.onClose();
  }

  // ── submit ─────────────────────────────────────────────────────────────────

  async onSubmit() {
    const productId = this.product?.id ?? this.product?.productId;
    if (!productId) {
      this.errorMessage = 'معرّف المنتج غير موجود';
      return;
    }

    // ── basic validation ────────────────────────────────────────────────────
    if (!this.productData.code?.trim()) {
      this.errorMessage = 'يرجى إدخال كود المنتج'; return;
    }
    if (!this.productData.nameAr?.trim()) {
      this.errorMessage = 'يرجى إدخال اسم المنتج'; return;
    }
    if (!this.productData.nameEn?.trim()) {
      this.errorMessage = 'يرجى إدخال اسم المنتج بالإنجليزية'; return;
    }
    if (!this.productData.descriptionAr?.trim()) {
      this.errorMessage = 'يرجى إدخال وصف المنتج'; return;
    }
    if (!this.productData.descriptionEn?.trim()) {
      this.errorMessage = 'يرجى إدخال وصف المنتج بالإنجليزية'; return;
    }
    if (!this.productData.allergens?.trim()) {
      this.errorMessage = 'يرجى إدخال مسببات الحساسية'; return;
    }
    if (!this.productData.allergensEn?.trim()) {
      this.errorMessage = 'يرجى إدخال مسببات الحساسية بالإنجليزية'; return;
    }
    if (!this.productData.price || parseFloat(this.productData.price) <= 0) {
      this.errorMessage = 'يرجى إدخال سعر صحيح أكبر من صفر'; return;
    }
    if (!this.productData.weightInKg || parseFloat(this.productData.weightInKg) <= 0) {
      this.errorMessage = 'يرجى إدخال وزن صحيح أكبر من صفر'; return;
    }

    // At least one image slot must be occupied (new file OR existing URL still present)
    const hasImage =
      this.primaryImage != null ||
      this.secondaryImage != null ||
      this.primaryImagePreview != null ||
      this.secondaryImagePreview != null;
    if (!hasImage) {
      this.errorMessage = 'يرجى رفع صورة واحدة على الأقل'; return;
    }
    if (this.selectedBranches.length === 0) {
      this.errorMessage = 'يرجى اختيار فرع واحد على الأقل'; return;
    }

    for (const b of this.selectedBranches) {
      if (b.quantity <= 0) {
        this.errorMessage = `يرجى إدخال رقم صحيح لكمية فرع ${b.branchName}`; return;
      }
      if (b.discount < 0) {
        this.errorMessage = `يرجى إدخال رقم صحيح لنسبة خصم فرع ${b.branchName}`; return;
      }
      if (!b.expiryDate) {
        this.errorMessage = `يرجى إدخال تاريخ الإنتهاء لفرع ${b.branchName}`; return;
      }
      if (!b.startDeliveryDate) {
        this.errorMessage = `يرجى إدخال تاريخ بداية التوصيل لفرع ${b.branchName}`; return;
      }
      if (!b.endDeliveryDate) {
        this.errorMessage = `يرجى إدخال تاريخ نهاية التوصيل لفرع ${b.branchName}`; return;
      }
    }

    this.isLoading = true;
    this.errorMessage = '';

    try {
      // ── Resolve each image slot to a File ──────────────────────────────────
      // For each slot:
      //   • If the user picked a new file → use it directly (no fetch needed).
      //   • If the user kept the existing image (preview still shown, no new file
      //     selected) → fetch the existing URL NOW and convert it to a File.
      //   • If the slot was cleared (remove button clicked) → skip it entirely.
      //
      // Fetching at submit time (instead of at load time) ensures there is no
      // async race where a late-resolving .then() overwrites the user's new file.

      const resolveSlot = async (
        newFile: File | null,
        existingUrl: string | undefined,
        previewStillShown: string | null,
        fallbackFilename: string
      ): Promise<File | null> => {
        // User chose a fresh file — use it as-is
        if (newFile) return newFile;
        // Slot was explicitly removed by the user — don't send anything
        if (!previewStillShown) return null;
        // Slot still shows the existing image — fetch it from the server
        if (existingUrl) return this.urlToFile(existingUrl, fallbackFilename);
        return null;
      };

      const [slot0, slot1] = await Promise.all([
        resolveSlot(
          this.primaryImage,
          this.existingImages[0],
          this.primaryImagePreview,
          'existing-primary.jpg'
        ),
        resolveSlot(
          this.secondaryImage,
          this.existingImages[1],
          this.secondaryImagePreview,
          'existing-secondary.jpg'
        )
      ]);

      const productImages: File[] = [];
      if (slot0) productImages.push(slot0);
      if (slot1) productImages.push(slot1);

      if (productImages.length === 0) {
        this.isLoading = false;
        this.errorMessage = 'يرجى رفع صورة واحدة على الأقل';
        this.cdr.detectChanges();
        return;
      }

      // ── Build FormData ────────────────────────────────────────────────────
      const formData = new FormData();

      formData.append('Id', this.productData.id);
      formData.append('Code', this.productData.code.trim());
      formData.append('Name', this.productData.nameAr.trim());
      formData.append('NameEn', this.productData.nameEn.trim());
      formData.append('Description', this.productData.descriptionAr.trim());
      formData.append('DescriptionEn', this.productData.descriptionEn.trim());
      formData.append('Allergens', this.productData.allergens.trim());
      formData.append('AllergensEn', this.productData.allergensEn.trim());
      formData.append('Price', parseFloat(this.productData.price).toString());
      formData.append('WeightInKg', parseFloat(this.productData.weightInKg).toString());

      const merchantId = this.authService.getMerchantId() ??
        (this.productData.merchantId ? Number(this.productData.merchantId) : null);
      if (merchantId != null) {
        formData.append('MerchantId', String(merchantId));
      }

      const contextBranchId = this.branchContext.getSelectedBranchId();
      const branchIdForProduct =
        contextBranchId != null && Number.isFinite(contextBranchId)
          ? contextBranchId
          : this.selectedBranches[0]?.branchId;
      if (branchIdForProduct != null && Number.isFinite(branchIdForProduct)) {
        formData.append('BranchId', String(branchIdForProduct));
      }

      // Images
      productImages.forEach((file, index) => {
        formData.append(`ProductImages[${index}].ImageFile`, file);
        formData.append(`ProductImages[${index}].ProductId`, this.productData.id);
      });

      // Branches
      this.selectedBranches.forEach((branch, index) => {
        formData.append(`ProductsBranches[${index}].ProductId`, productId);
        formData.append(`ProductsBranches[${index}].BranchId`, String(branch.branchId));
        formData.append(`ProductsBranches[${index}].Discount`, String(branch.discount ?? 0));
        formData.append(`ProductsBranches[${index}].ExpiryDate`, branch.expiryDate ?? '');
        formData.append(`ProductsBranches[${index}].Quantity`, String(branch.quantity ?? 0));
        formData.append(`ProductsBranches[${index}].StartDeliveryDate`, branch.startDeliveryDate ?? '');
        formData.append(`ProductsBranches[${index}].EndDeliveryDate`, branch.endDeliveryDate ?? '');
        formData.append(`ProductsBranches[${index}].Views`, String(branch.views ?? 0));
        formData.append(`ProductsBranches[${index}].Status`, String(branch.status ?? 0));
      });

      // ── PUT request ───────────────────────────────────────────────────────
      this.http.put<any>(this.apiService.getUrl('Products/Update'), formData).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.save.emit(response);
          this.onClose();
        },
        error: (error: HttpErrorResponse) => {
          this.isLoading = false;
          if (error.status === 400 && error.error) {
            const errs = error.error;
            const msgs: string[] = [];
            Object.keys(errs).forEach(key => {
              if (Array.isArray(errs[key])) msgs.push(...errs[key]);
              else msgs.push(errs[key]);
            });
            this.errorMessage = msgs.join('\n') || 'بيانات غير صحيحة';
          } else if (error.status === 0) {
            this.errorMessage = 'تعذر الاتصال بالخادم';
          } else {
            this.errorMessage = error.error?.message || 'حدث خطأ أثناء تحديث المنتج';
          }
          this.cdr.detectChanges();
        }
      });
    } catch {
      this.isLoading = false;
      this.errorMessage = 'حدث خطأ أثناء معالجة الصور';
      this.cdr.detectChanges();
    }
  }
}