import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { EnvironmentService } from 'src/environments/environment.service';
import { Alerte } from 'src/model/alerte';
import { AlerteCapteur } from 'src/model/alerteCapteur';
import { Baril } from 'src/model/baril';
import { Capteur } from 'src/model/capteur';
import { Conversation, Message } from 'src/model/conversation';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';
import { DeleteCapteur } from 'src/model/deleteCapteur';
import { Documentation } from 'src/model/documentation';
import { Dompeux } from 'src/model/dompeux';
import { Donnee } from 'src/model/donnee';
import { DonneeCapteur, PostDonneeCapteur } from 'src/model/donneeCapteur';
import { Erabliere } from 'src/model/erabliere';
import { ErabliereApiDocument } from 'src/model/erabliereApiDocument';
import { GetImageInfo } from 'src/model/imageInfo';
import { Note } from 'src/model/note';
import { PutCapteur } from 'src/model/putCapteur';
import { PutCustomerAccess } from 'src/model/putCustomerAccess';
import { WeatherForecase } from 'src/model/weatherforecast';

@Injectable({ providedIn: 'root' })
export class ErabliereApi {
    private _authService: IAuthorisationSerivce

    constructor(private _httpClient: HttpClient,
        authFactoryService: AuthorisationFactoryService,
        private _environmentService: EnvironmentService) {
        this._authService = authFactoryService.getAuthorisationService();
    }

    async getErabliere(idErabliereSelectionee: any): Promise<Erabliere> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<Erabliere[]>(
            this._environmentService.apiUrl + '/erablieres?$filter=id eq ' + idErabliereSelectionee + '&$expand=Capteurs($filter=afficherCapteurDashboard eq true;$orderby=indiceOrdre)',
            { headers: headers }).toPromise();

