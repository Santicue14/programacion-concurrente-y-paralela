import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

export interface DashboardStats {
  totalSales: number;
  totalRevenue: number;
  totalClients: number;
  totalVehicles: number;
  salesThisMonth: number;
  revenueThisMonth: number;
  averageTicket: number;
  conversionRate: number;
}

export interface SalesAnalytics {
  dailySales: DailySales[];
  monthlySales: MonthlySales[];
  salesByVehicleBrand: BrandSales[];
  topSellingVehicles: TopVehicle[];
  salesTrend: SalesTrend;
}

export interface DailySales {
  date: string;
  sales: number;
  revenue: number;
}

export interface MonthlySales {
  month: string;
  year: number;
  sales: number;
  revenue: number;
}

export interface BrandSales {
  brand: string;
  sales: number;
  revenue: number;
  percentage: number;
}

export interface TopVehicle {
  id: number;
  brand: string;
  model: string;
  year: number;
  salesCount: number;
  revenue: number;
}

export interface SalesTrend {
  period: string;
  growth: number;
  trend: 'up' | 'down' | 'stable';
}

export interface RecentSale {
  id: number;
  clientName: string;
  vehicleBrand: string;
  vehicleModel: string;
  amount: number;
  date: string;
}

export interface AnalyticsFilters {
  startDate?: string;
  endDate?: string;
  vehicleBrand?: string;
  clientId?: number;
  period?: 'daily' | 'weekly' | 'monthly' | 'yearly';
}

@Injectable({
  providedIn: 'root'
})
export class DashboardAnalyticsService {
  private apiUrl = `${environment.apiBaseUrl}/api/Analytics`;

  constructor(private http: HttpClient, private router: Router) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('access_token');
    if (!token) {
      this.router.navigate(['/login']);
      throw new Error('No authentication token found');
    }
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  // Get dashboard overview statistics
  getDashboardStats(filters?: AnalyticsFilters): Observable<DashboardStats> {
    const headers = this.getAuthHeaders();
    let params = new HttpParams();
    
    if (filters) {
      if (filters.startDate) params = params.set('startDate', filters.startDate);
      if (filters.endDate) params = params.set('endDate', filters.endDate);
      if (filters.vehicleBrand) params = params.set('vehicleBrand', filters.vehicleBrand);
      if (filters.clientId) params = params.set('clientId', filters.clientId.toString());
    }

    // For now, return mock data until backend is ready
    return this.getMockDashboardStats();
    
    // Uncomment when backend is ready:
    // return this.http.get<DashboardStats>(`${this.apiUrl}/dashboard-stats`, { headers, params });
  }

  // Get sales analytics data
  getSalesAnalytics(filters?: AnalyticsFilters): Observable<SalesAnalytics> {
    const headers = this.getAuthHeaders();
    let params = new HttpParams();
    
    if (filters) {
      if (filters.startDate) params = params.set('startDate', filters.startDate);
      if (filters.endDate) params = params.set('endDate', filters.endDate);
      if (filters.period) params = params.set('period', filters.period);
      if (filters.vehicleBrand) params = params.set('vehicleBrand', filters.vehicleBrand);
    }

    // For now, return mock data until backend is ready
    return this.getMockSalesAnalytics();
    
    // Uncomment when backend is ready:
    // return this.http.get<SalesAnalytics>(`${this.apiUrl}/sales-analytics`, { headers, params });
  }

  // Get recent sales
  getRecentSales(limit: number = 10): Observable<RecentSale[]> {
    const headers = this.getAuthHeaders();
    const params = new HttpParams().set('limit', limit.toString());

    // For now, return mock data until backend is ready
    return this.getMockRecentSales();
    
    // Uncomment when backend is ready:
    // return this.http.get<RecentSale[]>(`${this.apiUrl}/recent-sales`, { headers, params });
  }

  // Get sales comparison data
  getSalesComparison(period: 'week' | 'month' | 'quarter' | 'year'): Observable<any> {
    const headers = this.getAuthHeaders();
    const params = new HttpParams().set('period', period);

    // For now, return mock data until backend is ready
    return this.getMockSalesComparison();
    
    // Uncomment when backend is ready:
    // return this.http.get<any>(`${this.apiUrl}/sales-comparison`, { headers, params });
  }

