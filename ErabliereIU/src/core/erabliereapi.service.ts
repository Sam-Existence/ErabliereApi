import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { EnvironmentService } from 'src/environments/environment.service';
import { Alerte } from 'src/model/alerte';
import { Baril } from 'src/model/baril';
import { Documentation } from 'src/model/documentation';
import { Dompeux } from 'src/model/dompeux';
import { Donnee } from 'src/model/donnee';
import { DonneeCapteur, PostDonneeCapteur } from 'src/model/donneeCapteur';
import { Erabliere } from 'src/model/erabliere';
import { Note } from 'src/model/note';

@Injectable({ providedIn: 'root' })
export class ErabliereApi {
    private _authService: IAuthorisationSerivce

    constructor(private _httpClient: HttpClient,
                authFactoryService: AuthorisationFactoryService,
                private _environmentService: EnvironmentService) 
                { 
                    this._authService = authFactoryService.getAuthorisationService();
                }

    async getErablieresDashboard(): Promise<HttpResponse<Erabliere[]>> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.get<Erabliere[]>(this._environmentService.apiUrl + '/erablieres/dashboard', { headers: headers, observe: 'response' }).toPromise();
    }

    async getErablieres(): Promise<Erabliere[]> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.get<Erabliere[]>(this._environmentService.apiUrl + '/erablieres', { headers: headers }).toPromise();
    }

    async getErablieresExpandCapteurs(): Promise<Erabliere[]> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`).set('Accept', 'application/json');
        return await this._httpClient.get<Erabliere[]>(this._environmentService.apiUrl + '/erablieres?$expand=Capteurs($filter=afficherCapteurDashboard eq true)', { headers: headers }).toPromise();
    }

    async getAlertes(idErabliereSelectionnee:any): Promise<Alerte[]> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.get<Alerte[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes", { headers: headers }).toPromise();
    }

    async postAlerte(idErabliereSelectionnee:any, alerte:Alerte): Promise<any> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.post<Alerte>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes", alerte, { headers: headers }).toPromise();
    }

    async putAlerte(idErabliereSelectionnee:any, alerte:Alerte): Promise<any> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.put<Alerte>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes", alerte, { headers: headers }).toPromise();
    }

    async deleteAlerte(idErabliereSelectionnee:any, alerteId:any): Promise<any> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.delete(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes/" + alerteId, { headers: headers }).toPromise();
    }

    async getBarils(idErabliereSelectionnee:any): Promise<Baril[]> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.get<Baril[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/baril", { headers: headers }).toPromise();
    }

    async getDonnees(idErabliereSelectionnee:any, debutFiltre:string, finFiltre:string, xddr?:any): Promise<HttpResponse<Donnee[]>> {
        const token = await this._authService.getAccessToken();
        let headers = new HttpHeaders()
            .set('Authorization', `Bearer ${token}`);
        if (xddr != null) {
            headers = headers.set('x-ddr', xddr);
        }
        var httpCall = this._httpClient.get<Donnee[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/donnees?dd=" + debutFiltre + "&df=" + finFiltre, { headers: headers, observe: 'response' });
        return await httpCall.toPromise();
    }

    async getDonneesCapteur(idCapteur:any, debutFiltre:string, finFiltre: string, xddr?: any): Promise<HttpResponse<DonneeCapteur[]>> {
        const token = await this._authService.getAccessToken();
        let headers = new HttpHeaders()
            .set('Authorization', `Bearer ${token}`);
        if (xddr != null) {
            headers = headers.set('x-ddr', xddr);
        }
        var httpCall = this._httpClient.get<DonneeCapteur[]>(this._environmentService.apiUrl + '/capteurs/' + idCapteur + "/DonneesCapteur?dd=" + debutFiltre + "&df=" + finFiltre, { headers: headers, observe: 'response' });
        return await httpCall.toPromise();
    }

    async getDompeux(idErabliereSelectionnee:any, debutFiltre:string, finFiltre:string, xddr?:any): Promise<HttpResponse<Dompeux[]>> {
        const token = await this._authService.getAccessToken();
        let headers = new HttpHeaders()
            .set('Authorization', `Bearer ${token}`);
        if (xddr != null) {
            headers = headers.set('x-ddr', xddr);
        }
        var httpCall = this._httpClient.get<Dompeux[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/dompeux?dd=" + debutFiltre + "&df=" + finFiltre, { headers: headers, observe: 'response' });
        return await httpCall.toPromise();
    }

    async getDocumentations(idErabliereSelectionnee:any): Promise<Documentation[]> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.get<Documentation[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/documentation", { headers: headers }).toPromise();
    }

    async getNotes(idErabliereSelectionnee:any): Promise<Note[]> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.get<Note[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/notes?$orderby=NoteDate desc", { headers: headers }).toPromise();
    }

    async postNote(idErabliereSelectionnee:any, note:Note): Promise<any> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.post<Alerte>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/notes", note, { headers: headers }).toPromise();
    }

    async postDonneeCapteur(idCapteur: any, donneeCapteur: PostDonneeCapteur): Promise<any> {
        const token = await this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return await this._httpClient.post<DonneeCapteur>(this._environmentService.apiUrl + '/Capteurs/' + idCapteur + "/DonneesCapteur", donneeCapteur, { headers: headers }).toPromise();
    }
}