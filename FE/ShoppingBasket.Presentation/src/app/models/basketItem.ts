import { Discount } from "./discount";
import { Product } from "./product";

export class BasketItem {
    product!: Product;
    totalPrice!: number;
    totalPriceWithDiscount!: number;
    quantity!: number;
    discountApplayed?: Discount;
  
    constructor(init?: Partial<BasketItem>) {
      Object.assign(this, init);
      this.product = new Product(init?.product);
      this.discountApplayed = new Discount(init?.discountApplayed);
    }
  }
  