import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  /**
   * Check if the current user is an admin
   * Admin has roleId === 0
   */
  isAdmin(): boolean {
    const roleId = localStorage.getItem('roleId');
    return roleId !== null && roleId === '0';
  }

  /**
   * Check if the current user is a merchant (store owner)
   * Merchant has roleId === 1
   */
  isMerchant(): boolean {
    const roleId = localStorage.getItem('roleId');
    return roleId !== null && roleId === '1';
  }

  /**
   * Check if the current user is a merchant admin (branch manager)
   * Merchant admin has roleId === 2
   */
  isMerchantAdmin(): boolean {
    const roleId = localStorage.getItem('roleId');
    return roleId !== null && roleId === '2';
  }

  /**
   * Get the current user's roleId
   */
  getRoleId(): number | null {
    const roleId = localStorage.getItem('roleId');
    return roleId !== null ? parseInt(roleId, 10) : null;
  }

  /**
   * Get the current user data
   */
  getUser(): any {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  }

  /**
   * Merchant id from login (`resultData.merchantId`), also persisted under `merchantId`.
   * Falls back to JWT payload if needed.
   */
  getMerchantId(): number | null {
    const fromKey = this.parsePositiveInt(localStorage.getItem('merchantId'));
    if (fromKey != null) {
      return fromKey;
    }

    const userStr = localStorage.getItem('user');
    if (userStr) {
      try {
        const user = JSON.parse(userStr);
        const fromUser = this.parsePositiveInt(
          user?.merchantId ?? user?.MerchantId ?? user?.merchant?.id ?? user?.Merchant?.Id
        );
        if (fromUser != null) {
          return fromUser;
        }
      } catch {
        /* ignore */
      }
    }

    const token = localStorage.getItem('token');
    if (token) {
      const payload = this.parseJwtPayload(token);
      if (payload) {
        const fromJwt = this.parsePositiveInt(
          (payload as Record<string, unknown>)['merchantId'] ??
            (payload as Record<string, unknown>)['MerchantId']
        );
        if (fromJwt != null) {
          return fromJwt;
        }
      }
    }

    return null;
  }

  private parsePositiveInt(value: unknown): number | null {
    if (value === null || value === undefined || value === '') {
      return null;
    }
    const n = Number(value);
    if (!Number.isFinite(n) || n <= 0) {
      return null;
    }
    return n;
  }

  private parseJwtPayload(token: string): unknown | null {
    try {
      const parts = token.split('.');
      if (parts.length < 2) {
        return null;
      }
      let base64 = parts[1].replace(/-/g, '+').replace(/_/g, '/');
      const pad = base64.length % 4;
      if (pad) {
        base64 += '='.repeat(4 - pad);
      }
      return JSON.parse(atob(base64));
    } catch {
      return null;
    }
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    return localStorage.getItem('token') !== null;
  }
}
