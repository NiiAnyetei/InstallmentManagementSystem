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

type NewInstallmentDto = components['schemas']['NewInstallmentDto'];
type CyclePeriod = components['schemas']['CyclePeriod'];
type CustomerDto = components['schemas']['CustomerDto'];
type CustomersQuery = paths['/api/customers']['get']['parameters']['query'];

@Component({
  selector: 'app-add-installment-modal',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass,
    NgIf,
    NgSelectModule,
  ],
  templateUrl: './add-installment-modal.component.html',
  styleUrl: './add-installment-modal.component.scss'
})
export class AddInstallmentModalComponent {
  dialogRef = inject<DialogRef<DialogClosedResult>>(DialogRef<string>);
  data = inject(DIALOG_DATA);
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
    customerId: ['', Validators.required],
    item: ['', Validators.required],
    itemDetails: ['', Validators.required],
    amount: ['', [ Validators.required, Validators.pattern("^[0-9]*$")]],
    initialDeposit: ['', [ Validators.required, Validators.pattern("^[0-9]*$")]],
    cyclePeriod: [this.cyclePeriod, Validators.required],
    cycleNumber: ['', [ Validators.required, Validators.pattern("^[0-9]*$")]],
    paymentChannel: [this.paymentChannel, Validators.required],
  });

  constructor(private readonly formBuilder: FormBuilder, private installmentsService: InstallmentsService, private customersService: CustomersService) {}

  ngOnInit(): void {
    this.getCustomers();
  };

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

    const input = {...this.form.value} as unknown as NewInstallmentDto;    

    this.isSubmittingForm = true;

    this.installmentsService.createInstallment(input).subscribe({
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
