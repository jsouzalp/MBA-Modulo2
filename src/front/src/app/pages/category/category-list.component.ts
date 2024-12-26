import { Component } from '@angular/core';
import { MaterialModule } from 'src/app/material.module';


@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './category-list.component.html',
})

export class CategoryListComponent {}
