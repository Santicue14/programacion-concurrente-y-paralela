import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardStats } from '../../services/dashboard-analytics.service';

@Component({
  selector: 'app-stats-cards',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './stats-cards.component.html',
  styleUrls: ['./stats-cards.component.css']
})
export class StatsCardsComponent implements OnInit {
  @Input() stats: DashboardStats = {
    totalSales: null,
    totalRevenue: null,
    totalClients: null,
    totalVehicles: null,
    salesThisMonth: null,
    revenueThisMonth: null,

  };
  
  // Individual loading states
  @Input() totalSalesLoading: boolean = false;
  @Input() totalRevenueLoading: boolean = false;
  @Input() totalClientsLoading: boolean = false;
  @Input() totalVehiclesLoading: boolean = false;
  @Input() salesThisMonthLoading: boolean = false;
  @Input() revenueThisMonthLoading: boolean = false;


  constructor() { }

  ngOnInit(): void {
  }

  formatCurrency(value: number | null): string {
    if (value === null) return '--';
    return new Intl.NumberFormat('es-ES', {
      style: 'currency',
      currency: 'ARS',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(value);
  }

  formatNumber(value: number | null): string {
    if (value === null) return '--';
    return new Intl.NumberFormat('es-ES').format(value);
  }

  formatPercentage(value: number | null): string {
    if (value === null) return '--';
    return `${value.toFixed(1)}%`;
  }

  getGrowthClass(growth: number): string {
    if (growth > 0) return 'growth-positive';
    if (growth < 0) return 'growth-negative';
    return 'growth-neutral';
  }

  getGrowthIcon(growth: number): string {
    if (growth > 0) return 'fas fa-arrow-up';
    if (growth < 0) return 'fas fa-arrow-down';
    return 'fas fa-minus';
  }
} 