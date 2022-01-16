import { Component, Input, OnInit } from "@angular/core";
import { Subject } from "rxjs";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { AlerteCapteur } from "src/model/alerteCapteur";

@Component({
    selector: 'alerte-page',
    templateUrl: './alerte.component.html'
})
export class AlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi) { }

    ngOnInit(): void {
        this.displayEditFormSubject.subscribe(b => {
             this.displayEditForm = b == "alerte" || b == "alerteCapteur";
        });

        this.alerteEditFormSubject.subscribe(b => {
            if (this.alertes != undefined) {
                var i = this.alertes.findIndex(a => a.id == this.alerteEditForm?.id);

                this.alertes[i] = b;
            }
        });

        this.alerteCapteurEditFormSubject.subscribe(b => {
            if (this.alertesCapteur != undefined) {
                var i = this.alertesCapteur.findIndex(a => a.id == this.alerteCapteurEditForm?.id);

                this.alertesCapteur[i] = b;
            }
        });
    }

    @Input() alertes?: Array<Alerte>;
    @Input() alertesCapteur?: Array<AlerteCapteur>;
    @Input() idErabliereSelectionee:any

    displayEditFormSubject = new Subject<string>();
    displayEditFormObservable = this.displayEditFormSubject.asObservable();
    displayEditForm: boolean = false;

    alerteEditFormSubject = new Subject<Alerte>();
    alerteEditFormObservable = this.displayEditFormSubject.asObservable();
    alerteEditForm?: Alerte;

    displayEditAlerteCapteurFormSubject = new Subject<Boolean>();
    displayEditAlerteCapteurFormObservable = this.displayEditFormSubject.asObservable();
    displayEditAlerteCapteurForm: boolean = false;

    alerteCapteurEditFormSubject = new Subject<AlerteCapteur>();
    alerteCapteurEditFormObservable = this.alerteCapteurEditFormSubject.asObservable();
    alerteCapteurEditForm?: AlerteCapteur;

    editAlerte: boolean = false;
    editAlerteCapteur: boolean = false;

    onButtonModifierClick(alerteId:any) {
        this.alerteEditForm = this.alertes?.find(a => a.id == alerteId);
        this.alerteEditFormSubject.next(this.alerteEditForm);
        this.displayEditFormSubject.next("alerte");
        this.editAlerte = true;
        this.editAlerteCapteur = false;
    }

    onButtonModifierAlerteCapteurClick(alerteId:any) {
        this.alerteCapteurEditForm = this.alertesCapteur?.find(a => a.id == alerteId);
        this.alerteCapteurEditFormSubject.next(this.alerteCapteurEditForm);
        this.displayEditFormSubject.next("alerteCapteur");
        this.editAlerte = false;
        this.editAlerteCapteur = true;
    }

    onButtonDeleteClick(alerteId:any) {
        if (confirm("Voulez-vous vraiment supprimer l'alerte " + alerteId + " ? ")) {
            this._api.deleteAlerte(this.idErabliereSelectionee, alerteId)
                     .then(a => {
                        this._api.getAlertes(this.idErabliereSelectionee)
                                 .then(a => {
                                     this.alertes = a;
                                 });
                     });
        }
    }

    onButtonDeleteAlerteCapteurClick(idCapteur:any, alerteId:any) {
        if (confirm("Voulez-vous vraiment supprimer l'alerte capteur " + alerteId + " ? ")) {
            this._api.deleteAlerteCapteur(idCapteur, alerteId)
                     .then(a => {
                        this._api.getAlertesCapteur(this.idErabliereSelectionee)
                                 .then(a => {
                                     this.alertesCapteur = a;
                                 });
                     });
        }
    }
}