import { Component, inject, signal } from '@angular/core';
import {DIALOG_DATA, DialogRef} from '@angular/cdk/dialog';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { components, paths } from 'src/app/core/models/models';
import { NgClass, NgIf } from '@angular/common';
import { CustomersService } from 'src/app/core/services/customers/customers.service';
import { getCarrier, handleRequestError } from 'src/app/core/utils/custom-functions';
import { DialogClosedResult, LoadState, PaymentChannel } from 'src/app/core/models/custom.model';
import { InstallmentsService } from 'src/app/core/services/installments/installments.service';
import { NgSelectModule } from '@ng-select/ng-select';

type InstallmentDto = components['schemas']['InstallmentDto'];
type UpdatedInstallmentDto = components['schemas']['UpdatedInstallmentDto'];
type CyclePeriod = components['schemas']['CyclePeriod'];
type CustomerDto = components['schemas']['CustomerDto'];
type CustomersQuery = paths['/api/customers']['get']['parameters']['query'];

@Component({
  selector: 'app-edit-installment-modal',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass,
    NgIf,
    NgSelectModule,
  ],
  templateUrl: './edit-installment-modal.component.html',
  styleUrl: './edit-installment-modal.component.scss'
})
export class EditInstallmentModalComponent {
  dialogRef = inject<DialogRef<DialogClosedResult>>(DialogRef<string>);
  data = inject<InstallmentDto>(DIALOG_DATA);
  customers = signal<CustomerDto[]>([]);
  loadState: LoadState = 'Loading';
  cyclePeriod: CyclePeriod = 'Weekly';
  paymentChannel: PaymentChannel = 'mtn';
  submitted = false;
  isSubmittingForm = false;
  itemsPerPage: number = 10;
  currentPage: number = 1;
  totalItems: number = 0;

  form = this.formBuilder.group({
    customerId: [this.data.customer.id, Validators.required],
    item: [this.data.item, Validators.required],
    itemDetails: [this.data.itemDetails, Validators.required],
    amount: [this.data.amount, [ Validators.required, Validators.pattern("^[0-9]*$")]],
    initialDeposit: [this.data.initialDeposit, [ Validators.required, Validators.pattern("^[0-9]*$")]],
    cyclePeriod: [this.data.cyclePeriod, Validators.required],
    cycleNumber: [this.data.cycleNumber, [ Validators.required, Validators.pattern("^[0-9]*$")]],
    paymentChannel: [this.data.paymentChannel, Validators.required],
  });

  constructor(private readonly formBuilder: FormBuilder, private installmentsService: InstallmentsService, private customersService: CustomersService) {}

  ngOnInit(): void {
    this.getCustomers();
    // this.form = this.formBuilder.group({
    //   firstName: [this.data.firstName, Validators.required],
    //   lastName: [this.data.lastName, Validators.required],
    //   phoneNumber: [this.data.phoneNumber, [ Validators.required, Validators.pattern("^[0-9]*$"), Validators.minLength(9), Validators.maxLength(9)]],
    //   email: [this.data.email, [Validators.required, Validators.email]],
    //   identificationNumber: [this.data.identificationNumber, Validators.required],
    // });
  }

  getCustomers() {
    this.loadState = 'Loading';

    let query: CustomersQuery = {
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.customersService.getCustomers(query).subscribe({
      next: (data) => {
        this.totalItems = data.count;
        this.customers.set(data.items);
        this.loadState = 'Loaded';
      },
      error: (error) => {
        this.loadState = 'Error';
        handleRequestError(error)},
    });
  }
  
  fetchMore() {
    if(this.customers().length === this.totalItems) return;

    this.loadState = 'Loading';

    this.currentPage++;

    let query: CustomersQuery = {
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.customersService.getCustomers(query).subscribe({
      next: (data) => {
        this.totalItems = data.count;
        this.customers.update(values => {
          return [...values, ...data.items]
        });
        this.loadState = 'Loaded';
      },
      error: (error) => {
        this.loadState = 'Error';
        handleRequestError(error)},
    });
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) return;

    const input = {...this.form.value} as unknown as UpdatedInstallmentDto;    

    this.isSubmittingForm = true;

    this.installmentsService.updateInstallment(this.data.id, input).subscribe({
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

  onCustomerChange($event: any) {
    const customer: CustomerDto = $event;

    if(!customer) return;

    const carrier = getCarrier(customer.phoneNumber);

    this.form.patchValue({
      paymentChannel: carrier
    })
  }
}
