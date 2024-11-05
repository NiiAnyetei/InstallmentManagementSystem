import { Component, inject } from '@angular/core';
import {DIALOG_DATA, DialogRef} from '@angular/cdk/dialog';

@Component({
  selector: 'app-add-customer-modal',
  standalone: true,
  templateUrl: './add-customer-modal.component.html',
  styleUrl: './add-customer-modal.component.scss'
})
export class AddCustomerModalComponent {
  dialogRef = inject<DialogRef<string>>(DialogRef<string>);
  data = inject(DIALOG_DATA);

  ngOnInit(){
    console.log(this.data);
  }
}
