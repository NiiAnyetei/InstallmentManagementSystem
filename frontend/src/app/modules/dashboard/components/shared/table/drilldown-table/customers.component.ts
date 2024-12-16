import { Component, inject, OnInit, signal } from '@angular/core';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Dialog, DialogModule } from '@angular/cdk/dialog';
import { AddCustomerModalComponent } from '../../../../components/customers/modal/add-customer-modal/add-customer-modal.component';
import { NgxPaginationModule, PaginationInstance } from 'ngx-pagination';
import { CommonModule, NgClass } from '@angular/common';
import { CustomersService } from 'src/app/core/services/customers/customers.service';
import { components, paths } from 'src/app/core/models/models';
import { getPageRange, handleRequestError, handleRequestSuccess } from 'src/app/core/utils/custom-functions';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CdkMenu, CdkMenuItem, CdkMenuTrigger } from '@angular/cdk/menu';
import { DialogClosedResult, LoadState } from 'src/app/core/models/custom.model';
import { debounceTime, distinctUntilChanged } from 'rxjs';

type CustomersQuery = paths['/api/customers']['get']['parameters']['query'];
type CustomerDto = components['schemas']['CustomerDto'];
type CustomerDtoVM = CustomerDto & { detailsToggled?: boolean, menuToggled?: boolean };

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [
    AngularSvgIconModule,
    FormsModule,
    DialogModule,
    NgxPaginationModule,
    CommonModule,
    NgClass,
    CdkMenuTrigger, 
    CdkMenu, 
    CdkMenuItem,
    FormsModule,
    ReactiveFormsModule
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
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
})
export class CustomersComponent implements OnInit {
  dialog = inject(Dialog);
  customers = signal<CustomerDtoVM[]>([]);

  itemsPerPage: number = 10;
  currentPage: number = 1;
  totalItems: number = 0;

  loadState: LoadState = 'Loading';
  isFiltersOpen: boolean = false;
  submitted: boolean = false;
  isSubmittingForm: boolean = false;

  searchForm!: FormGroup;
  filtersForm!: FormGroup;

  phoneNumber: string = '';
  email: string = '';
  identificationNumber: string = '';

  config: PaginationInstance = {
    id: 'customers',
    itemsPerPage: this.itemsPerPage,
    currentPage: this.currentPage,
    totalItems: this.totalItems
  };

  onPageChange(number: number) {
    this.config.currentPage = number;
    this.currentPage = number;
    this.getCustomers();
  }

  onPageBoundsCorrection(number: number) {
    this.config.currentPage = number;
    this.currentPage = number;
    this.getCustomers();
  }

  onItemsPerPageChange(event: any) {
    const number = Number((event.target as HTMLInputElement).value);
    this.config.itemsPerPage = number;
    this.itemsPerPage = number;
    this.getCustomers();
  }

  constructor(private customersService: CustomersService, private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.initSearchForm();
    this.setupSearchListener();
    this.initFiltersForm();
    this.getCustomers();
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
        this.getCustomers();
      });
  }

  initSearchForm(): void {
    this.searchForm = this.formBuilder.group({
      searchTerm: ['']
    });
  }
  
  initFiltersForm(): void {
    this.filtersForm = this.formBuilder.group({
      phoneNumber: ['', [ Validators.pattern("^[0-9]*$"), Validators.minLength(9), Validators.maxLength(9)]],
      email: ['', [Validators.email]],
      identificationNumber: [''],
    });
  }

  getCustomers() {
    this.isFiltersOpen = false;
    this.loadState = 'Loading';

    const searchTerm = this.searchForm.get('searchTerm')?.value;
    const phoneNumber = this.filtersForm.get('phoneNumber')?.value;
    const email = this.filtersForm.get('email')?.value;
    const identificationNumber = this.filtersForm.get('identificationNumber')?.value;

    let query: CustomersQuery = {
      fullName: searchTerm,
      phoneNumber: phoneNumber,
      email: email,
      identificationNumber: identificationNumber,
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.customersService.getCustomers(query).subscribe({
      next: (data) => {
        this.config.totalItems = data.count;
        this.customers.set(data.items);
        this.loadState = 'Loaded';
      },
      error: (error) => {
        this.loadState = 'Error';
        handleRequestError(error)},
    });
  }

  openCreateModal() {
    const dialogRef = this.dialog.open<DialogClosedResult>(AddCustomerModalComponent);

    dialogRef.closed.subscribe((result) => {
      if(result?.isSuccess){
        handleRequestSuccess({message: "Operation successful."});
        this.getCustomers();
      }
    });
  }
  
  openEditModal(customer: CustomerDtoVM) {
    const dialogRef = this.dialog.open<DialogClosedResult>(AddCustomerModalComponent, {
      data: customer as CustomerDto,
    });

    dialogRef.closed.subscribe((result) => {
      if(result?.isSuccess){
        handleRequestSuccess({message: "Operation successful."});
        this.getCustomers();
      }
    });
  }

  toggleFilters(): void {
    this.isFiltersOpen = !this.isFiltersOpen;
  }

  toggleAllDetails(event: Event) {
    const value = (event.target as HTMLInputElement).checked;
    this.customers.update((customers) => {
      return customers.map((customer) => {
        return { ...customer, toggle: value };
      });
    });
  }

  toggleDetails(selectedCustomer: CustomerDtoVM) {
    let toggled = (selectedCustomer.detailsToggled = !selectedCustomer.detailsToggled);
    this.customers.update((customers) =>
      customers.map((customer) =>
        customer.id === selectedCustomer.id ? { ...selectedCustomer, detailsToggled: toggled } : customer,
      ),
    );
  }
  
  toggleItemMenu(selectedCustomer: CustomerDtoVM) {
    let toggled = (selectedCustomer.menuToggled = !selectedCustomer.menuToggled);
    this.customers.update((customers) =>
      customers.map((customer) =>
        customer.id === selectedCustomer.id ? { ...selectedCustomer, menuToggled: toggled } : customer,
      ),
    );
  }

  resetFilters() {
    this.filtersForm.reset({
      phoneNumber: '',
      email: '',
      identificationNumber: '',
    });
  }

  getDisplayRange(): { start: number; end: number } {
      const range = getPageRange(this.totalItems, this.currentPage, this.itemsPerPage);
      return range;
  }
}
