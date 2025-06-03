import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';
import { RouterModule } from '@angular/router';
import { MatBadgeModule } from '@angular/material/badge';
import { BasketService } from '../../core/services/basket.services';
import { MatSidenavModule } from '@angular/material/sidenav'
import { SidenavService } from '../../core/services/sidenav.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, 
    MatToolbarModule,
     MatButtonModule,
     MatIconModule, RouterModule,MatBadgeModule,MatSidenavModule],
  templateUrl: './header.html',
  styleUrls: ['./header.css']
})
export class Header {
    constructor(
      private auth: AuthService, 
      private basketService: BasketService, 
      private sideNavService: SidenavService,
      private cd: ChangeDetectorRef 
      ) {}
  get isAuthenticated(): boolean {
    return this.auth.isAuthenticated();
  }
  numberOfItemsInBasket = 0;


  toggleBasket() {
    this.sideNavService.toggle();
  }

  ngOnInit() {
    this.basketService.basket$.subscribe(basket => {
      this.numberOfItemsInBasket = basket == null ? 0 :basket.items.length;
      this.cd.detectChanges();
    });
  }
}