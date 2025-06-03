import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../core/services/basket.services';
import {MatTableModule} from '@angular/material/table';
import { Basket } from '../../models/basket';
import { CommonModule } from '@angular/common';
import { MatSortModule } from '@angular/material/sort';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { Product } from '../../models/product';
import { Discount } from '../../models/discount';
import { ChangeDetectorRef } from '@angular/core';

import { SidenavService } from '../../core/services/sidenav.service';

@Component({
  selector: 'app-basket',
  imports: [MatTableModule,CommonModule,MatSortModule,MatGridListModule,MatIconModule],
  templateUrl: './basket.html',
  styleUrl: './basket.css'
})
export class BasketComponent {
  displayedColumns: string[] = ['image','name', 'quantity', 'price']
  constructor(private basketService: BasketService, private cd: ChangeDetectorRef,
    private sideNavService: SidenavService, ) {}
  
  get numberOfItemsInBasket(): number {
    const basket = this.basketService.getBasket();    
    return basket ? basket.items.length : 0;
  }
  basket!:Basket;

  async removeProduct(product: Product, discount: Discount) {
    await this.basketService.removeProductFromBasket(product,discount);
  }

  async checkOut() {
    await this.basketService.checkOutBasket();
    this.sideNavService.toggle();
  }

  async cancel() {
    await this.basketService.cancelBasket();
    this.sideNavService.toggle();
  }

  ngOnInit() {
    this.basketService.basket$.subscribe(basket => {
      this.basket = basket || new Basket();
      this.cd.detectChanges();
    });
  }
}
