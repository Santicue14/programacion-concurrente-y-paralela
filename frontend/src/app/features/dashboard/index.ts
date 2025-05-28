// Dashboard Components
export { DashboardOverviewComponent } from './components/dashboard-overview/dashboard-overview.component';
export { StatsCardsComponent } from './components/stats-cards/stats-cards.component';
export { SalesAnalyticsComponent } from './components/sales-analytics/sales-analytics.component';

// Dashboard Services
export { DashboardAnalyticsService } from './services/dashboard-analytics.service';

// Dashboard Interfaces
export type {
  DashboardStats,
  SalesAnalytics,
  DailySales,
  MonthlySales,
  BrandSales,
  TopVehicle,
  SalesTrend,
  RecentSale,
  AnalyticsFilters
} from './services/dashboard-analytics.service'; 