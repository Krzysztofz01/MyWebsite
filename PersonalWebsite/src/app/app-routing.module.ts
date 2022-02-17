import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageAdminGuard } from './auth/guards/page-admin.guard';
import { PageAuthGuard } from './auth/guards/page-auth.guard';
import { PageAdminComponent } from './layout/page-admin/page-admin.component';
import { PageAuthComponent } from './layout/page-auth/page-auth.component';
import { PageGalleryComponent } from './layout/page-gallery/page-gallery.component';
import { PageMainComponent } from './layout/page-main/page-main.component';

const routes: Routes = [
  { path: '', component: PageMainComponent },
  { path: 'gallery', component: PageGalleryComponent },
  { path: 'auth', component: PageAuthComponent, canActivate: [PageAuthGuard] },
  { path: 'admin', component: PageAdminComponent, canActivate: [PageAdminGuard], canLoad: [PageAdminGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
