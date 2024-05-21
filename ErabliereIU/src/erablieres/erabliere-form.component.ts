// This a component that allows to add a new erabliere
import { Component, Input, OnInit } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { Erabliere } from 'src/model/erabliere';
import { GestionCapteursComponent } from '../capteurs/gestion-capteurs.component';
import { NgIf, NgClass } from '@angular/common';
import { InputErrorComponent } from '../formsComponents/input-error.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Capteur } from 'src/model/capteur';
import { ErabliereApi } from 'src/core/erabliereapi.service';

@Component({
    selector: 'erabliere-form',
    templateUrl: './erabliere-form.component.html',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        FormsModule,
        InputErrorComponent,
        NgIf,
        GestionCapteursComponent,
        NgClass,
    ],
})
export class ErabliereFormComponent implements OnInit {
    getDefaultErabliere() {
        let e = new Erabliere();
        e.afficherSectionBaril = true;
        e.afficherSectionDompeux = true;
        e.afficherTrioDonnees = true;
        e.ipRule = "-";
        e.isPublic = false;
        return e;
    }

    erabliere: Erabliere = this.getDefaultErabliere();
    plusdOptions: boolean = false;
    plusOptionsButtonText: string = "Plus d'options";
    @Input() errorObj?: any
    @Input() generalError?: string
    capteurs: Capteur[] = [];

    constructor(private readonly auth: AuthorisationFactoryService, private api: ErabliereApi) {

    }

    async ngOnInit() {
        if (this.auth.getAuthorisationService().type == "AuthDisabled") {
            this.erabliere.isPublic = true;
        }
    }

    async afficherPlusOptions() {
        this.plusdOptions = !this.plusdOptions;
        if (this.plusdOptions) {
            this.plusOptionsButtonText = "Moins d'options";
            if (this.erabliere.id != null) {
                this.capteurs = await this.api.getCapteurs(this.erabliere.id);
            }
        } else {
            this.plusOptionsButtonText = "Plus d'options";
        }
    }
}
