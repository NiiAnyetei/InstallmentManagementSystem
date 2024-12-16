import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCustomerModalComponent } from './edit-customer-modal.component';

describe('EditCustomerModalComponent', () => {
  let component: EditCustomerModalComponent;
  let fixture: ComponentFixture<EditCustomerModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditCustomerModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditCustomerModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
