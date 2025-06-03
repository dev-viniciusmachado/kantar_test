import { Discount } from "./discount";

export class Product {
    id!: string;
    name!: string;
    price!: number;
    imagePath!: string;
    discount: Discount[];
  
    constructor(data?: Partial<Product>) {
      Object.assign(this, data);
      if (data?.discount) {
        this.discount = data.discount.map(d => new Discount(d));
      } else {
        this.discount = [];
      }
    }
  }