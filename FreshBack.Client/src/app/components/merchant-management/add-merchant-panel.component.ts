import {
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
  inject,
  ChangeDetectorRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, of, timeout } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { LanguageService } from '../../services/language.service';
import { TranslationsService } from '../../services/translations.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

/** Collected merchant fields before building multipart FormData for Merchants/Create. */
export interface CreateMerchantPayload {
  name: string;
  nameEn: string;
  description: string;
  descriptionEn: string;
  story: string;
  storyEn: string;
  username: string;
  phoneNumber: string;
  password: string;
  status: number;
  categoryId: number;
}

interface CategoryOption {
  id: number;
  name: string;
  nameEn: string;
}

@Component({
  selector: 'app-add-merchant-panel',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './add-merchant-panel.component.html',
  styleUrls: ['./add-merchant-panel.component.scss']
})
export class AddMerchantPanelComponent implements OnInit {
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  /** Backend IFormFile name — change if API expects e.g. `Image` or `Photo`. */
  private static readonly MERCHANT_IMAGE_FORM_KEY = 'ImageFile';

  @ViewChild('merchantImageInput') merchantImageInput?: ElementRef<HTMLInputElement>;

  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private languageService = inject(LanguageService);
  private translationsService = inject(TranslationsService);
  private cdr = inject(ChangeDetectorRef);

  isLoadingCategories = false;
  isSaving = false;
  errorMessage = '';
  imageFile: File | null = null;
  imagePreview: string | null = null;
  categories: CategoryOption[] = [];
  /** Visual state while dragging files over the drop zone. */
  isImageDragOver = false;
  private imageDropDepth = 0;
  private readonly REQUEST_TIMEOUT = 15000;

  form = {
    name: '',
    nameEn: '',
    description: '',
    descriptionEn: '',
    story: '',
    storyEn: '',
    username: '',
    phoneNumber: '',
    password: '',
    status: 1,
    categoryId: '' as string | number
  };

  ngOnInit(): void {
    this.loadCategories();
  }

  onClose(): void {
    this.removeMerchantImage();
    this.close.emit();
  }

  onDropZoneDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onDropZoneDragEnter(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.imageDropDepth++;
    this.isImageDragOver = true;
    this.cdr.detectChanges();
  }

  onDropZoneDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.imageDropDepth = Math.max(0, this.imageDropDepth - 1);
    if (this.imageDropDepth === 0) {
      this.isImageDragOver = false;
      this.cdr.detectChanges();
    }
  }

  onDropZoneDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.imageDropDepth = 0;
    this.isImageDragOver = false;
    const file = event.dataTransfer?.files?.[0];
    if (file) {
      this.applyImageFile(file);
    }
    this.cdr.detectChanges();
  }

  onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) {
      return;
    }
    this.applyImageFile(file);
  }

  loadCategories() {
    this.isLoadingCategories = true;
    this.http.get<any>(this.apiService.getUrl('Categories/GetAll'))
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError(() => {
          this.isLoadingCategories = false;
          this.categories = [];
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe((response) => {
        this.isLoadingCategories = false;
        if (!response) {
          this.cdr.detectChanges();
          return;
        }
        const resultData = response.resultData !== undefined ? response.resultData : (response.data !== undefined ? response.data : response);
        const items = Array.isArray(resultData) ? resultData : (resultData ? [resultData] : []);
        this.categories = items
          .map((category: any) => ({
            id: Number(category.id ?? category.categoryId ?? 0),
            name: String(category.name ?? category.nameAr ?? ''),
            nameEn: String(category.nameEn ?? category.name ?? '')
          }))
          .filter((c: CategoryOption) => Number.isFinite(c.id) && c.id > 0);
        this.cdr.detectChanges();
      });
  }

  isArabic(): boolean {
    return this.languageService.isArabic();
  }

  triggerImagePicker(): void {
    this.merchantImageInput?.nativeElement?.click();
  }

  formatFileSize(bytes: number): string {
    if (!Number.isFinite(bytes) || bytes < 0) {
      return '';
    }
    if (bytes < 1024) {
      return `${bytes} B`;
    }
    if (bytes < 1024 * 1024) {
      return `${(bytes / 1024).toFixed(1)} KB`;
    }
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  }

  private applyImageFile(file: File): void {
    if (!file.type.startsWith('image/')) {
      this.errorMessage = this.translationsService.getSync('merchantInvalidImageType');
      this.cdr.detectChanges();
      return;
    }
    this.errorMessage = '';
    this.imageFile = file;
    const reader = new FileReader();
    reader.onload = (e) => {
      this.imagePreview = (e.target?.result as string) ?? null;
      this.cdr.detectChanges();
    };
    reader.readAsDataURL(file);
  }

  removeMerchantImage(): void {
    this.imageFile = null;
    this.imagePreview = null;
    this.imageDropDepth = 0;
    this.isImageDragOver = false;
    const el = this.merchantImageInput?.nativeElement;
    if (el) {
      el.value = '';
    }
  }

  private buildCreateMerchantFormData(payload: CreateMerchantPayload): FormData {
    const fd = new FormData();
    // PascalCase matches typical ASP.NET Core [FromForm] model binding.
    fd.append('Name', payload.name);
    fd.append('NameEn', payload.nameEn);
    fd.append('Description', payload.description);
    fd.append('DescriptionEn', payload.descriptionEn);
    fd.append('Story', payload.story);
    fd.append('StoryEn', payload.storyEn);
    fd.append('CategoryId', String(payload.categoryId ?? '0'));
    fd.append('Username', payload.username);
    fd.append('PhoneNumber', payload.phoneNumber);
    fd.append('Password', payload.password);
    fd.append('Status', String(payload.status));
    if (this.imageFile) {
      fd.append(AddMerchantPanelComponent.MERCHANT_IMAGE_FORM_KEY, this.imageFile, this.imageFile.name);
    }
    return fd;
  }

  onOverlayClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.onClose();
    }
  }

  isFormValid(): boolean {
    return !this.form.name.trim() || !this.form.nameEn.trim() ||
      !this.form.description.trim() || !this.form.descriptionEn.trim() ||
      !this.form.story.trim() || !this.form.storyEn.trim() ||
      !this.form.username.trim() && !this.form.password.trim() ||
      !this.form.categoryId || !this.imageFile;
  }

  onSubmit(): void {
    if (this.isSaving) return;

    const payload: CreateMerchantPayload = {
      name: this.form.name.trim(),
      nameEn: this.form.nameEn.trim(),
      description: this.form.description.trim(),
      descriptionEn: this.form.descriptionEn.trim(),
      story: this.form.story.trim(),
      storyEn: this.form.storyEn.trim(),
      username: this.form.username.trim(),
      phoneNumber: this.form.phoneNumber.trim(),
      password: this.form.password,
      status: Number(this.form.status),
      categoryId: Number(this.form.categoryId),
    };

    this.isSaving = true;
    this.errorMessage = '';
    this.cdr.detectChanges();

    const formData = this.buildCreateMerchantFormData(payload);

    this.http.post<any>(this.apiService.getUrl('Merchants/Create'), formData).subscribe({
      next: (response) => {
        this.isSaving = false;
        const failed = response?.succeeded === false;
        if (failed) {
          this.errorMessage =
            response?.message || this.translationsService.getSync('merchantCreateError');
          this.cdr.detectChanges();
          return;
        }
        this.removeMerchantImage();
        this.saved.emit();
        this.onClose();
        this.cdr.detectChanges();
      },
      error: (err: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage =
          err.error?.message || err.message || this.translationsService.getSync('merchantCreateError');
        this.cdr.detectChanges();
      }
    });
  }
}
