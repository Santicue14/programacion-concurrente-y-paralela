import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';

import { 
  DashboardAnalyticsService, 
  DashboardStats, 
  SalesAnalytics, 
  RecentSale, 
  AnalyticsFilters,
} from '../../services/dashboard-analytics.service';
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
  dashboardStats: DashboardStats = {
    totalSales: null,
    totalRevenue: null,
    totalClients: null,
    totalVehicles: null,
    salesThisMonth: null,
    revenueThisMonth: null,
  };
  
  salesAnalytics: SalesAnalytics = {
    dailySales: [],
    monthlySales: [],
    salesByVehicleBrand: [],
    topSellingVehicles: [],
    salesTrend: { period: '', growth: 0, trend: 'stable' }
  };
  
  recentSales: RecentSale[] = [];
  
  // Individual loading states for dashboard stats
  totalSalesLoading: boolean = true;
  totalRevenueLoading: boolean = true;
  totalClientsLoading: boolean = true;
  totalVehiclesLoading: boolean = true;
  salesThisMonthLoading: boolean = true;
  revenueThisMonthLoading: boolean = true;

  // Individual loading states for analytics
  dailySalesLoading: boolean = true;
  monthlySalesLoading: boolean = true;
  salesByBrandLoading: boolean = true;
  topVehiclesLoading: boolean = true;
  salesTrendLoading: boolean = true;
  recentSalesLoading: boolean = true;

  isLoading: boolean = true;
  
  constructor(
    private dashboardService: DashboardAnalyticsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.loadDashboardStats();
    this.loadAnalyticsData();
    this.loadRecentSales();
  }

  loadDashboardStats(): void {
    // Total Sales
    this.isLoading = true;
    this.totalSalesLoading = true;
    this.dashboardService.getTotalSales().subscribe({
      next: (totalSales) => {
        this.dashboardStats.totalSales = totalSales;
        this.totalSalesLoading = false;
      },
      error: (error) => {
        console.error('Error loading total sales:', error);
        this.totalSalesLoading = false;
      }
    });

    // Total Revenue
    this.totalRevenueLoading = true;
    this.dashboardService.getTotalRevenue().subscribe({
      next: (totalRevenue) => {
        this.dashboardStats.totalRevenue = totalRevenue;
        this.totalRevenueLoading = false;
      },
      error: (error) => {
        console.error('Error loading total revenue:', error);
        this.totalRevenueLoading = false;
      }
    });

    // Total Clients
    this.totalClientsLoading = true;
    this.dashboardService.getTotalClients().subscribe({
      next: (totalClients) => {
        this.dashboardStats.totalClients = totalClients;
        this.totalClientsLoading = false;
      },
      error: (error) => {
        console.error('Error loading total clients:', error);
        this.totalClientsLoading = false;
      }
    });

    // Total Vehicles
    this.totalVehiclesLoading = true;
    this.dashboardService.getTotalVehicles().subscribe({
      next: (totalVehicles) => {
        this.dashboardStats.totalVehicles = totalVehicles;
        this.totalVehiclesLoading = false;
      },
      error: (error) => {
        console.error('Error loading total vehicles:', error);
        this.totalVehiclesLoading = false;
      }
    });

    // Sales This Month
    this.salesThisMonthLoading = true;
    this.dashboardService.getSalesThisMonth().subscribe({
      next: (salesThisMonth) => {
        this.dashboardStats.salesThisMonth = salesThisMonth;
        this.salesThisMonthLoading = false;
      },
      error: (error) => {
        console.error('Error loading sales this month:', error);
        this.salesThisMonthLoading = false;
      }
    });

    // Revenue This Month
    this.revenueThisMonthLoading = true;
    this.dashboardService.getRevenueThisMonth().subscribe({
      next: (revenueThisMonth) => {
        this.dashboardStats.revenueThisMonth = revenueThisMonth;
        this.revenueThisMonthLoading = false;
      },
      error: (error) => {
        console.error('Error loading revenue this month:', error);
        this.revenueThisMonthLoading = false;
      }
    });
    this.isLoading = false;
  }

  loadAnalyticsData(): void {
    // Daily Sales
    this.isLoading = true;

    // Sales by Brand
    this.salesByBrandLoading = true;
    this.dashboardService.getSalesByBrand().subscribe({
      next: (salesByBrand) => {
        this.salesAnalytics.salesByVehicleBrand = salesByBrand;
        this.salesByBrandLoading = false;
      },
      error: (error) => {
        console.error('Error loading sales by brand:', error);
        this.salesByBrandLoading = false;
      }
    });

    // Top Vehicles
    this.topVehiclesLoading = true;
    this.dashboardService.getTopSellingVehicles().subscribe({
      next: (topVehicles) => {
        this.salesAnalytics.topSellingVehicles = topVehicles;
        this.topVehiclesLoading = false;
      },
      error: (error) => {
        console.error('Error loading top vehicles:', error);
        this.topVehiclesLoading = false;
      }
    });

    // Sales Trend
    this.salesTrendLoading = true;
    this.dashboardService.getSalesTrend().subscribe({
      next: (salesTrend) => {
        this.salesAnalytics.salesTrend = salesTrend;
        this.salesTrendLoading = false;
      },
      error: (error) => {
        console.error('Error loading sales trend:', error);
        this.salesTrendLoading = false;
      }
    });
    this.isLoading = false;
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

  get isAnalyticsLoading(): boolean {
    return this.dailySalesLoading || this.monthlySalesLoading ||
           this.salesByBrandLoading || this.topVehiclesLoading ||
           this.salesTrendLoading;
  }


  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('es-ES');
  }

  refreshData(): void {
    this.loadDashboardData();
  }
} 