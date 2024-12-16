import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddInstallmentModalComponent } from './add-installment-modal.component';

describe('AddInstallmentModalComponent', () => {
  let component: AddInstallmentModalComponent;
  let fixture: ComponentFixture<AddInstallmentModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddInstallmentModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddInstallmentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
