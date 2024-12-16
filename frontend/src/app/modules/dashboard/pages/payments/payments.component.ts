import { trigger, state, style, transition, animate } from '@angular/animations';
import { Dialog, DialogModule } from '@angular/cdk/dialog';
import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { NgxPaginationModule, PaginationInstance } from 'ngx-pagination';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { DialogClosedResult, LoadState } from 'src/app/core/models/custom.model';
import { components, paths } from 'src/app/core/models/models';
import { getPageRange, handleRequestError, handleRequestSuccess } from 'src/app/core/utils/custom-functions';
import { EditCustomerModalComponent } from '../../components/customers/modal/edit-customer-modal/edit-customer-modal.component';
import { LoadingComponent } from "../../components/shared/states/loading/loading.component";
import { LoadingErrorComponent } from "../../components/shared/states/loading-error/loading-error.component";
import { AddInstallmentModalComponent } from '../../components/installments/modal/add-installment-modal/add-installment-modal.component';
import { PaymentsService } from 'src/app/core/services/payments/payments.service';

type PaymentsQuery = paths['/api/payments']['get']['parameters']['query'];
type PaymentDto = components['schemas']['PaymentDto'];
type PaymentDtoVM = PaymentDto & { detailsToggled?: boolean, menuToggled?: boolean };

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [
    AngularSvgIconModule,
    FormsModule,
    DialogModule,
    NgxPaginationModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    LoadingComponent,
    LoadingErrorComponent
],
  animations: [
    trigger('openClose', [
      state(
        'open',
        style({
          opacity: 1,
          transform: 'translateY(0)',
          visibility: 'visible',
        }),
      ),
      state(
        'closed',
        style({
          opacity: 0,
          transform: 'translateY(-20px)',
          visibility: 'hidden',
        }),
      ),
      transition('open => closed', [animate('0.2s')]),
      transition('closed => open', [animate('0.2s')]),
    ]),
  ],
  templateUrl: './payments.component.html',
  styleUrl: './payments.component.scss'
})
export class PaymentsComponent {
dialog = inject(Dialog);
  payments = signal<PaymentDtoVM[]>([]);

  itemsPerPage: number = 10;
  currentPage: number = 1;
  totalItems: number = 0;

  loadState: LoadState = 'Loading';
  filtersLoadState: LoadState = 'Loaded';
  isFiltersOpen: boolean = false;
  submitted: boolean = false;
  isSubmittingForm: boolean = false;

  searchForm = this.formBuilder.nonNullable.group({
    searchTerm: ''
  });
  
  filtersForm = this.formBuilder.nonNullable.group({
    item: '',
    from: '',
    to: '',
  });

  phoneNumber: string = '';
  email: string = '';
  identificationNumber: string = '';

  config: PaginationInstance = {
    id: 'payments',
    itemsPerPage: this.itemsPerPage,
    currentPage: this.currentPage,
    totalItems: this.totalItems
  };

  onPageChange(number: number) {
    this.config.currentPage = number;
    this.currentPage = number;
    this.filterData();
  }

  onPageBoundsCorrection(number: number) {
    this.config.currentPage = number;
    this.currentPage = number;
    this.filterData();
  }

  onItemsPerPageChange(event: any) {
    const number = Number((event.target as HTMLInputElement).value);
    this.config.itemsPerPage = number;
    this.itemsPerPage = number;
    this.filterData();
  }

  constructor(private paymentsService: PaymentsService, private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.setupSearchListener();
    this.getData();
  }

  get f() {
    return this.filtersForm.controls;
  }

  ngOnDestroy(): void {}

  setupSearchListener() {
    this.searchForm.get('searchTerm')?.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.currentPage = 1;
        this.filterData();
      });
  }

  getData() {
    this.isFiltersOpen = false;
    this.loadState = 'Loading';

    let query: PaymentsQuery = {
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.paymentsService.getPayments(query).subscribe({
      next: (data) => {
        this.config.totalItems = data.count;
        this.totalItems = data.count;
        this.payments.set(data.items);
        this.loadState = 'Loaded';
      },
      error: (error) => {
        this.loadState = 'Error';
        handleRequestError(error)},
    });
  }
  
  filterData() {
    this.isFiltersOpen = false;
    this.filtersLoadState = 'Loading';

    const searchTerm = this.searchForm.get('searchTerm')?.value;
    const from = this.filtersForm.get('from')?.value;
    const to = this.filtersForm.get('to')?.value;

    let query: PaymentsQuery = {
      customer: searchTerm,
      from: from,
      to: to,
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.paymentsService.getPayments(query).subscribe({
      next: (data) => {
        this.config.totalItems = data.count;
        this.totalItems = data.count;
        this.payments.set(data.items);
        this.filtersLoadState = 'Loaded';
      },
      error: (error) => {
        this.filtersLoadState = 'Error';
        handleRequestError(error)},
    });
  }

  openCreateModal() {
    const dialogRef = this.dialog.open<DialogClosedResult>(AddInstallmentModalComponent);

    dialogRef.closed.subscribe((result) => {
      if(result?.isSuccess){
        handleRequestSuccess({message: "Operation successful."});
        this.resetFilters();
        this.getData();
      }
    });
  }
  
  openEditModal(payment: PaymentDtoVM) {
    let input = payment as PaymentDto;

    const dialogRef = this.dialog.open<DialogClosedResult>(EditCustomerModalComponent, {
      data: input,
    });

    dialogRef.closed.subscribe((result) => {
      if(result?.isSuccess){
        handleRequestSuccess({message: "Operation successful."});
        this.resetFilters();
        this.getData();
      }
    });
  }

  toggleFilters(): void {
    this.isFiltersOpen = !this.isFiltersOpen;
  }

  // toggleAllDetails(event: Event) {
  //   const value = (event.target as HTMLInputElement).checked;
  //   this.customers.update((customers) => {
  //     return customers.map((customer) => {
  //       return { ...customer, toggle: value };
  //     });
  //   });
  // }

  toggleDetails(selectedInstallment: PaymentDtoVM) {
    let toggled = (selectedInstallment.detailsToggled = !selectedInstallment.detailsToggled);
    this.payments.update((payments) =>
      payments.map((payment) =>
        payment.id === selectedInstallment.id ? { ...selectedInstallment, detailsToggled: toggled } : payment,
      ),
    );
  }
  
  // toggleItemMenu(selectedCustomer: CustomerDtoVM) {
  //   let toggled = (selectedCustomer.menuToggled = !selectedCustomer.menuToggled);
  //   this.customers.update((customers) =>
  //     customers.map((customer) =>
  //       customer.id === selectedCustomer.id ? { ...selectedCustomer, menuToggled: toggled } : customer,
  //     ),
  //   );
  // }

  resetFilters() {
    this.filtersForm.reset();
  }

  getDisplayRange(): { start: number; end: number } {
      const range = getPageRange(this.totalItems, this.currentPage, this.itemsPerPage);
      return range;
  }
}
