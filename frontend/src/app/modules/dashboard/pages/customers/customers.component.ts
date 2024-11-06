import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { User } from '../../models/user.model';
import { FormsModule } from '@angular/forms';
import { TableHeaderComponent } from '../../components/table-header/table-header.component';
import { TableFooterComponent } from '../../components/table-footer/table-footer.component';
import { TableRowComponent } from '../../components/table-row/table-row.component';
import { TableActionComponent } from '../../components/table-action/table-action.component';
import { toast } from 'ngx-sonner';
import { Dialog, DialogModule } from '@angular/cdk/dialog';
import { AddCustomerModalComponent } from '../../components/customers/modal/add-customer-modal/add-customer-modal.component';
import { NgxPaginationModule, PaginationInstance } from 'ngx-pagination';
import { CommonModule } from '@angular/common';
import { CustomersService } from 'src/app/core/services/customers/customers.service';
import { components, paths } from 'src/app/core/models/models';
import { handleRequestError } from 'src/app/core/utils/custom-functions';

type CustomersQuery = paths['/api/customers']['get']['parameters']['query']
type CustomersDto = components['schemas']['CustomersDto'];

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [
    AngularSvgIconModule,
    FormsModule,
    TableHeaderComponent,
    TableFooterComponent,
    TableRowComponent,
    TableActionComponent,
    DialogModule,
    NgxPaginationModule,
    CommonModule
  ],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss'
})
export class CustomersComponent implements OnInit {
  dialog = inject(Dialog);
  customers = signal<CustomersDto>({
    items: [],
    count: 0
  });
  public filter: string = '';
  public maxSize: number = 7;
  public directionLinks: boolean = true;
  public autoHide: boolean = false;
  public responsive: boolean = false;
  public config: PaginationInstance = {
    id: 'customers',
    itemsPerPage: 1,
    currentPage: 1,
    // totalItems: 100
  };
  public labels: any = {
    previousLabel: '',
    nextLabel: '',
    screenReaderPaginationLabel: 'Pagination',
    screenReaderPageLabel: 'page',
    screenReaderCurrentLabel: `You're on page`
  };
  public eventLog: string[] = [];

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

  constructor(private http: HttpClient, private customersService: CustomersService) {}

  // public toggleUsers(checked: boolean) {
  //   this.customers.update((customers) => {
  //     return customers.items.map((customer) => {
  //       return { ...customer, selected: checked };
  //     });
  //   });
  // }

  ngOnInit() {
    this.getCustomers();
  }

  ngOnDestroy(): void {
  }

  getCustomers(query?: CustomersQuery){
    this.customersService.getCustomers().subscribe({
      next: (data) => this.customers.set(data),
      error: (error) => handleRequestError(error),
    });
  }

  openModal() {
    const dialogRef = this.dialog.open<string>(AddCustomerModalComponent, {
      // width: '250px',
      // data: {name: this.name, animal: this.animal},
    });

    dialogRef.closed.subscribe(result => {
      console.log('The dialog was closed');
      // this.animal = result;
    });
  }
}
