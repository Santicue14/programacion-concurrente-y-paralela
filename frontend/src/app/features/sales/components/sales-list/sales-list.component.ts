import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { SaleService } from '../../services/sales-api.service';
import { Sale } from '../../../../core/models/sale.model';
import { VehicleService } from '../../../vehicles/services/vehicles-api.service';
declare var bootstrap: any; 

@Component({
  selector: 'app-sales-list',
  imports:[FormsModule, CommonModule],
  templateUrl: './sales-list.component.html',
  styleUrls: ['./sales-list.component.css']
})
export class SalesListComponent implements OnInit {
  sales: Sale[] = [];
  filteredSales: Sale[] = [];
  paginatedSales: Sale[] = [];
  
  // Paginación
  currentPage = 1;
  itemsPerPage = 10;
  totalPages = 1;
  
  // Filtros
  searchTerm = '';
  selectedMarca = '';

  // Ordenamiento
  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  selectedSale: Sale | null = null;

  constructor(private saleService: SaleService, private router: Router) { }

  ngOnInit(): void {	
    this.loadSales();
  }

  loadSales(): void {
    this.saleService.getVentas().subscribe({
      next: (data) => {
        this.sales = data;
        this.filterSales();
      },
      error: (error) => {
        console.error('Error al cargar ventas:', error);
      }
    });
  }

  sortBy(column: string): void {
    if (this.sortColumn === column) {
      // Si ya está ordenado por esta columna, cambia la dirección
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      // Si es una nueva columna, ordena ascendente por defecto
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.filterSales();
  }

  filterSales(): void {
    // Primero aplica los filtros
    this.filteredSales = this.sales.filter(sale => {
      const matchesSearch = !this.searchTerm || 
        sale.cliente?.nombre?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        sale.cliente?.apellido?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        sale.cliente?.email?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        sale.vehiculo?.marca?.toString().toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        sale.vehiculo?.modelo?.toString().toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesMarca = !this.selectedMarca || 
        sale.vehiculo?.marca?.toString() === this.selectedMarca;

      return matchesSearch && matchesMarca;
    });

    // Luego aplica el ordenamiento
    if (this.sortColumn) {
      this.filteredSales.sort((a, b) => {
        let valueA: any;
        let valueB: any;

        // Manejo especial para propiedades anidadas
        switch (this.sortColumn) {
          case 'cliente':
            valueA = a.cliente?.nombre || '';
            valueB = b.cliente?.nombre || '';
            break;
          case 'vehiculo':
            valueA = `${a.vehiculo?.marca || ''} ${a.vehiculo?.modelo || ''}`;
            valueB = `${b.vehiculo?.marca || ''} ${b.vehiculo?.modelo || ''}`;
            break;
          case 'fecha':
            valueA = new Date(a.fecha).getTime();
            valueB = new Date(b.fecha).getTime();
            break;
          case 'total':
            valueA = a.total;
            valueB = b.total;
            break;
          default:
            valueA = a[this.sortColumn as keyof Sale];
            valueB = b[this.sortColumn as keyof Sale];
        }

        // Manejo especial para números
        if (typeof valueA === 'number' && typeof valueB === 'number') {
          return this.sortDirection === 'asc' 
            ? valueA - valueB 
            : valueB - valueA;
        }

        // Para strings y fechas
        const stringA = String(valueA).toLowerCase();
        const stringB = String(valueB).toLowerCase();
        
        return this.sortDirection === 'asc'
          ? stringA.localeCompare(stringB)
          : stringB.localeCompare(stringA);
      });
    }

    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredSales.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    this.paginatedSales = this.filteredSales.slice(
      startIndex,
      startIndex + this.itemsPerPage
    );
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;
    
    if (this.totalPages <= maxPagesToShow) {
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
      let endPage = startPage + maxPagesToShow - 1;
      
      if (endPage > this.totalPages) {
        endPage = this.totalPages;
        startPage = Math.max(1, endPage - maxPagesToShow + 1);
      }
      
      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }
    }
    
    return pages;
  }

  onCreateSale(): void {
    this.router.navigate(['/sales/create'], { fragment: 'taskFormModal' });
  }
  
  onEditSale(sale: Sale): void {
    console.log(sale);
    this.router.navigate([`/sales/edit/${sale.id}`], { fragment: 'taskFormModal' });
  }     
  
  // Método para abrir el modal para tratar la eliminación
  onDeleteSale(sale: Sale): void {
    this.selectedSale = sale;

    // Obtener el modal y hacer el método show
    const deleteModal = new bootstrap.Modal(
      document.getElementById('deleteSaleModal')
    );
    deleteModal.show();
  }

  // Método para confirmar el borrado
  confirmDelete(): void {
    if (this.selectedSale) {
      this.saleService.deleteVenta(this.selectedSale.id).subscribe(
        () => {
          console.log('Venta borrada:', this.selectedSale);
          // Limpia la venta seleccionada
          this.selectedSale = null; 
          // Recarga las ventas o actualiza la vista
          this.loadSales();
        },
        (error: any) => {
          console.error('Error al borrar la venta:', error);
        }
      );
    }

    // Cierra el modal
    const deleteModal = bootstrap.Modal.getInstance(
      document.getElementById('deleteSaleModal')
    );
    deleteModal.hide();
  }

  // Método para obtener las marcas únicas para el filtro
  get marcas(): string[] {
    const marcasSet = new Set<string>();
    this.sales.forEach(sale => {
      if (sale.vehiculo?.marca) {
        marcasSet.add(sale.vehiculo.marca.toString());
      }
    });
    return Array.from(marcasSet).sort();
  }
}
