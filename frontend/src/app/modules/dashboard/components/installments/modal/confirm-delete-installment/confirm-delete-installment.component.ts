import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';
import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { DialogClosedResult } from 'src/app/core/models/custom.model';
import { components } from 'src/app/core/models/models';
import { InstallmentsService } from 'src/app/core/services/installments/installments.service';
import { handleRequestError } from 'src/app/core/utils/custom-functions';

type InstallmentDto = components['schemas']['InstallmentDto'];

@Component({
  selector: 'app-confirm-delete-installment',
  standalone: true,
  imports: [NgIf],
  templateUrl: './confirm-delete-installment.component.html',
  styleUrl: './confirm-delete-installment.component.scss'
})
export class ConfirmDeleteInstallmentComponent {
  dialogRef = inject<DialogRef<DialogClosedResult>>(DialogRef<string>);
  data = inject<InstallmentDto>(DIALOG_DATA);
  isSubmitting: boolean = false;

  constructor(private installmentsService: InstallmentsService) {}

  onSubmit() {
    this.installmentsService.deleteInstallment(this.data.id).subscribe({
      next: (data) => {
        this.isSubmitting = false;
        this.dialogRef.close({isSuccess: true});
      },
      error: (error) => {
        this.isSubmitting = false;
        handleRequestError(error);
      }
    });
  }
}
