import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryTransactionComponent } from './category-transaction.component';

describe('CategoryTransactionComponent', () => {
  let component: CategoryTransactionComponent;
  let fixture: ComponentFixture<CategoryTransactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryTransactionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
