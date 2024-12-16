import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PagesComponent } from './pages.component';
import { CustomersComponent } from '../dashboard/pages/customers/customers.component';
import { InstallmentsComponent } from '../dashboard/pages/installments/installments.component';
import { BillsComponent } from '../dashboard/pages/bills/bills.component';
import { PaymentsComponent } from '../dashboard/pages/payments/payments.component';

const routes: Routes = [
  // { path: '', component: PagesComponent },
  { path: 'customers', component: CustomersComponent },
  { path: 'installments', component: InstallmentsComponent },
  { path: 'payments', component: PaymentsComponent },
  { path: 'bills', component: BillsComponent },
  { path: '**', redirectTo: 'errors/404' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
