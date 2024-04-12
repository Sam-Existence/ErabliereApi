import { Component, Input } from '@angular/core';
import { th } from 'date-fns/locale';
import { Note } from 'src/model/note';

@Component({
  selector: 'app-rappel',
  standalone: true,
  imports: [],
  templateUrl: './rappel.component.html'
})
export class RappelComponent {
  @Input() note: Note;

  constructor() {
    this.note = new Note();
   }

   getExcerpt(text: string, length: number = 100): string {
    return text.length > length ? text.slice(0, length) + '...' : text;
  }
}
