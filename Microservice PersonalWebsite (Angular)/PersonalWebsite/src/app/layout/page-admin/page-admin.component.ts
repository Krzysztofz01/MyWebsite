import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { ImageDisplay } from 'src/app/models/image-display.model';
import { ProjectDisplay } from 'src/app/models/project-display.model';
import { ApiCommunicationService } from 'src/app/services/api-communication.service';

@Component({
  selector: 'app-page-admin',
  templateUrl: './page-admin.component.html',
  styleUrls: ['./page-admin.component.css']
})
export class PageAdminComponent implements OnInit {
  public galleryUploadGroup: FormGroup;
  public projects: Array<ProjectDisplay>;
  public images: Array<ImageDisplay>;

  constructor(private authService: AuthService, private apiSerivce: ApiCommunicationService, private router: Router) { }

  ngOnInit(): void {
    this.galleryUploadGroup = new FormGroup({
      name: new FormControl('', [ Validators.required ]),
      category: new FormControl('', [ Validators.required ]),
      image: new FormControl('', [ Validators.required ])
    });

    this.apiSerivce.projectShowDisplay(this.authService.getToken())
      .subscribe((res) => {
        res.forEach(x => {
          if(x.imageUrl == "default") {
            x.imageUrl = "Is not set!";
          } else {
            x.imageUrl = "Is set!";
          }
        });
        this.projects = res;
      });

    this.apiSerivce.imageShowDisplay(this.authService.getToken())
      .subscribe((res) => {
        this.images = res; 
      });
  }

  public updateProjectImage(e: Event, id: number): void {
    const file = (e.target as HTMLInputElement).files[0];

    this.apiSerivce.setProjectImage(id, file, this.authService.getToken())
        .subscribe((res) => {
          this.projects.find(x => x.id == id).imageUrl = "Is set!";
        },
        (err) => {
          console.error(err);
        });
  }

  public projectDisplay(id: number): void {
    const newState = !this.projects.find(x => x.id == id).display;
    this.apiSerivce.projectChangeDisplay(id, newState, this.authService.getToken())
    .subscribe(() => {
      this.projects.find(x => x.id == id).display = newState;
    });
  }

  public imageDisplay(id: number): void {
    const newState = !this.images.find(x => x.id == id).display;
    this.apiSerivce.imageChangeDisplay(id, newState, this.authService.getToken())
      .subscribe(() => {
        this.images.find(x => x.id == id).display = newState;
      });
  }

  public onGalleryImageSelect(e: Event): void {
    const file = (e.target as HTMLInputElement).files[0];
    this.galleryUploadGroup.get('image').setValue(file);
  }

  public gallerySubmit(): void {
    if(this.galleryUploadGroup.valid) {
      this.apiSerivce.uploadGalleryImage(
        this.galleryUploadGroup.get('name').value,
        this.galleryUploadGroup.get('category').value,
        this.galleryUploadGroup.get('image').value,
        this.authService.getToken()
      ).subscribe(() => {
        this.galleryUploadGroup.reset();
        this.apiSerivce.imageShowDisplay(this.authService.getToken())
          .subscribe((res) => {
            this.images = res; 
          });
      });
    }
  }

  public logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
