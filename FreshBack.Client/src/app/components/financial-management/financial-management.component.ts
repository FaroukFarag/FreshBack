import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ElementRef, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { timeout, catchError, of, forkJoin } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { TranslationsService } from '../../services/translations.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import * as echarts from 'echarts/core';
import { BarChart, PieChart } from 'echarts/charts';
import {
  GridComponent,
  TooltipComponent,
  LegendComponent
} from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';

echarts.use([BarChart, PieChart, GridComponent, TooltipComponent, LegendComponent, CanvasRenderer]);

interface WithdrawalRequest {
  id: string;
  merchant: string;
  value: number;
  date: string;
  status: 'pending' | 'completed' | 'rejected';
  statusLabel: string;
  statusKey?: 'withdrawStatusPending' | 'withdrawStatusCompleted' | 'withdrawStatusRejected';
}

@Component({
  selector: 'app-financial-management',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './financial-management.component.html',
  styleUrls: ['./financial-management.component.scss']
})
export class FinancialManagementComponent implements OnInit, AfterViewInit, OnDestroy {
  private authService = inject(AuthService);
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private translationsService = inject(TranslationsService);
  private cdr = inject(ChangeDetectorRef);
  private readonly REQUEST_TIMEOUT = 5000;
  private resizeListener = () => this.resizeCharts();
  private merchantTypeChart?: echarts.ECharts;
  private regionRevenueChart?: echarts.ECharts;

  @ViewChild('merchantTypeChartEl', { static: false }) merchantTypeChartEl?: ElementRef<HTMLDivElement>;
  @ViewChild('regionRevenueChartEl', { static: false }) regionRevenueChartEl?: ElementRef<HTMLDivElement>;
  
  isAdmin = false;
  isLoadingFinancialData = false;
  errorMessage = '';
  selectedMonth = 'october';
  monthKeys: Array<{ value: string; labelKey: string }> = [
    { value: 'october', labelKey: 'monthOctober' },
    { value: 'november', labelKey: 'monthNovember' },
    { value: 'december', labelKey: 'monthDecember' }
  ];

  // Key Metrics
  financialMetrics = {
    pendingPayments: 1554,
    totalCommissions: 65452,
    commissionsChange: 4.8,
    averageOrderValue: 77.45,
    averageOrderChange: -4.8,
    totalRevenue: 654524,
    revenueChange: 1.9
  };

  merchantTypeChartData: Array<{ name: string; value: number }> = [
    { name: 'عائل', value: 70452 },
    { name: 'مطعم', value: 53000 },
    { name: 'سوبر ماركت', value: 39000 },
    { name: 'كافيه', value: 24500 },
    { name: 'أخرى', value: 12000 }
  ];

  regionRevenueChartData: Array<{ name: string; value: number; color: string }> = [
    { name: 'Riyadh', value: 32, color: '#78350F' },
    { name: 'Eastern', value: 25, color: '#92400E' },
    { name: 'Madinah', value: 20, color: '#B45309' },
    { name: 'Jeddah', value: 15, color: '#D97706' },
    { name: 'Tabuk', value: 8, color: '#F08721' }
  ];

  // Withdrawal Requests
  withdrawalRequests: WithdrawalRequest[] = [
    {
      id: '#1234',
      merchant: 'مخبز النور',
      value: 1345,
      date: '2025-12-28',
      status: 'pending',
      statusLabel: 'قيد الإنتظار',
      statusKey: 'withdrawStatusPending'
    },
    {
      id: '#1235',
      merchant: 'مطعم الذواقة',
      value: 2700,
      date: '2025-12-29',
      status: 'completed',
      statusLabel: 'مكتمل',
      statusKey: 'withdrawStatusCompleted'
    },
    {
      id: '#1236',
      merchant: 'سوبر ماركت الخير',
      value: 4500,
      date: '2025-12-28',
      status: 'pending',
      statusLabel: 'قيد الإنتظار',
      statusKey: 'withdrawStatusPending'
    },
    {
      id: '#1237',
      merchant: 'مطعم',
      value: 800,
      date: '2025-12-29',
      status: 'rejected',
      statusLabel: 'مرفوض',
      statusKey: 'withdrawStatusRejected'
    },
    {
      id: '#1238',
      merchant: 'مخبز',
      value: 2000,
      date: '2025-12-28',
      status: 'completed',
      statusLabel: 'مكتمل',
      statusKey: 'withdrawStatusCompleted'
    }
  ];

  isMerchant = false;

  // Merchant Metrics
  merchantMetrics = {
    currentBalance: 12548,
    totalSales: 12548,
    commissions: 1258,
    commissionPercentage: 10
  };

