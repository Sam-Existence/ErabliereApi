import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Erabliere} from "../../../model/erabliere";

@Component({
    selector: 'erabliere-list',
    standalone: true,
    imports: [],
    templateUrl: './erabliere-list.component.html',
    styleUrl: './erabliere-list.component.css',

})
export class ErabliereListComponent {
    @Input() erablieres: Erabliere[] = [];
    @Output() erabliereASupprimer: EventEmitter<Erabliere> = new EventEmitter();

    signalerSuppression(erabliere: Erabliere) {
        this.erabliereASupprimer.emit(erabliere);
    }
}
