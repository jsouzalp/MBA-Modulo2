import { Component } from '@angular/core';
import { MaterialModule } from 'src/app/material.module';


@Component({
  selector: 'app-budget-by-category',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './budget-by-category.component.html',
})

export class BudgetByCategoryComponent {}
