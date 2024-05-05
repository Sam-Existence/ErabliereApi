import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { NgFor } from '@angular/common';
import { Note } from 'src/model/note';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { RappelComponent } from './rappel.component';
import { RappelService } from "../core/today-rappels.service";

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
export class RappelsComponent {
    @Input() idErabliereSelectionnee: any;
    todayReminders: Note[] = [];

    constructor(private rappelService: RappelService) { }

    async ngOnChanges(changes: SimpleChanges) {
        if (changes.idErabliereSelectionnee &&
            changes.idErabliereSelectionnee.currentValue !== changes.idErabliereSelectionnee.previousValue &&
            changes.idErabliereSelectionnee.currentValue) {
            try {
                this.todayReminders = await this.rappelService.getTodaysReminders(this.idErabliereSelectionnee);
            } catch (error) {
                console.error('Error getting today\'s reminders', error);
            }
        }
    }
}
