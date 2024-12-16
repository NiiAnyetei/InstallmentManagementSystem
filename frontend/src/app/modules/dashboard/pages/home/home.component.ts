import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { FormGroup, FormsModule } from '@angular/forms';
import { toast } from 'ngx-sonner';
import { RouterModule } from '@angular/router';
import { NgxPaginationModule, PaginationInstance } from 'ngx-pagination';
import { LoadState } from 'src/app/core/models/custom.model';
import { CommonModule } from '@angular/common';
import { components, paths } from 'src/app/core/models/models';
import { CustomersService } from 'src/app/core/services/customers/customers.service';
import { getPageRange, handleRequestError } from 'src/app/core/utils/custom-functions';
import { InitialsPipe } from "../../../../core/pipe/initials.pipe";
import { ThousandSuffixesPipe } from 'src/app/core/pipe/thousand-suffix.pipe';
import { MetricsService } from 'src/app/core/services/metrics/metrics.service';

type CustomersQuery = paths['/api/customers']['get']['parameters']['query'];
type CustomerDto = components['schemas']['CustomerDto'];
type CustomerDtoVM = CustomerDto & { detailsToggled?: boolean, menuToggled?: boolean };
type MetricDto = components['schemas']['MetricDto'];

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    AngularSvgIconModule,
    FormsModule,
    RouterModule,
    NgxPaginationModule,
    CommonModule,
    InitialsPipe,
    ThousandSuffixesPipe
],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  customers = signal<CustomerDtoVM[]>([]);
  metrics = signal<MetricDto[]>([]);

  itemsPerPage: number = 10;
  currentPage: number = 1;
  totalItems: number = 0;

  metricsLoadState: LoadState = 'Loading';
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

  metricsItems = [
    {
      svg: '<svg viewBox="0 0 24 24" width="24" height="24" stroke="currentColor" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round" class="size-8"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>',
      href: '/customers'
    },
    {
      svg: '<svg viewBox="0 0 24 24" width="24" height="24" stroke="currentColor" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round" class="size-8"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>',
      href: '/customers'
    },
    {
      svg: '<svg viewBox="0 0 24 24" width="24" height="24" stroke="currentColor" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round" class="size-8"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>',
      href: '/customers'
    },
    {
      svg: '<svg viewBox="0 0 24 24" width="24" height="24" stroke="currentColor" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round" class="size-8"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>',
      href: '/customers'
    }
  ]

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

  constructor(private customersService: CustomersService, private metricsService: MetricsService) {}

  ngOnInit() {
    this.getDashboardMetrics();
    this.getCustomers();
  }

  ngOnDestroy(): void {
  }

  getDashboardMetrics() {
    this.metricsLoadState = 'Loading';

    this.metricsService.getDashboardMetrics().subscribe({
      next: (data) => {
        this.metrics.set(data);
        this.metricsLoadState = 'Loaded';
      },
      error: (error) => {
        this.metricsLoadState = 'Error';
        handleRequestError(error)},
    });
  }

  getCustomers() {
    this.isFiltersOpen = false;
    this.loadState = 'Loading';

    // const searchTerm = this.searchForm.get('searchTerm')?.value;
    // const phoneNumber = this.filtersForm.get('phoneNumber')?.value;
    // const email = this.filtersForm.get('email')?.value;
    // const identificationNumber = this.filtersForm.get('identificationNumber')?.value;

    let query: CustomersQuery = {
      // fullName: searchTerm,
      // phoneNumber: phoneNumber,
      // email: email,
      // identificationNumber: identificationNumber,
      limit: this.itemsPerPage,
      offset: (this.currentPage - 1) * this.itemsPerPage,
    }

    this.customersService.getCustomers(query).subscribe({
      next: (data) => {
        this.config.totalItems = data.count;
        this.totalItems = data.count;
        this.customers.set(data.items);
        this.loadState = 'Loaded';
      },
      error: (error) => {
        this.loadState = 'Error';
        handleRequestError(error)},
    });
  }

  getDisplayRange(): { start: number; end: number } {
      const range = getPageRange(this.totalItems, this.currentPage, this.itemsPerPage);
      return range;
  }
}
