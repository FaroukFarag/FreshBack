import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  //private readonly baseUrl: string = 'http://localhost:5273/api/';
  private readonly baseUrl: string = 'http://8.208.12.34:8089/api/';
  /**
   * Get the base URL for API requests
   */
  getBaseUrl(): string {
    return this.baseUrl;
  }

  /**
   * Construct full URL by appending endpoint to base URL
   * @param endpoint - API endpoint (e.g., 'users', 'products', 'auth/login')
   * @returns Full URL string
   */
  getUrl(endpoint: string): string {
    // Remove leading slash from endpoint if present
    const cleanEndpoint = endpoint.startsWith('/') ? endpoint.slice(1) : endpoint;
    return `${this.baseUrl}${cleanEndpoint}`;
  }
}

