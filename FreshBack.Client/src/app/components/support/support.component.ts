import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface FAQItem {
  id: number;
  question: string;
  answer: string;
  isExpanded: boolean;
}

interface ContactInfo {
  type: 'hours' | 'phone' | 'email';
  icon: string;
  label: string;
  value: string;
}

interface Video {
  id: number;
  title: string;
  duration: string;
  thumbnail: string;
}

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.scss']
})
export class SupportComponent {
  contactInfo: ContactInfo[] = [
    {
      type: 'hours',
      icon: 'clock',
      label: 'ساعات العمل',
      value: '08:00 - 22:00'
    },
    {
      type: 'phone',
      icon: 'phone',
      label: 'رقم التواصل',
      value: '05 468732256'
    },
    {
      type: 'email',
      icon: 'email',
      label: 'البريد الإلكتروني',
      value: 'Support@platform.sa'
    }
  ];

  faqItems: FAQItem[] = [
    {
      id: 1,
      question: 'كيف أضيف منتج فائض جديد؟',
      answer: 'انتقل إلى صفحة "إدارة المنتجات الفائضة" واضغط على زر "إضافة منتج فائض جديد". املأ النموذج السريع أو المتقدم حسب احتياجك.',
      isExpanded: true
    },
    {
      id: 2,
      question: 'كيف يتم حساب العمولة؟',
      answer: 'يتم حساب العمولة كنسبة مئوية من إجمالي المبيعات. يتم تحديد النسبة حسب نوع المنتج وحجم المبيعات. يمكنك الاطلاع على التفاصيل في صفحة الإدارة المالية.',
      isExpanded: false
    },
    {
      id: 3,
      question: 'متى يمكنني سحب أرباحي؟',
      answer: 'يمكنك سحب أرباحك في أي وقت بعد اكتمال العملية. يتم معالجة طلبات السحب خلال 24-48 ساعة عمل. يمكنك تتبع حالة الطلب من صفحة الإدارة المالية.',
      isExpanded: false
    },
    {
      id: 4,
      question: 'كيف يعمل محرك التسعير الذكي؟',
      answer: 'يستخدم محرك التسعير الذكي خوارزميات متقدمة لتحليل الطلب والعرض والتاريخ السابق للمبيعات. يقترح الأسعار المثلى بناءً على هذه البيانات لزيادة المبيعات وتقليل الفائض.',
      isExpanded: false
    },
    {
      id: 5,
      question: 'هل يمكنني التعديل على بيانات المنتج بعد نشره؟',
      answer: 'نعم، يمكنك تعديل بيانات المنتج في أي وقت من صفحة إدارة المنتجات. بعض التعديلات قد تتطلب موافقة قبل التطبيق.',
      isExpanded: false
    }
  ];

  videos: Video[] = [
    {
      id: 1,
      title: 'كيفية إضافة منتج جديد',
      duration: '4:12',
      thumbnail: ''
    },
    {
      id: 2,
      title: 'استخدام محرك التسعير الذكي',
      duration: '5:30',
      thumbnail: ''
    },
    {
      id: 3,
      title: 'إدارة الطلبات والمبيعات',
      duration: '3:45',
      thumbnail: ''
    },
    {
      id: 4,
      title: 'قراءة الإحصائيات والأداء',
      duration: '6:20',
      thumbnail: ''
    }
  ];

  toggleFAQ(item: FAQItem) {
    item.isExpanded = !item.isExpanded;
  }

  getIconPath(icon: string): string {
    const iconMap: { [key: string]: string } = {
      clock: 'clock',
      phone: 'phone',
      email: 'email'
    };
    return iconMap[icon] || icon;
  }
}