  // Merchant Transactions
  transactions: Array<{
    id: string;
    product: string;
    quantity: number;
    price: number;
    commission: number;
    net: number;
    date: string;
    time: string;
  }> = [
    {
      id: '#1234',
      product: 'خبز فرنسي',
      quantity: 3,
      price: 34,
      commission: 3.4,
      net: 21.6,
      date: '2025-12-20',
      time: '14:22'
    },
    {
      id: '#1235',
      product: 'كرواسون',
      quantity: 5,
      price: 45,
      commission: 4.5,
      net: 40.6,
      date: '2025-12-20',
      time: '14:22'
    },
    {
      id: '#1236',
      product: 'باجيت',
      quantity: 5,
      price: 28,
      commission: 5.2,
      net: 30.5,
      date: '2025-12-20',
      time: '14:22'
    },
    {
      id: '#1237',
      product: 'خبز البيتا',
      quantity: 8,
      price: 50,
      commission: 2.8,
      net: 15.2,
      date: '2025-12-20',
      time: '14:22'
    },
    {
      id: '#1238',
      product: 'خبز العرايس',
      quantity: 12,
      price: 22,
      commission: 6.1,
      net: 50.1,
      date: '2025-12-20',
      time: '14:22'
    },
    {
      id: '#1239',
      product: 'خبز التوست',
      quantity: 20,
      price: 39,
      commission: 3.9,
      net: 28.3,
      date: '2025-12-20',
      time: '14:22'
    },
    {
      id: '#1240',
      product: 'خبز الشوفان',
      quantity: 10,
      price: 30,
      commission: 4.0,
      net: 35.0,
      date: '2025-12-20',
      time: '14:22'
    }
  ];

  ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isMerchant = !this.isAdmin;
    
