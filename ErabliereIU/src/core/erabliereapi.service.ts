import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable, PartialObserver } from 'rxjs';
import { AuthorisationService } from 'src/authorisation/authorisation-service.component';
import { environment } from 'src/environments/environment';
import { Alerte } from 'src/model/alerte';
import { Baril } from 'src/model/baril';
import { Dompeux } from 'src/model/dompeux';
import { Donnee } from 'src/model/donnee';
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

    getDonnees(idErabliereSelectionnee:any, debutFiltre:string, finFiltre:string, xddr?:any): Promise<HttpResponse<Donnee[]>> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

            if (xddr != null) {
                headers.set('x-ddr', xddr);
            }

            var httpCall = this._httpClient.get<Donnee[]>(environment.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/donnees?dd=" + debutFiltre + "&df=" + finFiltre, {headers: headers, observe: 'response'})
            
            return httpCall.toPromise();
        });
    }

    getDompeux(idErabliereSelectionnee:any, debutFiltre:string, finFiltre:string, xddr?:any): Promise<HttpResponse<Dompeux[]>> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

            if (xddr != null) {
                headers.set('x-ddr', xddr);
            }

            var httpCall = this._httpClient.get<Dompeux[]>(environment.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/dompeux?dd=" + debutFiltre + "&df=" + finFiltre, {headers: headers, observe: 'response'})
            
            return httpCall.toPromise();
        });
    }
}