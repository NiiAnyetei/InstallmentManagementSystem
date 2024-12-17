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
import { InstallmentsService } from 'src/app/core/services/installments/installments.service';
import { LoadingComponent } from "../../components/shared/states/loading/loading.component";
import { LoadingErrorComponent } from "../../components/shared/states/loading-error/loading-error.component";
import { AddInstallmentModalComponent } from '../../components/installments/modal/add-installment-modal/add-installment-modal.component';
import { BadgesComponent } from "../../components/shared/badges/badges.component";
import { PaymentChannelPipe } from 'src/app/core/pipe/payment-channel.pipe';
import { CyclePeriodPipe } from 'src/app/core/pipe/cycle-period.pipe';

type InstallmentsQuery = paths['/api/installments']['get']['parameters']['query'];
type InstallmentDto = components['schemas']['InstallmentDto'];
type InstallmentStatus = components['schemas']['InstallmentStatus'];
type InstallmentDtoVM = InstallmentDto & { detailsToggled?: boolean, menuToggled?: boolean };

@Component({
  selector: 'app-installments',
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
    LoadingErrorComponent,
    BadgesComponent,
    PaymentChannelPipe,
    CyclePeriodPipe
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
  templateUrl: './installments.component.html',
  styleUrl: './installments.component.scss'
})
export class InstallmentsComponent {
  dialog = inject(Dialog);
  installments = signal<InstallmentDtoVM[]>([]);

  itemsPerPage: number = 10;
  currentPage: number = 1;
  totalItems: number = 0;

  loadState: LoadState = 'Loading';
  filtersLoadState: LoadState = 'Loaded';
  installmentStatus: InstallmentStatus = 'Active';
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
    status: '',
  });

  phoneNumber: string = '';
  email: string = '';
  identificationNumber: string = '';

  config: PaginationInstance = {
    id: 'installments',
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

  constructor(private installmentsService: InstallmentsService, private formBuilder: FormBuilder) {}

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

    let query: InstallmentsQuery = {
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.installmentsService.getInstallments(query).subscribe({
      next: (data) => {
        this.config.totalItems = data.count;
        this.totalItems = data.count;
        this.installments.set(data.items);
        this.loadState = 'Loaded';
        this.filtersLoadState = 'Loaded';
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
    const item = this.filtersForm.get('item')?.value;
    const from = this.filtersForm.get('from')?.value;
    const to = this.filtersForm.get('to')?.value;
    const status = this.filtersForm.get('status')?.value as InstallmentStatus;

    let query: InstallmentsQuery = {
      customer: searchTerm,
      item: item,
      from: from,
      to: to,
      status: status,
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.installmentsService.getInstallments(query).subscribe({
      next: (data) => {
        this.config.totalItems = data.count;
        this.totalItems = data.count;
        this.installments.set(data.items);
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
  
  openEditModal(installment: InstallmentDtoVM) {
    let input = installment as InstallmentDto;

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

  toggleDetails(selectedInstallment: InstallmentDtoVM) {
    let toggled = (selectedInstallment.detailsToggled = !selectedInstallment.detailsToggled);
    this.installments.update((installments) =>
      installments.map((installment) =>
        installment.id === selectedInstallment.id ? { ...selectedInstallment, detailsToggled: toggled } : installment,
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
