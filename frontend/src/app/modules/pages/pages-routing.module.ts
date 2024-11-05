import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PagesComponent } from './pages.component';
import { CustomersComponent } from '../dashboard/pages/customers/customers.component';

const routes: Routes = [
  // { path: '', component: PagesComponent },
  { path: 'customers', component: CustomersComponent },
  { path: 'installments', component: CustomersComponent },
  { path: 'payments', component: CustomersComponent },
  { path: 'bills', component: CustomersComponent },
  { path: '**', redirectTo: 'errors/404' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
