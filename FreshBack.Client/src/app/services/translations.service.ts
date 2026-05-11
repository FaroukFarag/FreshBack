import { Injectable, inject } from '@angular/core';
import { LanguageService, Language } from './language.service';
import { Observable, map } from 'rxjs';

export interface Translations {
  // Layout & Navigation
  dashboard: string;
  branchManagement: string;
  inventoryManagement: string;
  productsManagement: string;
  ordersAndSales: string;
  financialManagement: string;
  statisticsAndPerformance: string;
  storeManagement: string;
  notificationsAndCommunication: string;
  supportAndHelp: string;
  merchantsControlPanel: string;
  merchantManagement: string;
  systemSettings: string;
  currentBranch: string;
  logout: string;
  language: string;
  ordersManagement: string;
  statisticsAndAnalytics: string;
  contentAndNotificationsManagement: string;

  // Branch Management
  addNewBranch: string;
  branchDetails: string;
  branchName: string;
  branchNameEn: string;
  location: string;
  neighborhood: string;
  neighborhoodEn: string;
  area: string;
  operatingHours: string;
  revenue: string;
  currentSurplus: string;
  active: string;
  inactive: string;
  delete: string;
  save: string;
  cancel: string;
  noBranches: string;
  getCurrentLocation: string;
  openingTime: string;
  closingTime: string;
  latitude: string;
  longitude: string;

  // Inventory Management
  excessInventoryManagement: string;
  dailySurplusForecast: string;
  basedOnPOS: string;
  productName: string;
  currentStock: string;
  expectedSurplus: string;
  expirationTime: string;
  suggestedAction: string;
  addToSurplus: string;
  smartPricingEngine: string;
  pricingRecommendations: string;
  originalPrice: string;
  suggestedPrice: string;
  applySuggestedPrice: string;
  bestTimeToPublish: string;
  smartTip: string;

  // Orders
  orderCategories: string;
  new: string;
  confirmed: string;
  delivered: string;
  searchOrder: string;
  orderNumber: string;
  customerName: string;
  viewDetails: string;
  confirmOrder: string;
  rejectOrder: string;
  rejectionReason: string;
  confirmDelivery: string;
  newOrderNeedsConfirmation: string;
  deliveryInProgress: string;
  orderDelivered: string;
  receiptTime: string;
  paymentMethod: string;
  productList: string;
  quantity: string;
  totalAmount: string;

  // Products Management Page
  searchProduct: string;
  discountPercentage: string;
  productStatus: string;
  dateAdded: string;
  expiryDate: string;
  productCode: string;
  vendorName: string;
  actions: string;
  expired: string;
  sold: string;
  blocked: string;
  loadingProducts: string;
  noProducts: string;
  noProductsDescription: string;
  table: string;
  of: string;
  sessionExpired: string;
  connectionFailed: string;
  loadProductsError: string;
  loadProductsFailed: string;
  noAccess: string;
  addNewExcessProduct: string;
  productPrice: string;
  views: string;
  salesPercentage: string;
  lowStock: string;
  copy: string;

  // Merchant Management Page
  merchant: string;
  suspended: string;
  pending: string;
  merchantsList: string;
  searchMerchant: string;
  geographicRegion: string;
  salesVolume: string;
  activityType: string;
  region: string;
  ordersCount: string;
  rating: string;
  loadingMerchants: string;
  retry: string;
  noMerchantsAvailable: string;
  addNewMerchant: string;
  merchantNameEnLabel: string;
  merchantDescription: string;
  merchantDescriptionEn: string;
  merchantStory: string;
  merchantStoryEn: string;
  category: string;
  loadingCategories: string;
  selectCategory: string;
  username: string;
  phoneNumber: string;
  password: string;
  merchantStatus: string;
  selectArea: string;
  merchantCreateError: string;
  merchantImage: string;
  merchantChooseImage: string;
  merchantRemoveImage: string;
  merchantInvalidImageType: string;
  merchantImageDropHint: string;
  merchantImageFormatsHint: string;
  merchantReplaceImage: string;
  merchantName: string;
  riyal: string;
  deactivateMerchantAccount: string;
  activateMerchant: string;
  contact: string;
  merchantDetails: string;
  storeStorySection: string;
  merchantData: string;
  viewLocation: string;
  contactNumber: string;
  customerReviews: string;
  deleteReview: string;
  confirmDeactivateTitle: string;
  deactivateModalDescription: string;
  deactivateReasonPlaceholder: string;
  confirmDeactivate: string;
  deactivateReason1: string;
  deactivateReason2: string;
  deactivateReason3: string;
  deactivateReason4: string;
  deactivateReason5: string;
  salesVolumeLow: string;
  salesVolumeMedium: string;
  salesVolumeHigh: string;
  loadMerchantsError: string;
  loadMerchantsFailed: string;

  // Dashboard
  activeCustomersCount: string;
  customerUnit: string;
  ofThem: string;
  thisWeek: string;
  thisMonth: string;
  registeredMerchantsCount: string;
  monthlyGrowthRate: string;
  comparedToPreviousMonth: string;
  wasteSavedAmount: string;
  kg: string;
  activeOrdersCount: string;
  averageDailyOrders: string;
  orderUnit: string;
  sinceHour: string;
  totalSales: string;
  daily: string;
  weekly: string;
  monthly: string;
  percentFromLastWeek: string;
  regionActivityMap: string;
  mapApiKeyMessage: string;
  mapApiKeySub: string;
  activityVeryHigh: string;
  activityMedium: string;
  activityLow: string;
  latestOrdersAndActivities: string;
  value: string;
  time: string;
  delivery: string;
  completed: string;
  occupancyRate: string;
  hourlySalesRate: string;
  weeklySales: string;
  date: string;
  wednesday: string;
  productsSoldCount: string;
  productUnit: string;
  productsRemainingCount: string;
  savedWeightAmount: string;
  kilogram: string;
  soldPiecesNote: string;
  egp: string;

