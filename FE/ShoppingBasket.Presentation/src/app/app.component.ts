import { Header } from './shared/header/header';
import { RouterModule } from '@angular/router';
import { MatSidenavModule,MatSidenav } from '@angular/material/sidenav'
import { BasketComponent } from './features/basket/basket';
import { Component, ViewChild } from '@angular/core';
import { SidenavService } from './core/services/sidenav.service';
import { Observable } from 'rxjs';
import { LoadingService } from './core/loading/loading';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from './shared/loading/loading';
@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [RouterModule,Header,MatSidenavModule, BasketComponent, CommonModule, LoadingComponent],
})
export class AppComponent {
  @ViewChild('sideNav') sidenav!: MatSidenav;
  loading$: Observable<boolean>;

  constructor(private sidenavService: SidenavService, private loadingService: LoadingService) {
    this.loading$ = this.loadingService.loading$;
  }

  ngAfterViewInit(): void {
    this.sidenavService.setSidenav(this.sidenav);
  }

}