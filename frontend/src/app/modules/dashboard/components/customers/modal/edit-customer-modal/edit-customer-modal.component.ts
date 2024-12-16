import { Component, inject } from '@angular/core';
import {DIALOG_DATA, DialogRef} from '@angular/cdk/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { components } from 'src/app/core/models/models';
import { NgClass, NgIf } from '@angular/common';
import { CustomersService } from 'src/app/core/services/customers/customers.service';
import { handleRequestError } from 'src/app/core/utils/custom-functions';
import { DialogClosedResult } from 'src/app/core/models/custom.model';

type UpdatedCustomerDto = components['schemas']['UpdatedCustomerDto'];
type CustomerDto = components['schemas']['CustomerDto'];

@Component({
  selector: 'app-edit-customer-modal',
  standalone: true,
  templateUrl: './edit-customer-modal.component.html',
  styleUrl: './edit-customer-modal.component.scss',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass,
    NgIf
  ]
})
export class EditCustomerModalComponent {
  dialogRef = inject<DialogRef<DialogClosedResult>>(DialogRef<string>);
  data = inject<CustomerDto>(DIALOG_DATA);
  form!: FormGroup;
  submitted = false;
  isSubmittingForm = false;

  constructor(private readonly formBuilder: FormBuilder, private customersService: CustomersService) {}

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      firstName: [this.data.firstName, Validators.required],
      lastName: [this.data.lastName, Validators.required],
      phoneNumber: [this.data.phoneNumber, [ Validators.required, Validators.pattern("^[0-9]*$"), Validators.minLength(9), Validators.maxLength(9)]],
      email: [this.data.email, [Validators.required, Validators.email]],
      identificationNumber: [this.data.identificationNumber, Validators.required],
    });
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) return;

    const input = {...this.form.value} as UpdatedCustomerDto;
    input.phoneNumber = `+233${input.phoneNumber}`;

    this.isSubmittingForm = true;

    this.customersService.updateCustomer(this.data.id, input).subscribe({
      next: (data) => {
        this.isSubmittingForm = false;
        this.dialogRef.close({isSuccess: true});
      },
      error: (error) => {
        this.isSubmittingForm = false;
        handleRequestError(error);
      }
    });
  }
}
