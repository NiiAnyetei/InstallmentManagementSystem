import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeleteInstallmentComponent } from './confirm-delete-installment.component';

describe('ConfirmDeleteInstallmentComponent', () => {
  let component: ConfirmDeleteInstallmentComponent;
  let fixture: ComponentFixture<ConfirmDeleteInstallmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmDeleteInstallmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmDeleteInstallmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
