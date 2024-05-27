import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Erabliere} from "../../../model/erabliere";
import {AdminErabliereAccessListComponent} from "../../../access/admin-erabliere-access-list.component";
import {CustomerAccess} from "../../../model/customerAccess";

@Component({
    selector: 'erabliere-list',
    standalone: true,
    imports: [
        AdminErabliereAccessListComponent
    ],
    templateUrl: './erabliere-list.component.html',
    styleUrl: './erabliere-list.component.css',

})
export class ErabliereListComponent {
    @Input() erablieres: Erabliere[] = [];
    @Output() erabliereASupprimer: EventEmitter<Erabliere> = new EventEmitter();
    @Output() erabliereAModifier: EventEmitter<Erabliere> = new EventEmitter();

    showAccess: { [id: string]: boolean } = {}

    toggleAccess(id: string): void {
        this.showAccess[id] = !this.showAccess[id];
    }

    MAJAcces(acces: CustomerAccess[], erabliere: Erabliere) {
        erabliere.customerErablieres = acces;
    }

    signalerSuppression(erabliere: Erabliere) {
        this.erabliereASupprimer.emit(erabliere);
    }

    signalerModification(erabliere: Erabliere) {
        this.erabliereAModifier.emit(erabliere);
    }
}
