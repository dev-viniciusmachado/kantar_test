import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../core/services/auth.service';
import { BasketService } from '../../core/services/basket.services';

@Component({
  selector: 'app-user',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User {
  form;
  errorMessage: string | null = "Generic error occurred";
  hasError: boolean = false;
  constructor(private fb: FormBuilder, private auth: AuthService, private basketService: BasketService) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }
  isLoginMode = true;

  toggleMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  async onSubmit() {
    if (this.form.invalid) return;

    const { email, password } = this.form.value;

    if(this.isLoginMode)
      await this.auth.login(email!, password!);
    else
      await this.auth.register(email!, password!);

    await this.basketService.loadCustomerBasket();
    this.form.reset();
  }

  isAuthenticated(): boolean {
    return this.auth.isAuthenticated();
  }

  logout() {
    this.auth.clearUser();
    this.basketService.clearBasket();
  }
}
