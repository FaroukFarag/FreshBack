import { Component, HostListener, OnInit, OnDestroy, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive, Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { filter, Subscription, combineLatest, timeout, catchError, of } from 'rxjs';
import { LanguageService, Language } from '../../services/language.service';
import { TranslationsService, Translations } from '../../services/translations.service';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { BranchContextService } from '../../services/branch-context.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy {
  currentLanguage = 'العربية';
  languageDropdownOpen = false;
  pageTitle = 'الرئيسية';
  translations: Translations | null = null;

  /** Branch Manager (roleId = 2): show Branch Manager sidebar. */
  get isBranchManagerRole(): boolean {
    return this.authService.getRoleId() === 2;
  }

  private languageService = inject(LanguageService);
  private translationsService = inject(TranslationsService);
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  private branchContext = inject(BranchContextService);
  private languageSubscription?: Subscription;
  private translationsSubscription?: Subscription;
  private readonly REQUEST_TIMEOUT = 5000;

  constructor(private router: Router) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.updatePageTitle();
      if (this.authService.getRoleId() === 2 && (event.urlAfterRedirects === '/dashboard' || event.urlAfterRedirects === '/dashboard/')) {
        this.router.navigate(['/dashboard/live-orders'], { replaceUrl: true });
      }
    });
    this.updatePageTitle();
  }

  ngOnInit() {
    // Load initial translations
    this.loadTranslations();
    
    // Load branches
    this.loadBranches();
    
    // Subscribe to language changes
    this.languageSubscription = combineLatest([
      this.languageService.getCurrentLanguage(),
      this.router.events.pipe(filter(event => event instanceof NavigationEnd))
    ]).subscribe(() => {
      this.loadTranslations();
      this.updateLanguageLabel();
      this.updatePageTitle();
    });
    
    // Initial update
    this.updateLanguageLabel();
    this.updatePageTitle();
  }

  /**
   * Sets default branch in context (first from API) for features that send BranchId (e.g. product create).
   * Header branch selector was removed.
   */
  loadBranches() {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('No token found in localStorage. Please login again.');
      return;
    }

    this.http
      .get<any>(this.apiService.getUrl('Branches/GetAll'))
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading branches:', error);
          return of({ resultData: [] });
        })
      )
      .subscribe({
        next: (response) => {
          const resultData =
            response.resultData !== undefined
              ? response.resultData
              : response.data !== undefined
                ? response.data
                : response;

          if (resultData === null || resultData === undefined || (Array.isArray(resultData) && resultData.length === 0)) {
            this.branchContext.setSelectedBranch(null);
            return;
          }

          const branchesArray = Array.isArray(resultData) ? resultData : [resultData];
          const first = branchesArray[0];
          this.branchContext.setSelectedBranch({
            id: first.id,
            name: this.languageService.getLocalizedName(first),
            nameAr: first.nameAr || first.name,
            nameEn: first.nameEn || first.name
          });
        }
      });
  }

  ngOnDestroy() {
    this.languageSubscription?.unsubscribe();
    this.translationsSubscription?.unsubscribe();
  }

  /** Sync fallback so sidebar/header never show blank before async load */
  get t(): Translations {
    return this.translations ?? this.translationsService.getTranslationsSync();
  }

  loadTranslations() {
    this.translationsSubscription?.unsubscribe();
    this.translationsSubscription = this.translationsService.getTranslations().subscribe(translations => {
      this.translations = translations;
      this.updateMenuItems(translations);
      this.updatePageTitle();
    });
  }

  updateLanguageLabel() {
    const currentLang = this.languageService.getCurrentLanguageValue();
    this.currentLanguage = currentLang === 'ar' ? 'العربية' : 'English';
  }

  updatePageTitle() {
    const url = this.router.url;
    if (!this.translations) {
      this.translations = this.translationsService.getTranslationsSync();
    }

    const titleMap: { [key: string]: keyof Translations } = {
      '/dashboard': 'dashboard',
      '/dashboard/live-orders': 'liveOrders',
      '/merchants': 'merchantManagement',
      '/branches': 'branchManagement',
      '/inventory': 'inventoryManagement',
      '/dashboard/products': 'productsManagement',
      '/dashboard/orders': 'ordersManagement',
      '/financial': 'financialManagement',
      '/statistics': 'statisticsAndAnalytics',
      '/store-management': 'storeManagement',
      '/notifications': 'notificationsAndCommunication',
      '/support': 'supportAndHelp',
      '/settings': 'systemSettings'
    };

    const titleKey = titleMap[url] || 'dashboard';
    this.pageTitle = this.translations[titleKey];
  }

  updateMenuItems(translations: Translations) {
    const roleId = this.authService.getRoleId();
    const isAdmin = roleId === 0;
    const isBranchManager = roleId === 2; // Only roleId 2 gets Branch Manager sidebar (Live Orders, Order History)
    
    if (isAdmin) {
      // Admin sidebar menu items
      this.menuItems = [
        { 
          icon: 'assets/icons/home.svg', 
          label: translations.dashboard, 
          route: '/dashboard' 
        },
        { 
          icon: 'assets/icons/user.svg', 
          label: translations.merchantManagement, 
          route: '/merchants' 
        },
        { 
          icon: 'assets/icons/productsManagment.svg', 
          label: translations.productsManagement, 
          route: '/dashboard/products' 
        },
        { 
          icon: 'assets/icons/orders.svg', 
          label: translations.ordersManagement, 
          route: '/dashboard/orders' 
        },
        { 
          icon: 'assets/icons/money.svg', 
          label: translations.financialManagement, 
          route: '/financial' 
        },
        { 
          icon: 'assets/icons/performance.svg', 
          label: translations.statisticsAndAnalytics, 
          route: '/statistics' 
        },
        { 
          icon: 'assets/icons/notifications.svg', 
          label: translations.contentAndNotificationsManagement, 
          route: '/notifications' 
        },
        { 
          icon: 'assets/icons/more.svg', 
          label: translations.systemSettings, 
          route: '/settings' 
        }
      ];
    } else if (isBranchManager) {
      // roleId = 2 only: Branch Manager sidebar (Live Orders + Order History)
      this.menuItems = [
        { 
          icon: 'assets/icons/orders.svg', 
          label: translations.liveOrders, 
          route: '/dashboard/live-orders' 
        },
        { 
          icon: 'assets/icons/3.svg', 
          label: translations.orderHistory, 
          route: '/dashboard/orders' 
        }
      ];
    } else {
      // Merchant sidebar menu items (non-admin, non-branch-manager)
      this.menuItems = [
        { 
          icon: 'assets/icons/home.svg', 
          label: translations.dashboard, 
          route: '/dashboard' 
        },
        { 
          icon: 'assets/icons/branches.svg', 
          label: translations.branchManagement, 
          route: '/branches' 
        },
        { 
          icon: 'assets/icons/productsManagment.svg', 
          label: translations.productsManagement, 
          route: '/dashboard/products' 
        },
        { 
          icon: 'assets/icons/3.svg', 
          label: translations.inventoryManagement, 
          route: '/inventory' 
        },
        { 
          icon: 'assets/icons/orders.svg', 
          label: translations.ordersAndSales, 
          route: '/dashboard/orders' 
        },
        { 
          icon: 'assets/icons/money.svg', 
          label: translations.financialManagement, 
          route: '/financial' 
        },
        { 
          icon: 'assets/icons/performance.svg', 
          label: translations.statisticsAndPerformance, 
          route: '/statistics' 
        },
        { 
          icon: 'assets/icons/more.svg', 
          label: translations.storeManagement, 
          route: '/store-management' 
        },
        { 
          icon: 'assets/icons/notifications.svg', 
          label: translations.notificationsAndCommunication, 
          route: '/notifications' 
        },
        { 
          icon: 'assets/icons/help.svg', 
          label: translations.supportAndHelp, 
          route: '/support' 
        }
      ];
    }
  }
  

  languages: { code: Language; label: string }[] = [
    { code: 'ar', label: 'العربية' },
    { code: 'en', label: 'English' }
  ];

  toggleLanguageDropdown() {
    this.languageDropdownOpen = !this.languageDropdownOpen;
  }

  selectLanguage(language: { code: Language; label: string }) {
    this.languageService.setLanguage(language.code);
    this.currentLanguage = language.label;
    this.languageDropdownOpen = false;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.dropdown-wrapper')) {
      this.languageDropdownOpen = false;
    }
  }

  logout() {
    // TODO: Implement logout functionality
    // Clear local storage, tokens, etc.
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  menuItems: { icon: string; label: string; route: string }[] = [];
}
