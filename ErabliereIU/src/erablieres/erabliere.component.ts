import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Alerte } from 'src/model/alerte';
import { AlerteCapteur } from 'src/model/alerteCapteur';
import { Documentation } from 'src/model/documentation';
import { Erabliere } from 'src/model/erabliere';
import { Note } from 'src/model/note';

@Component({
    selector: 'erablieres',
    templateUrl: 'erabliere.component.html'
})
export class ErabliereComponent implements OnInit {
    erablieres?: Array<Erabliere>;
    etat: string = "Chargement des erablieres...";
    erabliereSelectionnee?:Erabliere;
    idSelectionnee?:any
    @Input() cacheMenuErabliere?:boolean;
    @Input() pageSelectionnee?:number = 0;
    alertes?: Array<Alerte>;
    alertesCapteur?: Array<AlerteCapteur>;
    documentations?: Array<Documentation>;
    notes?: Array<Note>;
    private _authService: IAuthorisationSerivce
    
    constructor(private _erabliereApi: ErabliereApi, authFactory: AuthorisationFactoryService, private _router: Router) {
        this.erabliereSelectionnee = undefined;
        this._authService = authFactory.getAuthorisationService();
        this._authService.loginChanged.subscribe(loggedIn => {
            if (loggedIn) {
                this.ngOnInit();
            }
            else {
                this.erablieres = undefined;
                this.erabliereSelectionnee = undefined;
                this.idSelectionnee = undefined;
                this.etat = "Vous n'êtes pas connecté";
            }
        });
    }

    async ngOnInit() {
        this.etat = "Chargement des erablieres...";

        const erablieres = await (this._erabliereApi.getErablieresExpandCapteurs().catch(err => {
            console.log(err);
            this.etat = "Erreur lors du chargement des érablieres";
        }));

        if (erablieres != null) {
            this.erablieres = erablieres.sort((a, b) => {
                if (a.indiceOrdre != null && b.indiceOrdre == null)
                {
                    return -1;
                }
                else if (b.indiceOrdre != null && a.indiceOrdre == null)
                {
                    return 1;
                }
                else if (a.indiceOrdre != null && b.indiceOrdre != null)
                {
                    return a.indiceOrdre - b.indiceOrdre;
                }

                return a.nom?.localeCompare(b.nom ?? "") ?? 0;
            });

            if (this.erablieres.length > 0) {
                this.etat = "Chargement des erablieres terminé";
                this.erabliereSelectionnee = this.erablieres[0];
                this.idSelectionnee = this.erabliereSelectionnee.id;

                // verifier quel est la page selectionnee base sur l'url
                if (this._router.url.indexOf("/alertes") > -1) {
                    this.pageSelectionnee = 1;
                }
                else if (this._router.url.indexOf("/documentations") > -1) {
                    this.pageSelectionnee = 4;
                }
                else if (this._router.url.indexOf("/notes") > -1) {
                    this.pageSelectionnee = 5;
                }
                else if (this._router.url.indexOf("/apropos") > -1) {
                    this.pageSelectionnee = 3;
                }
                else {
                    this.pageSelectionnee = 0;
                }

                this.handleErabliereLiClick(this.erabliereSelectionnee.id);
            }
            else {
                this.etat = "Aucune erablière";
            }
        }
    }

    handleErabliereLiClick(idErabliere: number) {
        if (this.erablieres == null || this.erablieres == undefined) {
            return;
        }

        this.erabliereSelectionnee = this.erablieres.find(e => e.id === idErabliere);

        this.idSelectionnee = this.erabliereSelectionnee?.id;

        if (this.pageSelectionnee == 1) {
            this.loadAlertes();
        }

        if (this.pageSelectionnee == 4) {
            this.loadDocumentations();
        }

        if (this.pageSelectionnee == 5) {
            this.loadNotes();
        }
    }

    loadAlertes() {
        this._erabliereApi.getAlertes(this.erabliereSelectionnee?.id).then(alertes => {
            this.alertes = alertes;
        });
        this._erabliereApi.getAlertesCapteur(this.erabliereSelectionnee?.id).then(alertesCapteur => {
            this.alertesCapteur = alertesCapteur;
        });
    }

    loadDocumentations() {
        this._erabliereApi.getDocumentations(this.erabliereSelectionnee?.id).then(documentations => {
            this.documentations = documentations;
        });
    }

    loadNotes() {
        this._erabliereApi.getNotes(this.erabliereSelectionnee?.id).then(notes => {
            this.notes = notes;
        });
    }
}