import { Component, inject } from '@angular/core';
import {DIALOG_DATA, DialogRef} from '@angular/cdk/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { components } from 'src/app/core/models/models';
import { NgClass, NgIf } from '@angular/common';
import { CustomersService } from 'src/app/core/services/customers/customers.service';
import { handleRequestError } from 'src/app/core/utils/custom-functions';
import { DialogClosedResult } from 'src/app/core/models/custom.model';

type NewCustomerDto = components['schemas']['NewCustomerDto'];

@Component({
  selector: 'app-add-customer-modal',
  standalone: true,
  templateUrl: './add-customer-modal.component.html',
  styleUrl: './add-customer-modal.component.scss',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass,
    NgIf
  ]
})
export class AddCustomerModalComponent {
  dialogRef = inject<DialogRef<DialogClosedResult>>(DialogRef<string>);
  data = inject(DIALOG_DATA);
  form!: FormGroup;
  submitted = false;
  isSubmittingForm = false;

  constructor(private readonly formBuilder: FormBuilder, private customersService: CustomersService) {}

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: ['', [ Validators.required, Validators.pattern("^[0-9]*$"), Validators.minLength(9), Validators.maxLength(9)]],
      email: ['', [Validators.required, Validators.email]],
      identificationNumber: ['', Validators.required],
    });
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) return;

    const input = {...this.form.value} as NewCustomerDto;
    input.phoneNumber = `+233${input.phoneNumber}`;

    this.isSubmittingForm = true;

    this.customersService.createCustomer(input).subscribe({
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
