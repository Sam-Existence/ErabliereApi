import { Component, Input, OnInit } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { AlerteCapteur } from "src/model/alerteCapteur";

@Component({
  selector: 'alerte-page',
  templateUrl: './alerte.component.html',
  styleUrls: ['./alerte.component.css']
})
export class AlerteComponent implements OnInit {
  @Input() alertes?: Array<Alerte>;
  @Input() alertesCapteur?: Array<AlerteCapteur>;
  @Input() idErabliereSelectionee: any

  displayEditFormSubject;
  displayEditFormObservable;
  displayEditForm: boolean;

  alerteEditFormSubject;
  alerteEditFormObservable;
  alerteEditForm?: Alerte;

  displayEditAlerteCapteurFormSubject: Subject<Boolean>;
  displayEditAlerteCapteurFormObservable: Observable<string>;
  displayEditAlerteCapteurForm: boolean;

  alerteCapteurEditFormSubject: Subject<AlerteCapteur>;
  alerteCapteurEditFormObservable: Observable<AlerteCapteur>;
  alerteCapteurEditForm?: AlerteCapteur;

  editAlerte: boolean;
  editAlerteCapteur: boolean;

  constructor(private _api: ErabliereApi) {
    this.displayEditFormSubject = new Subject<string>();
    this.displayEditFormObservable = this.displayEditFormSubject.asObservable();

    this.alerteEditFormSubject = new Subject<Alerte>();
    this.alerteEditFormObservable = this.displayEditFormSubject.asObservable();
    this.displayEditForm = false;

    this.displayEditAlerteCapteurFormSubject = new Subject<Boolean>();
    this.displayEditAlerteCapteurFormObservable = this.displayEditFormSubject.asObservable();
    this.displayEditAlerteCapteurForm = false;

    this.alerteCapteurEditFormSubject = new Subject<AlerteCapteur>()
    this.alerteCapteurEditFormObservable = this.alerteCapteurEditFormSubject.asObservable();

    this.editAlerte = false;
    this.editAlerteCapteur = false;
  }

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

  onButtonModifierClick(alerteId: any) {
    this.alerteEditForm = this.alertes?.find(a => a.id == alerteId);
    if (this.alerteEditForm != null)
      this.alerteEditFormSubject.next(this.alerteEditForm);
    this.displayEditFormSubject.next("alerte");
    this.editAlerte = true;
    this.editAlerteCapteur = false;
  }

  onButtonModifierAlerteCapteurClick(alerteId: any) {
    this.alerteCapteurEditForm = this.alertesCapteur?.find(a => a.id == alerteId);
    if (this.alerteCapteurEditForm != null)
      this.alerteCapteurEditFormSubject.next(this.alerteCapteurEditForm);
    this.displayEditFormSubject.next("alerteCapteur");
    this.editAlerte = false;
    this.editAlerteCapteur = true;
  }

  onButtonDeleteClick(alerteId: any) {
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

  onButtonDeleteAlerteCapteurClick(idCapteur: any, alerteId: any) {
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

  onButtonDesactiverAlerteCapteurClick(capteurId: any, alerteId: any) {
    this._api.desactiverAlerteCapteur(capteurId, alerteId)
      .then(a => {
        this._api.getAlertesCapteur(this.idErabliereSelectionee)
          .then(a => {
            this.alertesCapteur = a;
          });
      });
  }

  onButtonActiverAlerteCapteurClick(capteurId: any, alerteId: any) {
    this._api.activerAlerteCapteur(capteurId, alerteId)
      .then(a => {
        this._api.getAlertesCapteur(this.idErabliereSelectionee)
          .then(a => {
            this.alertesCapteur = a;
          });
      });
  }

  onButtonDesactiverAlerteClick(alerteId: any) {
    this._api.desactiverAlerte(this.idErabliereSelectionee, alerteId)
      .then(a => {
        this._api.getAlertes(this.idErabliereSelectionee)
          .then(a => {
            this.alertes = a;
          });
      });
  }

  onButtonActiverAlerteClick(alerteId: any) {
    this._api.activerAlerte(this.idErabliereSelectionee, alerteId)
      .then(a => {
        this._api.getAlertes(this.idErabliereSelectionee)
          .then(a => {
            this.alertes = a;
          });
      });
  }

  formatStringNumber(str?: string, symbol?: string) {
    if (str == null || str == "" || str == NaN.toString()) {
      return null;
    }

    return (parseInt(str) / 10).toFixed(1) + " " + symbol;
  }

  formatStringNumberBase10(str?: string, symbol?: string) {
    if (str == null || str == "") {
      return null;
    }

    return (parseInt(str)) + " " + symbol;
  }

  formatNumber(i?: number, symbol?: string) {
    if (i == null) {
      return null;
    }

    // formt the number with one decimal place
    var formatted = (i / 10).toFixed(1);

    return formatted + " " + symbol;
  }
}
