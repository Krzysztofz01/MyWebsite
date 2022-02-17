import { Component, OnInit } from '@angular/core';
import { Image } from 'src/app/models/image.model';
import { ApiCommunicationService } from 'src/app/services/api-communication.service';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-page-gallery',
  templateUrl: './page-gallery.component.html',
  styleUrls: ['./page-gallery.component.css']
})
export class PageGalleryComponent implements OnInit {
  public imagesContainer: Array<Image>;
  public selectPhotographyOnly: boolean;

  constructor(private apiService : ApiCommunicationService,  private cacheService: CacheManagerService) { }

  ngOnInit(): void {
    this.selectPhotographyOnly = true;

    const images = this.cacheService.load(environment.CACHE_IMAGE_ARRAY);
    if(images == null) {
      this.apiService.getGalleryImages()
        .subscribe((response) => {
          response.forEach(x => {
            x.url = `${ environment.apiBaseUrl }${ x.url }`;
          });

          this.imagesContainer = response;
          this.cacheService.delete(environment.CACHE_IMAGE_ARRAY);
          this.cacheService.save({
            key: environment.CACHE_IMAGE_ARRAY,
            data: response,
            expirationMinutes: 60
          });
        });
    } else {
      this.imagesContainer = images;
    }
  }

  public select(photography: boolean): void {
    this.selectPhotographyOnly = photography;
  }

  public check(category: string): boolean {
    const ctg = category.toLowerCase();
    
    if(this.selectPhotographyOnly) {
      if(ctg != "design") return true;
      return false;
    } else {
      if(ctg == "design") return true;
      return false;
    }
  }
}