  // Mock data methods (remove when backend is ready)
  private getMockDashboardStats(): Observable<DashboardStats> {
    const mockStats: DashboardStats = {
      totalSales: 156,
      totalRevenue: 2450000,
      totalClients: 89,
      totalVehicles: 45,
      salesThisMonth: 23,
      revenueThisMonth: 385000,
      averageTicket: 15705,
      conversionRate: 68.5
    };
    return of(mockStats);
  }

  private getMockSalesAnalytics(): Observable<SalesAnalytics> {
    const mockAnalytics: SalesAnalytics = {
      dailySales: this.generateMockDailySales(),
      monthlySales: this.generateMockMonthlySales(),
      salesByVehicleBrand: [
        { brand: 'Toyota', sales: 45, revenue: 720000, percentage: 28.8 },
        { brand: 'Honda', sales: 38, revenue: 608000, percentage: 24.4 },
        { brand: 'Ford', sales: 32, revenue: 512000, percentage: 20.5 },
        { brand: 'Chevrolet', sales: 25, revenue: 400000, percentage: 16.0 },
        { brand: 'Nissan', sales: 16, revenue: 256000, percentage: 10.3 }
      ],
      topSellingVehicles: [
        { id: 1, brand: 'Toyota', model: 'Corolla', year: 2023, salesCount: 12, revenue: 240000 },
        { id: 2, brand: 'Honda', model: 'Civic', year: 2023, salesCount: 10, revenue: 220000 },
        { id: 3, brand: 'Ford', model: 'Focus', year: 2022, salesCount: 8, revenue: 160000 },
        { id: 4, brand: 'Chevrolet', model: 'Cruze', year: 2023, salesCount: 7, revenue: 140000 },
        { id: 5, brand: 'Nissan', model: 'Sentra', year: 2022, salesCount: 6, revenue: 120000 }
      ],
      salesTrend: { period: 'Este mes', growth: 15.3, trend: 'up' }
    };
    return of(mockAnalytics);
  }

  private getMockRecentSales(): Observable<RecentSale[]> {
    const mockSales: RecentSale[] = [
      { id: 1, clientName: 'Juan Pérez', vehicleBrand: 'Toyota', vehicleModel: 'Corolla', amount: 20000, date: '2024-01-15' },
      { id: 2, clientName: 'María García', vehicleBrand: 'Honda', vehicleModel: 'Civic', amount: 22000, date: '2024-01-14' },
      { id: 3, clientName: 'Carlos López', vehicleBrand: 'Ford', vehicleModel: 'Focus', amount: 18500, date: '2024-01-13' },
      { id: 4, clientName: 'Ana Martínez', vehicleBrand: 'Chevrolet', vehicleModel: 'Cruze', amount: 19500, date: '2024-01-12' },
      { id: 5, clientName: 'Luis Rodríguez', vehicleBrand: 'Nissan', vehicleModel: 'Sentra', amount: 17800, date: '2024-01-11' }
    ];
    return of(mockSales);
  }

  private getMockSalesComparison(): Observable<any> {
    const mockComparison = {
      current: { period: 'Este mes', sales: 23, revenue: 385000 },
      previous: { period: 'Mes anterior', sales: 20, revenue: 334000 },
      growth: { sales: 15.0, revenue: 15.3 }
    };
    return of(mockComparison);
  }

  private generateMockDailySales(): DailySales[] {
    const sales: DailySales[] = [];
    const today = new Date();
    
    for (let i = 29; i >= 0; i--) {
      const date = new Date(today);
      date.setDate(date.getDate() - i);
      
      sales.push({
        date: date.toISOString().split('T')[0],
        sales: Math.floor(Math.random() * 5) + 1,
        revenue: (Math.floor(Math.random() * 50000) + 10000)
      });
    }
    
    return sales;
  }

  private generateMockMonthlySales(): MonthlySales[] {
    const sales: MonthlySales[] = [];
    const months = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 
                   'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
    
    for (let i = 0; i < 12; i++) {
      sales.push({
        month: months[i],
        year: 2024,
        sales: Math.floor(Math.random() * 30) + 10,
        revenue: (Math.floor(Math.random() * 500000) + 200000)
      });
    }
    
    return sales;
  }
} 