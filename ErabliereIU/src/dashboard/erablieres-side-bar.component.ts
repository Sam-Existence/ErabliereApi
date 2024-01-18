import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Erabliere } from 'src/model/erabliere';
import { NgIf, NgFor } from '@angular/common';
import { AjouterErabliereComponent } from 'src/erablieres/ajouter-erabliere.component';
import { ModifierErabliereComponent } from 'src/erablieres/modifier-erabliere.component';
import { Subject } from 'rxjs';

@Component({
    selector: 'erablieres-side-bar',
    templateUrl: 'erablieres-side-bar.component.html',
    standalone: true,
    imports: [AjouterErabliereComponent, ModifierErabliereComponent, NgIf, NgFor]
})
export class ErabliereSideBarComponent implements OnInit {
  erablieres?: Array<Erabliere>;
  etat: string = "";
  erabliereSelectionnee?: Erabliere;
  idSelectionnee?: any
  @Input() showMenuSubject: Subject<boolean> = new Subject<boolean>();
  private _authService: IAuthorisationSerivce
  loggedIn: Boolean = false;
  authDisabled: boolean = false;

  constructor(private _erabliereApi: ErabliereApi, 
      authFactory: AuthorisationFactoryService, 
      private _router: Router,
      private route: ActivatedRoute) {
    this.erabliereSelectionnee = undefined;
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
        this.erablieres = undefined;
        this.erabliereSelectionnee = undefined;
        this.idSelectionnee = undefined;
        this.etat = "Vous n'êtes pas connecté";
      }
    });
  }

  async ngOnInit() {
    // Set cacheMenuErabliere to true to cache the menu when the path start with /apropos
    this._router.events.subscribe((val) => {
      console.log("router event");
      console.log(this._router.url);
      if (this._router.url.split("/").length > 1) {
        let page = this._router.url.split("/")[1];
        if (page == "apropos") {
          this.showMenuSubject.next(false);
        }
        else {
          this.showMenuSubject.next(true);
        }
      }
      else {
        this.showMenuSubject.next(true);
      }
    });
    this.loggedIn = await this._authService.isLoggedIn();
    await this.loadErablieresPage();
  }

  async loadErablieresPage() {
    const titreChargement = "Chargement des érablières...";

    if (this.etat == titreChargement) {
      return new Promise<void>((resolve, reject) => { });
    }

    this.etat = titreChargement;

    const erablieres = await (this._erabliereApi.getErablieresExpandCapteurs(true).catch(err => {
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
        this.erabliereSelectionnee = this.erablieres[0];
        this.idSelectionnee = this.erabliereSelectionnee.id;

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

    console.log(this._router.url);
    if (this._router.url.split("/").length > 3) {
      let page = this._router.url.split("/")[3];
      console.log(page);
      this._router.navigate(["/e", idErabliere, page]);
    }
    else {
      this._router.navigate(["/e", idErabliere]);
    }
  }

  @ViewChild(ModifierErabliereComponent) modifierErabliereComponent?: ModifierErabliereComponent;

  openEditErabliereForm(erabliere: Erabliere) {
    if (this.modifierErabliereComponent != undefined) {
      if (this.modifierErabliereComponent.erabliereForm != undefined) {
        this.modifierErabliereComponent.erabliereForm.erabliere = { ...erabliere };
        this.modifierErabliereComponent.modifierAccesUtilisateurs?.refreashAccess(erabliere.id);
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