  // Orders Management Page
  all: string;
  allOrders: string;
  searchOrdersPlaceholder: string;
  orderStatus: string;
  orderStatusReceived: string;
  orderStatusScheduled: string;
  orderStatusPending: string;
  orderStatusCancelled: string;
  totalValue: string;
  orderCreationTime: string;
  loadingOrders: string;
  noOrders: string;
  noOrdersToDisplay: string;
  noOrdersInSection: string;
  ordersEmptyHint: string;
  clearFilters: string;
  copyOrderNumberAction: string;
  refresh: string;
  orderDetailsFor: string;
  orderReadyAwaitingCustomer: string;
  oneOrder: string;
  orderDetails: string;
  customerData: string;
  paymentSummary: string;
  email: string;
  phone: string;
  price: string;
  discount: string;
  fees: string;
  finalTotal: string;
  products: string;
  rejectReasonTitle: string;
  selectRejectReason: string;
  confirmRejection: string;
  orderNumberLabel: string;
  rejectReason1: string;
  rejectReason2: string;
  rejectReason3: string;
  rejectReason4: string;
  rejectReason5: string;
  loadOrdersError: string;

  // Branch Manager (roleId = 2)
  branchManagerDashboard: string;
  liveOrders: string;
  orderHistory: string;
  excessProductsManagement: string;
  listOfOrders: string;
  searchByOrderNumber: string;
  searchHere: string;
  sortByOrder: string;
  store: string;
  page: string;
  scanQR: string;
  scanQRForCustomer: string;
  clickToStartScanning: string;
  activateCamera: string;
  or: string;
  enterQRCodeManually: string;
  enterQRCodePlaceholder: string;
  confirm: string;
  outForDelivery: string;
  awaitingCustomer: string;
  notPaid: string;
  paid: string;
  pickupFromBranch: string;
  deliveryMethodDelivery: string;
  deliveryType: string;
  pickup: string;
  mobileNumber: string;

  // Financial Management Page
  pendingPayments: string;
  totalCommissions: string;
  averageOrderValue: string;
  totalRevenues: string;
  financialAnalytics: string;
  monthOctober: string;
  monthNovember: string;
  monthDecember: string;
  exportReportPdf: string;
  exportReportExcel: string;
  revenueByMerchantType: string;
  revenueByRegion: string;
  merchantTypeStores: string;
  merchantTypeRestaurants: string;
  merchantTypeSupermarkets: string;
  merchantTypeCafes: string;
  merchantTypeOthers: string;
  regionRiyadh: string;
  regionEastern: string;
  regionMadinah: string;
  regionJeddah: string;
  regionTabuk: string;
  withdrawalRequests: string;
  withdrawStatusPending: string;
  withdrawStatusCompleted: string;
  withdrawStatusRejected: string;
  approve: string;
  reject: string;

  // Merchant Financial Management
  currentBalance: string;
  netProfits: string;
  beforeCommission: string;
  commissionPercentage: string;
  withdrawProfits: string;
  withdrawProfitsDescription: string;
  transactionLog: string;
  transactionNumber: string;
  product: string;
  commission: string;
  net: string;
  financialTip: string;
  financialTipText: string;
  performanceAnalysis: string;
  performanceAnalysisText: string;

  // System Settings (Admin)
  commissionSettings: string;
  commissionTypeRatio: string;
  commissionTypeFixed: string;
  commissionTypeVariable: string;
  fixedCommissionRate: string;
  bakeriesCommission: string;
  restaurantsCommission: string;
  storesCommission: string;
  saveCommissionSettings: string;
  availablePaymentMethods: string;
  paymentCreditCards: string;
  paymentApplePay: string;
  paymentStcPay: string;
  paymentMada: string;
  paymentCashOnDelivery: string;
  enabled: string;
  disabled: string;
  userPermissions: string;
  addNewUser: string;
  jobRole: string;
  geographicRegionManagement: string;
  addNewRegion: string;
  deliveryFees: string;
  name: string;
  status: string;
  edit: string;
  editRegion: string;
  regionNameLabel: string;
  regionNameEnglishLabel: string;
  currentRegions: string;
  saveRegionChanges: string;
  addRegionWithPlus: string;

  // Common
  today: string;
  moreThan7Days: string;
  filter: string;
  loading: string;
  error: string;
  success: string;
  close: string;
  yes: string;
  no: string;
  ok: string;
  notificationCenter: string;
  newNotifications: string;
  markAllAsRead: string;
  loadingNotifications: string;
  noNotifications: string;
  noNotificationsDescription: string;
  notificationsTimeoutError: string;
  notificationsLoadError: string;
  unknownTimeAgo: string;
  notification: string;
  now: string;
  ago: string;
  minute: string;
  minutes: string;
  hour: string;
  hours: string;
  day: string;
  days: string;
}

