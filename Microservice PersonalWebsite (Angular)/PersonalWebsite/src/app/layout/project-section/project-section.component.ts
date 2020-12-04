import { Component, OnInit } from '@angular/core';
import { ApiCommunicationService } from 'src/app/services/api-communication.service';
import { Project } from './../../models/project.model';

@Component({
  selector: 'project-section',
  templateUrl: './project-section.component.html',
  styleUrls: ['./project-section.component.css']
})
export class ProjectSectionComponent implements OnInit {
  projects: Array<Project>;

  constructor(private apiService : ApiCommunicationService) {
    this.projects = new Array<Project>();

    this.apiService.getGithubProjects().toPromise().then(data => {
      for(let key in data) {
        if(data.hasOwnProperty(key)) {
          const element : Project = data[key];
          element.projectCreated = new Date(element.projectCreated).toDateString();
          element.projectUpdated = new Date(element.projectUpdated).toDateString();
          
          this.projects.push(data[key]);
        } 
      }
    });

  }

  ngOnInit(): void { }
}
