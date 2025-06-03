import { Basket } from "./basket";

export class PagedBasketResponse {
    totalCount!: number;
    pageSize!: number;
    pageNumber!: number;
    baskets!: Basket[];

    constructor(data?: Partial<PagedBasketResponse>) {
        Object.assign(this, data);
      }
  }