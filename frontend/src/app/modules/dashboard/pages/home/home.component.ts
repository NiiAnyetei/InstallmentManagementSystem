import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { User } from '../../models/user.model';
import { FormsModule } from '@angular/forms';
import { TableHeaderComponent } from '../../components/table-header/table-header.component';
import { TableFooterComponent } from '../../components/table-footer/table-footer.component';
import { TableRowComponent } from '../../components/table-row/table-row.component';
import { TableActionComponent } from '../../components/table-action/table-action.component';
import { toast } from 'ngx-sonner';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    AngularSvgIconModule,
    FormsModule,
    TableHeaderComponent,
    TableFooterComponent,
    TableRowComponent,
    TableActionComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
  }
}