        if (rtn != null && rtn.length > 0) {
            return rtn[0];
        }
        return new Erabliere();
    }

    async getErablieres(my: boolean): Promise<Erabliere[]> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<Erabliere[]>(this._environmentService.apiUrl + '/erablieres?my=' + my, { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getErablieresExpandCapteurs(my: boolean): Promise<Erabliere[]> {
        let headers = await this.getHeaders();
        headers = headers.set('Accept', 'application/json');
        const rtn = await this._httpClient.get<Erabliere[]>(
            this._environmentService.apiUrl + '/erablieres?my=' + my + '&$expand=Capteurs($filter=afficherCapteurDashboard eq true)', { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async putErabliere(erabliere: Erabliere): Promise<void> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<void>(this._environmentService.apiUrl + '/erablieres/' + erabliere.id, erabliere, { headers: headers }).toPromise();
    }

    async getAlertes(idErabliereSelectionnee: any): Promise<Alerte[]> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<Alerte[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes?additionalProperties=true", { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getAlertesCapteur(idErabliereSelectionnee: any): Promise<AlerteCapteur[]> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<AlerteCapteur[]>(
            this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertesCapteur?additionnalProperties=true&include=Capteur",
            { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getCapteurs(idErabliereSelectionnee: any): Promise<Capteur[]> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<Capteur[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/capteurs?$orderby=indiceOrdre", { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async postCapteur(idErabliereSelectionnee: any, capteur: PutCapteur) {
        const headers = await this.getHeaders();
        return await this._httpClient.post<Capteur>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/capteurs", capteur, { headers: headers }).toPromise();
    }

    async putCapteur(idErabliereSelectionnee: any, capteur: Capteur) {
        const headers = await this.getHeaders();
        return await this._httpClient.put<Capteur>(
            this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/capteurs",
            {
                ...capteur,
                idErabliere: idErabliereSelectionnee
            },
            { headers: headers }).toPromise();
    }

    async deleteCapteur(idErabliereSelectionnee: any, capteur: DeleteCapteur) {
        const headers = await this.getHeaders();
        return await this._httpClient.delete<DeleteCapteur>(
            this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/capteurs",
            {
                body: capteur,
                headers: headers
            }).toPromise();
    }

    async postAlerte(idErabliereSelectionnee: any, alerte: Alerte): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.post<Alerte>(
            this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes",
            alerte,
            { headers: headers }).toPromise();
    }

    async putAlerte(idErabliereSelectionnee: any, alerte: Alerte): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<Alerte>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes?additionalProperties=true", alerte, { headers: headers }).toPromise();
    }

    async putAlerteCapteur(idCapteur: any, alerte: AlerteCapteur): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<AlerteCapteur>(this._environmentService.apiUrl + '/Capteurs/' + idCapteur + "/alerteCapteurs?additionalProperties=true", alerte, { headers: headers }).toPromise();
    }

    async deleteAlerte(idErabliereSelectionnee: any, alerteId: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.delete(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/alertes/" + alerteId, { headers: headers }).toPromise();
    }

    async deleteAlerteCapteur(idCapteur: any, alerteId: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.delete(this._environmentService.apiUrl + '/Capteurs/' + idCapteur + "/AlerteCapteurs/" + alerteId, { headers: headers }).toPromise();
    }

    async getBarils(idErabliereSelectionnee: any): Promise<Baril[]> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<Baril[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/baril", { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getDonnees(idErabliereSelectionnee: any, debutFiltre: string, finFiltre: string, xddr?: any): Promise<HttpResponse<Donnee[]>> {
        let headers = await this.getHeaders();
        if (xddr != null) {
            headers = headers.set('x-ddr', xddr);
        }
        var httpCall = this._httpClient.get<Donnee[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/donnees?dd=" + debutFiltre + "&df=" + finFiltre, { headers: headers, observe: 'response' });
        const rtn = await httpCall.toPromise();
        return rtn ?? new HttpResponse();
    }

    async getDonneesCapteur(idCapteur: any, debutFiltre: string, finFiltre: string, xddr?: any): Promise<HttpResponse<DonneeCapteur[]>> {
        let headers = await this.getHeaders();
        if (xddr != null) {
            headers = headers.set('x-ddr', xddr);
        }
        var httpCall = this._httpClient.get<DonneeCapteur[]>(this._environmentService.apiUrl + '/capteurs/' + idCapteur + "/DonneesCapteur?dd=" + debutFiltre + "&df=" + finFiltre, { headers: headers, observe: 'response' });
        const rtn = await httpCall.toPromise();
        return rtn ?? new HttpResponse();
    }

    async getDompeux(idErabliereSelectionnee: any, debutFiltre: string, finFiltre: string, xddr?: any): Promise<HttpResponse<Dompeux[]>> {
        let headers = await this.getHeaders();
        if (xddr != null) {
            headers = headers.set('x-ddr', xddr);
        }
        var httpCall = this._httpClient.get<Dompeux[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/dompeux?dd=" + debutFiltre + "&df=" + finFiltre, { headers: headers, observe: 'response' });
        const rtn = await httpCall.toPromise();
        return rtn ?? new HttpResponse();
    }

    async getDocumentations(idErabliereSelectionnee:any, skip: number = 0, top?: number): Promise<Documentation[]> {
        const headers = await this.getHeaders();
        let odataOptions = "?$select=id,idErabliere,created,title,text,fileExtension";
            odataOptions += "&$skip=" + skip;
            odataOptions += top ? "&$top=" + top : "";
        const rtn = await this._httpClient.get<Documentation[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/documentation" + odataOptions, { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getDocumentationBase64(idErabliereSelectionnee: any, idDocumentation: any): Promise<Documentation[]> {
        let headers = await this.getHeaders();
        headers = headers.set('Accept', 'application/json');
        const rtn = await this._httpClient.get<Documentation[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/documentation?$select=file&$filter=id eq " + idDocumentation, { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getDocumentationCount(idErabliereSelectionnee:any): Promise<number> {
        let headers = await this.getHeaders();
        headers = headers.set('Accept', 'application/json');
        const rtn = await this._httpClient.get<number>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/documentation/quantite", { headers: headers }).toPromise();
        return rtn ?? 0;
    }

    async deleteDocumentation(idErabliereSelectionnee:any, idDocumentation:any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.delete(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/documentation/" + idDocumentation, { headers: headers }).toPromise();
    }

    async getNotes(idErabliereSelectionnee:any, skip: number = 0, top?: number): Promise<Note[]> {
        const headers = await this.getHeaders();
        let odataOptions = "?$orderby=NoteDate desc";
            odataOptions += "&$skip=" + skip;
            odataOptions += top ? "&$top=" + top : "";
            odataOptions += "&$select=id,idErabliere,noteDate,created,text,title,fileExtension,notificationFilter,reminderDate";
        const rtn = await this._httpClient.get<Note[]>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/notes" + odataOptions, { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getNotesCount(idErabliereSelectionnee:any): Promise<number> {
        let headers = await this.getHeaders();
        headers = headers.set('Accept', 'application/json');
        const rtn = await this._httpClient.get<number>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/notes/quantite", { headers: headers }).toPromise();
        return rtn ?? 0;
    }

    async getNoteImage(idErabliere: any, id: any) {
        let headers = await this.getHeaders();
        return await this._httpClient.get(this._environmentService.apiUrl + '/erablieres/' + idErabliere + "/notes/" + id + "/image", { headers: headers, responseType: 'arraybuffer' }).toPromise();
    }

    async postNote(idErabliereSelectionnee:any, note:Note): Promise<Note> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.post<Note>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/notes", note, { headers: headers }).toPromise();
        return rtn ?? new Note();
    }

    async deleteNote(idErabliereSelectionnee: any, noteId: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.delete(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/notes/" + noteId, { headers: headers }).toPromise();
    }

    async postErabliere(erabliere: Erabliere): Promise<Erabliere> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.post<Erabliere>(this._environmentService.apiUrl + '/erablieres', erabliere, { headers: headers }).toPromise();
        return rtn ?? new Erabliere();
    }

    async postDonneeCapteur(idCapteur: any, donneeCapteur: PostDonneeCapteur): Promise<DonneeCapteur> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.post<DonneeCapteur>(this._environmentService.apiUrl + '/Capteurs/' + idCapteur + "/DonneesCapteur", donneeCapteur, { headers: headers }).toPromise();
        return rtn ?? new DonneeCapteur();
    }

    async postDocument(idErabliereSelectionee: any, document: ErabliereApiDocument): Promise<any> {
        let headers = await this.getHeaders();
        headers = headers.set('Accept', 'application/json');
        return await this._httpClient.post<any>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionee + "/documentation", document, { headers: headers }).toPromise();
    }

    async postAlerteCapteur(idCapteur: any, alerteCapteur: AlerteCapteur): Promise<AlerteCapteur> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.post<AlerteCapteur>(this._environmentService.apiUrl + '/Capteurs/' + idCapteur + "/AlerteCapteurs", alerteCapteur, { headers: headers }).toPromise();
        return rtn ?? new AlerteCapteur();
    }

    async desactiverAlerteCapteur(idCapteur: any, idAlerte: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<AlerteCapteur>(this._environmentService.apiUrl + '/Capteurs/' + idCapteur + "/AlerteCapteurs/" + idAlerte + "/Desactiver", { idCapteur: idCapteur, id: idAlerte }, { headers: headers }).toPromise();
    }

    async activerAlerteCapteur(idCapteur: any, idAlerte: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<AlerteCapteur>(this._environmentService.apiUrl + '/Capteurs/' + idCapteur + "/AlerteCapteurs/" + idAlerte + "/Activer", { idCapteur: idCapteur, id: idAlerte }, { headers: headers }).toPromise();
    }

    async desactiverAlerte(idErabliere: any, idAlerte: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<AlerteCapteur>(this._environmentService.apiUrl + '/Erablieres/' + idErabliere + "/Alertes/" + idAlerte + "/Desactiver", { idErabliere: idErabliere, id: idAlerte }, { headers: headers }).toPromise();
    }

    async activerAlerte(idErabliere: any, idAlerte: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<AlerteCapteur>(this._environmentService.apiUrl + '/Erablieres/' + idErabliere + "/Alertes/" + idAlerte + "/Activer", { idErabliere: idErabliere, id: idAlerte }, { headers: headers }).toPromise();
    }

    async putNote(idErabliereSelectionnee: any, note: Note): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<Note>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/notes/" + note.id, note, { headers: headers }).toPromise();
    }

    async putDocumentation(idErabliereSelectionnee: any, documentation: Documentation): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.put<Documentation>(this._environmentService.apiUrl + '/erablieres/' + idErabliereSelectionnee + "/documentation/" + documentation.id, documentation, { headers: headers }).toPromise();
    }

    async getCustomers(): Promise<Customer[]> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<Customer[]>(this._environmentService.apiUrl + '/Customers', { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async getCustomersAccess(idErabliere: any): Promise<CustomerAccess[]> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<CustomerAccess[]>(this._environmentService.apiUrl + '/Erablieres/' + idErabliere + "/CustomersAccess", { headers: headers }).toPromise();
        return rtn ?? [];
    }

    async putCustomerAccess(idErabliere: any, customerAccess: PutCustomerAccess): Promise<CustomerAccess> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.put<CustomerAccess>(this._environmentService.apiUrl + '/Erablieres/' + idErabliere + "/CustomerErabliere", customerAccess, { headers: headers }).toPromise();
        return rtn ?? new CustomerAccess();
    }

    async deleteCustomerAccess(idErabliere: any, idCustomer: any): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.delete(this._environmentService.apiUrl + '/Erablieres/' + idErabliere + "/CustomersAccess/" + idCustomer, { headers: headers }).toPromise();
    }

    async deleteErabliere(idErabliere: any, erabliere: Erabliere): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.delete(this._environmentService.apiUrl + '/Erablieres/' + idErabliere, { headers: headers, body: erabliere }).toPromise();
    }

    async getWeatherForecast(idErabliere: any): Promise<WeatherForecase> {
        const headers = await this.getHeaders();
        const rtn = await this._httpClient.get<WeatherForecase>(this._environmentService.apiUrl + '/Erablieres/' + idErabliere + "/WeatherForecast", { headers: headers }).toPromise();
        return rtn ?? new WeatherForecase();
    }

    async startCheckoutSession(): Promise<any> {
        return await this._httpClient.post<any>(this._environmentService.apiUrl + "/Checkout", {}, {}).toPromise();
    }

    async postPrompt(prompt: { Prompt: string; ConversationId: any; }) {
        const headers = await this.getHeaders();
        return this._httpClient.post<any>(this._environmentService.apiUrl + "/ErabliereAI/Prompt", prompt, { headers: headers }).toPromise();
    }

    async getConversations(search?: string, top?: number, skip?: number): Promise<Conversation[]> {
        const headers = await this.getHeaders();
        let url = this._environmentService.apiUrl + "/ErabliereAI/Conversations?$orderby=lastMessageDate desc";
        if (isNotNullOrWhitespace(search)) {
            url += "&$filter=messages/any(m: contains(m/content, '" + search + "'))";
        }
        if (top) {
            url += "&$top=" + top;
        }
        if (skip) {
            url += "&$skip=" + skip;
        }
        const arr = await this._httpClient.get<Conversation[]>(url, { headers: headers }).toPromise();
        if (arr) {
            return arr;
        }
        else {
            return [];
        }
    }

    async getMessages(conversationId: any): Promise<Message[]> {
        const headers = await this.getHeaders();
        const url = this._environmentService.apiUrl + "/ErabliereAI/Conversations/" + conversationId + "/Messages";
        const arr = await this._httpClient.get<Message[]>(url, { headers: headers }).toPromise();
        if (arr) {
            return arr;
        }
        else {
            return [];
        }
    }

    async deleteConversation(id: any) {
        const headers = await this.getHeaders();
        return this._httpClient.delete<any>(this._environmentService.apiUrl + "/ErabliereAI/Conversations/" + id, { headers: headers }).toPromise();
    }

    async getOpenApiSpec(): Promise<any> {
        return await this._httpClient.get<any>(this._environmentService.apiUrl + "/api/v1/swagger.json", {}).toPromise();
    }

    async getCallAccessToken(uid: number, channel: string) {
        const headers = await this.getHeaders();
        return await this._httpClient.get<any>(this._environmentService.apiUrl + `/Calls/GetAccessToken?uid=${uid}&channel=${channel}`, { headers: headers }).toPromise();
    }

    async getCallAppId(): Promise<any> {
        const headers = await this.getHeaders();
        return await this._httpClient.get<any>(this._environmentService.apiUrl + "/Calls/GetAppId", { headers: headers }).toPromise();
    }

    async getImages(idErabliereSelectionnee: any, take: number, skip: number = 0, search?: string): Promise<GetImageInfo[]> {
        const headers = await this.getHeaders();
        let url = this._environmentService.apiUrl + 
            '/erablieres/' + idErabliereSelectionnee + 
            "/ImagesCapteur?take=" + take + 
            "&skip=" + skip;

        if (isNotNullOrWhitespace(search)) {
            url += "&search=" + search;
        }

        return await this._httpClient.get<any>(
            url, 
            { headers: headers })
            .toPromise();
    }

    async traduire(message: string) {
        const headers = await this.getHeaders();
        return await this._httpClient.post<any>(this._environmentService.apiUrl + '/ErabliereAI/Traduction?from=en&to=fr', { text: message }, { headers: headers }).toPromise();
    }

    async getHeaders(): Promise<HttpHeaders> {
        const token = await this._authService.getAccessToken();
        return new HttpHeaders().set('Authorization', `Bearer ${token}`);
    }
}

function isNotNullOrWhitespace(search: string | undefined) {
    return search != null && search.trim() !== '';
}
