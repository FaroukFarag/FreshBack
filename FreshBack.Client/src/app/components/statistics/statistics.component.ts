import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ElementRef, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { LanguageService } from '../../services/language.service';
import { BranchContextService } from '../../services/branch-context.service';

interface TopProduct {
  name: string;
  sales: number;
  quantity: number;
}

interface MarketMetric {
  label: string;
  value: number;
  text: string;
  color: string;
}

interface CustomerReview {
  date: string;
  name: string;
  rating: number;
  text: string;
}

interface TopMerchant {
  name: string;
  sales: number;
}

interface ProductCategory {
  name: string;
  percentage: number;
  color: string;
}

@Component({
  selector: 'app-statistics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit, AfterViewInit, OnDestroy {
  private authService = inject(AuthService);
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private languageService = inject(LanguageService);
  private branchContext = inject(BranchContextService);
  private cdr = inject(ChangeDetectorRef);
  @ViewChild('peakHoursChart', { static: false }) peakHoursChart?: ElementRef<HTMLCanvasElement>;
  @ViewChild('merchantPeakChart', { static: false }) merchantPeakChart?: ElementRef<HTMLCanvasElement>;
  private branchSubscription?: Subscription;
  private selectedBranchId: number | null = null;
  
  isAdmin = false;
  isMerchant = false;

  // Merchant dashboard data (roleId 1)
  topProducts: TopProduct[] = [];
  isLoadingTopProducts = false;
  topProductsError = '';
  weekTip = 'الخبز الفرنسي يباع بنسبة 80% عند تطبيق خصم 40% قبل ساعتين من الانتهاء. جرب هذه الاستراتيجية لزيادة مبيعاتك!';
  marketMetrics: MarketMetric[] = [
    { label: 'معدل المبيعات', value: 70, text: 'أعلى من 70 % من المتاجر', color: 'green' },
    { label: 'رضا العملاء', value: 50, text: 'أعلى من 50 % من المتاجر', color: 'orange' },
    { label: 'سرعة التسليم', value: 30, text: 'ابطأ من 70 % من المتاجر', color: 'red' }
  ];
  improvementMessage = 'أنت تتفوق في المبيعات عالج مشكلة تأخير الطلبات لتصبح التاجر الأول في منطقتك بلا منازع';
  overallRating = 4.5;
  reviewsCount = 13;
  customerReviews: CustomerReview[] = [
    { date: '2024-12-22', name: 'أحمد محمد العايبي', rating: 4, text: 'تجربة راقية ومريحة من اللحظة الأولى بسهولة الطلب تمكس عناية واضحة براحة العميل.' },
    { date: '2024-12-21', name: 'سارة أحمد', rating: 5, text: 'خدمة ممتازة ومنتجات طازجة. أنصح بشدة!' },
    { date: '2024-12-20', name: 'محمد خالد', rating: 4, text: 'تجربة إيجابية، التوصيل سريع والمنتجات ذات جودة عالية.' }
  ];

  // KPIs
  reorderRate = 87;
  averageFulfillmentTime = 2.3;

  // Top 10 Merchants
  topMerchants: TopMerchant[] = [];

  // Most Requested Products
  productCategories: ProductCategory[] = [];

  // Environmental Impact
  totalFoodSaved = 445.42;
  carbonFootprintReduction = 445.42;

  // Peak Hours Data (24 hours)
  peakHoursData: Array<{ hour: number; value: number }> = [];

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin();
    this.isMerchant = this.authService.isMerchant();
    // Initialize peak hours data
    this.peakHoursData = Array.from({ length: 24 }, (_, i) => ({
      hour: i,
      value: this.getHourlyValue(i)
    }));
    if (this.isMerchant) {
      this.loadTopTenProducts();
    }
    if (this.isAdmin) {
      this.loadTopTenMerchants();
      this.loadTopTenProductsForAdmin();
    }

    this.branchSubscription = this.branchContext.currentBranch$.subscribe((branch) => {
      const nextBranchId = branch?.id ?? null;
      if (nextBranchId !== this.selectedBranchId) {
        this.selectedBranchId = nextBranchId;
        if (this.isAdmin) {
          this.loadTopTenMerchants();
          this.loadTopTenProductsForAdmin();
        }
      }
    });
  }

  private loadTopTenProducts(): void {
    this.isLoadingTopProducts = true;
    this.topProductsError = '';
    const url = this.apiService.getUrl('Statistics/GetTopTenProducts');
    this.http.get<any>(url).subscribe({
      next: (res) => {
        this.isLoadingTopProducts = false;
        const items = res?.resultData ?? res?.data ?? res?.items ?? (Array.isArray(res) ? res : []);
        this.topProducts = (Array.isArray(items) ? items : []).map((item: any) =>
          this.mapToTopProduct(item)
        );
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoadingTopProducts = false;
        this.topProductsError = err?.error?.message ?? err?.message ?? 'فشل تحميل المنتجات';
        this.topProducts = [];
        this.cdr.detectChanges();
      }
    });
  }

  private loadTopTenMerchants(): void {
    const url = this.apiService.getUrl('Statistics/GetTopTenMerchants');
    this.http.get<any>(url).subscribe({
      next: (res) => {
        const items = this.filterBySelectedBranch(this.extractResultArray(res));
        this.topMerchants = items.map((item: any) => this.mapToTopMerchant(item)).slice(0, 10);
        this.cdr.detectChanges();
      },
      error: () => {
        this.topMerchants = [];
        this.cdr.detectChanges();
      }
    });
  }

  private loadTopTenProductsForAdmin(): void {
    const url = this.apiService.getUrl('Statistics/GetTopTenProducts');
    this.http.get<any>(url).subscribe({
      next: (res) => {
        const items = this.filterBySelectedBranch(this.extractResultArray(res));
        this.productCategories = this.mapToProductCategories(items);
        this.cdr.detectChanges();
      },
      error: () => {
        this.productCategories = [];
        this.cdr.detectChanges();
      }
    });
  }

  private extractResultArray(res: any): any[] {
    if (Array.isArray(res)) return res;

    const direct = res?.resultData ?? res?.data ?? res?.items ?? res?.list ?? res?.records;
    if (Array.isArray(direct)) return direct;

    if (direct && typeof direct === 'object') {
      const nested = this.findFirstArrayInObject(direct);
      if (nested.length) return nested;
    }

    if (res && typeof res === 'object') {
      return this.findFirstArrayInObject(res);
    }

    return [];
  }

  private findFirstArrayInObject(obj: any): any[] {
    if (!obj || typeof obj !== 'object') return [];
    for (const key of Object.keys(obj)) {
      const value = obj[key];
      if (Array.isArray(value)) return value;
      if (value && typeof value === 'object') {
        const nested = this.findFirstArrayInObject(value);
        if (nested.length) return nested;
      }
    }
    return [];
  }

  private mapToTopProduct(item: any): TopProduct {
    const product = item.product ?? item.Product ?? item.ProductDto ?? item;
    const name = this.languageService.getLocalizedName(product) ||
      (product?.nameAr ?? product?.NameAr ?? product?.name ?? product?.Name ?? product?.nameEn ?? product?.NameEn ??
      item.productName ?? item.ProductName ?? item.name ?? item.Name ?? '—');
    const sales = Number(item.totalSales ?? item.TotalSales ?? item.sales ?? item.Sales ?? item.revenue ?? item.Revenue ?? item.amount ?? item.Amount ?? 0);
    const quantity = Number(item.quantity ?? item.Quantity ?? item.quantitySold ?? item.QuantitySold ?? item.piecesSold ?? item.PiecesSold ?? item.soldCount ?? item.SoldCount ?? 0);
    return { name: String(name), sales, quantity };
  }

  private mapToTopMerchant(item: any): TopMerchant {
    const merchant = item.merchant ?? item.Merchant ?? item.store ?? item.Store ?? item;
    const name = this.languageService.getLocalizedName(merchant) ||
      (merchant?.nameAr ?? merchant?.NameAr ?? merchant?.name ?? merchant?.Name ?? merchant?.nameEn ?? merchant?.NameEn ??
      item.merchantName ?? item.MerchantName ?? item.storeName ?? item.StoreName ?? item.tradeName ?? item.TradeName ?? item.name ?? item.Name ?? '—');
    const sales = Number(item.totalSales ?? item.TotalSales ?? item.sales ?? item.Sales ?? item.revenue ?? item.Revenue ?? item.amount ?? item.Amount ?? 0);
    return { name: String(name), sales };
  }

  private mapToProductCategories(items: any[]): ProductCategory[] {
    const palette = ['#F08721', '#D97706', '#B45309', '#92400E', '#78350F', '#F59E0B', '#EA580C', '#A16207', '#C2410C', '#9A3412'];

    const mapped = items.map((item: any, index: number) => {
      const product = item.product ?? item.Product ?? item.ProductDto ?? item;
      const name = this.languageService.getLocalizedName(product) ||
        (product?.nameAr ?? product?.NameAr ?? product?.name ?? product?.Name ?? product?.nameEn ?? product?.NameEn ??
        item.productName ?? item.ProductName ?? item.name ?? item.Name ?? '—');
      const rawValue = Number(
        item.percentage ?? item.Percentage ??
        item.totalOrders ?? item.TotalOrders ??
        item.ordersCount ?? item.OrdersCount ??
        item.count ?? item.Count ??
        item.quantity ?? item.Quantity ??
        item.quantitySold ?? item.QuantitySold ??
        item.piecesSold ?? item.PiecesSold ??
        item.soldCount ?? item.SoldCount ??
        item.totalSales ?? item.TotalSales ??
        item.sales ?? item.Sales ?? 0
      );
      return {
        name: String(name),
        value: isNaN(rawValue) ? 0 : rawValue,
        color: palette[index % palette.length]
      };
    }).filter(x => x.value > 0);

    const total = mapped.reduce((sum, x) => sum + x.value, 0);
    if (total <= 0) {
      const names = items.map((item, index) => {
        const product = item.product ?? item.Product ?? item.ProductDto ?? item;
        const n = this.languageService.getLocalizedName(product) ||
          (product?.nameAr ?? product?.NameAr ?? product?.name ?? product?.Name ?? product?.nameEn ?? product?.NameEn ??
          item.productName ?? item.ProductName ?? item.name ?? item.Name ?? '—');
        return { name: String(n), color: palette[index % palette.length] };
      }).filter(x => x.name && x.name !== '—').slice(0, 10);
      if (!names.length) return [];
      const equalPercentage = Math.max(1, Math.floor(100 / names.length));
      return names.map(x => ({ name: x.name, percentage: equalPercentage, color: x.color }));
    }

    return mapped.map(x => ({
      name: x.name,
      percentage: Math.max(1, Math.round((x.value / total) * 100)),
      color: x.color
    }));
  }

  private filterBySelectedBranch(items: any[]): any[] {
    if (!this.selectedBranchId) return items;
    const filtered = items.filter((item: any) => {
      const branchId = Number(item.branchId ?? item.BranchId ?? item.branch?.id ?? item.Branch?.Id ?? item.storeBranchId ?? item.StoreBranchId ?? 0);
      return branchId === this.selectedBranchId;
    });
    return filtered.length ? filtered : items;
  }

  ngOnDestroy(): void {
    this.branchSubscription?.unsubscribe();
  }

  ngAfterViewInit() {
    if (this.isAdmin) {
      this.drawPeakHoursChart();
    } else if (this.isMerchant) {
      setTimeout(() => this.drawMerchantPeakChart(), 0);
    }
  }

  getHourlyValue(hour: number): number {
    // Data matching the image: peaks at 07-08, 16-17, and 21-22
    const baseData = [
      150,  // 00
      180,  // 01
      200,  // 02
      220,  // 03
      250,  // 04
      280,  // 05
      320,  // 06
      350,  // 07 (first peak)
      340,  // 08
      300,  // 09
      280,  // 10
      300,  // 11
      400,  // 12
      500,  // 13
      600,  // 14
      700,  // 15
      900,  // 16 (main peak)
      950,  // 17 (main peak)
      850,  // 18
      700,  // 19
      800,  // 20
      850,  // 21 (second peak)
      820,  // 22
      600   // 23
    ];
    return baseData[hour] || 400;
  }

  downloadReport(type: 'food' | 'carbon') {
    console.log(`Download ${type} report`);
  }

  getTopMerchantsLeft(): TopMerchant[] {
    return this.topMerchants.slice(0, 5);
  }

  getTopMerchantsRight(): TopMerchant[] {
    return this.topMerchants.slice(5, 10);
  }

  getDonutChartData() {
    const radius = 80;
    const circumference = 2 * Math.PI * radius;
    let currentOffset = 0;
    
    return this.productCategories.map(category => {
      const percentage = category.percentage;
      const dashLength = (percentage / 100) * circumference;
      const gapLength = circumference;
      const strokeDasharray = `${dashLength} ${gapLength}`;
      const strokeDashoffset = -currentOffset;
      
      currentOffset += dashLength;
      
      return {
        ...category,
        strokeDasharray,
        strokeDashoffset: strokeDashoffset.toString()
      };
    });
  }

  drawPeakHoursChart() {
    if (!this.peakHoursChart) return;

    const canvas = this.peakHoursChart.nativeElement;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const width = canvas.width;
    const height = canvas.height;
    const padding = 60;
    const chartWidth = width - padding * 2;
    const chartHeight = height - padding * 2;

    const maxValue = 1000;
    const data = this.peakHoursData.map(d => d.value);

    ctx.clearRect(0, 0, width, height);

    // Draw grid lines
    ctx.strokeStyle = '#f0f0f0';
    ctx.lineWidth = 1;
    [0, 200, 400, 600, 800, 1000].forEach(value => {
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.beginPath();
      ctx.moveTo(padding, y);
      ctx.lineTo(width - padding, y);
      ctx.stroke();
    });

    // Draw hour labels (00-23)
    ctx.fillStyle = '#666';
    ctx.font = '11px Arial';
    ctx.textAlign = 'center';
    for (let i = 0; i < 24; i++) {
      const x = padding + (i / 23) * chartWidth;
      ctx.fillText(i.toString().padStart(2, '0'), x, height - padding + 20);
    }

    // Draw Y-axis labels
    ctx.textAlign = 'right';
    ctx.fillStyle = '#666';
    ctx.font = '11px Arial';
    [0, 200, 400, 600, 800, 1000].forEach(value => {
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.fillText(value.toString(), padding - 10, y + 4);
    });

    // Draw area chart with gradient
    ctx.beginPath();
    ctx.moveTo(padding, height - padding);
    
    data.forEach((value, index) => {
      const x = padding + (index / 23) * chartWidth;
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.lineTo(x, y);
    });
    
    ctx.lineTo(padding + chartWidth, height - padding);
    ctx.closePath();

    // Create gradient
    const gradient = ctx.createLinearGradient(0, padding, 0, height - padding);
    gradient.addColorStop(0, 'rgba(240, 135, 33, 0.3)');
    gradient.addColorStop(1, 'rgba(240, 135, 33, 0.05)');
    
    ctx.fillStyle = gradient;
    ctx.fill();

    // Draw line
    ctx.beginPath();
    ctx.strokeStyle = '#F08721';
    ctx.lineWidth = 2;
    ctx.moveTo(padding, height - padding - (data[0] / maxValue) * chartHeight);
    
    data.forEach((value, index) => {
      const x = padding + (index / 23) * chartWidth;
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.lineTo(x, y);
    });
    
    ctx.stroke();

    // Draw data points
    ctx.fillStyle = '#F08721';
    data.forEach((value, index) => {
      const x = padding + (index / 23) * chartWidth;
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.beginPath();
      ctx.arc(x, y, 3, 0, Math.PI * 2);
      ctx.fill();
    });

    // Draw tooltip at peak (17:00)
    const peakIndex = 17;
    const peakX = padding + (peakIndex / 23) * chartWidth;
    const peakY = height - padding - (data[peakIndex] / maxValue) * chartHeight;
    
    // Tooltip background
    ctx.fillStyle = 'rgba(0, 0, 0, 0.8)';
    ctx.fillRect(peakX - 60, peakY - 30, 120, 25);
    
    // Tooltip text
    ctx.fillStyle = 'white';
    ctx.font = '11px Arial';
    ctx.textAlign = 'center';
    ctx.fillText(`عدد الطلبات ${data[peakIndex]}`, peakX, peakY - 12);
  }

  drawMerchantPeakChart() {
    const canvasRef = this.merchantPeakChart;
    if (!canvasRef?.nativeElement) return;

    const canvas = canvasRef.nativeElement;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    const width = canvas.width;
    const height = canvas.height;
    const padding = 50;
    const chartWidth = width - padding * 2;
    const chartHeight = height - padding * 2;
    const maxValue = 1200;
    const data = this.peakHoursData.map(d => d.value);

    ctx.clearRect(0, 0, width, height);

    // Y-axis labels
    ctx.fillStyle = '#666';
    ctx.font = '11px Arial';
    ctx.textAlign = 'right';
    [0, 300, 600, 900, 1200].forEach(value => {
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.fillText(value.toString(), padding - 8, y + 4);
    });

    // Hour labels (00-23)
    ctx.textAlign = 'center';
    for (let i = 0; i < 24; i++) {
      const x = padding + (i / 23) * chartWidth;
      ctx.fillText(i.toString().padStart(2, '0'), x, height - padding + 18);
    }

    // Area fill with gradient
    ctx.beginPath();
    ctx.moveTo(padding, height - padding);
    data.forEach((value, index) => {
      const x = padding + (index / 23) * chartWidth;
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.lineTo(x, y);
    });
    ctx.lineTo(padding + chartWidth, height - padding);
    ctx.closePath();
    const gradient = ctx.createLinearGradient(0, padding, 0, height - padding);
    gradient.addColorStop(0, 'rgba(244, 162, 97, 0.4)');
    gradient.addColorStop(1, 'rgba(244, 162, 97, 0.05)');
    ctx.fillStyle = gradient;
    ctx.fill();

    // Line
    ctx.beginPath();
    ctx.strokeStyle = '#f4a261';
    ctx.lineWidth = 2;
    ctx.moveTo(padding, height - padding - (data[0] / maxValue) * chartHeight);
    data.forEach((value, index) => {
      const x = padding + (index / 23) * chartWidth;
      const y = height - padding - (value / maxValue) * chartHeight;
      ctx.lineTo(x, y);
    });
    ctx.stroke();
  }
}
