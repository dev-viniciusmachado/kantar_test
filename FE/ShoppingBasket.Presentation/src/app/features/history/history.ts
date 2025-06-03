import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { PagedBasketResponse } from '../../models/pagedBasket';
import { ApiService } from '../../core/services/api.service';
import { ChangeDetectorRef } from '@angular/core';
import { LoadingService } from '../../core/loading/loading';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatDividerModule
  ],
  templateUrl: './history.html',
  styleUrl: './history.css'
})
export class History {
  history!: PagedBasketResponse;
  totalCount = 0;
  pageSize = 10;
  pageNumber = 1;
  loading = false;
  columns = ['id', 'closedAt','products', 'totalPriceWithDiscount'];

  ngOnInit(): void {
    this.fetchHistory();
  }

  constructor(private api: ApiService, private cd: ChangeDetectorRef,
    private loadingService: LoadingService) {}
  
    fetchHistory(): void {
    this.loading = true;
  
    this.loadingService.show();
    const params = {
      page: (this.pageNumber).toString(),
      pageSize: this.pageSize.toString()
    };

    this.api.get<PagedBasketResponse>(`baskets/customer/history?orderBy=ClosedAt&isDescending=true&page=${params.page}&pageSize=${params.pageSize}`).subscribe({
      next: (res) => {
        this.history = res;
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching history:', err);
        this.loading = false;
      },
      complete: () => {
        this.loadingService.hide();
      }
    });
  }

  onPageChange(event: PageEvent): void {
    console.log('Page changed:', event);
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageIndex+1;
    this.fetchHistory();
  }

  getItemNames(items: any[]): string {
    return items.map(p => p.product.name+` (x${p.quantity})`).join(', ');
  }
}
