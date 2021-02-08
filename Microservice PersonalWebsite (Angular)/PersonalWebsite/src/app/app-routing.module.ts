import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageGalleryComponent } from './layout/page-gallery/page-gallery.component';
import { PageMainComponent } from './layout/page-main/page-main.component';

const routes: Routes = [
  { path: '', component: PageMainComponent },
  { path: 'gallery', component: PageGalleryComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
