import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
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
export class RappelComponent implements AfterViewInit {
  @Input() note: Note;
  @Input() index!: number;
  @ViewChild(ModalRappelComponent, { static: false }) modalRappelComponent!: ModalRappelComponent;
  private modalInstance: any;
  constructor() {
    this.note = new Note();
  }

    ngAfterViewInit(): void {
        if (this.modalRappelComponent && this.modalRappelComponent.modalRappel) {
            const modalElement = this.modalRappelComponent.modalRappel.nativeElement;
            this.modalInstance = new Modal(modalElement);
        } else {
            console.error('ModalRappelComponent or modalRappel is not defined');
        }
    }

    openModal(): void {
        if (!this.modalInstance && this.modalRappelComponent && this.modalRappelComponent.modalRappel) {
            const modalElement = this.modalRappelComponent.modalRappel.nativeElement;
            this.modalInstance = new Modal(modalElement);
        }
        if (this.modalInstance) {
            this.modalRappelComponent.openModal();
        } else {
            console.error('ModalRappelComponent or modalRappel is not defined');
        }
    }
  getExcerpt(text: string | undefined, length: number = 100): string {
    return text && text.length > length ? text.slice(0, length) + '...' : text || '';
  }
}
