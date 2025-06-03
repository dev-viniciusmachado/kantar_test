import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-loading',
  templateUrl: './loading.html',
  standalone: true, 
  styleUrls: ['./loading.css'],
  imports: [CommonModule]
})
export class LoadingComponent {
  @Input() visible : boolean | null = false;
}