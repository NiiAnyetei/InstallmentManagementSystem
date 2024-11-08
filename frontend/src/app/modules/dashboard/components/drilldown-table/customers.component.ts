import { Component, inject, OnInit, signal } from '@angular/core';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { FormGroup, FormsModule } from '@angular/forms';
import { Dialog, DialogModule } from '@angular/cdk/dialog';
import { AddCustomerModalComponent } from '../../components/customers/modal/add-customer-modal/add-customer-modal.component';
import { NgxPaginationModule, PaginationInstance } from 'ngx-pagination';
import { CommonModule, NgClass } from '@angular/common';
import { CustomersService } from 'src/app/core/services/customers/customers.service';
import { components, paths } from 'src/app/core/models/models';
import { handleRequestError } from 'src/app/core/utils/custom-functions';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CdkMenu, CdkMenuItem, CdkMenuTrigger } from '@angular/cdk/menu';

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
    CdkMenuItem
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
  filter: string = '';
  maxSize: number = 7;
  directionLinks: boolean = true;
  autoHide: boolean = false;
  responsive: boolean = false;
  config: PaginationInstance = {
    id: 'customers',
    itemsPerPage: 1,
    currentPage: 1,
    // totalItems: 100
  };
  labels: any = {
    previousLabel: '',
    nextLabel: '',
    screenReaderPaginationLabel: 'Pagination',
    screenReaderPageLabel: 'page',
    screenReaderCurrentLabel: `You're on page`,
  };
  eventLog: string[] = [];
  searchForm: FormGroup | undefined;
  isFiltersOpen: boolean = false;

  onPageChange(number: number) {
    this.logEvent(`pageChange(${number})`);
    this.config.currentPage = number;
  }

  onPageBoundsCorrection(number: number) {
    this.logEvent(`pageBoundsCorrection(${number})`);
    this.config.currentPage = number;
  }

  private logEvent(message: string) {
    // this.eventLog.unshift(`${new Date().toISOString()}: ${message}`);
    console.log(message);
  }

  constructor(private customersService: CustomersService) {}

  ngOnInit() {
    this.getCustomers();
  }

  ngOnDestroy(): void {}

  getCustomers(query?: CustomersQuery) {
    this.customersService.getCustomers().subscribe({
      next: (data) => {
        this.config.totalItems = data.count;
        this.customers.set(data.items);
      },
      error: (error) => handleRequestError(error),
    });
  }

  openCreateModal() {
    const dialogRef = this.dialog.open<string>(AddCustomerModalComponent, {
      // width: '250px',
      // data: {name: this.name, animal: this.animal},
    });

    dialogRef.closed.subscribe((result) => {
      console.log('The dialog was closed');
      // this.animal = result;
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
}
