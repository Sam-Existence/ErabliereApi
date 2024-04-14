import {Component, ElementRef, Input, ViewChild, OnInit, AfterViewInit} from '@angular/core';
import { Note } from 'src/model/note';
import { Modal } from "bootstrap";

@Component({
  selector: 'app-modal-rappel',
  standalone: true,
  imports: [],
  templateUrl: './modal-rappel.component.html',
  styleUrl: './modal-rappel.component.css'
})
export class ModalRappelComponent implements AfterViewInit {
    @Input() note: Note;
    @ViewChild('modalRappel') modalRappel!: ElementRef;
    private modalInstance: any;
    @Input() index!: number;

    constructor() {
        this.note = new Note();
    }

    ngAfterViewInit(): void {
        if (this.modalRappel) {
            const modalElement = this.modalRappel.nativeElement;
            this.modalInstance = new Modal(modalElement);
        } else {
            console.error('modalRappel is not defined');
        }
    }

    openModal(): void {
        if (!this.modalInstance && this.modalRappel) {
            const modalElement = this.modalRappel.nativeElement;
            this.modalInstance = new Modal(modalElement);
        }
        if (this.modalInstance) {
            this.modalInstance.show();
        } else {
            console.error('Modal instance is not defined');
        }
    }
}
