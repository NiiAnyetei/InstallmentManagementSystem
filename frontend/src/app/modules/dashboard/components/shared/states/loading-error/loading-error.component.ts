import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-loading-error',
  standalone: true,
  imports: [],
  templateUrl: './loading-error.component.html',
  styleUrl: './loading-error.component.scss'
})
export class LoadingErrorComponent {
  @Output() onRetry: EventEmitter<any> = new EventEmitter();

  retry() {
    this.onRetry.emit();
  }
}
