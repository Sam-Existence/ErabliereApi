// This a component that allows to add a new erabliere
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
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
        return e;
    }

    erabliere: Erabliere = this.getDefaultErabliere();
    plusdOptions: boolean = false;
    plusOptionsButtonText: string = "Plus d'options";

    ngOnInit() {
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