import {Component, OnInit, Input, SimpleChanges, OnChanges} from '@angular/core';
import { NgFor } from '@angular/common';
import { Note } from 'src/model/note';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { RappelComponent } from './rappel.component';

@Component({
  selector: 'app-rappels',
  standalone: true,
  imports: [
    NgFor,
    RappelComponent
  ],
  styleUrls: ['./rappels.component.css'] ,
  templateUrl: './rappels.component.html'
})
export class RappelsComponent implements OnChanges {
    @Input() idErabliereSelectionnee: any;
    todayReminders: Note[] = [];

    constructor(private erabliereapiService: ErabliereApi) { }

  async ngOnChanges(changes: SimpleChanges) {
    if (changes.idErabliereSelectionnee &&
      changes.idErabliereSelectionnee.currentValue !== changes.idErabliereSelectionnee.previousValue &&
      changes.idErabliereSelectionnee.currentValue) {
      try {
        this.todayReminders = await this.erabliereapiService.getActiveRappelNotes(this.idErabliereSelectionnee);

        await this.erabliereapiService.putNotePeriodiciteDue(this.idErabliereSelectionnee);
      } catch (error) {
        console.error('Error getting today\'s reminders', error);
      }
    }
  }
}
