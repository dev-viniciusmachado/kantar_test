import { Injectable } from '@angular/core';
import { Product } from '../../models/product';
import { AuthService } from './auth.service';
import { ApiService } from './api.service';
import { Basket } from '../../models/basket';
import { Discount } from '../../models/discount';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { PagedBasketResponse } from '../../models/pagedBasket';
import { LoadingService } from '../loading/loading';

@Injectable({ providedIn: 'root' })
export class BasketService {
  private storageKey = 'basket';

  private basketSubject = new BehaviorSubject<Basket| null>(this.getBasket());
  basket$ = this.basketSubject.asObservable();

  constructor(
    private authService: AuthService,
    private apiService: ApiService,
    private loadingService: LoadingService
  ) {}

  private saveBasket(basket: Basket) {
    localStorage.setItem(this.storageKey, JSON.stringify(basket));
  }

  getBasket(): Basket | null {
    const data = localStorage.getItem(this.storageKey);
    return data ? JSON.parse(data) : null;
  }

  clearBasket() {
    localStorage.removeItem(this.storageKey);
  }

  async addProductToBasket(product: Product, quantity: number): Promise<void> {
    this.loadingService.show();
    const basket = this.getBasket();
    const owner = this.authService.getUser();

    if (!basket) {
      await this.createNewBasket(product, quantity, owner.guestId);
    } else {
      await this.addProduct(basket, product, quantity);
    }
    this.loadingService.hide();
  }

  private async addProduct(basket: Basket, product: Product, quantity: number): Promise<void> {
    const request = {
      basketId: basket.id,
      productId: product.id,
      quantity
    };

    try {
      const updatedBasket = await firstValueFrom(
        this.apiService.post<Basket>(`baskets/${basket.id}/product`, request)
      );
      this.basketSubject.next(updatedBasket);
      this.saveBasket(updatedBasket);
    } catch (err) {
      console.error('Erro to add a new product', err);
      throw err;
    }
  }

  private async createNewBasket(product: Product, quantity: number, ownerId: string): Promise<void> {
    const request = {
      guestId: ownerId,
      productId: product.id,
      quantity
    };

    try {
      const newBasket = await firstValueFrom(
        this.apiService.post<Basket>('baskets', request)
      );
      this.basketSubject.next(newBasket);
      this.saveBasket(newBasket);
    } catch (err) {
      console.error('Erro to create a new basket', err);
      throw err;
    }
  }

  async removeProductFromBasket(product: Product, discount: Discount | null = null): Promise<void> {
    this.loadingService.show();
    const basket = this.getBasket();
    if (!basket) return;

    let request = `baskets/${basket.id}/product/${product.id}`;
    if (discount) {
      request += `?discountId=${discount.id}`;
    }

    try {
      const updatedBasket = await firstValueFrom(
        this.apiService.delete<Basket>(request)
      );
      this.basketSubject.next(updatedBasket);
      this.saveBasket(updatedBasket);
    } catch (err) {
      console.error('Erro to delete the product from basket:', err);
      throw err;
    } finally{
      this.loadingService.hide();
    }
  }

  async checkOutBasket(): Promise<void> {
    this.loadingService.show();
    const basket = this.getBasket();
    if (!basket) return;

    let request = `baskets/${basket.id}/close`;

    try {
      var response = await firstValueFrom(
        this.apiService.patch(request,'')
      );
      console.log('Basket closed successfully:', response);
      this.basketSubject.next(null);
      this.clearBasket();
    } catch (err) {
      console.error('Erro to close from basket:', err);
      throw err;
    } finally {
      this.loadingService.hide();
    }
  }

  async cancelBasket(): Promise<void> {
    this.loadingService.show();
    const basket = this.getBasket();
    if (!basket) return;

    let request = `baskets/${basket.id}/cancel`;

    try {
      var response = await firstValueFrom(
        this.apiService.patch(request,'')
      );
      console.log('Basket closed successfully:', response);
      this.basketSubject.next(null);
      this.clearBasket();
    } catch (err) {
      console.error('Erro to close from basket:', err);
      throw err;
    } finally {
      this.loadingService.hide();
    }
  }

  async loadCustomerBasket(): Promise<void> {
    try {
      const basket = await firstValueFrom(
        this.apiService.get<Basket>(`baskets/customer/current`)
      );
      console.log('Loaded customer basket:', basket);
      this.basketSubject.next(basket);
      this.saveBasket(basket);
    } catch (err) {
      console.error('Erro to load customer basket:', err);
      throw err;
    }
  }
}
