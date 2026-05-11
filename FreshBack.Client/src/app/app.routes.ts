import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'products',
        loadComponent: () => import('./components/products/products.component').then(m => m.ProductsComponent)
      },
      {
        path: 'orders',
        loadComponent: () => import('./components/order-management/order-management.component').then(m => m.OrderManagementComponent)
      },
      {
        path: 'live-orders',
        loadComponent: () => import('./components/order-management/order-management.component').then(m => m.OrderManagementComponent),
        data: { liveOrders: true }
      }
    ]
  },
  {
    path: 'inventory',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/inventory-management/inventory-management.component').then(m => m.InventoryManagementComponent)
      }
    ]
  },
  {
    path: 'financial',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/financial-management/financial-management.component').then(m => m.FinancialManagementComponent)
      }
    ]
  },
  {
    path: 'statistics',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/statistics/statistics.component').then(m => m.StatisticsComponent)
      }
    ]
  },
  {
    path: 'store-management',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/store-management/store-management.component').then(m => m.StoreManagementComponent)
      }
    ]
  },
  {
    path: 'merchants',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/merchant-management/merchant-management.component').then(m => m.MerchantManagementComponent)
      }
    ]
  },
  {
    path: 'branches',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/branch-management/branch-management.component').then(m => m.BranchManagementComponent)
      }
    ]
  },
  {
    path: 'notifications',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/notifications/notifications.component').then(m => m.NotificationsComponent)
      }
    ]
  },
  {
    path: 'support',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/support/support.component').then(m => m.SupportComponent)
      }
    ]
  },
  {
    path: 'settings',
    loadComponent: () => import('./components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/system-settings/system-settings.component').then(m => m.SystemSettingsComponent)
      }
    ]
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];
