import { Component } from '@angular/core';
import { MaterialModule } from 'src/app/material.module';


@Component({
  selector: 'app-budget-general',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './budget-general.component.html',
})

export class BudgetGeneralComponent {}
