import {Component, Input} from '@angular/core';
import { Note } from 'src/model/note';

@Component({
  selector: 'app-modal-rappel',
  standalone: true,
  imports: [],
  templateUrl: './modal-rappel.component.html',
  styleUrl: './modal-rappel.component.css'
})
export class ModalRappelComponent {
    @Input() note: Note;

    constructor() {
        this.note = new Note();
    }
}
