import { Component, Input, OnChanges, SimpleChanges } from "@angular/core";
import { Subject } from "rxjs";
import { Erabliere } from "src/model/erabliere";
import { BarilsComponent } from "../barils/barils.component";
import { CapteurPannelsComponent } from "../donnees/sub-panel/capteur-pannels.component";
import { DonneesComponent } from "../donnees/donnees.component";
import { NgIf } from "@angular/common";

@Component({
    selector: 'graphique',
    template: `
            <div class="row">
                <donnees-panel *ngIf="erabliere?.afficherTrioDonnees == true || erabliere?.afficherSectionDompeux == true" 
                            [initialErabliere]="erabliere"
                            [erabliereSubject]="resetErabliere"></donnees-panel>
                <capteur-pannels [capteurs]="erabliere?.capteurs" [erabliere]="erabliere"></capteur-pannels>
                <barils-panel *ngIf="erabliere?.afficherSectionBaril == true" 
                            [erabliereId]="erabliere?.id"></barils-panel>
            </div>
    `,
    standalone: true,
    imports: [NgIf, DonneesComponent, CapteurPannelsComponent, BarilsComponent]
})
export class GraphiqueComponent implements OnChanges {
   
    @Input() erabliere?:Erabliere

    resetErabliere: Subject<Erabliere> = new Subject<Erabliere>();

    ngOnChanges(changes: SimpleChanges): void {
        this.resetChildForm();
    }

    resetChildForm(){
        if (this.erabliere != null)
            this.resetErabliere.next(this.erabliere);
    }

    notNullOrWitespace(arg0?: string): any {
        if (arg0 == null)
            return false;
        return arg0.trim().length > 0;
    }
}