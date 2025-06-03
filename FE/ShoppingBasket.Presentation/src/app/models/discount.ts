export class Discount {
    id!: string;
    name!: string;
    rate!: string;
  
    constructor(data?: Partial<Discount>) {
      Object.assign(this, data);
    }
  }