const ARABIC_TRANSLATIONS: Translations = {
  dashboard: 'الرئيسية',
  branchManagement: 'إدارة الفروع',
  inventoryManagement: 'إدارة المخزون الفائض',
  productsManagement: 'إدارة المنتجات الفائضة',
  ordersAndSales: 'الطلبات والمبيعات',
  ordersManagement: 'إدارة الطلبات',
  financialManagement: 'الإدارة المالية',
  statisticsAndPerformance: 'الإحصائيات والأداء',
  statisticsAndAnalytics: 'الإحصائيات والتحليلات',
  storeManagement: 'إدارة المتجر والمظهر',
  notificationsAndCommunication: 'الإشعارات والتواصل',
  contentAndNotificationsManagement: 'إدارة المحتوى والإشعارات',
  supportAndHelp: 'الدعم والمساعدة',
  merchantsControlPanel: 'لوحة التحكم الادارية',
  branchManagerDashboard: 'لوحة مدير الفرع',
  liveOrders: 'الطلبات الحية',
  orderHistory: 'سجل الطلبات',
  excessProductsManagement: 'إدارة المنتجات الفائضة',
  listOfOrders: 'قائمة الطلبات',
  searchByOrderNumber: 'ابحث برقم الطلب',
  searchHere: 'ابحث هنا...',
  sortByOrder: 'فرز حسب الطلب',
  store: 'المتجر',
  page: 'صفحة',
  scanQR: 'امسح QR',
  scanQRForCustomer: 'مسح رمز QR للعميل',
  clickToStartScanning: 'اضغط للبدء في المسح',
  activateCamera: 'تفعيل الكاميرا',
  or: 'او',
  enterQRCodeManually: 'ادخال رمز QR يدويا',
  enterQRCodePlaceholder: 'أدخل رمز QR',
  confirm: 'تأكيد',
  outForDelivery: 'خرج للتوصيل',
  awaitingCustomer: 'في انتظار العميل',
  notPaid: 'لم يتم الدفع',
  paid: 'تم الدفع',
  pickupFromBranch: 'استلام من الفرع',
  deliveryMethodDelivery: 'توصيل',
  deliveryType: 'نوع التوصيل',
  pickup: 'استلام',
  mobileNumber: 'رقم الجوال',
  merchantManagement: 'إدارة التجار',
  systemSettings: 'إعدادات النظام',
  currentBranch: 'الفرع الحالي : فرع الخليج',
  logout: 'تسجيل الخروج',
  language: 'اللغة',

  addNewBranch: 'إضافة فرع جديد',
  branchDetails: 'تفاصيل الفرع',
  branchName: 'اسم الفرع',
  branchNameEn: 'اسم الفرع بالإنجليزية',
  location: 'الموقع',
  neighborhood: 'الحي',
  neighborhoodEn: 'الحي بالإنجليزية',
  area: 'المنطقة',
  operatingHours: 'ساعات العمل',
  revenue: 'الإيرادات',
  currentSurplus: 'الفائض الحالي',
  active: 'نشط',
  inactive: 'غير نشط',
  delete: 'حذف',
  save: 'حفظ',
  cancel: 'إلغاء',
  noBranches: 'لا توجد فروع',
  getCurrentLocation: 'الحصول على الموقع الحالي',
  openingTime: 'وقت الفتح',
  closingTime: 'وقت الإغلاق',
  latitude: 'خط العرض',
  longitude: 'خط الطول',

  excessInventoryManagement: 'إدارة المخزون الفائض',
  dailySurplusForecast: 'توقعات الفائض اليومي',
  basedOnPOS: 'بناء على نقاط البيع (POS)',
  productName: 'اسم المنتج',
  currentStock: 'المخزون الحالي',
  expectedSurplus: 'الفائض المتوقع',
  expirationTime: 'وقت الإنتهاء',
  suggestedAction: 'الإجراء المقترح',
  addToSurplus: 'إضافة للفائض',
  smartPricingEngine: 'محرك التسعير الذكي',
  pricingRecommendations: 'توصيات أسعار بناءً على الإنتهاء والطلب',
  originalPrice: 'السعر الأصلي',
  suggestedPrice: 'السعر المقترح',
  applySuggestedPrice: 'تطبيق السعر المقترح',
  bestTimeToPublish: 'أفضل وقت للنشر',
  smartTip: 'نصيحة ذكية',

  orderCategories: 'أقسام الطلبات',
  new: 'جديد',
  confirmed: 'مؤكد',
  delivered: 'مسلم',
  searchOrder: 'ابحث عن طلب',
  orderNumber: 'رقم الطلب',
  customerName: 'اسم العميل',
  viewDetails: 'عرض التفاصيل',
  confirmOrder: 'تأكيد الطلب',
  rejectOrder: 'رفض الطلب مع ذكر السبب',
  rejectionReason: 'سبب رفض الطلب',
  confirmDelivery: 'تأكيد التسليم',
  newOrderNeedsConfirmation: 'جديد - يحتاج تأكيد',
  deliveryInProgress: 'موعد جاري التوصيل',
  orderDelivered: 'تم التسليم',
  receiptTime: 'وقت الإستلام',
  paymentMethod: 'طريقة الدفع',
  productList: 'قائمة المنتجات',
  quantity: 'الكمية',
  totalAmount: 'المجموع الكلي',

  searchProduct: 'ابحث عن منتج',
  discountPercentage: 'نسبة الخصم',
  productStatus: 'حالة المنتج',
  dateAdded: 'تاريخ الإضافة',
  expiryDate: 'تاريخ الإنتهاء',
  productCode: 'كود المنتج',
  vendorName: 'اسم التاجر',
  actions: 'الإجرائات',
  expired: 'منتهي',
  sold: 'مباع',
  blocked: 'محظور',
  loadingProducts: 'جاري تحميل المنتجات...',
  noProducts: 'لا توجد منتجات',
  noProductsDescription: 'لا توجد منتجات للعرض في الوقت الحالي. ابدأ بإضافة منتج جديد.',
  table: 'جدول',
  of: 'من',
  sessionExpired: 'انتهت صلاحية الجلسة. يرجى تسجيل الدخول مرة أخرى',
  connectionFailed: 'فشل الاتصال بالخادم. يرجى التحقق من الاتصال بالإنترنت',
  loadProductsError: 'حدث خطأ أثناء تحميل المنتجات',
  loadProductsFailed: 'فشل تحميل المنتجات',
  noAccess: 'غير مصرح لك بالوصول. يرجى تسجيل الدخول مرة أخرى',
  addNewExcessProduct: 'إضافة منتج فائض جديد',
  productPrice: 'سعر المنتج',
  views: 'المشاهدات',
  salesPercentage: 'نسبة المبيعات',
  lowStock: 'مخزون منخفض',
  copy: 'نسخ',

  merchant: 'تاجر',
  suspended: 'موقوف',
  pending: 'في الإنتظار',
  merchantsList: 'قائمة التجار',
  searchMerchant: 'ابحث عن تاجر',
  geographicRegion: 'المنطقة الجغرافية',
  salesVolume: 'حجم المبيعات',
  activityType: 'نوع النشاط',
  region: 'المنطقة',
  ordersCount: 'عدد الطلبات',
  rating: 'التقييم',
  loadingMerchants: 'جاري تحميل التجار...',
  retry: 'إعادة المحاولة',
  noMerchantsAvailable: 'لا توجد تجار متاحين',
  addNewMerchant: 'إضافة تاجر جديد',
  merchantNameEnLabel: 'الاسم بالإنجليزية',
  merchantDescription: 'الوصف',
  merchantDescriptionEn: 'الوصف بالإنجليزية',
  merchantStory: 'قصة المتجر',
  merchantStoryEn: 'قصة المتجر (إنجليزي)',
  category: 'الفئة',
  loadingCategories: 'جارى تحميل الفئات',
  selectCategory: 'اختار الفئة',
  username: 'اسم المستخدم',
  phoneNumber: 'رقم الجوال',
  password: 'كلمة المرور',
  merchantStatus: 'الحالة',
  selectArea: 'اختر المنطقة',
  merchantCreateError: 'تعذر إنشاء التاجر',
  merchantImage: 'صورة التاجر',
  merchantChooseImage: 'اختر صورة',
  merchantRemoveImage: 'إزالة الصورة',
  merchantInvalidImageType: 'يرجى اختيار ملف صورة (مثل PNG أو JPEG)',
  merchantImageDropHint: 'اسحب الصورة هنا أو انقر للاختيار',
  merchantImageFormatsHint: 'PNG، JPEG، WebP، GIF — يُفضّل أقل من 5 ميجابايت',
  merchantReplaceImage: 'استبدال الصورة',
  merchantName: 'اسم التاجر',
  riyal: 'ريال',
  deactivateMerchantAccount: 'إيقاف حساب التاجر',
  activateMerchant: 'تفعيل',
  contact: 'التواصل',
  merchantDetails: 'تفاصيل التاجر',
  storeStorySection: 'قصة المتجر ورسالة الإستدامة',
  merchantData: 'بيانات التاجر',
  viewLocation: 'عرض الموقع',
  contactNumber: 'رقم التواصل',
  customerReviews: 'مراجعات العملاء',
  deleteReview: 'حذف المراجعة',
  confirmDeactivateTitle: 'تأكيد إيقاف حساب التاجر',
  deactivateModalDescription: 'برجاء اختيار سبب الإيقاف سيؤدي هذا الإجراء إلى منع التاجر من استقبال طلبات جديدة أو الوصول إلى لوحة التحكم.',
  deactivateReasonPlaceholder: 'سبب إيقاف حساب التاجر',
  confirmDeactivate: 'تأكيد الإيقاف',
  deactivateReason1: 'انتهاك شروط الاستخدام',
  deactivateReason2: 'شكاوى متعددة من العملاء',
  deactivateReason3: 'عدم الالتزام بجودة الخدمة',
  deactivateReason4: 'مشاكل في الدفع',
  deactivateReason5: 'سبب آخر',
  salesVolumeLow: 'منخفض',
  salesVolumeMedium: 'متوسط',
  salesVolumeHigh: 'عالي',
  loadMerchantsError: 'حدث خطأ أثناء تحميل التجار',
  loadMerchantsFailed: 'فشل تحميل التجار',

  activeCustomersCount: 'عدد العملاء النشطين',
  customerUnit: 'عميل',
  ofThem: 'منهم',
  thisWeek: 'هذا الأسبوع',
  thisMonth: 'هذا الشهر',
  registeredMerchantsCount: 'عدد التجار المسجلين',
  monthlyGrowthRate: 'نسبة النمو الشهري',
  comparedToPreviousMonth: 'مقارنة بالشهر السابق',
  wasteSavedAmount: 'كمية الهدر الموفرة',
  kg: 'كجم',
  activeOrdersCount: 'عدد الطلبات النشطة',
  averageDailyOrders: 'متوسط عدد الطلبات يوميا',
  orderUnit: 'طلب',
  sinceHour: 'منذ ساعة',
  totalSales: 'إجمالي المبيعات',
  daily: 'يومي',
  weekly: 'أسبوعي',
  monthly: 'شهري',
  percentFromLastWeek: 'عن الأسبوع السابق',
  regionActivityMap: 'خريطة نشاط المناطق',
  mapApiKeyMessage: 'يرجى إضافة مفتاح Google Maps API',
  mapApiKeySub: 'Please add your Google Maps API key in index.html',
  activityVeryHigh: 'نشاط مرتفع جدا',
  activityMedium: 'نشاط متوسط',
  activityLow: 'نشاط منخفض',
  latestOrdersAndActivities: 'أحدث الطلبات والأنشطة',
  value: 'القيمة',
  time: 'الوقت',
  delivery: 'قيد التوصيل',
  completed: 'مكتمل',
  occupancyRate: 'معدل الاشغال',
  hourlySalesRate: 'معدل المبيعات في الساعة',
  weeklySales: 'المبيعات الأسبوعية',
  date: 'التاريخ',
  wednesday: 'يوم الأربعاء',
  productsSoldCount: 'عدد المنتجات المباعة',
  productUnit: 'منتج',
  productsRemainingCount: 'عدد المنتجات المتبقية',
  savedWeightAmount: 'كمية الهدر الموفر',
  kilogram: 'كيلوجرام',
  soldPiecesNote: 'تم بيع {qty} قطعة',
  egp: 'ج.م',

  all: 'الكل',
  allOrders: 'جميع الطلبات',
  searchOrdersPlaceholder: 'ابحث برقم الطلب أو التاجر أو حالة الطلب',
  orderStatus: 'حالة الطلب',
  orderStatusReceived: 'مستلم',
  orderStatusScheduled: 'موعد',
  orderStatusPending: 'معلق',
  orderStatusCancelled: 'ملغي',
  totalValue: 'القيمة الإجمالية',
  orderCreationTime: 'وقت إنشاء الطلب',
  loadingOrders: 'جاري تحميل الطلبات...',
  noOrders: 'لا توجد طلبات',
  noOrdersToDisplay: 'لا توجد طلبات للعرض',
  noOrdersInSection: 'لا توجد طلبات في هذا القسم حالياً',
  ordersEmptyHint: 'لم يتم العثور على بيانات مطابقة لبحثك أو عوامل التصفية.',
  clearFilters: 'مسح التصفية',
  copyOrderNumberAction: 'نسخ رقم الطلب',
  refresh: 'تحديث',
  orderDetailsFor: 'تفاصيل طلب رقم',
  orderReadyAwaitingCustomer: 'الطلب جاهز وفي انتظار العميل',
  oneOrder: 'طلب واحد',
  orderDetails: 'تفاصيل الطلب',
  customerData: 'بيانات العميل',
  paymentSummary: 'ملخص الدفع',
  email: 'البريد الإلكتروني',
  phone: 'رقم الجوال',
  price: 'السعر',
  discount: 'الخصم',
  fees: 'الرسوم',
  finalTotal: 'المجموع النهائي',
  products: 'المنتجات',
  rejectReasonTitle: 'سبب رفض الطلب رقم',
  selectRejectReason: 'أختر سبب رفض الطلب من القائمة',
  confirmRejection: 'تأكيد الرفض',
  orderNumberLabel: 'رقم الطلب',
  rejectReason1: 'المخزون غير كافي',
  rejectReason2: 'المنتج غير متوفر',
  rejectReason3: 'طلب غير صالح',
  rejectReason4: 'معلومات العميل غير كاملة',
  rejectReason5: 'سبب آخر',
  loadOrdersError: 'حدث خطأ أثناء تحميل الطلبات',

  pendingPayments: 'المدفوعات المعلقة',
  totalCommissions: 'إجمالي العمولات',
  averageOrderValue: 'متوسط قيمة الطلب',
  totalRevenues: 'إجمالي الإيرادات',
  financialAnalytics: 'التحليلات المالية',
  monthOctober: 'شهر اكتوبر',
  monthNovember: 'شهر نوفمبر',
  monthDecember: 'شهر ديسمبر',
  exportReportPdf: 'تصدير التقرير pdf',
  exportReportExcel: 'تصدير التقرير excel',
  revenueByMerchantType: 'الإيرادات حسب نوع التاجر',
  revenueByRegion: 'الإيرادات حسب المنطقة',
  merchantTypeStores: 'متاجر',
  merchantTypeRestaurants: 'مطاعم',
  merchantTypeSupermarkets: 'سوبر ماركت',
  merchantTypeCafes: 'مقاهي',
  merchantTypeOthers: 'أخرى',
  regionRiyadh: 'الرياض',
  regionEastern: 'الشرقية',
  regionMadinah: 'المدينة المنورة',
  regionJeddah: 'جدة',
  regionTabuk: 'تبوك',
  withdrawalRequests: 'طلبات السحب',
  withdrawStatusPending: 'قيد الإنتظار',
  withdrawStatusCompleted: 'مكتمل',
  withdrawStatusRejected: 'مرفوض',
  approve: 'موافقة',
  reject: 'رفض',

  currentBalance: 'الرصيد الحالي',
  netProfits: 'صافي الأرباح',
  beforeCommission: 'قبل العمولة',
  commissionPercentage: '%10 من المبيعات',
  withdrawProfits: 'سحب الأرباح',
  withdrawProfitsDescription: 'يمكنك سحب أرباحك إلى حسابك البنكي',
  transactionLog: 'سجل المعاملات',
  transactionNumber: 'رقم المعاملة',
  product: 'المنتج',
  commission: 'العمولة',
  net: 'الصافي',
  financialTip: 'نصيحة مالية',
  financialTipText: 'قم بسحب أرباحك بشكل دوري لتحسين إدارة التدفق النقدي لمتجرك.',
  performanceAnalysis: 'تحليل الأداء',
  performanceAnalysisText: 'أرباحك زادت بنسبة 15% مقارنة بالأسبوع الماضي - استمر بالعمل الرائع!',

  commissionSettings: 'إعدادات العمولة',
  commissionTypeRatio: 'نوع النسبة',
  commissionTypeFixed: 'ثابتة',
  commissionTypeVariable: 'متغيرة حسب نوع التاجر',
  fixedCommissionRate: 'نسبة العمولة الثابتة (%)',
  bakeriesCommission: 'عمولة المخابز (%)',
  restaurantsCommission: 'عمولة المطاعم (%)',
  storesCommission: 'عمولة المتاجر (%)',
  saveCommissionSettings: 'حفظ إعدادات العمولة',
  availablePaymentMethods: 'طرق الدفع المتاحة',
  paymentCreditCards: 'بطاقات الائتمان (Visa | Master Card)',
  paymentApplePay: 'Apple Pay (Apple)',
  paymentStcPay: 'STC Pay (STC)',
  paymentMada: 'مدى (Saudi Payment)',
  paymentCashOnDelivery: 'الدفع عند الاستلام (نقداً)',
  enabled: 'مفعل',
  disabled: 'معطل',
  userPermissions: 'صلاحيات المستخدمين',
  addNewUser: 'إضافة مستخدم جديد',
  jobRole: 'الدور الوظيفي',
  geographicRegionManagement: 'إدارة المناطق الجغرافية',
  addNewRegion: 'إضافة منطقة جديدة',
  deliveryFees: 'رسوم التوصيل',
  name: 'الإسم',
  status: 'الحالة',
  edit: 'تعديل',
  editRegion: 'تعديل منطقة',
  regionNameLabel: 'المنطقة:',
  regionNameEnglishLabel: 'المنطقة (English):',
  currentRegions: 'المناطق الحالية',
  saveRegionChanges: 'حفظ التعديلات',
  addRegionWithPlus: '+ إضافة',

  today: 'اليوم',
  moreThan7Days: 'أكثر من 7 أيام',
  filter: 'تصفية',
  loading: 'جاري التحميل...',
  error: 'خطأ',
  success: 'نجح',
  close: 'إغلاق',
  yes: 'نعم',
  no: 'لا',
  ok: 'موافق',
  notificationCenter: 'مركز الإشعارات',
  newNotifications: 'إشعارات جديدة',
  markAllAsRead: 'تحديد الكل كمقروء',
  loadingNotifications: 'جاري تحميل الإشعارات...',
  noNotifications: 'لا توجد إشعارات',
  noNotificationsDescription: 'لا توجد إشعارات للعرض حالياً. سيتم إشعارك عند وجود تحديثات جديدة.',
  notificationsTimeoutError: 'انتهت مهلة الطلب. يرجى التحقق من الاتصال بالإنترنت وإعادة المحاولة',
  notificationsLoadError: 'حدث خطأ أثناء تحميل الإشعارات',
  unknownTimeAgo: 'منذ وقت غير محدد',
  notification: 'إشعار',
  now: 'الآن',
  ago: 'منذ',
  minute: 'دقيقة',
  minutes: 'دقائق',
  hour: 'ساعة',
  hours: 'ساعات',
  day: 'يوم',
  days: 'أيام'
};

