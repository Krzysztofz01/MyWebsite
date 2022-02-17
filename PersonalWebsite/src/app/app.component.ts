import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent{
  title = 'PersonalWebsite';

  constructor(private router: Router) {}
  
  public checkIndex(): boolean {
    if(this.router.url == "/") return true;
    return false;
  }
}
