import { Component } from '@angular/core';
import { MaterialModule } from 'src/app/material.module';


@Component({
  selector: 'app-transaction-create',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transaction-create.component.html',
})

export class TransactionCreateComponent {}
