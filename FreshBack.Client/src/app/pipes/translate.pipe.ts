import { Pipe, PipeTransform, inject, OnDestroy } from '@angular/core';
import { TranslationsService } from '../services/translations.service';
import { LanguageService } from '../services/language.service';

@Pipe({
  name: 'translate',
  standalone: true,
  pure: false // Allow dynamic updates when language changes
})
export class TranslatePipe implements PipeTransform, OnDestroy {
  private translationsService = inject(TranslationsService);
  private languageService = inject(LanguageService);
  private lastKey: string = '';
  private lastValue: string = '';
  private lastLanguage = '';

  transform(key: string): string {
    const currentLanguage = this.languageService.getCurrentLanguageValue();
    if (key === this.lastKey && currentLanguage === this.lastLanguage) {
      return this.lastValue;
    }

    this.lastLanguage = currentLanguage;
    this.lastKey = key;
    this.lastValue = this.translationsService.getSync(key as any) || key;
    return this.lastValue;
  }

  ngOnDestroy() {
    return;
  }
}
