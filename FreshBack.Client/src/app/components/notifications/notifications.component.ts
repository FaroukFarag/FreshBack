import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { timeout, catchError, of } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { TranslationsService } from '../../services/translations.service';
import { LanguageService } from '../../services/language.service';

interface Notification {
  id: number;
  title: string;
  description: string;
  timestamp: string;
  type: 'new' | 'success' | 'info' | 'warning';
  icon: 'bell' | 'checkmark';
  isRead: boolean;
}

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  notifications: Notification[] = [];
  isLoadingNotifications = false;
  errorMessage = '';
  private http = inject(HttpClient);
  private apiService = inject(ApiService);
  private translationsService = inject(TranslationsService);
  private languageService = inject(LanguageService);
  private cdr = inject(ChangeDetectorRef);
  private readonly REQUEST_TIMEOUT = 5000;

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications() {
    this.isLoadingNotifications = true;
    this.errorMessage = '';

    this.http.get<any>(this.apiService.getUrl('Notifications/GetAll'))
      .pipe(
        timeout(this.REQUEST_TIMEOUT),
        catchError((error) => {
          this.isLoadingNotifications = false;
          if (error.name === 'TimeoutError') {
            this.errorMessage = this.translationsService.getSync('notificationsTimeoutError');
          } else {
            console.error('Error loading notifications:', error);
            this.errorMessage = error.error?.message || this.translationsService.getSync('notificationsLoadError');
          }
          this.cdr.detectChanges();
          return of([]);
        })
      )
      .subscribe({
        next: (response) => {
          this.isLoadingNotifications = false;
          console.log('Notifications API response:', response);
          
          // Handle different response structures
          let resultData = null;
          
          // Check if response is already an array
          if (Array.isArray(response)) {
            resultData = response;
          } 
          // Check for resultData property
          else if (response.resultData !== undefined && response.resultData !== null) {
            resultData = response.resultData;
          }
          // Check for data property
          else if (response.data !== undefined && response.data !== null) {
            resultData = response.data;
          }
          // Check if response itself is the data (and not a wrapper object)
          else if (response && typeof response === 'object' && !response.succeeded && !response.message) {
            resultData = response;
          }
          
          if (resultData === null || resultData === undefined || (Array.isArray(resultData) && resultData.length === 0)) {
            this.notifications = [];
            this.cdr.detectChanges();
            return;
          }
          
          const notificationsArray = Array.isArray(resultData) ? resultData : [resultData];
          
          // Map API response to Notification interface
          this.notifications = notificationsArray.map((notification: any) => this.mapNotification(notification));
          this.cdr.detectChanges();
        },
        error: (error: HttpErrorResponse) => {
          this.isLoadingNotifications = false;
          console.error('Error loading notifications:', error);
          
          if (error.status === 0) {
            this.errorMessage = this.translationsService.getSync('connectionFailed');
          } else if (error.status === 408 || error.status === 504) {
            this.errorMessage = this.translationsService.getSync('notificationsTimeoutError');
          } else {
            this.errorMessage = error.error?.message || this.translationsService.getSync('notificationsLoadError');
          }
          
          this.notifications = [];
          this.cdr.detectChanges();
        }
      });
  }

  /**
   * Map API notification response to Notification interface
   */
  private mapNotification(notification: any): Notification {
    // Determine notification type based on API data
    let type: 'new' | 'success' | 'info' | 'warning' = 'info';
    let icon: 'bell' | 'checkmark' = 'bell';
    
    // Map notification type from API (adjust based on your API response structure)
    if (notification.type) {
      const notificationType = notification.type.toLowerCase();
      if (notificationType === 'success' || notificationType === 'completed' || notificationType === 'received') {
        type = 'success';
        icon = 'checkmark';
      } else if (notificationType === 'new' || notificationType === 'pending') {
        type = 'new';
        icon = 'bell';
      } else if (notificationType === 'warning' || notificationType === 'alert') {
        type = 'warning';
        icon = 'bell';
      }
    }
    
    // Determine if notification is read
    const isRead = notification.isRead !== undefined ? notification.isRead : 
                   (notification.read !== undefined ? notification.read : 
                   (notification.status === 'read' ? true : false));
    
    // Format timestamp
    let timestamp = notification.timestamp || notification.createdAt || notification.date || '';
    if (timestamp) {
      timestamp = this.formatTimestamp(timestamp);
    } else {
      timestamp = '';
    }
    
    return {
      id: notification.id || notification.notificationId || 0,
      title: notification.title || notification.subject || this.translationsService.getSync('notification'),
      description: notification.content || notification.description || notification.message || notification.body || '',
      timestamp: timestamp,
      type: type,
      icon: icon,
      isRead: isRead
    };
  }

  /**
   * Format timestamp to relative time (e.g., "منذ 5 دقائق")
   */
  private formatTimestamp(dateString: string): string {
    try {
      const date = new Date(dateString);
      const now = new Date();
      const diffMs = now.getTime() - date.getTime();
      const diffMins = Math.floor(diffMs / 60000);
      const diffHours = Math.floor(diffMs / 3600000);
      const diffDays = Math.floor(diffMs / 86400000);
      
      if (diffMins < 1) {
        return this.translationsService.getSync('now');
      } else if (diffMins < 60) {
        const unit = diffMins === 1 ? this.translationsService.getSync('minute') : this.translationsService.getSync('minutes');
        return this.buildRelativeTime(diffMins, unit);
      } else if (diffHours < 24) {
        const unit = diffHours === 1 ? this.translationsService.getSync('hour') : this.translationsService.getSync('hours');
        return this.buildRelativeTime(diffHours, unit);
      } else if (diffDays < 7) {
        const unit = diffDays === 1 ? this.translationsService.getSync('day') : this.translationsService.getSync('days');
        return this.buildRelativeTime(diffDays, unit);
      } else {
        const locale = this.languageService.getCurrentLanguageValue() === 'en' ? 'en-US' : 'ar-SA';
        return date.toLocaleDateString(locale);
      }
    } catch (e) {
      return dateString;
    }
  }

  private buildRelativeTime(value: number, unit: string): string {
    if (this.languageService.getCurrentLanguageValue() === 'en') {
      return `${value} ${unit} ${this.translationsService.getSync('ago')}`;
    }
    return `${this.translationsService.getSync('ago')} ${value} ${unit}`;
  }

  get newNotificationsCount(): number {
    return this.notifications.filter(n => !n.isRead).length;
  }

  markAllAsRead() {
    this.notifications.forEach(notification => {
      notification.isRead = true;
    });
  }

  markAsRead(notification: Notification) {
    notification.isRead = true;
  }

  getNotificationClass(notification: Notification): string {
    if (!notification.isRead && notification.type === 'new') {
      return 'notification-new';
    }
    if (notification.type === 'success') {
      return 'notification-success';
    }
    return 'notification-default';
  }

  getDotColor(notification: Notification): string {
    if (!notification.isRead && notification.type === 'new') {
      return '#f4a261';
    }
    if (notification.type === 'success') {
      return '#2d8659';
    }
    return '#999';
  }
}
