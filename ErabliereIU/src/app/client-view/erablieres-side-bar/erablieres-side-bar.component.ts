import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Erabliere } from 'src/model/erabliere';
import { AjouterErabliereComponent } from 'src/erablieres/ajouter-erabliere.component';
import { ModifierErabliereComponent } from 'src/erablieres/modifier-erabliere.component';

@Component({
    selector: 'erablieres-side-bar',
    templateUrl: 'erablieres-side-bar.component.html',
    standalone: true,
    imports: [AjouterErabliereComponent, ModifierErabliereComponent]
})
export class ErabliereSideBarComponent implements OnInit {
  private _authService: IAuthorisationSerivce

  @ViewChild(ModifierErabliereComponent) modifierErabliereComponent?: ModifierErabliereComponent;

  @Input() idSelectionne?: string;
  @Input() thereIsAtLeastOneErabliere: boolean = false;

  @Output() thereIsAtLeastOneErabliereChange = new EventEmitter<boolean>();
  @Output() idSelectionneChange = new EventEmitter<string>();

  authDisabled: boolean = false;
  erablieres: Array<Erabliere> = [];
  etat: string = "";
  erabliereSelectionnee?: Erabliere;
  loggedIn: boolean = false;

  constructor(private _erabliereApi: ErabliereApi,
      authFactory: AuthorisationFactoryService,
      private _router: Router) {
    this._authService = authFactory.getAuthorisationService();
    if (this._authService.type == "AuthDisabled") {
      this.authDisabled = true;
    }
    this._authService.loginChanged.subscribe(loggedIn => {
      this.loggedIn = loggedIn;
      if (loggedIn) {
        this.loadErablieresPage();
      }
      else {
        this.erablieres = [];
        this.etat = "Vous n'êtes pas connecté";
      }
    });
  }

  async ngOnInit() {
    this.loggedIn = await this._authService.isLoggedIn();

    await this.loadErablieresPage();
  }

  async loadErablieresPage() {
    const titreChargement = "Chargement des érablières...";

    if (this.etat == titreChargement) {
      return new Promise<void>((resolve, reject) => { });
    }

    this.etat = titreChargement;

    const erablieres = await (this._erabliereApi.getErablieres(true).catch(err => {
      console.error(err);
      this.etat = "Erreur lors du chargement des érablieres";
    }));

    if (erablieres != null) {
      this.erablieres = erablieres.sort((a, b) => {
        if (a.indiceOrdre != null && b.indiceOrdre == null) {
          return -1;
        }
        else if (b.indiceOrdre != null && a.indiceOrdre == null) {
          return 1;
        }
        else if (a.indiceOrdre != null && b.indiceOrdre != null) {
          return a.indiceOrdre - b.indiceOrdre;
        }

        return a.nom?.localeCompare(b.nom ?? "") ?? 0;
      });

      if (this.erablieres.length > 0) {
        this.etat = "Chargement des erablieres terminé";

        this.erabliereSelectionnee = this.erablieres.find(e => e.id === this.idSelectionne);
        if(!this.erabliereSelectionnee) {
          this.erabliereSelectionnee = this.erablieres[0];
          this.handleErabliereLiClick(this.erabliereSelectionnee.id);
        }
        this.thereIsAtLeastOneErabliere = true;
        this.thereIsAtLeastOneErabliereChange.emit(true);
      }
      else {
        this.etat = "Aucune erablière";
        this.thereIsAtLeastOneErabliere = false;
        this.thereIsAtLeastOneErabliereChange.emit(false);
      }
    }
  }

  handleErabliereLiClick(idErabliere: number) {
    if (!this.erablieres){
      return;
    }

    this.erabliereSelectionnee = this.erablieres.find(e => e.id === idErabliere);

    this.idSelectionne = this.erabliereSelectionnee!.id;
    this.idSelectionneChange.emit(this.idSelectionne);

    const urlParts = this._router.url.split("/");
    if (urlParts.length > 1 && urlParts[1] == "e") {
      if (urlParts.length > 3) {
        let page = this._router.url.split("/")[3];
        this._router.navigate(["/e", idErabliere, page]);
      } else {
        this._router.navigate(["/e", idErabliere, "graphiques"]);
      }
    }
  }

  async openEditErabliereForm(erabliere: Erabliere) {
    if (this.modifierErabliereComponent != undefined) {
      if (this.modifierErabliereComponent.erabliereForm != undefined) {
        this.modifierErabliereComponent.erabliereForm.erabliere = { ...erabliere };
      }
      else {
        console.log("erabliereForm is undefined");
      }
    }
    else {
      console.log("modifierErabliereComponent is undefined");
    }
  }
}
