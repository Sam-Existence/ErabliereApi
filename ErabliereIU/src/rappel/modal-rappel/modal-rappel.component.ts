import {
    Component,
    ElementRef,
    Input,
    ViewChild,
    AfterContentChecked,
    Output,
    EventEmitter
} from '@angular/core';
import { Note } from 'src/model/note';
//import { Modal } from "bootstrap";

@Component({
  selector: 'app-modal-rappel',
  standalone: true,
  imports: [],
  templateUrl: './modal-rappel.component.html',
  styleUrl: './modal-rappel.component.css'
})
export class ModalRappelComponent implements AfterContentChecked {
    @Input() note: Note;
    @ViewChild('modalRappel') modalRappel!: ElementRef;
    private modalInstance: any;
    @Input() index!: number;
    //@Output() modalInitialized = new EventEmitter<Modal>();

    constructor() {
        this.note = new Note();
    }

    ngAfterContentChecked(): void {
        if (this.modalRappel && !this.modalInstance) {
            const modalElement = this.modalRappel.nativeElement;
            // this.modalInstance = new Modal(modalElement);
            // this.modalInitialized.emit(this.modalInstance);
        }
    }
}
