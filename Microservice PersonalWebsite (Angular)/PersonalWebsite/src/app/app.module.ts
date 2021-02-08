import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { ApiCommunicationService } from './services/api-communication.service';
import { ProjectSectionComponent } from './layout/project-section/project-section.component';
import { CommercialSectionComponent } from './layout/commercial-section/commercial-section.component';
import { ArtSectionComponent } from './layout/art-section/art-section.component';
import { SocialmediaSectionComponent } from './layout/socialmedia-section/socialmedia-section.component';
import { AnimatedDigitComponent } from './layout/animated-digit/animated-digit.component';
import { PageMainComponent } from './layout/page-main/page-main.component';
import { AppRoutingModule } from './app-routing.module';
import { PageGalleryComponent } from './layout/page-gallery/page-gallery.component';

@NgModule({
  declarations: [
    AppComponent,
    ProjectSectionComponent,
    CommercialSectionComponent,
    ArtSectionComponent,
    SocialmediaSectionComponent,
    AnimatedDigitComponent,
    PageMainComponent,
    PageGalleryComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [ApiCommunicationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
