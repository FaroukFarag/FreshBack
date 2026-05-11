import { Component, OnInit, inject, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { forkJoin, catchError, of, timeout } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { GoogleMapsModule } from '@angular/google-maps';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { TranslationsService } from '../../services/translations.service';
import { LanguageService } from '../../services/language.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, GoogleMapsModule, TranslatePipe],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private translationService = inject(TranslationsService);
  private languageService = inject(LanguageService);
  private cdr = inject(ChangeDetectorRef);
  private readonly requestTimeoutMs = 8000;

  isAdmin = false;
  isGoogleMapsLoaded = false;
  isLoadingDashboardKpis = false;

  // Google Map config (centered on Saudi Arabia)
  mapCenter: google.maps.LatLngLiteral = { lat: 23.8859, lng: 45.0792 };
  mapZoom = 5;
  mapOptions: google.maps.MapOptions = {
    disableDefaultUI: false,
    zoomControl: true,
    mapTypeControl: false,
    streetViewControl: false,
    fullscreenControl: true,
    styles: [
      {
        featureType: 'poi',
        elementType: 'labels',
        stylers: [{ visibility: 'off' }]
      }
    ]
  };

  totalRevenueData: any = null;
  // Admin view data
  adminStats = {
    activeCustomers: 0,
    activeCustomersThisWeek: 0,
    registeredMerchants: 0,
    activeMerchants: 0,
    monthlyGrowthRate: 0,
    wasteSaved: 0,
    activeOrders: 0,
    ordersSinceHour: 0,
    totalSales: 0,
    salesGrowthPercentage: 0
  };

  salesPeriod: 'daily' | 'weekly' | 'monthly' = 'weekly';

  latestOrders: any[] = [];

  currentPage = 1;
  totalPages = 3;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
    this.checkGoogleMapsLoaded();
    if (this.isAdmin) {
      this.loadAdminDashboardKpis();

      this.loadLatestOrders();
    }

    else {
      this.loadDashboardKpis();

      this.loadHourlySales();

      this.loadWeeklySales();

      this.startClock()

      this.languageService.getCurrentLanguage().subscribe(() => {
        // re-interpolate with the same qty whenever language switches
        if (this.occupancyRate) {
          this.soldPiecesNoteText = this.translationService
            .soldPiecesNote(this.occupancyRate.thisMonth);
        }
      });
    }
  }

  ngOnDestroy(): void {
    if (this.clockInterval) clearInterval(this.clockInterval);
  }

  private parseDashboardNumber(res: unknown): number | null {
    if (res == null) return null;
    if (typeof res === 'number' && Number.isFinite(res)) return res;
    if (typeof res === 'string') {
      const n = parseFloat(res.replace(/,/g, ''));
      return Number.isFinite(n) ? n : null;
    }
    if (typeof res !== 'object') return null;
    const o = res as Record<string, unknown>;
    const direct =
      o['resultData'] ?? o['data'] ?? o['Data'] ?? o['value'] ?? o['Value'] ?? o['weight'] ?? o['Weight'] ??
      o['savedProductsWeight'] ?? o['SavedProductsWeight'] ?? o['monthlyGrowthPercentage'] ??
      o['MonthlyGrowthPercentage'] ?? o['percentage'] ?? o['Percentage'];
    if (typeof direct === 'number' && Number.isFinite(direct)) return direct;
    if (typeof direct === 'string') {
      const n = parseFloat(direct.replace(/,/g, ''));
      return Number.isFinite(n) ? n : null;
    }
    if (direct && typeof direct === 'object') {
      const inner = direct as Record<string, unknown>;
      const n = Number(
        inner['weight'] ?? inner['Weight'] ?? inner['value'] ?? inner['Value'] ??
        inner['monthlyGrowthPercentage'] ?? inner['percentage'] ?? inner['Percentage']
      );
      return Number.isFinite(n) ? n : null;
    }
    return null;
  }

  loadAdminDashboardKpis(): void {
    this.isLoadingDashboardKpis = true;
    forkJoin({
      activeCustomers: this.http.get<any>(this.apiService.getUrl('Dashboard/ActiveCustomers')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      registeredMerchants: this.http.get<any>(this.apiService.getUrl('Dashboard/RegisteredMerchants')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      activeOrders: this.http.get<any>(this.apiService.getUrl('Dashboard/ActiveOrders')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      weight: this.http.get<unknown>(this.apiService.getUrl('Dashboard/SavedProductsWeight')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      growth: this.http.get<unknown>(this.apiService.getUrl('Dashboard/MonthlyGrowthPercentage')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      totalRevenue: this.http.get<any>(this.apiService.getUrl('Dashboard/TotalRevenue')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      )
    }).subscribe({
      next: ({ weight, growth, activeCustomers, registeredMerchants, activeOrders, totalRevenue }) => {
        const w = this.parseDashboardNumber(weight);
        const g = this.parseDashboardNumber(growth);
        if (w != null) this.adminStats.wasteSaved = w;
        if (g != null) this.adminStats.monthlyGrowthRate = g;

        if (activeCustomers.succeeded) {
          this.adminStats.activeCustomers = activeCustomers.resultData.totalCustomers;
          this.adminStats.activeCustomersThisWeek = activeCustomers.resultData.newThisWeek;
        }

        if (registeredMerchants.succeeded) {
          this.adminStats.registeredMerchants = registeredMerchants.resultData.totalMerchants;
          this.adminStats.activeMerchants = registeredMerchants.resultData.activeMerchants;
        }

        if (activeOrders.succeeded) {
          this.adminStats.activeOrders = activeOrders.resultData.activeOrdersAverage;
          this.adminStats.ordersSinceHour = activeOrders.resultData.newSinceLastHour;        
        }

        if (totalRevenue.succeeded) {
          this.totalRevenueData = totalRevenue.resultData
          this.adminStats.totalSales = totalRevenue.resultData.thisWeek;
          this.adminStats.salesGrowthPercentage = totalRevenue.resultData.PercentageComparedToLastWeek;
        }

        this.isLoadingDashboardKpis = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoadingDashboardKpis = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadDashboardKpis(): void {
    this.isLoadingDashboardKpis = true;
    forkJoin({
      revenue: this.http.get<any>(this.apiService.getUrl('Dashboard/TotalRevenue')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      occupancy: this.http.get<any>(this.apiService.getUrl('Dashboard/OccupancyRate')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      weight: this.http.get<any>(this.apiService.getUrl('Dashboard/MerchantSavedProductsWeight')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      soldProducts: this.http.get<any>(this.apiService.getUrl('Dashboard/SoldProducts')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      ),
      remainingProducts: this.http.get<any>(this.apiService.getUrl('Dashboard/RemainingProducts')).pipe(
        timeout(this.requestTimeoutMs),
        catchError(() => of(null))
      )
    }).subscribe({
      next: ({ revenue, occupancy, weight, soldProducts, remainingProducts }) => {
        if (!revenue.succeeded) {
          this.isLoadingDashboardKpis = false;
          this.cdr.detectChanges();
          return;
        }

        this.revenueStats.today = revenue.resultData.today;
        this.revenueStats.thisWeek = revenue.resultData.thisWeek;
        this.revenueStats.thisMonth = revenue.resultData.thisMonth;
        this.revenueStats.growth = revenue.resultData.percentageComparedToLastMonth;

        if (!occupancy.succeeded) {
          this.isLoadingDashboardKpis = false;
          this.cdr.detectChanges();
          return;
        }

        this.occupancyRate.thisMonth = occupancy.resultData.thisMonth;
        this.occupancyRate.percentage = occupancy.resultData.percentageComparedToLastMonth;

        this.soldPiecesNoteText = this.translationService.soldPiecesNote(occupancy.resultData.thisMonth);

        if (!weight.succeeded) {
          this.isLoadingDashboardKpis = false;
          this.cdr.detectChanges();
          return;
        }

        this.stats.savedWeightAmount = weight.resultData;

        if (!soldProducts.succeeded) {
          this.isLoadingDashboardKpis = false;
          this.cdr.detectChanges();
          return;
        }

        this.stats.productsSold = soldProducts.resultData;

        if (!remainingProducts.succeeded) {
          this.isLoadingDashboardKpis = false;
          this.cdr.detectChanges();
          return;
        }

        this.stats.productsRemaining = remainingProducts.resultData;

        this.isLoadingDashboardKpis = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoadingDashboardKpis = false;
        this.cdr.detectChanges();
      }
    });
  }

  checkGoogleMapsLoaded(): void {
    // Check if Google Maps is loaded
    if (typeof google !== 'undefined' && google.maps) {
      this.isGoogleMapsLoaded = true;
    } else {
      // Try to check after a delay (script might still be loading)
      setTimeout(() => {
        if (typeof google !== 'undefined' && google.maps) {
          this.isGoogleMapsLoaded = true;
        }
      }, 1000);
    }
  }

  setSalesPeriod(period: 'daily' | 'weekly' | 'monthly'): void {
    this.salesPeriod = period;

    if(period === 'daily') {
      this.adminStats.totalSales = this.totalRevenueData.today;
      this.adminStats.salesGrowthPercentage = this.totalRevenueData.PercentageComparedToYesterday;
    }

    else if(period === 'weekly') {
      this.adminStats.totalSales = this.totalRevenueData.thisWeek;
      this.adminStats.salesGrowthPercentage = this.totalRevenueData.PercentageComparedToLastWeek;
    }

    else if(period === 'monthly') {
      this.adminStats.totalSales = this.totalRevenueData.thisMonth;
      this.adminStats.salesGrowthPercentage = this.totalRevenueData.PercentageComparedToLastMonth;
    }
  }

  getStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      'confirmed': 'status-confirmed',
      'delivery': 'status-delivery',
      'new': 'status-new',
      'completed': 'status-completed'
    };
    return statusMap[status] || '';
  }

  revenueStats = {
    today: 0,
    thisWeek: 0,
    thisMonth: 0,
    growth: 0
  };

  occupancyRate = {
    percentage: 0,
    thisMonth: 0
  };

  soldPiecesNoteText = '';

  hourlySalesPoints: { hour: number; label: string; revenue: number }[] = [];
  isLoadingHourlySales = false;

  private loadLatestOrders(): void {
    this.http.get<any>(this.apiService.getUrl('Dashboard/LatestOrders')).subscribe({
      next: (res) => {
        this.latestOrders = res.resultData ?? [];
      },
      error: () => {
      }
    });
  }

  private loadHourlySales(): void {
    this.isLoadingHourlySales = true;
    this.http.get<any>(this.apiService.getUrl('Dashboard/HourlySales')).subscribe({
      next: (res) => {
        this.isLoadingHourlySales = false;
        this.hourlySalesPoints = res.resultData?.points ?? [];
      },
      error: () => {
        this.isLoadingHourlySales = false;
      }
    });
  }

  get hasHourlyData(): boolean {
    return this.hourlySalesPoints.some(p => p.revenue > 0);
  }

  getHourlyPoints(): string {
    if (!this.hourlySalesPoints.length) return '';

    const xStart = 30;
    const xEnd = 780;
    const svgHeight = 250;
    const topPad = 20;
    const maxRevenue = Math.max(...this.hourlySalesPoints.map(p => p.revenue));

    if (maxRevenue === 0) return '';

    const xStep = (xEnd - xStart) / 23;

    return this.hourlySalesPoints
      .map((p, i) => {
        const x = xStart + i * xStep;
        const y = svgHeight - (p.revenue / maxRevenue) * (svgHeight - topPad);
        return `${x.toFixed(1)},${y.toFixed(1)}`;
      })
      .join(' ');
  }

  getHourlyDots(): { cx: number; cy: number; revenue: number; hour: number }[] {
    if (!this.hourlySalesPoints.length) return [];

    const xStart = 30;
    const xEnd = 780;
    const svgHeight = 250;
    const topPad = 20;
    const maxRevenue = Math.max(...this.hourlySalesPoints.map(p => p.revenue));
    const xStep = (xEnd - xStart) / 23;
    const midY = svgHeight / 2;

    return this.hourlySalesPoints.map((p, i) => ({
      cx: xStart + i * xStep,
      cy: maxRevenue === 0
        ? midY
        : svgHeight - (p.revenue / maxRevenue) * (svgHeight - topPad),
      revenue: p.revenue,
      hour: p.hour
    }));
  }

  currentTime = '';
  private clockInterval?: ReturnType<typeof setInterval>;

  private startClock(): void {
    const tick = () => {
      const now = new Date();
      this.currentTime = `${String(now.getHours()).padStart(2, '0')}:${String(now.getMinutes()).padStart(2, '0')}`;
    };
    tick();
    this.clockInterval = setInterval(tick, 60_000);
  }

  weeklySalesPoints: { dayIndex: number; date: string; dayNameAr: string; dayNameEn: string; revenue: number }[] = [];
  isLoadingWeeklySales = false;

  private loadWeeklySales(): void {
    this.isLoadingWeeklySales = true;
    this.http.get<any>(this.apiService.getUrl('Dashboard/WeeklySales')).subscribe({
      next: (res) => {
        this.isLoadingWeeklySales = false;
        this.weeklySalesPoints = res.resultData?.points ?? [];
      },
      error: () => {
        this.isLoadingWeeklySales = false;
      }
    });
  }

  getWeeklyPoints(): string {
    if (!this.weeklySalesPoints.length) return '';

    const svgWidth = 500;
    const svgHeight = 250;
    const maxRevenue = Math.max(...this.weeklySalesPoints.map(p => p.revenue), 1);
    const xStep = (svgWidth) / 6;

    return this.weeklySalesPoints
      .map((p, i) => {
        const x = 50 + i * xStep;
        const y = svgHeight - (p.revenue / maxRevenue) * (svgHeight - 20);
        return `${x},${y}`;
      })
      .join(' ');
  }

  getWeeklyDots(): { cx: number; cy: number; revenue: number; label: string; date: string }[] {
    if (!this.weeklySalesPoints.length) return [];

    const svgWidth = 500;
    const svgHeight = 250;
    const maxRevenue = Math.max(...this.weeklySalesPoints.map(p => p.revenue), 1);
    const xStep = svgWidth / 6;

    return this.weeklySalesPoints.map((p, i) => ({
      cx: 50 + i * xStep,
      cy: svgHeight - (p.revenue / maxRevenue) * (svgHeight - 20),
      revenue: p.revenue,
      label: this.languageService.isEnglish() ? p.dayNameEn : p.dayNameAr,
      date: p.date
    }));
  }

  get todayDayName(): string {
    const today = new Date();

    const jsToArabicIndex: Record<number, number> = { 0: 1, 1: 2, 2: 3, 3: 4, 4: 5, 5: 6, 6: 0 };
    const arabicIndex = jsToArabicIndex[today.getDay()];
    const point = this.weeklySalesPoints[arabicIndex];

    if (!point) return '';

    return this.languageService.isEnglish() ? point.dayNameEn : point.dayNameAr;
  }

  get todayDayNumber(): string {
    return String(new Date().getDate()).padStart(2, '0');
  }

  stats = {
    productsSold: 0,
    productsRemaining: 0,
    savedWeightAmount: 0
  };
}