import { Injectable } from '@angular/core';
import { CanActivate, Router, CanLoad } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PageAdminGuard implements CanActivate, CanLoad {
  
  constructor(private authService: AuthService, private router: Router) { }
  
  canActivate(): boolean {
    return this.canLoad();
  }

  canLoad(): boolean {
    if(!this.authService.isLoggenIn()) {
      this.router.navigate(['/auth']);
      return false;
    }
    return true;
  }
}
