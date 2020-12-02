import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Image } from './../models/image.model';
import { Project } from './../models/project.model';
import { environment } from './../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiCommunicationService {
  private baseApiUrl : string = environment.apiBaseUrl;
  private galleryEndpointUrl : string = "/api/gallery";
  private projectsEndpointUrl : string = "/api/github";

  constructor(private httpClient: HttpClient) { }

  public getGalleryImages() : Observable<Array<Image>> {
    return this.httpClient
      .get<Array<Image>>(this.baseApiUrl + this.galleryEndpointUrl);
  }

  public getGithubProjects() : Observable<Array<Project>> {
    return this.httpClient
    .get<Array<Project>>(this.baseApiUrl + this.projectsEndpointUrl);
  }
}