    if (this.isAdmin) {
      this.loadAdminFinancialData();
    } else {
      this.loadMerchantFinancialData();
    }
  }

  ngAfterViewInit(): void {
    if (this.isAdmin) {
      this.queueRenderCharts();
      window.addEventListener('resize', this.resizeListener);
    }
  }

  ngOnDestroy(): void {
    window.removeEventListener('resize', this.resizeListener);
    this.merchantTypeChart?.dispose();
    this.regionRevenueChart?.dispose();
  }

  loadAdminFinancialData() {
    this.isLoadingFinancialData = true;
    this.errorMessage = '';

    // Load all admin financial data in parallel
    forkJoin({
      totalRevenues: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetTotalRevenues')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading total revenues:', error);
          return of(null);
        })
      ),
      orderAverageValue: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetOrderAverageValue')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading order average value:', error);
          return of(null);
        })
      ),
      totalCommissions: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetTotalCommissions')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading total commissions:', error);
          return of(null);
        })
      ),
      revenuesByArea: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetTotalRevenuesByArea')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading revenues by area:', error);
          return of(null);
        })
      ),
      revenuesByMerchant: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetTotalRevenuesByMerchant')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading revenues by merchant:', error);
          return of(null);
        })
      )
    }).subscribe({
      next: (responses) => {
        this.isLoadingFinancialData = false;
        
        // Map total revenues
        if (responses.totalRevenues) {
          const totalRevenue = this.extractValue(responses.totalRevenues);
          if (totalRevenue !== null) {
            this.financialMetrics.totalRevenue = totalRevenue;
          }
        }

        // Map order average value
        if (responses.orderAverageValue) {
          const avgValue = this.extractValue(responses.orderAverageValue);
          if (avgValue !== null) {
            this.financialMetrics.averageOrderValue = avgValue;
          }
        }

        // Map total commissions
        if (responses.totalCommissions) {
          const totalCommissions = this.extractValue(responses.totalCommissions);
          if (totalCommissions !== null) {
            this.financialMetrics.totalCommissions = totalCommissions;
          }
        }

        // Map revenues by area (for donut chart)
        if (responses.revenuesByArea) {
          const areaData = this.mapAreaRevenuesForChart(responses.revenuesByArea);
          if (areaData.length) this.regionRevenueChartData = areaData;
        }

        // Map revenues by merchant (for bar chart)
        if (responses.revenuesByMerchant) {
          const merchantData = this.mapMerchantRevenuesForChart(responses.revenuesByMerchant);
          if (merchantData.length) this.merchantTypeChartData = merchantData;
        }
        this.cdr.detectChanges();
        this.queueRenderCharts();
      },
      error: (error: HttpErrorResponse) => {
        this.isLoadingFinancialData = false;
        this.cdr.detectChanges();
        console.error('Error loading financial data:', error);
        
        if (error.status === 0) {
          this.errorMessage = this.translationsService.getSync('connectionFailed');
        } else {
          this.errorMessage = error.error?.message || this.translationsService.getSync('error');
        }
      }
    });
  }

  loadMerchantFinancialData() {
    this.isLoadingFinancialData = true;
    this.errorMessage = '';

    // Load merchant-specific financial data
    forkJoin({
      totalRevenues: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetTotalRevenues')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading total revenues:', error);
          return of(null);
        })
      ),
      orderAverageValue: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetOrderAverageValue')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading order average value:', error);
          return of(null);
        })
      ),
      totalCommissions: this.http.get<any>(this.apiService.getUrl('FinanceManagement/GetTotalCommissions')).pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          console.error('Error loading total commissions:', error);
          return of(null);
        })
      )
    }).subscribe({
      next: (responses) => {
        this.isLoadingFinancialData = false;
        
        // Map total revenues to merchant metrics
        if (responses.totalRevenues) {
          const totalRevenue = this.extractValue(responses.totalRevenues);
          if (totalRevenue !== null) {
            // Calculate merchant metrics based on total revenue
            // Assuming 10% commission rate
            const commissionRate = 0.1;
            this.merchantMetrics.totalSales = totalRevenue;
            this.merchantMetrics.commissions = Math.round(totalRevenue * commissionRate);
            this.merchantMetrics.currentBalance = totalRevenue - this.merchantMetrics.commissions;
          }
        }

        // Map order average value
        if (responses.orderAverageValue) {
          const avgValue = this.extractValue(responses.orderAverageValue);
          if (avgValue !== null) {
            this.financialMetrics.averageOrderValue = avgValue;
          }
        }

        // Map total commissions to merchant metrics
        if (responses.totalCommissions) {
          const totalCommissions = this.extractValue(responses.totalCommissions);
          if (totalCommissions !== null) {
            this.merchantMetrics.commissions = totalCommissions;
            this.merchantMetrics.currentBalance = this.merchantMetrics.totalSales - this.merchantMetrics.commissions;
          }
        }
        this.cdr.detectChanges();
      },
      error: (error: HttpErrorResponse) => {
        this.isLoadingFinancialData = false;
        this.cdr.detectChanges();
        console.error('Error loading merchant financial data:', error);
        
        if (error.status === 0) {
          this.errorMessage = this.translationsService.getSync('connectionFailed');
        } else {
          this.errorMessage = error.error?.message || this.translationsService.getSync('error');
        }
      }
    });
  }

  /**
   * Extract numeric value from API response
   * Handles different response structures
   */
  private extractValue(response: any): number | null {
    if (response === null || response === undefined) {
      return null;
    }

    // If response is a number
    if (typeof response === 'number') {
      return response;
    }

    // If response has resultData property
    if (response.resultData !== undefined && response.resultData !== null) {
      const value = typeof response.resultData === 'number' ? response.resultData : response.resultData.value || response.resultData.total || response.resultData.amount;
      return typeof value === 'number' ? value : null;
    }

    // If response has data property
    if (response.data !== undefined && response.data !== null) {
      const value = typeof response.data === 'number' ? response.data : response.data.value || response.data.total || response.data.amount;
      return typeof value === 'number' ? value : null;
    }

    // If response has value, total, or amount property
    if (response.value !== undefined && response.value !== null) {
      return typeof response.value === 'number' ? response.value : null;
    }
    if (response.total !== undefined && response.total !== null) {
      return typeof response.total === 'number' ? response.total : null;
    }
    if (response.amount !== undefined && response.amount !== null) {
      return typeof response.amount === 'number' ? response.amount : null;
    }

    // If response is an object with numeric properties, try to find the first number
    if (typeof response === 'object') {
      for (const key in response) {
        if (typeof response[key] === 'number') {
          return response[key];
        }
      }
    }

    return null;
  }

  private mapMerchantRevenuesForChart(response: any): Array<{ name: string; value: number }> {
    const items = this.extractArray(response);
    return items
      .map((item: any) => ({
        name: String(item.name ?? item.Name ?? item.merchantType ?? item.MerchantType ?? item.category ?? item.Category ?? '—'),
        value: Number(item.total ?? item.Total ?? item.value ?? item.Value ?? item.amount ?? item.Amount ?? item.revenue ?? item.Revenue ?? 0)
      }))
      .filter(x => x.value > 0);
  }

  private mapAreaRevenuesForChart(response: any): Array<{ name: string; value: number; color: string }> {
    const palette = ['#78350F', '#92400E', '#B45309', '#D97706', '#F08721', '#FB923C', '#FDBA74'];
    const items = this.extractArray(response);
    return items
      .map((item: any, index: number) => ({
        name: String(item.name ?? item.Name ?? item.areaName ?? item.AreaName ?? item.region ?? item.Region ?? `Area ${index + 1}`),
        value: Number(item.percentage ?? item.Percentage ?? item.value ?? item.Value ?? item.total ?? item.Total ?? 0),
        color: palette[index % palette.length]
      }))
      .filter(x => x.value > 0);
  }

  private extractArray(response: any): any[] {
    const data = response?.resultData ?? response?.data ?? response?.items ?? response?.list ?? response;
    if (Array.isArray(data)) return data;
    if (data && typeof data === 'object') {
      for (const key of Object.keys(data)) {
        if (Array.isArray(data[key])) return data[key];
      }
    }
    return [];
  }

  private renderCharts(): void {
    if (!this.merchantTypeChartEl?.nativeElement || !this.regionRevenueChartEl?.nativeElement) return;

    if (!this.merchantTypeChart) {
      this.merchantTypeChart = echarts.init(this.merchantTypeChartEl.nativeElement);
    }
    if (!this.regionRevenueChart) {
      this.regionRevenueChart = echarts.init(this.regionRevenueChartEl.nativeElement);
    }

    this.merchantTypeChart.setOption({
      title: {
        text: 'الإيرادات حسب نوع التاجر',
        left: 'center',
        bottom: 0,
        textStyle: {
          fontSize: 18,
          fontWeight: 700,
          color: '#1f2937'
        }
      },
      grid: { left: 56, right: 24, top: 24, bottom: 70 },
      tooltip: {
        trigger: 'item',
        backgroundColor: '#fff',
        borderColor: '#eee',
        borderWidth: 1,
        textStyle: { color: '#111827', fontSize: 14, fontWeight: 600 },
        padding: [8, 10],
        formatter: (params: any) => `${params.value.toLocaleString()}`
      },
      xAxis: {
        type: 'category',
        data: this.merchantTypeChartData.map(x => x.name),
        axisTick: { show: false },
        axisLine: { lineStyle: { color: '#e5e7eb' } },
        axisLabel: { color: '#6b7280', fontSize: 12, margin: 12 }
      },
      yAxis: {
        type: 'log',
        min: 1,
        logBase: 10,
        axisLine: { show: false },
        axisTick: { show: false },
        axisLabel: {
          color: '#6b7280',
          fontSize: 11,
          formatter: (value: number) => {
            if (value >= 1000) return value.toLocaleString();
            return String(value);
          }
        },
        splitLine: { show: false }
      },
      series: [
        {
          type: 'bar',
          data: this.merchantTypeChartData.map(x => x.value),
          barWidth: 42,
          barCategoryGap: '35%',
          itemStyle: {
            color: '#F08721',
            borderRadius: [6, 6, 0, 0]
          },
          emphasis: {
            itemStyle: {
              color: '#e6791b'
            }
          }
        }
      ]
    });

    this.regionRevenueChart.setOption({
      tooltip: { trigger: 'item' },
      legend: { orient: 'vertical', right: 0, top: 'center', textStyle: { color: '#333', fontSize: 12 } },
      series: [
        {
          type: 'pie',
          radius: ['58%', '78%'],
          center: ['32%', '50%'],
          label: { show: false },
          labelLine: { show: false },
          data: this.regionRevenueChartData.map(x => ({
            name: x.name,
            value: x.value,
            itemStyle: { color: x.color }
          }))
        }
      ]
    });
  }

  private resizeCharts(): void {
    this.merchantTypeChart?.resize();
    this.regionRevenueChart?.resize();
  }

  private queueRenderCharts(): void {
    // Wait one tick so *ngIf creates chart DOM elements before init.
    setTimeout(() => this.renderCharts(), 0);
  }

  withdrawProfits() {
    console.log('Withdraw profits');
  }

  exportToExcel() {
    console.log('Export to Excel');
  }

  exportToPDF() {
    console.log('Export to PDF');
  }

  viewWithdrawalDetails(request: WithdrawalRequest) {
    console.log('View withdrawal details:', request);
  }

  approveWithdrawal(request: WithdrawalRequest) {
    console.log('Approve withdrawal:', request);
    request.status = 'completed';
    request.statusLabel = 'مكتمل';
    request.statusKey = 'withdrawStatusCompleted';
  }

  rejectWithdrawal(request: WithdrawalRequest) {
    console.log('Reject withdrawal:', request);
    request.status = 'rejected';
    request.statusLabel = 'مرفوض';
    request.statusKey = 'withdrawStatusRejected';
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'pending':
        return 'status-pending';
      case 'completed':
        return 'status-completed';
      case 'rejected':
        return 'status-rejected';
      default:
        return '';
    }
  }

  getAbsoluteValue(value: number): number {
    return Math.abs(value);
  }
}
