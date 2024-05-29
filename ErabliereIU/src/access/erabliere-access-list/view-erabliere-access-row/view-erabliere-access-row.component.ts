import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { CustomerAccess } from 'src/model/customerAccess';

@Component({
    selector: 'tr[view-erabliere-access-row]',
    templateUrl: 'view-erabliere-access-row.component.html',
    standalone: true
})
export class ViewErabliereAccessRowComponent implements OnInit {
    @Input() acces?: CustomerAccess;
    lecture: boolean = false;
    creation: boolean = false;
    modification: boolean = false;
    suppression: boolean = false;

    @Output() accesASupprimer = new EventEmitter<CustomerAccess>();
    @Output() accesAModifier = new EventEmitter<CustomerAccess>();

    ngOnInit() {
        if (this.acces?.access) {
            this.lecture = !!(this.acces.access & 1);
            this.creation = !!(this.acces.access & 2);
            this.modification = !!(this.acces.access & 4);
            this.suppression = !!(this.acces.access & 8);
        }
    }

    signalerSupression() {
        this.accesASupprimer.emit(this.acces);
    }

    signalerModification() {
        this.accesAModifier.emit(this.acces);
    }
}
