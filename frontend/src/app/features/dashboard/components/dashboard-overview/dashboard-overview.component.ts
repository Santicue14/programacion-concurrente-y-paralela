import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { DashboardAnalyticsService, DashboardStats, SalesAnalytics, RecentSale, AnalyticsFilters } from '../../services/dashboard-analytics.service';
import { StatsCardsComponent } from '../stats-cards/stats-cards.component';
import { SalesAnalyticsComponent } from '../sales-analytics/sales-analytics.component';

@Component({
  selector: 'app-dashboard-overview',
  standalone: true,
  imports: [CommonModule, FormsModule, StatsCardsComponent, SalesAnalyticsComponent],
  templateUrl: './dashboard-overview.component.html',
  styleUrls: ['./dashboard-overview.component.css']
})
export class DashboardOverviewComponent implements OnInit {
  // Data properties
  dashboardStats: DashboardStats | null = null;
  salesAnalytics: SalesAnalytics | null = null;
  recentSales: RecentSale[] = [];
  
  // Loading states
  statsLoading: boolean = true;
  analyticsLoading: boolean = true;
  recentSalesLoading: boolean = true;

  // Filter properties
  filters: AnalyticsFilters = {
    period: 'monthly'
  };
  
  // Filter options
  periodOptions = [
    { value: 'daily', label: 'Diario' },
    { value: 'weekly', label: 'Semanal' },
    { value: 'monthly', label: 'Mensual' },
    { value: 'yearly', label: 'Anual' }
  ];

  vehicleBrands: string[] = ['Toyota', 'Honda', 'Ford', 'Chevrolet', 'Nissan'];

  constructor(
    private dashboardService: DashboardAnalyticsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.loadStats();
    this.loadAnalytics();
    this.loadRecentSales();
  }

  loadStats(): void {
    this.statsLoading = true;
    this.dashboardService.getDashboardStats(this.filters).subscribe({
      next: (stats) => {
        this.dashboardStats = stats;
        this.statsLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard stats:', error);
        this.statsLoading = false;
      }
    });
  }

  loadAnalytics(): void {
    this.analyticsLoading = true;
    this.dashboardService.getSalesAnalytics(this.filters).subscribe({
      next: (analytics) => {
        this.salesAnalytics = analytics;
        this.analyticsLoading = false;
      },
      error: (error) => {
        console.error('Error loading sales analytics:', error);
        this.analyticsLoading = false;
      }
    });
  }

  loadRecentSales(): void {
    this.recentSalesLoading = true;
    this.dashboardService.getRecentSales(10).subscribe({
      next: (sales) => {
        this.recentSales = sales;
        this.recentSalesLoading = false;
      },
      error: (error) => {
        console.error('Error loading recent sales:', error);
        this.recentSalesLoading = false;
      }
    });
  }

  onFiltersChange(): void {
    this.loadDashboardData();
  }

  onPeriodChange(): void {
    this.onFiltersChange();
  }

  onDateRangeChange(): void {
    this.onFiltersChange();
  }

  onBrandChange(): void {
    this.onFiltersChange();
  }

  clearFilters(): void {
    this.filters = {
      period: 'monthly'
    };
    this.loadDashboardData();
  }

  // Navigation methods
  navigateToSales(): void {
    this.router.navigate(['/sales']);
  }

  navigateToClients(): void {
    this.router.navigate(['/clients']);
  }

  navigateToVehicles(): void {
    this.router.navigate(['/vehicles']);
  }

  navigateToCreateSale(): void {
    this.router.navigate(['/sales/create']);
  }

  // Utility methods
  get isLoading(): boolean {
    return this.statsLoading || this.analyticsLoading || this.recentSalesLoading;
  }

  get hasDateRange(): boolean {
    return !!(this.filters.startDate && this.filters.endDate);
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('es-ES');
  }

  refreshData(): void {
    this.loadDashboardData();
  }
} 