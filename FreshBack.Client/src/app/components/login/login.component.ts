import { Component, inject, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { LanguageService, Language } from '../../services/language.service';

const LOGIN_STRINGS = {
  ar: {
    title: 'تسجيل الدخول',
    subtitle: 'أدخل بياناتك للوصول إلى لوحة التحكم',
    username: 'اسم المستخدم',
    usernamePlaceholder: 'أدخل اسم المستخدم',
    password: 'كلمة المرور',
    passwordPlaceholder: 'أدخل كلمة المرور',
    showPassword: 'إظهار كلمة المرور',
    hidePassword: 'إخفاء كلمة المرور',
    rememberMe: 'تذكرني',
    loginButton: 'تسجيل الدخول',
    loggingIn: 'جاري تسجيل الدخول...',
    or: 'او',
    needHelp: 'بحاجة إلى مساعدة؟',
    contactSupport: 'تواصل مع الدعم',
    panelTitle: 'لوحة التحكم الإدارية',
    panelDescription: 'إدارة شاملة لجميع العمليات التجارية والطلبات والمنتجات من مكان واحد',
    manageMerchants: 'إدارة التجار',
    trackOrders: 'تتبع الطلبات',
    advancedStats: 'إحصائيات متقدمة',
    language: 'اللغة',
    validationError: 'يرجى إدخال اسم المستخدم وكلمة المرور',
    loginFailed: 'فشل تسجيل الدخول. يرجى التحقق من بيانات الاعتماد',
    noToken: 'فشل تسجيل الدخول. لم يتم العثور على رمز الوصول',
    invalidCredentials: 'اسم المستخدم أو كلمة المرور غير صحيحة',
    invalidData: 'بيانات غير صحيحة',
    connectionError: 'تعذر الاتصال بالخادم. يرجى التحقق من الاتصال بالإنترنت',
    serverError: 'حدث خطأ أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى'
  },
  en: {
    title: 'Login',
    subtitle: 'Enter your credentials to access the control panel',
    username: 'Username',
    usernamePlaceholder: 'Enter username',
    password: 'Password',
    passwordPlaceholder: 'Enter password',
    showPassword: 'Show password',
    hidePassword: 'Hide password',
    rememberMe: 'Remember me',
    loginButton: 'Login',
    loggingIn: 'Logging in...',
    or: 'or',
    needHelp: 'Need help?',
    contactSupport: 'Contact support',
    panelTitle: 'Administrative Control Panel',
    panelDescription: 'Comprehensive management of all business operations, orders, and products from one place',
    manageMerchants: 'Manage merchants',
    trackOrders: 'Track orders',
    advancedStats: 'Advanced statistics',
    language: 'Language',
    validationError: 'Please enter username and password',
    loginFailed: 'Login failed. Please check your credentials',
    noToken: 'Login failed. No access token received',
    invalidCredentials: 'Invalid username or password',
    invalidData: 'Invalid data',
    connectionError: 'Unable to connect. Please check your internet connection',
    serverError: 'An error occurred while logging in. Please try again'
  }
};

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  username = '';
  password = '';
  rememberMe = false;
  backgroundImage = 'assets/bg.png';
  errorMessage = '';
  isLoading = false;
  showPassword = false;
  t = LOGIN_STRINGS.ar;
  currentLang: Language = 'ar';
  languageLabel = 'English';

  private router = inject(Router);
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private languageService = inject(LanguageService);
  private cd = inject(ChangeDetectorRef);
  private languageSubscription?: Subscription;

  ngOnInit() {
    this.updateLanguage();
    this.languageSubscription = this.languageService.getCurrentLanguage().subscribe(lang => {
      this.currentLang = lang;
      this.updateLanguage();
    });
  }

  ngOnDestroy() {
    this.languageSubscription?.unsubscribe();
  }

  updateLanguage() {
    this.currentLang = this.languageService.getCurrentLanguageValue();
    this.t = LOGIN_STRINGS[this.currentLang];
    this.languageLabel = this.currentLang === 'ar' ? 'English' : 'العربية';
  }

  switchLanguage() {
    this.languageService.setLanguage(this.currentLang === 'ar' ? 'en' : 'ar');
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onLogin() {
    // Validate inputs
    if (!this.username || !this.password) {
      this.errorMessage = this.t.validationError;
      return;
    }

    // Clear previous error
    this.errorMessage = '';
    this.isLoading = true;

    // Prepare login data
    const loginData = {
      userName: this.username.trim(),
      password: this.password
    };

    // Send login request
    this.http.post<any>(this.apiService.getUrl('Users/Login'), loginData)
      .subscribe({
        next: (response) => {
          this.isLoading = false;
          
          // Check if the API response indicates failure
          if (response.succeeded === false || response.succeeded === 'false') {
            this.errorMessage = response.message || this.t.loginFailed;
            
            console.error('Login failed:', response.message);

            this.cd.detectChanges();
            
            return;
          }
          
          // Handle different response structures (resultData, data, or direct)
          // Check for null explicitly, not just undefined
          let resultData = null;
          if (response.resultData !== undefined && response.resultData !== null) {
            resultData = response.resultData;
          } else if (response.data !== undefined && response.data !== null) {
            resultData = response.data;
          } else if (response.resultData === undefined && response.data === undefined) {
            // If neither resultData nor data exist, use response directly
            resultData = response;
          }
          
          // Store token if provided (check multiple possible locations)
          // Safely check resultData and all possible token locations
          let token = null;
          if (resultData && typeof resultData === 'object') {
            token = resultData.token || resultData.accessToken;
          }
          if (!token) {
            token = response.token || response.accessToken;
          }
          
          if (token) {
            console.log('Login successful - Token stored:', token.substring(0, 20) + '...');
            localStorage.setItem('token', token);
            
            // Store user data if provided (with null check)
            let user = null;
            if (resultData && typeof resultData === 'object') {
              user = resultData.user || resultData;
            }
            if (!user) {
              user = response.user || response;
            }
            if (user && typeof user === 'object') {
              localStorage.setItem('user', JSON.stringify(user));
              // Store roleId separately for easy access
              if (user.roleId !== undefined) {
                localStorage.setItem('roleId', user.roleId.toString());
              }
              const merchantId = user.merchantId ?? user.MerchantId;
              if (merchantId != null && merchantId !== '') {
                localStorage.setItem('merchantId', String(merchantId));
              } else {
                localStorage.removeItem('merchantId');
              }
            }
            
            // Store remember me preference
            if (this.rememberMe) {
              localStorage.setItem('rememberMe', 'true');
            } else {
              localStorage.removeItem('rememberMe');
            }
            
            // Navigate to dashboard on success
            this.router.navigate(['/dashboard']);
          } else {
            console.warn('Login response received but no token found:', response);
            this.errorMessage = response.message || this.t.noToken;
          }

          this.cd.detectChanges();
        },
        error: (error: HttpErrorResponse) => {
          this.isLoading = false;
          
          // Handle different error cases
          if (error.status === 401) {
            this.errorMessage = error.error?.message || this.t.invalidCredentials;
          } else if (error.status === 400) {
            this.errorMessage = error.error?.message || this.t.invalidData;
          } else if (error.status === 0) {
            this.errorMessage = this.t.connectionError;
          } else {
            this.errorMessage = error.error?.message || this.t.serverError;
          }
          
          console.error('Login error:', error);

          this.cd.detectChanges();
        }
      });
  }
}
