import { Component, OnInit } from '@angular/core';
import { CacheManagerService } from './../../services/cache-manager.service';
import { ApiCommunicationService } from 'src/app/services/api-communication.service';
import { Project } from './../../models/project.model';
import { LocalStorageOptions } from './../../models/local-storage-options.model';
import { environment } from 'src/environments/environment';

const CACHE_KEY: string = "PROJECTS";

@Component({
  selector: 'project-section',
  templateUrl: './project-section.component.html',
  styleUrls: ['./project-section.component.css']
})
export class ProjectSectionComponent implements OnInit {
  projects: Array<Project>;

  constructor(private apiService : ApiCommunicationService,  private cacheService: CacheManagerService) {
    this.projects = new Array<Project>();

    const cachedData = this.cacheService.load(CACHE_KEY);
    if(cachedData == null) {
      this.cacheService.delete(CACHE_KEY);
      const options : LocalStorageOptions = { key: CACHE_KEY, data: new Array<Project>(), expirationMinutes: environment.cacheMinutesGithub }
      
      this.apiService.getGithubProjects().toPromise().then(data => {
        for(let key in data) {
          if(data.hasOwnProperty(key)) {
            const element : Project = data[key];
            element.projectCreated = new Date(element.projectCreated).toDateString();
            element.projectUpdated = new Date(element.projectUpdated).toDateString();
            
            this.projects.push(data[key]);
            options.data.push(data[key]);
          } 
        }
        this.cacheService.save(options);
      });
    } else {
      this.projects = cachedData;
    }
  }

  ngOnInit(): void { }
}
