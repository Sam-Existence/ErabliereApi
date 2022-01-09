import { Component, Input, OnInit } from "@angular/core";
import { Subject } from "rxjs";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";

@Component({
    selector: 'alerte-page',
    templateUrl: './alerte.component.html'
})
export class AlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi) { }

    ngOnInit(): void {
        this.displayEditFormSubject.subscribe(b => {
             this.displayEditForm = b.valueOf();
        });

        this.alerteEditFormSubject.subscribe(b => {
            if (this.alertes != undefined) {
                var i = this.alertes.findIndex(a => a.id == this.alerteEditForm?.id);

                this.alertes[i] = b;
            }
        });
    }

    @Input() alertes?: Array<Alerte>;

    @Input() idErabliereSelectionee:any

    displayEditFormSubject = new Subject<Boolean>();
    displayEditFormObservable = this.displayEditFormSubject.asObservable();
    displayEditForm: boolean = false;

    alerteEditFormSubject = new Subject<Alerte>();
    alerteEditFormObservable = this.displayEditFormSubject.asObservable();
    alerteEditForm?: Alerte;

    onButtonModifierClick(alerteId:any) {
        this.alerteEditForm = this.alertes?.find(a => a.id == alerteId);
        this.alerteEditFormSubject.next(this.alerteEditForm);
        this.displayEditFormSubject.next(true);
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
}