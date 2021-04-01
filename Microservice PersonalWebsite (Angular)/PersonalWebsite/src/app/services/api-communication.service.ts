import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Image } from './../models/image.model';
import { Project } from './../models/project.model';
import { environment } from './../../environments/environment';
import { Observable } from 'rxjs';
import { ImageDisplay } from '../models/image-display.model';
import { ProjectDisplay } from '../models/project-display.model';

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

  public uploadGalleryImage(name: string, category: string, file: File, token: string) : Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const formData = new FormData();
    formData.append("name", name);
    formData.append("category", category);
    formData.append("file", file);

    return this.httpClient
      .post(`${this.baseApiUrl}${this.galleryEndpointUrl}/upload`, formData, { headers });
  }

  public imageChangeDisplay(id: number, display: boolean, token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient
      .post(`${this.baseApiUrl}${this.galleryEndpointUrl}/display`, { id, display }, { headers });
  }

  public imageShowDisplay(token: string): Observable<Array<ImageDisplay>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient
      .get<Array<ImageDisplay>>(`${this.baseApiUrl}${this.galleryEndpointUrl}/display`, { headers });
  }

  public getGithubProjects() : Observable<Array<Project>> {
    return this.httpClient
      .get<Array<Project>>(this.baseApiUrl + this.projectsEndpointUrl);
  }

  public setProjectImage(id: number, file: File, token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const formData = new FormData();
    formData.append("id", id.toString());
    formData.append("File", file);

    return this.httpClient
      .post(`${this.baseApiUrl}${this.projectsEndpointUrl}/image`, formData, { headers });
  }

  public projectChangeDisplay(id: number, display: boolean, token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient
      .post(`${this.baseApiUrl}${this.projectsEndpointUrl}/display`, { id, display }, { headers });
  }

  public projectShowDisplay(token: string): Observable<Array<ProjectDisplay>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient
      .get<Array<ProjectDisplay>>(`${this.baseApiUrl}${this.projectsEndpointUrl}/display`, { headers });
  }
}