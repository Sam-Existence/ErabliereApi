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

  public dimension?: string = "col-md-6";

  constructor(private api: ErabliereApi) {
  }

  ngOnChanges(changes: SimpleChanges): void {
      if (changes.capteurs) {
          this.dimension = this.capteurs?.find(capteur => capteur.dimension)?.dimension ?? "col-md-6";
      }
  }

  changerDimension(dimension: number) {
      this.dimension = `col-md-${dimension}`;
      for (let capteur of this.capteurs!) {
          capteur.dimension = this.dimension;
          this.api.putCapteur(this.erabliere!.id, capteur)
      }
  }
}
