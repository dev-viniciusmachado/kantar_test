import { Injectable } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';
import { Auth } from '../../models/auth';
import { ApiService } from './api.service';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { BasketService } from './basket.services';

// This service handles user authentication and storage in localStorage.
@Injectable({ providedIn: 'root' })
export class AuthService {

  private authSubject = new BehaviorSubject<Auth| null>(this.getUser());
  auth$ = this.authSubject.asObservable();

  constructor(private api: ApiService) {}
  private storageKey = 'user';

  saveUser(user: Auth) {
    localStorage.setItem(this.storageKey, JSON.stringify(user));
  }

  getUser() : Auth {
    const data = localStorage.getItem(this.storageKey);
    return data ? JSON.parse(data) : this.createGuestUser();
  }

  clearUser() {
    localStorage.removeItem(this.storageKey);
  }
  createGuestUser() : Auth {
    const user = Auth.createToGuest(uuidv4());
    this.saveUser(user);

    return user;
  }

  isAuthenticated(): boolean {
    const user = this.getUser();
    return user !== null && user.guestId === undefined;
  }

  async login(email: string, password: string): Promise<void> {
    const user = await firstValueFrom( 
      this.api.post<Auth>('identity/login', { email, password }));
      console.log(user);
      this.authSubject.next(user);
      this.saveUser(user);
  }

  async register(email: string, password: string): Promise<void> {
    const response = await firstValueFrom( 
      this.api.post<Auth>('identity/register', { email, password }));
    
      const user = await firstValueFrom( 
        this.api.post<Auth>('identity/login', { email, password }));
    this.authSubject.next(user);
    this.saveUser(user);    
  }
  
  async refreshToken(refreshToken: string): Promise<Auth> {
    const user = await firstValueFrom(
      this.api.post<Auth>('identity/refresh', { refreshToken })
    );

    this.authSubject.next(user);
    this.saveUser(user);
    return user;
  }

}