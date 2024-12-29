import {
  Component,
  Output,
  EventEmitter,
  Input,
  ViewEncapsulation,
  OnInit,
} from '@angular/core';
import { MaterialModule } from 'src/app/material.module';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { MatButtonModule } from '@angular/material/button';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { UserService } from 'src/app/services/user.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule, NgScrollbarModule, MaterialModule, MatButtonModule],
  templateUrl: './header.component.html',
  encapsulation: ViewEncapsulation.None,
})

export class HeaderComponent implements OnInit {
  @Input() showToggle = true;
  @Input() toggleChecked = false;
  @Output() toggleMobileNav = new EventEmitter<void>();
  @Output() toggleCollapsed = new EventEmitter<void>();
  email: string;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private router: Router, private localStorageUtils: LocalStorageUtils,private loginSevice: UserService) { }
  ngOnInit(): void {
    this.email = this.localStorageUtils.getEmail();
  }

  logout() {


        this.loginSevice.logout()
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.localStorageUtils.clear();
              this.router.navigate(['/login']);
            },
            error: (fail) => {
              console.log('fail', fail);
            }
          });
  }
}
