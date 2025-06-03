import { Component } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Product } from '../../models/product';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { BasketService } from '../../core/services/basket.services';
import { ChangeDetectorRef } from '@angular/core';
import { LoadingService } from '../../core/loading/loading';
@Component({
  selector: 'app-store',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    MatGridListModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    FormsModule
  ],
  templateUrl: './store.html',
  styleUrls: ['./store.css']  
})
export class Store {
  products: Product[] = [];
  quantities: { [productId: string]: number } = {};
  constructor(private apiService: ApiService, private basketService: BasketService, private cd: ChangeDetectorRef,private loadingService: LoadingService) {
    this.loadProducts();
  }

  loadProducts() {
    this.loadingService.show();
    this.apiService.get<Product[]>('products').subscribe({
      next: data => (
        this.products = data, this.cd.detectChanges()),
      error: err => console.error('Error:', err),
      
      complete: () => {
        this.loadingService.hide();
      }
    });
  }

  addToBasket(product: Product) {
    const quantity = this.quantities[product.id] || 1;
    this.basketService.addProductToBasket(product, quantity);
    this.quantities[product.id] = 0; // Reset quantity after adding to basket
  }
}
