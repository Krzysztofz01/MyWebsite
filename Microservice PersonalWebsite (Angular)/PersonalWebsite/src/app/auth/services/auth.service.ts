import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Jwtoken } from 'src/app/models/jwtoken.model';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { environment } from 'src/environments/environment';
import { catchError, mapTo, tap } from 'rxjs/operators';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private cacheKey: string = environment.CAHCE_JWT;

  constructor(private httpClient: HttpClient, private cacheService: CacheManagerService) { }

  public login(email: string, password: string): Observable<boolean> {
    return this.httpClient.post<Jwtoken>(this.url(), { email, password })
      .pipe(
        tap(token => this.setAuthToken(token.bearerToken)),
        mapTo(true),
        catchError(err => {
          console.error(err);
          return of(false);
        })
      );
  }

  public isLoggenIn(): boolean {
    if(this.cacheService.load(this.cacheKey) == null) {
      return false;
    }
    return true;
  }

  public getToken(): string {
    const { token } = this.cacheService.load(this.cacheKey);
    return token;
  }

  public logout(): void {
    this.cacheService.delete(this.cacheKey);
  }

  private setAuthToken(token: string): void {
    const payload: any = jwt_decode(token);
    const tokenExpiration = (payload.exp - payload.iat) / 60;

    this.cacheService.delete(this.cacheKey);
    this.cacheService.save({ key: this.cacheKey,  data: { token }, expirationMinutes: tokenExpiration});
  }

  private url(): string {
    return `${ environment.apiBaseUrl }/api/auth/login`;
  }
}
