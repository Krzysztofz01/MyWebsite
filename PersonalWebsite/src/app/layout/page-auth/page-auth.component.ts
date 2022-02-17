import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';

@Component({
  selector: 'app-page-auth',
  templateUrl: './page-auth.component.html',
  styleUrls: ['./page-auth.component.css']
})
export class PageAuthComponent implements OnInit {
  public loginFormGroup: FormGroup;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.loginFormGroup = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      password: new FormControl('', [ Validators.required ])
    });
  }

  public loginSubmit(): void {
    if(this.loginFormGroup.valid) {
      this.authService.login(this.loginFormGroup.get('email').value, this.loginFormGroup.get('password').value)
        .subscribe((response) => {
          if(response) {
            this.router.navigate(['/admin']);
          } else {
            console.error("Auth process failed!");
          }
        },
        (error) => {
          console.error(error.name);
        });
    }
  }
}