const ENGLISH_TRANSLATIONS: Translations = {
  dashboard: 'Dashboard',
  branchManagement: 'Branch Management',
  inventoryManagement: 'Excess Inventory Management',
  productsManagement: 'Products Management',
  ordersAndSales: 'Orders and Sales',
  ordersManagement: 'Orders Management',
  financialManagement: 'Financial Management',
  statisticsAndPerformance: 'Statistics and Performance',
  statisticsAndAnalytics: 'Statistics and Analytics',
  storeManagement: 'Store Management',
  notificationsAndCommunication: 'Notifications and Communication',
  contentAndNotificationsManagement: 'Content and Notifications Management',
  supportAndHelp: 'Support and Help',
  merchantsControlPanel: 'Merchants Control Panel',
  branchManagerDashboard: 'Branch Manager Dashboard',
  liveOrders: 'Live Orders',
  orderHistory: 'Order History',
  excessProductsManagement: 'Excess Products Management',
  listOfOrders: 'List of Orders',
  searchByOrderNumber: 'Search by order number',
  searchHere: 'Search here...',
  sortByOrder: 'Sort by Order',
  store: 'Store',
  page: 'Page',
  scanQR: 'Scan QR',
  scanQRForCustomer: 'Scan QR code for customer',
  clickToStartScanning: 'Click to start scanning',
  activateCamera: 'Activate Camera',
  or: 'Or',
  enterQRCodeManually: 'Enter QR code manually',
  enterQRCodePlaceholder: 'Enter QR code',
  confirm: 'Confirm',
  outForDelivery: 'Out for Delivery',
  awaitingCustomer: 'Awaiting Customer',
  notPaid: 'Not Paid',
  paid: 'Paid',
  pickupFromBranch: 'Pickup from Branch',
  deliveryMethodDelivery: 'Delivery',
  deliveryType: 'Delivery Type',
  pickup: 'Pickup',
  mobileNumber: 'Mobile Number',
  merchantManagement: 'Merchant Management',
  systemSettings: 'System Settings',
  currentBranch: 'Current Branch: Gulf Branch',
  logout: 'Logout',
  language: 'Language',

  addNewBranch: 'Add New Branch',
  branchDetails: 'Branch Details',
  branchName: 'Branch Name',
  branchNameEn: 'Branch Name (English)',
  location: 'Location',
  neighborhood: 'Neighborhood',
  neighborhoodEn: 'Neighborhood (English)',
  area: 'Area',
  operatingHours: 'Operating Hours',
  revenue: 'Revenue',
  currentSurplus: 'Current Surplus',
  active: 'Active',
  inactive: 'Inactive',
  delete: 'Delete',
  save: 'Save',
  cancel: 'Cancel',
  noBranches: 'No branches available',
  getCurrentLocation: 'Get Current Location',
  openingTime: 'Opening Time',
  closingTime: 'Closing Time',
  latitude: 'Latitude',
  longitude: 'Longitude',

  excessInventoryManagement: 'Excess Inventory Management',
  dailySurplusForecast: 'Daily Surplus Forecast',
  basedOnPOS: 'Based on Point of Sale (POS)',
  productName: 'Product Name',
  currentStock: 'Current Stock',
  expectedSurplus: 'Expected Surplus',
  expirationTime: 'Expiration Time',
  suggestedAction: 'Suggested Action',
  addToSurplus: 'Add to Surplus',
  smartPricingEngine: 'Smart Pricing Engine',
  pricingRecommendations: 'Pricing recommendations based on expiration and demand',
  originalPrice: 'Original Price',
  suggestedPrice: 'Suggested Price',
  applySuggestedPrice: 'Apply Suggested Price',
  bestTimeToPublish: 'Best Time to Publish',
  smartTip: 'Smart Tip',

  orderCategories: 'Order Categories',
  new: 'New',
  confirmed: 'Confirmed',
  delivered: 'Delivered',
  searchOrder: 'Search for order',
  orderNumber: 'Order Number',
  customerName: 'Customer Name',
  viewDetails: 'View Details',
  confirmOrder: 'Confirm Order',
  rejectOrder: 'Reject Order with Reason',
  rejectionReason: 'Rejection Reason',
  confirmDelivery: 'Confirm Delivery',
  newOrderNeedsConfirmation: 'New - Needs Confirmation',
  deliveryInProgress: 'Delivery in Progress',
  orderDelivered: 'Order Delivered',
  receiptTime: 'Receipt Time',
  paymentMethod: 'Payment Method',
  productList: 'Product List',
  quantity: 'Quantity',
  totalAmount: 'Total Amount',

  searchProduct: 'Search for product',
  discountPercentage: 'Discount Percentage',
  productStatus: 'Product Status',
  dateAdded: 'Date Added',
  expiryDate: 'Expiry Date',
  productCode: 'Product Code',
  vendorName: 'Vendor Name',
  actions: 'Actions',
  expired: 'Expired',
  sold: 'Sold',
  blocked: 'Blocked',
  loadingProducts: 'Loading products...',
  noProducts: 'No products',
  noProductsDescription: 'No products to display at the moment. Start by adding a new product.',
  table: 'Table',
  of: 'of',
  sessionExpired: 'Session expired. Please log in again.',
  connectionFailed: 'Connection failed. Please check your internet connection.',
  loadProductsError: 'An error occurred while loading products.',
  loadProductsFailed: 'Failed to load products.',
  noAccess: 'You are not authorized. Please log in again.',
  addNewExcessProduct: 'Add New Excess Product',
  productPrice: 'Product Price',
  views: 'Views',
  salesPercentage: 'Sales Percentage',
  lowStock: 'Low Stock',
  copy: 'Copy',

  merchant: 'merchant',
  suspended: 'Suspended',
  pending: 'Pending',
  merchantsList: 'Merchants List',
  searchMerchant: 'Search for merchant',
  geographicRegion: 'Geographic Region',
  salesVolume: 'Sales Volume',
  activityType: 'Activity Type',
  region: 'Region',
  ordersCount: 'Number of Orders',
  rating: 'Rating',
  loadingMerchants: 'Loading merchants...',
  retry: 'Retry',
  noMerchantsAvailable: 'No merchants available',
  addNewMerchant: 'Add New Merchant',
  merchantNameEnLabel: 'Name (English)',
  merchantDescription: 'Description',
  merchantDescriptionEn: 'Description (English)',
  merchantStory: 'Store story',
  merchantStoryEn: 'Store story (English)',
  category: 'Category',
  selectCategory: 'Select Category',
  loadingCategories: 'Loading categories...',
  username: 'Username',
  phoneNumber: 'Phone number',
  password: 'Password',
  merchantStatus: 'Status',
  selectArea: 'Select area',
  merchantCreateError: 'Could not create merchant',
  merchantImage: 'Merchant image',
  merchantChooseImage: 'Choose image',
  merchantRemoveImage: 'Remove image',
  merchantInvalidImageType: 'Please choose an image file (e.g. PNG or JPEG)',
  merchantImageDropHint: 'Drop an image here or click to browse',
  merchantImageFormatsHint: 'PNG, JPEG, WebP, GIF — under 5 MB recommended',
  merchantReplaceImage: 'Replace image',
  merchantName: 'Merchant Name',
  riyal: 'SAR',
  deactivateMerchantAccount: 'Deactivate merchant account',
  activateMerchant: 'Activate',
  contact: 'Contact',
  merchantDetails: 'Merchant Details',
  storeStorySection: 'Store story and sustainability message',
  merchantData: 'Merchant Data',
  viewLocation: 'View Location',
  contactNumber: 'Contact Number',
  customerReviews: 'Customer Reviews',
  deleteReview: 'Delete review',
  confirmDeactivateTitle: 'Confirm deactivate merchant account',
  deactivateModalDescription: 'Please select a reason for deactivation. This action will prevent the merchant from receiving new orders or accessing the control panel.',
  deactivateReasonPlaceholder: 'Reason for deactivation',
  confirmDeactivate: 'Confirm deactivation',
  deactivateReason1: 'Terms of use violation',
  deactivateReason2: 'Multiple customer complaints',
  deactivateReason3: 'Failure to meet service quality',
  deactivateReason4: 'Payment issues',
  deactivateReason5: 'Other reason',
  salesVolumeLow: 'Low',
  salesVolumeMedium: 'Medium',
  salesVolumeHigh: 'High',
  loadMerchantsError: 'An error occurred while loading merchants.',
  loadMerchantsFailed: 'Failed to load merchants.',

  activeCustomersCount: 'Active Customers',
  customerUnit: 'customer',
  ofThem: 'of them',
  thisWeek: 'this week',
  thisMonth: 'this month',
  registeredMerchantsCount: 'Registered Merchants',
  monthlyGrowthRate: 'Monthly Growth Rate',
  comparedToPreviousMonth: 'Compared to previous month',
  wasteSavedAmount: 'Saved Waste Amount',
  kg: 'kg',
  activeOrdersCount: 'Active Orders Count',
  averageDailyOrders: 'Average daily order count',
  orderUnit: 'order',
  sinceHour: 'since hour',
  totalSales: 'Total Sales',
  daily: 'Daily',
  weekly: 'Weekly',
  monthly: 'Monthly',
  percentFromLastWeek: 'from last week',
  regionActivityMap: 'Region Activity Map',
  mapApiKeyMessage: 'Please add your Google Maps API key',
  mapApiKeySub: 'Please add your Google Maps API key in index.html',
  activityVeryHigh: 'Very high activity',
  activityMedium: 'Medium activity',
  activityLow: 'Low activity',
  latestOrdersAndActivities: 'Latest Orders and Activities',
  value: 'Value',
  time: 'Time',
  delivery: 'Delivery in progress',
  completed: 'Completed',
  occupancyRate: 'Occupancy Rate',
  hourlySalesRate: 'Hourly sales rate',
  weeklySales: 'Weekly sales',
  date: 'Date',
  wednesday: 'Wednesday',
  productsSoldCount: 'Products sold',
  productUnit: 'product',
  productsRemainingCount: 'Products remaining',
  savedWeightAmount: 'Saved weight amount',
  kilogram: 'kilogram',
  soldPiecesNote: '{qty} marked pieces sold',
  egp: 'EGP',

  all: 'All',
  allOrders: 'All Orders',
  searchOrdersPlaceholder: 'Search by order number or merchant or order status',
  orderStatus: 'Order Status',
  orderStatusReceived: 'Received',
  orderStatusScheduled: 'Scheduled',
  orderStatusPending: 'Pending',
  orderStatusCancelled: 'Cancelled',
  totalValue: 'Total Value',
  orderCreationTime: 'Order creation time',
  loadingOrders: 'Loading orders...',
  noOrders: 'No orders',
  noOrdersToDisplay: 'No orders to display',
  noOrdersInSection: 'No orders in this section at the moment',
  ordersEmptyHint: 'No orders match your search or filters.',
  clearFilters: 'Clear filters',
  copyOrderNumberAction: 'Copy order number',
  refresh: 'Refresh',
  orderDetailsFor: 'Order details #',
  orderReadyAwaitingCustomer: 'Order ready and waiting for customer',
  oneOrder: 'One order',
  orderDetails: 'Order details',
  customerData: 'Customer data',
  paymentSummary: 'Payment summary',
  email: 'Email',
  phone: 'Phone number',
  price: 'Price',
  discount: 'Discount',
  fees: 'Fees',
  finalTotal: 'Final total',
  products: 'Products',
  rejectReasonTitle: 'Rejection reason for order #',
  selectRejectReason: 'Select a rejection reason from the list',
  confirmRejection: 'Confirm rejection',
  orderNumberLabel: 'Order number',
  rejectReason1: 'Insufficient stock',
  rejectReason2: 'Product unavailable',
  rejectReason3: 'Invalid order',
  rejectReason4: 'Incomplete customer information',
  rejectReason5: 'Other reason',
  loadOrdersError: 'An error occurred while loading orders.',

  pendingPayments: 'Pending Payments',
  totalCommissions: 'Total Commissions',
  averageOrderValue: 'Average Order Value',
  totalRevenues: 'Total Revenues',
  financialAnalytics: 'Financial Analytics',
  monthOctober: 'October',
  monthNovember: 'November',
  monthDecember: 'December',
  exportReportPdf: 'Export Report PDF',
  exportReportExcel: 'Export Report Excel',
  revenueByMerchantType: 'Revenue by Merchant Type',
  revenueByRegion: 'Revenue by Region',
  merchantTypeStores: 'Stores',
  merchantTypeRestaurants: 'Restaurants',
  merchantTypeSupermarkets: 'Supermarkets',
  merchantTypeCafes: 'Cafes',
  merchantTypeOthers: 'Others',
  regionRiyadh: 'Riyadh',
  regionEastern: 'Eastern',
  regionMadinah: 'Madinah',
  regionJeddah: 'Jeddah',
  regionTabuk: 'Tabuk',
  withdrawalRequests: 'Withdrawal Requests',
  withdrawStatusPending: 'Pending',
  withdrawStatusCompleted: 'Completed',
  withdrawStatusRejected: 'Rejected',
  approve: 'Approve',
  reject: 'Reject',

  currentBalance: 'Current Balance',
  netProfits: 'Net Profits',
  beforeCommission: 'Before Commission',
  commissionPercentage: '10% of Sales',
  withdrawProfits: 'Withdraw Profits',
  withdrawProfitsDescription: 'You can withdraw your profits to your bank account',
  transactionLog: 'Transaction Log',
  transactionNumber: 'Transaction Number',
  product: 'Product',
  commission: 'Commission',
  net: 'Net',
  financialTip: 'Financial Tip',
  financialTipText: 'Withdraw your profits periodically to improve your store\'s cash flow management.',
  performanceAnalysis: 'Performance Analysis',
  performanceAnalysisText: 'Your profits increased by 15% compared to last week - keep up the great work!',

  commissionSettings: 'Commission Settings',
  commissionTypeRatio: 'Ratio Type',
  commissionTypeFixed: 'Fixed',
  commissionTypeVariable: 'Variable based on merchant type',
  fixedCommissionRate: 'Fixed Commission Rate (%)',
  bakeriesCommission: 'Bakeries Commission (%)',
  restaurantsCommission: 'Restaurants Commission (%)',
  storesCommission: 'Stores Commission (%)',
  saveCommissionSettings: 'Save Commission Settings',
  availablePaymentMethods: 'Available Payment Methods',
  paymentCreditCards: 'Credit Cards (Visa | Master Card)',
  paymentApplePay: 'Apple Pay (Apple)',
  paymentStcPay: 'STC Pay (STC)',
  paymentMada: 'Mada (Saudi Payment)',
  paymentCashOnDelivery: 'Cash on Delivery',
  enabled: 'Enabled',
  disabled: 'Disabled',
  userPermissions: 'User Permissions',
  addNewUser: 'Add New User',
  jobRole: 'Job Role',
  geographicRegionManagement: 'Geographic Region Management',
  addNewRegion: 'Add New Region',
  deliveryFees: 'Delivery Fees',
  name: 'Name',
  status: 'Status',
  edit: 'Edit',
  editRegion: 'Edit Region',
  regionNameLabel: 'Region:',
  regionNameEnglishLabel: 'Region (English):',
  currentRegions: 'Current Regions',
  saveRegionChanges: 'Save changes',
  addRegionWithPlus: '+ Add',

  today: 'Today',
  moreThan7Days: 'More than 7 days',
  filter: 'Filter',
  loading: 'Loading...',
  error: 'Error',
  success: 'Success',
  close: 'Close',
  yes: 'Yes',
  no: 'No',
  ok: 'OK',
  notificationCenter: 'Notification Center',
  newNotifications: 'new notifications',
  markAllAsRead: 'Mark all as read',
  loadingNotifications: 'Loading notifications...',
  noNotifications: 'No notifications',
  noNotificationsDescription: 'There are no notifications to display right now. You will be notified when new updates arrive.',
  notificationsTimeoutError: 'Request timeout. Please check your internet connection and try again.',
  notificationsLoadError: 'An error occurred while loading notifications.',
  unknownTimeAgo: 'Unknown time ago',
  notification: 'Notification',
  now: 'Now',
  ago: 'ago',
  minute: 'minute',
  minutes: 'minutes',
  hour: 'hour',
  hours: 'hours',
  day: 'day',
  days: 'days'
};

@Injectable({
  providedIn: 'root'
})
export class TranslationsService {
  private languageService = inject(LanguageService);

  /**
   * Get all translations for current language
   */
  getTranslations(): Observable<Translations> {
    return this.languageService.getCurrentLanguage().pipe(
      map(lang => lang === 'ar' ? ARABIC_TRANSLATIONS : ENGLISH_TRANSLATIONS)
    );
  }

  /**
   * Get translations synchronously
   */
  getTranslationsSync(): Translations {
    const lang = this.languageService.getCurrentLanguageValue();
    return lang === 'ar' ? ARABIC_TRANSLATIONS : ENGLISH_TRANSLATIONS;
  }

  /**
   * Get a specific translation key
   */
  get(key: keyof Translations): Observable<string> {
    return this.getTranslations().pipe(
      map(translations => translations[key])
    );
  }

  /**
   * Get a specific translation key synchronously
   */
  getSync(key: keyof Translations): string {
    const translations = this.getTranslationsSync();
    return translations[key];
  }

  soldPiecesNote(qty: number): string {
  return this.getSync('soldPiecesNote').replace('{qty}', qty.toString());
}
}
