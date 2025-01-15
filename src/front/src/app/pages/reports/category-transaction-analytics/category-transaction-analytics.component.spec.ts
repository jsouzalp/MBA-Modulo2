import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryTransactionAnalyticsComponent } from './category-transaction-analytics.component';

describe('CategoryTransactionAnalyticsComponent', () => {
  let component: CategoryTransactionAnalyticsComponent;
  let fixture: ComponentFixture<CategoryTransactionAnalyticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryTransactionAnalyticsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryTransactionAnalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
