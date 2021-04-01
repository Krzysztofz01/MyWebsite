import { Component, OnInit } from '@angular/core';
import { CacheManagerService } from './../../services/cache-manager.service';
import { ApiCommunicationService } from 'src/app/services/api-communication.service';
import { Project } from './../../models/project.model';
import { environment } from 'src/environments/environment';
import { env } from 'process';

@Component({
  selector: 'project-section',
  templateUrl: './project-section.component.html',
  styleUrls: ['./project-section.component.css']
})
export class ProjectSectionComponent implements OnInit {
  public projectsContainer: Array<Project> = [];

  constructor(private apiService : ApiCommunicationService,  private cacheService: CacheManagerService) {}

  ngOnInit(): void {
    const projects = this.cacheService.load(environment.CACHE_GITHUB_ARRAY);
    if(projects == null) {
      this.apiService.getGithubProjects()
        .subscribe((response) => {
          response.forEach(x => {
            const liveProject = environment.liveProjects.find(p => p.name == x.name.toLowerCase());
            if(liveProject != null) {
              x.liveUrl = liveProject.url;
            } else {
              x.liveUrl = null
            }
            
            x.imageUrl = `${ environment.apiBaseUrl }${ x.imageUrl }`;
          });

          this.projectsContainer = response;
          this.cacheService.delete(environment.CACHE_GITHUB_ARRAY);
          this.cacheService.save({
            key: environment.CACHE_GITHUB_ARRAY,
            data: response,
            expirationMinutes: 60
          });
        });
    } else {
      this.projectsContainer = projects;
    }
  }

  public formatDate(date: Date): string {
    date = new Date(date);
    const d = date.getDate();
    const m = date.getMonth() + 1;
    const y = date.getFullYear();
    const H = date.getHours();
    const M = date.getMinutes();

    return `${ (d <= 9) ? '0' + d.toString() : d }.${ (m <= 9) ? '0' + m.toString() : m }.${ y } ${ (H <= 9) ? '0' + H.toString() : H }:${ (M <= 9) ? '0' + M.toString() : M }`;
  }
}
