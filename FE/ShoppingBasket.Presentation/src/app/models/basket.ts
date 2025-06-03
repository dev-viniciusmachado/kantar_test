import { BasketItem } from "./basketItem";
import { formatDate } from '@angular/common';
export class Basket {
    id!: string;
    ownerId!: string;
    totalPrice!: number;
    totalPriceWithDiscount!: number;
    createdAt!: string;
    closedAt!: string;
    
    items: BasketItem[] = [];
  
    constructor(init?: Partial<Basket>) {
      Object.assign(this, init);
      this.items = init?.items?.map(i => new BasketItem(i)) || [];
    }

  }