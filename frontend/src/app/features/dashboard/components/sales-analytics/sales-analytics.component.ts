import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SalesAnalytics, DailySales, BrandSales, TopVehicle, RecentSale } from '../../services/dashboard-analytics.service';

@Component({
  selector: 'app-sales-analytics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sales-analytics.component.html',
  styleUrls: ['./sales-analytics.component.css']
})
export class SalesAnalyticsComponent implements OnInit, OnChanges {
  @Input() analytics: SalesAnalytics | null = null;
  @Input() recentSales: RecentSale[] = [];
  @Input() loading: boolean = false;

  // Chart data
  chartData: any = null;
  brandChartData: any = null;

  constructor() { }

  ngOnInit(): void {
    this.updateChartData();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['analytics'] && this.analytics) {
      this.updateChartData();
    }
  }

  updateChartData(): void {
    if (!this.analytics) return;

    // Prepare daily sales chart data
    this.chartData = {
      labels: this.analytics.dailySales.map(sale => this.formatDate(sale.date)),
      datasets: [
        {
          label: 'Ventas',
          data: this.analytics.dailySales.map(sale => sale.sales),
          borderColor: '#4a90e2',
          backgroundColor: 'rgba(74, 144, 226, 0.1)',
          tension: 0.4
        },
        {
          label: 'Ingresos (Miles)',
          data: this.analytics.dailySales.map(sale => sale.revenue / 1000),
          borderColor: '#28a745',
          backgroundColor: 'rgba(40, 167, 69, 0.1)',
          tension: 0.4
        }
      ]
    };

    // Prepare brand sales chart data
    this.brandChartData = {
      labels: this.analytics.salesByVehicleBrand.map(marca => marca.marca),
      datasets: [{
        data: this.analytics.salesByVehicleBrand.map(marca => marca.total),
        backgroundColor: [
          '#4a90e2',
          '#28a745',
          '#ffc107',
          '#dc3545',
          '#6f42c1'
        ]
      }]
    };

    this.loading = false;
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', { 
      month: 'short', 
      day: 'numeric' 
    });
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('es-ES', {
      style: 'currency',
      currency: 'ARS',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(value);
  }

  formatNumber(value: number): string {
    return new Intl.NumberFormat('es-ES').format(value);
  }

  formatPercentage(value: number): string {
    return `${value.toFixed(1)}%`;
  }

  getTrendIcon(trend: string | null): string {
    switch (trend) {
      case 'up': return 'fas fa-arrow-up';
      case 'down': return 'fas fa-arrow-down';
      default: return 'fas fa-minus';
    }
  }

  getTrendClass(trend: string | null): string {
    switch (trend) {
      case 'up': return 'trend-up';
      case 'down': return 'trend-down';
      default: return 'trend-neutral';
    }
  }

  getMaxSales(): number {
    if (!this.analytics?.dailySales) return 0;
    return Math.max(...this.analytics.dailySales.map(sale => sale.sales));
  }

  getMaxRevenue(): number {
    if (!this.analytics?.dailySales) return 0;
    return Math.max(...this.analytics.dailySales.map(sale => sale.revenue));
  }

  getSalesBarWidth(sales: number): string {
    const max = this.getMaxSales();
    return `${(sales / max) * 100}%`;
  }

  getBrandBarWidth(total: number): string {
    if (!this.analytics?.salesByVehicleBrand?.length) return '0%';
    const max = Math.max(...this.analytics.salesByVehicleBrand.map(marca => marca.total));
    return `${(total / max) * 100}%`;
  }

  getBrandColor(index: number): string {
    const colors = ['#4a90e2', '#28a745', '#ffc107', '#dc3545', '#6f42c1'];
    return colors[index % colors.length];
  }
} 