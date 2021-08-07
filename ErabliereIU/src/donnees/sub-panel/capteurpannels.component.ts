import { Component, Input, OnInit } from '@angular/core';
import { Capteur } from 'src/model/capteur';

@Component({
    selector: 'capteur-panels',
    template: `
        <div class="border-top">
          <div class="row">
            <div class="col-md-6">
              <div *ngFor="let capteur of capteurs">
                <graph-panel [titre]="capteur.nom" 
                             [symbole]="capteur.symbole"
                             [backendAction]="capteur?.id"></graph-panel>
              </div>
            </div>
          </div>
        </div>
    `
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs?: Capteur[];

  ngOnInit(): void {

  }
  
}