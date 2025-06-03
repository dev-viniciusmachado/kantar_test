export class Auth {
    tokenType!: string;
    accessToken!: string;
    expiresIn!: number;
    refreshToken!: string;
    guestId!: string;
  
    constructor(init?: Partial<Auth>) {
      Object.assign(this, init);
    }

    static createToGuest (guestId: string): Auth {
        return new Auth({ guestId });
      }
  }