import { Component, Input, ViewChild } from '@angular/core';
import { Modal } from 'bootstrap';
import { Note } from 'src/model/note';
import { ModalRappelComponent} from "./modal-rappel/modal-rappel.component";

@Component({
    selector: 'app-rappel',
    standalone: true,
    imports: [
        ModalRappelComponent
    ],
    styleUrls: ['./rappel.component.css'],
    templateUrl: './rappel.component.html'
})
export class RappelComponent {
    @Input() note: Note;
    @Input() index!: number;
    @ViewChild(ModalRappelComponent, { static: false }) modalRappelComponent!: ModalRappelComponent;
    private modalInstance: any;
    constructor() {
        this.note = new Note();
    }

    onModalInitialized(modalInstance: Modal): void {
        this.modalInstance = modalInstance;
    }
    
    getExcerpt(text: string | undefined, length: number = 100): string {
    return text && text.length > length ? text.slice(0, length) + '...' : text || '';
    }
}
