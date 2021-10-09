import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { EnvironmentService } from 'src/environments/environment.service';
import { Alerte } from 'src/model/alerte';
import { Baril } from 'src/model/baril';
import { Dompeux } from 'src/model/dompeux';
import { Donnee } from 'src/model/donnee';
import { DonneeCapteur } from 'src/model/donneeCapteur';
import { Erabliere } from 'src/model/erabliere';

@Injectable({ providedIn: 'root' })
export class ErabliereApi {
    private _authService: IAuthorisationSerivce

    constructor(private _httpClient: HttpClient,
                authFactoryService: AuthorisationFactoryService,
                private _environmentService: EnvironmentService) 
                { 
                    this._authService = authFactoryService.getAuthorisationService();
                }

    getErablieresDashboard(): Promise<HttpResponse<Erabliere[]>> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Erabliere[]>(this._environmentService.apiUrl + '/erablieres/dashboard', { headers: headers, observe: 'response' }).toPromise();
        });
    }

    getErablieres(): Promise<Erabliere[]> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Erabliere[]>(this._environmentService.apiUrl + '/erablieres', {headers: headers}).toPromise();
        });
    }

    getErablieresExpandCapteurs(): Promise<Erabliere[]> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Erabliere[]>(this._environmentService.apiUrl + '/erablieres?$expand=Capteurs($filter=afficherCapteurDashboard eq true)', {headers: headers}).toPromise();
        });
    }

    getAlertes(idErabliereSelectionnee:any): Promise<Alerte[]> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Alerte[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes", {headers: headers}).toPromise();
        });
    }

    postAlerte(idErabliereSelectionnee:any, alerte:Alerte): Promise<any> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.post<Alerte>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes", alerte, {headers: headers}).toPromise();
        });
    }

    putAlerte(idErabliereSelectionnee:any, alerte:Alerte): Promise<any> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.put<Alerte>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes", alerte, {headers: headers}).toPromise();
        });
    }

    getBarils(idErabliereSelectionnee:any): Promise<Baril[]> {
        return this._authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this._httpClient.get<Baril[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/baril", {headers: headers}).toPromise();
        });
    }

    getDonnees(idErabliereSelectionnee:any, debutFiltre:string, finFiltre:string, xddr?:any): Promise<HttpResponse<Donnee[]>> {
        return this._authService.getAccessToken().then(token => {
            let headers = new HttpHeaders()
                                .set('Authorization', `Bearer ${token}`);

            if (xddr != null) {
                headers = headers.set('x-ddr', xddr);
            }

            var httpCall = this._httpClient.get<Donnee[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/donnees?dd=" + debutFiltre + "&df=" + finFiltre, {headers: headers, observe: 'response'})
            
            return httpCall.toPromise();
        });
    }

    getDonneesCapteur(idCapteur:any, debutFiltre:string, finFiltre: string, xddr?: any): Promise<HttpResponse<DonneeCapteur[]>> {
        return this._authService.getAccessToken().then(token => {
            let headers = new HttpHeaders()
                                .set('Authorization', `Bearer ${token}`);

            if (xddr != null) {
                headers = headers.set('x-ddr', xddr);
            }

            var httpCall = this._httpClient.get<DonneeCapteur[]>(this._environmentService.apiUrl + '/capteurs/' + idCapteur + "/DonneesCapteur?dd=" + debutFiltre + "&df=" + finFiltre, {headers: headers, observe: 'response'})
            
            return httpCall.toPromise();
        });
    }

    getDompeux(idErabliereSelectionnee:any, debutFiltre:string, finFiltre:string, xddr?:any): Promise<HttpResponse<Dompeux[]>> {
        return this._authService.getAccessToken().then(token => {
            let headers = new HttpHeaders()
                                .set('Authorization', `Bearer ${token}`);

            if (xddr != null) {
                headers = headers.set('x-ddr', xddr);
            }

            var httpCall = this._httpClient.get<Dompeux[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/dompeux?dd=" + debutFiltre + "&df=" + finFiltre, {headers: headers, observe: 'response'})
            
            return httpCall.toPromise();
        });
    }
}