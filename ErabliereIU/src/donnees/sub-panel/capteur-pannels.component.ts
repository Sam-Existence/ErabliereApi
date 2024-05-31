import { NgClass } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Capteur } from 'src/model/capteur';
import { Erabliere } from 'src/model/erabliere';
import { GraphPannelComponent } from './graph-pannel.component';

@Component({
    selector: 'capteur-pannels',
    templateUrl: './capteur-pannels.component.html',
    standalone: true,
    imports: [GraphPannelComponent, NgClass]
})
export class CapteurPannelsComponent implements OnChanges {
    @Input() capteurs?: Capteur[] = []
    @Input() erabliere?: Erabliere

    public tailleString?: string = "col-md-6";

    constructor(private _api: ErabliereApi) {
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.capteurs) {
            let taille = this.capteurs?.find(capteur => capteur.taille)?.taille;
            if (taille) {
                this.tailleString = `col-md-${taille}`;
            } else {
                this.tailleString = `col-md-6`;
            }
        }
    }

    changerDimension(taille: number) {
        this.tailleString = `col-md-${taille}`;
        if (this.capteurs) {
            for (let capteur of this.capteurs!) {
                capteur.taille = taille
            }

            this._api.putCapteurs(this.erabliere?.id, this.capteurs);
        }
    }
}
