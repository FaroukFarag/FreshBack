import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { timeout, catchError, of } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { TranslationsService } from '../../services/translations.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { LanguageService } from '../../services/language.service';

interface PaymentMethod {
  id: string;
  nameKey: string;
  enabled: boolean;
}

interface SystemUser {
  id: string;
  name: string;
  jobRole: string;
  email: string;
  active: boolean;
  userName?: string;
  phoneNumber?: string;
  roleId?: number;
}

interface GeographicRegion {
  id: string;
  name: string;
  nameEn?: string;
  deliveryFees: number;
  active: boolean;
}

interface SystemRole {
  id: number;
  name: string;
}

interface CommissionCategoryPayload {
  id: number;
  commissionId: number;
  categoryId: number;
  percentageOfTotal: number;
}

interface CommissionUpdatePayload {
  id: number;
  type: number;
  fixedAmount: number;
  categoryCommissions: CommissionCategoryPayload[];
}

@Component({
  selector: 'app-system-settings',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './system-settings.component.html',
  styleUrls: ['./system-settings.component.scss']
})
export class SystemSettingsComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private translationsService = inject(TranslationsService);
  private languageService = inject(LanguageService);
  private cdr = inject(ChangeDetectorRef);
  private readonly REQUEST_TIMEOUT = 5000;
  private commissionId = 0;
  private categoryCommissionMeta: { id: number; commissionId: number; categoryId: number }[] = [];

  get isAdmin(): boolean {
    return this.authService.getRoleId() === 0;
  }

  // Commission Settings
  commissionType: 'fixed' | 'variable' = 'fixed';
  fixedCommissionRate = 10;
  bakeriesCommission = 10;
  restaurantsCommission = 10;
  storesCommission = 10;
  isLoadingCommissions = false;
  commissionsError = '';

  // Payment Methods
  paymentMethods: PaymentMethod[] = [
    { id: 'credit', nameKey: 'paymentCreditCards', enabled: false },
    { id: 'apple', nameKey: 'paymentApplePay', enabled: false },
    { id: 'stc', nameKey: 'paymentStcPay', enabled: false },
    { id: 'mada', nameKey: 'paymentMada', enabled: false },
    { id: 'cod', nameKey: 'paymentCashOnDelivery', enabled: true }
  ];

  // Users (User Permissions)
  users: SystemUser[] = [];
  isLoadingUsers = false;
  usersError = '';
  isAddUserPanelOpen = false;
  addUserError = '';
  newUserUserName = '';
  newUserEmail = '';
  newUserPassword = '';
  newUserPhoneNumber = '';
  newUserRoleId: number | null = null;
  /** Shown only in edit mode; saved with PUT Users/Update. */
  editUserActive = true;
  roles: SystemRole[] = [];
  isLoadingRoles = false;
  isEditingUser = false;
  editingUserId: number | null = null;

  // Regions (Geographic Region Management)
  regions: GeographicRegion[] = [];
  isLoadingRegions = false;
  regionsError = '';
  isAddRegionPanelOpen = false;
  isEditingRegion = false;
  editingRegionId: number | null = null;
  newRegionName = '';
  newRegionNameEn = '';
  newRegionDeliveryFees: number | null = null;

  saveCommissionSettings(): void {
    this.updateCommissions();
  }

  togglePaymentMethod(pm: PaymentMethod): void {
    return;
  }

  addNewUser(): void {
    this.isAddRegionPanelOpen = false;
    this.isEditingUser = false;
    this.editingUserId = null;
    this.addUserError = '';
    this.newUserUserName = '';
    this.newUserEmail = '';
    this.newUserPassword = '';
    this.newUserPhoneNumber = '';
    this.newUserRoleId = null;
    this.editUserActive = true;
    this.isAddUserPanelOpen = true;
    this.loadRoles();
  }

  closeAddUserPanel(): void {
    this.isAddUserPanelOpen = false;
    this.addUserError = '';
    this.isEditingUser = false;
    this.editingUserId = null;
    this.editUserActive = true;
  }

  editUser(user: SystemUser): void {
    const uid = Number(user.id);
    if (!Number.isFinite(uid) || uid <= 0) {
      return;
    }
    this.isAddRegionPanelOpen = false;
    this.isEditingRegion = false;
    this.isEditingUser = true;
    this.editingUserId = uid;
    this.addUserError = '';
    this.newUserUserName = (user.userName || user.email || '').trim();
    this.newUserEmail = user.email;
    this.newUserPassword = '';
    this.newUserPhoneNumber = (user.phoneNumber || '').trim();
    this.newUserRoleId = user.roleId !== undefined && user.roleId !== null ? user.roleId : null;
    this.editUserActive = user.active;
    this.isAddUserPanelOpen = true;
    this.loadRoles();
  }

  toggleEditUserActive(): void {
    this.editUserActive = !this.editUserActive;
  }

  submitNewUserFromPanel(): void {
    const userName = this.newUserUserName.trim();
    const email = this.newUserEmail.trim();
    const passwordTrimmed = this.newUserPassword.trim();
    const phoneNumber = this.newUserPhoneNumber.trim();
    const roleId = Number(this.newUserRoleId ?? 0);

    if (!userName || !email) {
      this.addUserError = 'Username and email are required.';
      this.cdr.detectChanges();
      return;
    }

    if (!passwordTrimmed) {
      this.addUserError = 'Password is required.';
      this.cdr.detectChanges();
      return;
    }

    if (this.newUserRoleId === null || this.newUserRoleId === undefined) {
      this.addUserError = 'Please select a role.';
      this.cdr.detectChanges();
      return;
    }

    this.addUserError = '';

    if (this.isEditingUser && this.editingUserId !== null && this.editingUserId > 0) {
      const payload = {
        id: this.editingUserId,
        userName,
        email,
        password: passwordTrimmed,
        phoneNumber,
        roleId: isNaN(roleId) ? 0 : roleId,
        isActive: this.editUserActive
      };

      this.http
        .put<any>(this.apiService.getUrl('Users/Update'), payload, { observe: 'response' })
        .pipe(
          timeout(this.REQUEST_TIMEOUT),
          catchError((err: unknown) => {
            this.addUserError = this.formatHttpErrorForDisplay(err);
            this.cdr.detectChanges();
            return of(null);
          })
        )
        .subscribe({
          next: (res: HttpResponse<any> | null) => {
            if (res && res.status >= 200 && res.status < 300) {
              this.resetUserFormAfterSave();
              this.closeAddUserPanel();
              this.loadUsers();
            }
            this.cdr.detectChanges();
          }
        });
      return;
    }

    const payload = {
      id: 0,
      userName,
      email,
      password: passwordTrimmed,
      phoneNumber,
      roleId: isNaN(roleId) ? 0 : roleId
    };

    this.http
      .post<any>(this.apiService.getUrl('Users/Create'), payload)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err: unknown) => {
          this.addUserError = this.formatHttpErrorForDisplay(err);
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          if (res) {
            this.resetUserFormAfterSave();
            this.closeAddUserPanel();
            this.loadUsers();
          }
          this.cdr.detectChanges();
        }
      });
  }

  private resetUserFormAfterSave(): void {
    this.newUserUserName = '';
    this.newUserEmail = '';
    this.newUserPassword = '';
    this.newUserPhoneNumber = '';
    this.newUserRoleId = null;
    this.addUserError = '';
    this.isEditingUser = false;
    this.editingUserId = null;
    this.editUserActive = true;
  }

  /**
   * Reads a trimmed string from a plain object without dot-access on index signatures (TS4111-safe).
   */
  private readErrorBodyString(obj: Record<string, unknown>, key: string): string {
    const v = obj[key];
    return typeof v === 'string' ? v.trim() : '';
  }

  /**
   * Builds a readable message from API errors (ASP.NET ValidationProblemDetails, plain message, 404, etc.).
   */
  private formatHttpErrorForDisplay(err: unknown): string {
    if (err instanceof HttpErrorResponse) {
      if (err.status === 0) {
        return this.translationsService.getSync('connectionFailed');
      }
      const body = err.error;
      if (typeof body === 'string') {
        const t = body.trim();
        if (t) return t;
      }
      if (body && typeof body === 'object') {
        const o = body as Record<string, unknown>;
        const single =
          this.readErrorBodyString(o, 'message') ||
          this.readErrorBodyString(o, 'Message') ||
          this.readErrorBodyString(o, 'detail') ||
          this.readErrorBodyString(o, 'Detail') ||
          this.readErrorBodyString(o, 'error') ||
          this.readErrorBodyString(o, 'Error');
        if (single) return single;

        const errorsBagRaw = o['errors'] ?? o['Errors'];
        const errorsBag =
          errorsBagRaw && typeof errorsBagRaw === 'object'
            ? (errorsBagRaw as Record<string, unknown>)
            : undefined;
        if (errorsBag) {
          const lines: string[] = [];
          for (const key of Object.keys(errorsBag)) {
            const val = errorsBag[key];
            if (Array.isArray(val)) {
              for (const item of val) {
                if (typeof item === 'string' && item.trim()) {
                  lines.push(`${key}: ${item.trim()}`);
                }
              }
            } else if (typeof val === 'string' && val.trim()) {
              lines.push(`${key}: ${val.trim()}`);
            }
          }
          if (lines.length) return lines.join('\n');
        }

        const title =
          this.readErrorBodyString(o, 'title') || this.readErrorBodyString(o, 'Title');
        if (title) return title;
      }
      if (err.status === 404) {
        return this.translationsService.getSync('error');
      }
    }
    return this.translationsService.getSync('error');
  }

  addNewRegion(): void {
    this.isAddUserPanelOpen = false;
    this.isEditingUser = false;
    this.editingUserId = null;
    this.isEditingRegion = false;
    this.editingRegionId = null;
    this.newRegionName = '';
    this.newRegionNameEn = '';
    this.newRegionDeliveryFees = null;
    this.isAddRegionPanelOpen = true;
  }

  closeAddRegionPanel(): void {
    this.isAddRegionPanelOpen = false;
  }

  addRegionFromPanel(): void {
    const name = this.newRegionName.trim();
    const nameEn = this.newRegionNameEn.trim() || name;
    const fee = Number(this.newRegionDeliveryFees ?? 0);
    if (!name) return;

    if (this.isEditingRegion && this.editingRegionId !== null) {
      this.updateRegionFromPanel(this.editingRegionId, name, nameEn, isNaN(fee) ? 0 : fee);
      return;
    }

    const payload = {
      id: 0,
      name,
      nameEn,
      deliveryFees: isNaN(fee) ? 0 : fee
    };

    this.http
      .post<any>(this.apiService.getUrl('Areas/Create'), payload)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err) => {
          this.regionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          if (res) {
            this.newRegionName = '';
            this.newRegionNameEn = '';
            this.newRegionDeliveryFees = null;
            this.isEditingRegion = false;
            this.editingRegionId = null;
            this.closeAddRegionPanel();
            this.loadRegions();
          }
          this.cdr.detectChanges();
        }
      });
  }

  toggleRegionStatus(region: GeographicRegion): void {
    const payload = {
      id: Number(region.id) || 0,
      name: region.name || '',
      nameEn: region.nameEn || region.name || '',
      deliveryFees: Number(region.deliveryFees) || 0
    };

    this.http
      .put<any>(this.apiService.getUrl('Areas/Update'), payload)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err) => {
          this.regionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          if (res) {
            this.loadRegions();
          }
          this.cdr.detectChanges();
        }
      });
  }

  editRegion(region: GeographicRegion): void {
    this.isAddUserPanelOpen = false;
    this.isEditingUser = false;
    this.editingUserId = null;
    this.isEditingRegion = true;
    this.editingRegionId = Number(region.id) || 0;
    this.newRegionName = region.name;
    this.newRegionNameEn = region.nameEn || region.name;
    this.newRegionDeliveryFees = region.deliveryFees;
    this.isAddRegionPanelOpen = true;
  }

  ngOnInit(): void {
    if (this.isAdmin) {
      this.loadCommissions();
      this.loadRegions();
      this.loadUsers();
      this.loadRoles();
    }
  }

  loadRoles(): void {
    this.isLoadingRoles = true;
    this.http
      .get<any>(this.apiService.getUrl('Roles/GetAll'))
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError(() => {
          this.isLoadingRoles = false;
          this.roles = [];
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          this.isLoadingRoles = false;
          if (res) {
            const items = this.extractUsersArray(res);
            this.roles = items
              .map((item: any) => this.mapRoleOption(item))
              .filter((r) => Number.isFinite(r.id));
          }
          this.cdr.detectChanges();
        }
      });
  }

  private mapRoleOption(item: any): SystemRole {
    const id = Number(item.id ?? item.Id ?? item.roleId ?? item.RoleId ?? 0);
    const name =
      this.languageService.getLocalizedName(item) ||
      String(item.name ?? item.Name ?? item.roleName ?? item.RoleName ?? item.title ?? item.Title ?? `Role ${id}`);
    return { id, name };
  }

  loadUsers(): void {
    this.isLoadingUsers = true;
    this.usersError = '';
    const body = { pageSize: 10, pageNumber: 1 } as const;

    this.http
      .post<any>(this.apiService.getUrl('Users/GetAllPaginated'), body)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err: unknown) => {
          this.isLoadingUsers = false;
          this.usersError = this.formatHttpErrorForDisplay(err);
          this.users = [];
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          this.isLoadingUsers = false;
          if (res) {
            const items = this.extractUsersArray(res);
            this.users = items.map((item: any) => this.mapUserToSystemUser(item));
          }
          this.cdr.detectChanges();
        },
        error: (err: unknown) => {
          this.isLoadingUsers = false;
          this.usersError = this.formatHttpErrorForDisplay(err);
          this.users = [];
          this.cdr.detectChanges();
        }
      });
  }

  private extractUsersArray(res: any): any[] {
    if (Array.isArray(res)) return res;
    const rd = res?.resultData ?? res?.data ?? res?.items;
    if (Array.isArray(rd)) return rd;
    if (rd && typeof rd === 'object') {
      const inner = rd.items ?? rd.Items ?? rd.data ?? rd.Data ?? rd.result ?? rd.Result;
      if (Array.isArray(inner)) return inner;
    }
    return this.findFirstArrayInObject(res);
  }

  private mapUserToSystemUser(item: any): SystemUser {
    const id = String(item.id ?? item.Id ?? item.userId ?? item.UserId ?? '');
    const email = String(item.email ?? item.Email ?? item.emailAddress ?? item.EmailAddress ?? '');
    const first = String(item.firstName ?? item.FirstName ?? '').trim();
    const last = String(item.lastName ?? item.LastName ?? '').trim();
    const combined = [first, last].filter(Boolean).join(' ').trim();
    const name =
      this.languageService.getLocalizedName(item) ||
      String(item.fullName ?? item.FullName ?? item.name ?? item.Name ?? item.userName ?? item.UserName ?? (combined || '—'));
    const jobRole = String(
      item.jobRole ?? item.JobRole ?? item.roleName ?? item.RoleName ?? item.role ?? item.Role ?? item.title ?? item.Title ?? '—'
    );
    const active =
      item.isActive !== undefined ? !!item.isActive :
      item.IsActive !== undefined ? !!item.IsActive :
      item.active !== undefined ? !!item.active :
      item.Active !== undefined ? !!item.Active :
      item.status === true || item.status === 1 || item.Status === true || item.Status === 1;

    const userNameRaw = String(item.userName ?? item.UserName ?? '').trim();
    const phoneNumber = String(
      item.phoneNumber ?? item.PhoneNumber ?? item.mobileNumber ?? item.MobileNumber ?? item.phone ?? item.Phone ?? ''
    ).trim();
    const roleIdRaw = Number(item.roleId ?? item.RoleId ?? item.role?.id ?? item.Role?.Id ?? item.roleID ?? NaN);
    const roleId = Number.isFinite(roleIdRaw) ? roleIdRaw : undefined;
    const userName = userNameRaw || email;

    return { id, name, jobRole, email, active, userName, phoneNumber, roleId };
  }

  loadRegions(): void {
    this.isLoadingRegions = true;
    this.regionsError = '';
    const body = { pageSize: 10, pageNumber: 1 } as const;

    this.http
      .post<any>(this.apiService.getUrl('Areas/GetAllPaginated'), body)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err) => {
          this.isLoadingRegions = false;
          this.regionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.regions = [];
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          this.isLoadingRegions = false;
          if (res) {
            const items = this.extractAreasArray(res);
            this.regions = items.map((item: any) => this.mapAreaToRegion(item));
          }
          this.cdr.detectChanges();
        },
        error: (err: HttpErrorResponse) => {
          this.isLoadingRegions = false;
          this.regionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.regions = [];
          this.cdr.detectChanges();
        }
      });
  }

  private extractAreasArray(res: any): any[] {
    if (Array.isArray(res)) return res;
    const rd = res?.resultData ?? res?.data ?? res?.items;
    if (Array.isArray(rd)) return rd;
    if (rd && typeof rd === 'object') {
      const inner = rd.items ?? rd.Items ?? rd.data ?? rd.Data ?? rd.result ?? rd.Result;
      if (Array.isArray(inner)) return inner;
    }
    return this.findFirstArrayInObject(res);
  }

  private findFirstArrayInObject(obj: any): any[] {
    if (!obj || typeof obj !== 'object') return [];
    for (const key of Object.keys(obj)) {
      const value = obj[key];
      if (Array.isArray(value) && value.length && typeof value[0] === 'object') return value;
      if (value && typeof value === 'object') {
        const nested = this.findFirstArrayInObject(value);
        if (nested.length) return nested;
      }
    }
    return [];
  }

  private mapAreaToRegion(item: any): GeographicRegion {
    const nameEn = String(item.nameEn ?? item.NameEn ?? item.name ?? item.Name ?? '');
    const id = String(item.id ?? item.Id ?? item.areaId ?? item.AreaId ?? '');
    const name =
      this.languageService.getLocalizedName(item) ||
      String(item.nameAr ?? item.NameAr ?? item.name ?? item.Name ?? item.nameEn ?? item.NameEn ?? item.areaName ?? item.AreaName ?? '—');
    const deliveryFees = Number(
      item.deliveryFee ?? item.DeliveryFee ?? item.deliveryFees ?? item.DeliveryFees ?? item.fee ?? item.Fee ?? 0
    );
    const active =
      item.isActive !== undefined ? !!item.isActive :
      item.IsActive !== undefined ? !!item.IsActive :
      item.active !== undefined ? !!item.active :
      item.Active !== undefined ? !!item.Active :
      item.status === true || item.status === 1 || item.Status === true || item.Status === 1;

    return { id, name, nameEn, deliveryFees, active };
  }

  private updateRegionFromPanel(id: number, name: string, nameEn: string, deliveryFees: number): void {
    const payload = {
      id,
      name,
      nameEn,
      deliveryFees
    };

    this.http
      .put<any>(this.apiService.getUrl('Areas/Update'), payload)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err) => {
          this.regionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          if (res) {
            this.newRegionName = '';
            this.newRegionNameEn = '';
            this.newRegionDeliveryFees = null;
            this.isEditingRegion = false;
            this.editingRegionId = null;
            this.closeAddRegionPanel();
            this.loadRegions();
          }
          this.cdr.detectChanges();
        }
      });
  }

  loadCommissions(): void {
    this.isLoadingCommissions = true;
    this.commissionsError = '';

    this.http
      .get<any>(this.apiService.getUrl('Commissions/GetAll'))
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err) => {
          this.isLoadingCommissions = false;
          this.commissionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          this.isLoadingCommissions = false;
          if (res) {
            this.mapCommissionsFromResponse(res);
          }
          this.cdr.detectChanges();
        },
        error: (err: HttpErrorResponse) => {
          this.isLoadingCommissions = false;
          this.commissionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.cdr.detectChanges();
        }
      });
  }

  private mapCommissionsFromResponse(res: any): void {
    const items = res.resultData ?? res.data ?? res.items ?? (Array.isArray(res) ? res : []);
    const list = Array.isArray(items) ? items : [];
    this.commissionId = Number(res.id ?? res.commissionId ?? res.result?.id ?? 0) || 0;

    this.categoryCommissionMeta = list.slice(0, 3).map((item: any, index: number) => ({
      id: Number(item.id ?? item.categoryCommissionId ?? 0) || 0,
      commissionId: Number(item.commissionId ?? this.commissionId ?? 0) || 0,
      categoryId: Number(item.categoryId ?? item.idCategory ?? index + 1) || 0
    }));

    const byKey = (...keywords: string[]): number | null => {
      for (const kw of keywords) {
        const k = kw.toLowerCase();
        const item = list.find((x: any) => {
          const name = String(x.nameAr ?? x.NameAr ?? x.name ?? x.Name ?? x.nameEn ?? x.categoryName ?? '').toLowerCase();
          const type = String(x.categoryType ?? x.type ?? x.category ?? '').toLowerCase();
          return name.includes(k) || type.includes(k) || name.includes(kw) || type.includes(kw);
        });
        if (item) return this.extractCommission(item);
      }
      return null;
    };

    const fixed = res.defaultCommission ?? res.fixedCommission ?? res.defaultRate ?? res.fixedRate ?? (list.length === 1 ? this.extractCommission(list[0]) : null);
    if (fixed != null) this.fixedCommissionRate = Number(fixed);

    const bakeries = byKey('مخبز', 'bakery', 'bakeries');
    if (bakeries != null) this.bakeriesCommission = bakeries;

    const restaurants = byKey('مطعم', 'restaurant', 'restaurants');
    if (restaurants != null) this.restaurantsCommission = restaurants;

    const stores = byKey('متجر', 'store', 'stores');
    if (stores != null) this.storesCommission = stores;

    if (list.length > 1) this.commissionType = 'variable';
  }

  private extractCommission(item: any): number | null {
    if (!item) return null;
    const v = item.commission ?? item.Commission ?? item.percentage ?? item.Percentage ?? item.rate ?? item.Rate ?? item.value ?? item.Value;
    if (typeof v === 'number' && !isNaN(v)) return v;
    const n = parseFloat(v);
    return isNaN(n) ? null : n;
  }

  private updateCommissions(): void {
    const payload = this.buildCommissionUpdatePayload();
    this.commissionsError = '';

    this.http
      .put<any>(this.apiService.getUrl('Commissions/Update'), payload)
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((err) => {
          this.commissionsError = err.status === 0
            ? this.translationsService.getSync('connectionFailed')
            : (err.error?.message || this.translationsService.getSync('error'));
          this.cdr.detectChanges();
          return of(null);
        })
      )
      .subscribe({
        next: (res) => {
          if (res) {
            this.commissionId = Number(res.id ?? res.resultData?.id ?? this.commissionId) || this.commissionId;
          }
          this.cdr.detectChanges();
        }
      });
  }

  private buildCommissionUpdatePayload(): CommissionUpdatePayload {
    const type = this.commissionType === 'fixed' ? 0 : 1;
    const categoryDefaults = [
      this.bakeriesCommission,
      this.restaurantsCommission,
      this.storesCommission
    ];

    const categoryCommissions: CommissionCategoryPayload[] = categoryDefaults.map((percentage, index) => {
      const meta = this.categoryCommissionMeta[index];
      return {
        id: meta?.id ?? 0,
        commissionId: meta?.commissionId ?? this.commissionId,
        categoryId: meta?.categoryId ?? index + 1,
        percentageOfTotal: Number(percentage) || 0
      };
    });

    return {
      id: this.commissionId,
      type,
      fixedAmount: Number(this.fixedCommissionRate) || 0,
      categoryCommissions
    };
  }
}
