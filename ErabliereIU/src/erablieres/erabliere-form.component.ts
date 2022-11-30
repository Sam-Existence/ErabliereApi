// This a component that allows to add a new erabliere
import { Component, Input, OnInit } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'erabliere-form',
    templateUrl: './erabliere-form.component.html',
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

    constructor(private readonly auth: AuthorisationFactoryService) {
        
    }

    ngOnInit() {
        if (this.auth.getAuthorisationService().type == "AuthDisabled") {
            this.erabliere.isPublic = true;
        }
    }

    afficherPlusOptions() {
        this.plusdOptions = !this.plusdOptions;
        if (this.plusdOptions) {
            this.plusOptionsButtonText = "Moins d'options";
        } else {
            this.plusOptionsButtonText = "Plus d'options";
        }
    }
}