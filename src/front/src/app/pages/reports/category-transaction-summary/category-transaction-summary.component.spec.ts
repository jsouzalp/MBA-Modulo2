import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryTransactionSummaryComponent } from './category-transaction-summary.component';

describe('CategoryTransactionSummaryComponent', () => {
  let component: CategoryTransactionSummaryComponent;
  let fixture: ComponentFixture<CategoryTransactionSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryTransactionSummaryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryTransactionSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
