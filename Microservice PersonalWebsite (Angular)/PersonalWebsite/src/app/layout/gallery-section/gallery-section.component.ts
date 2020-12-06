import { Component, OnInit } from '@angular/core';
import { Image } from './../../models/image.model';
import { LocalStorageOptions } from './../../models/local-storage-options.model';
import { ApiCommunicationService} from './../../services/api-communication.service';
import { CacheManagerService } from './../../services/cache-manager.service';
import { environment } from 'src/environments/environment';

const CACHE_KEY : string = "GALLERY";

@Component({
  selector: 'gallery-section',
  templateUrl: './gallery-section.component.html',
  styleUrls: ['./gallery-section.component.css']
})
export class GallerySectionComponent implements OnInit {
  public images = new Array<Image>();

  constructor(private apiService: ApiCommunicationService, private cacheService: CacheManagerService) {
    const cachedData = this.cacheService.load(CACHE_KEY);
    if(cachedData == null) {
      this.cacheService.delete(CACHE_KEY);
      const options : LocalStorageOptions = { key: CACHE_KEY, data: new Array<Image>(), expirationMinutes: environment.cacheMinutesGallery }
      
      this.apiService.getGalleryImages().toPromise().then(data => {
        for(let key in data) {
          if(data.hasOwnProperty(key)) {         
            this.images.push(data[key]);
            options.data.push(data[key]);
          } 
        }
        this.cacheService.save(options);
      });
    } else {
      this.images = cachedData;
    }
  }

  ngOnInit(): void { }
}
