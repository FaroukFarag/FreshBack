import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LanguageService } from '../services/language.service';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const languageService = inject(LanguageService);
  const token = localStorage.getItem('token');
  const currentLanguage = languageService.getCurrentLanguageValue();
  const headers: { [key: string]: string } = {};

  if (token && token.trim() !== '') {
    headers['Authorization'] = `Bearer ${token.trim()}`;
  }

  headers['Accept-Language'] = currentLanguage;

  const clonedReq = req.clone({
    setHeaders: headers
  });
  
  return next(clonedReq);
};
