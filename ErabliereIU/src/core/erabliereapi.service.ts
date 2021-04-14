import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { AuthorisationService } from 'src/authorisation/authorisation-service.component';
import { environment } from 'src/environments/environment';
import { Alerte } from 'src/model/alerte';
import { Baril } from 'src/model/baril';
import { Erabliere } from 'src/model/erabliere';

@Injectable({ providedIn: 'root' })
export class ErabliereApi {
    constructor(private _httpClient: HttpClient,
                private _authService: AuthorisationService) { }

    getErablieres(): Promise<Erabliere[]> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Erabliere[]>(environment.apiUrl + '/erablieres', {headers: headers}).toPromise();
        });
    }

    getAlertes(idErabliereSelectionnee:any): Promise<Alerte[]> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Alerte[]>(environment.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes", {headers: headers}).toPromise();
        });
    }

    getBarils(idErabliereSelectionnee:any): Promise<Baril[]> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Baril[]>(environment.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/baril", {headers: headers}).toPromise();
        });
    }
}