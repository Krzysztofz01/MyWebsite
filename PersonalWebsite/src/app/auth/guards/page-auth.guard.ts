import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PageAuthGuard implements CanActivate {
  
  constructor(private authService: AuthService, private router: Router) { }
  
  canActivate(): boolean {
    if(this.authService.isLoggenIn()) {
      this.router.navigate(['/admin']);
      return false;
    }
    return true;
  }
}
