import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export type Language = 'ar' | 'en';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private readonly LANGUAGE_KEY = 'selectedLanguage';
  private readonly DEFAULT_LANGUAGE: Language = 'ar';
  
  private currentLanguage$: BehaviorSubject<Language>;

  constructor() {
    // Get language from localStorage or use default
    const savedLanguage = localStorage.getItem(this.LANGUAGE_KEY) as Language;
    const initialLanguage = savedLanguage && (savedLanguage === 'ar' || savedLanguage === 'en') 
      ? savedLanguage 
      : this.DEFAULT_LANGUAGE;
    
    this.currentLanguage$ = new BehaviorSubject<Language>(initialLanguage);
    this.setLanguageDirection(initialLanguage);
  }

  /**
   * Get current language as Observable
   */
  getCurrentLanguage(): Observable<Language> {
    return this.currentLanguage$.asObservable();
  }

  /**
   * Get current language as value
   */
  getCurrentLanguageValue(): Language {
    return this.currentLanguage$.value;
  }

  /**
   * Set current language
   */
  setLanguage(language: Language): void {
    if (language !== 'ar' && language !== 'en') {
      console.warn('Invalid language. Using default:', this.DEFAULT_LANGUAGE);
      language = this.DEFAULT_LANGUAGE;
    }

    this.currentLanguage$.next(language);
    localStorage.setItem(this.LANGUAGE_KEY, language);
    this.setLanguageDirection(language);
  }

  /**
   * Check if current language is Arabic
   */
  isArabic(): boolean {
    return this.currentLanguage$.value === 'ar';
  }

  /**
   * Check if current language is English
   */
  isEnglish(): boolean {
    return this.currentLanguage$.value === 'en';
  }

  /**
   * Get localized value from an object
   * @param obj - Object with field and fieldEn properties (e.g., name/nameEn, neighborhood/neighborhoodEn)
   * @param field - Base field name (e.g., 'name', 'neighborhood', 'location')
   * @returns Localized string value
   */
  getLocalizedField<T extends Record<string, any>>(
    obj: T | null | undefined,
    field: string
  ): string {
    if (!obj) return '';

    const currentLang = this.currentLanguage$.value;
    
    // For Arabic: use the field as-is (e.g., 'name', 'neighborhood')
    // For English: use field + 'En' (e.g., 'nameEn', 'neighborhoodEn')
    const fieldName = currentLang === 'ar' ? field : `${field}En`;
    
    // Try the language-specific field first, then fallback to base field
    return obj[fieldName] || (currentLang === 'ar' ? '' : obj[field] || '');
  }

  /**
   * Get localized name from object
   * Handles: name (Arabic), nameEn (English), nameAr (Arabic alternative)
   */
  getLocalizedName<T extends { name?: string; nameEn?: string; nameAr?: string }>(
    obj: T | null | undefined
  ): string {
    if (!obj) return '';

    const currentLang = this.currentLanguage$.value;
    
    if (currentLang === 'ar') {
      // For Arabic: prefer nameAr, then name, then empty
      return obj.nameAr || obj.name || '';
    } else {
      // For English: prefer nameEn, then empty (don't use Arabic name)
      return obj.nameEn || '';
    }
  }

  /**
   * Set document direction based on language
   */
  private setLanguageDirection(language: Language): void {
    document.documentElement.setAttribute('dir', language === 'ar' ? 'rtl' : 'ltr');
    document.documentElement.setAttribute('lang', language);
  }

  /**
   * Toggle between Arabic and English
   */
  toggleLanguage(): void {
    const newLang = this.currentLanguage$.value === 'ar' ? 'en' : 'ar';
    this.setLanguage(newLang);
  }
}
