import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ApiCommunicationService } from './services/api-communication.service';
import { ProjectSectionComponent } from './layout/project-section/project-section.component';
import { GallerySectionComponent } from './layout/gallery-section/gallery-section.component';

@NgModule({
  declarations: [
    AppComponent,
    ProjectSectionComponent,
    GallerySectionComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [ApiCommunicationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
