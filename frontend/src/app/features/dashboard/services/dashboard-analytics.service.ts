import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { finalize, Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

export interface DashboardStats {
  totalSales: number | null;
  totalRevenue: number | null;
  totalClients: number | null;
  totalVehicles: number | null;
  salesThisMonth: number | null;
  revenueThisMonth: number | null;
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
  marca: string;
  total: number;
  porcentaje: number;
}

export interface TopVehicle {
  id: number;
  marca: string;
  modelo: string;
  cantidadVentas: number;
}

export interface SalesTrend {
  period: string;
  growth: number;
  trend: 'up' | 'down' | 'stable';
}

export interface RecentSale {
  id: number;
  cliente: {
    nombre: string;
    apellido: string;
    email: string;
  };
  vehiculo: {
    marca: string;
    modelo: string;
    precio: number;
  };
  fecha: string;
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
  private apiUrl = `${environment.apiBaseUrl}/api`;

  public isLoadingTotalSales: boolean = false;
  public isLoadingTotalRevenue: boolean = false;
  public isLoadingTotalClients: boolean = false;
  public isLoadingTotalVehicles: boolean = false;
  public isLoadingSalesThisMonth: boolean = false;
  public isLoadingRevenueThisMonth: boolean = false;
  constructor(private http: HttpClient, private router: Router) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('access_token');
    if (!token) {
      this.router.navigate(['/login']);
      throw new Error('No authentication token found');
    }
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getTotalSales(): Observable<number> {
    this.isLoadingTotalSales = true;
    return this.http.get<number>(`${this.apiUrl}/Venta/total-sales`, { headers: this.getAuthHeaders() }).pipe(
      finalize(() => this.isLoadingTotalSales = false)
    );
  }

  getTotalRevenue(): Observable<number> {
    this.isLoadingTotalRevenue = true;
    return this.http.get<number>(`${this.apiUrl}/Venta/total-revenue`, { headers: this.getAuthHeaders() }).pipe(
      finalize(() => this.isLoadingTotalRevenue = false)
    );
  }

  getTotalClients(): Observable<number> {
    this.isLoadingTotalClients = true;
    return this.http.get<number>(`${this.apiUrl}/Cliente/total-clients`, { headers: this.getAuthHeaders() }).pipe(
      finalize(() => this.isLoadingTotalClients = false)
    );
  }

  getTotalVehicles(): Observable<number> {
    this.isLoadingTotalVehicles = true;   
    return this.http.get<number>(`${this.apiUrl}/Vehiculo/total-vehicles`, { headers: this.getAuthHeaders() }).pipe(
      finalize(() => this.isLoadingTotalVehicles = false)
    );
  }

  getSalesThisMonth(): Observable<number> {
    const headers = this.getAuthHeaders();
    return this.http.get<number>(`${this.apiUrl}/Venta/sales-this-month`, { headers }).pipe(
      finalize(() => this.isLoadingSalesThisMonth = false)
    );
  }

  getRevenueThisMonth(): Observable<number> {
    const headers = this.getAuthHeaders();
    return this.http.get<number>(`${this.apiUrl}/Venta/revenue-this-month`, { headers }).pipe(
      finalize(() => this.isLoadingRevenueThisMonth = false)
    );
  }


  getSalesByBrand(): Observable<BrandSales[]> {
    return this.http.get<BrandSales[]>(`${this.apiUrl}/Venta/sales-by-brand`, { headers: this.getAuthHeaders() });
  }

  getTopSellingVehicles(): Observable<TopVehicle[]> {
    return this.http.get<TopVehicle[]>(`${this.apiUrl}/Venta/sales-by-model`, { headers: this.getAuthHeaders() });
  }

  getSalesTrend(filters?: AnalyticsFilters): Observable<SalesTrend> {
    const headers = this.getAuthHeaders();
    let params = new HttpParams();
    
    if (filters) {
      if (filters.startDate) params = params.set('startDate', filters.startDate);
      if (filters.endDate) params = params.set('endDate', filters.endDate);
      if (filters.period) params = params.set('period', filters.period);
    }

    // For now, return mock data until backend is ready
    const mockSalesTrend: SalesTrend = { period: 'Este mes', growth: 15.3, trend: 'up' };
    return of(mockSalesTrend);
    
    // Uncomment when backend is ready:
    // return this.http.get<SalesTrend>(`${this.apiUrl}/Analytics/sales-trend`, { headers, params });
  }

  // Get recent sales
  getRecentSales(limit: number = 10): Observable<RecentSale[]> {
    return this.http.get<RecentSale[]>(`${this.apiUrl}/Venta/last-sales`, { headers: this.getAuthHeaders()});
  }

  // Get sales comparison data
  getSalesComparison(period: 'week' | 'month' | 'quarter' | 'year'): Observable<any> {
    const headers = this.getAuthHeaders();
    const params = new HttpParams().set('period', period);

    // For now, return mock data until backend is ready
    return of(this.getMockSalesComparisonData());
    
    // Uncomment when backend is ready:
    // return this.http.get<any>(`${this.apiUrl}/Analytics/sales-comparison`, { headers, params });
  }



  private getMockSalesComparisonData(): any {
    return {
      current: { period: 'Este mes', sales: 23, revenue: 385000 },
      previous: { period: 'Mes anterior', sales: 20, revenue: 334000 },
      growth: { sales: 15.0, revenue: 15.3 }
    };
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