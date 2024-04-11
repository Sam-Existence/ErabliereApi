import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { NgFor } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
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
  templateUrl: './rappels.component.html'
})
export class RappelsComponent implements OnInit {
  @Input() idErabliereSelectionnee: any;
  todayReminders: Note[] = [];

  constructor(private erabliereapiService: ErabliereApi, private _route: ActivatedRoute) { }

  async ngOnChanges(changes: SimpleChanges) {
    console.log('ngOnChanges called');
    if (changes.idErabliereSelectionnee && changes.idErabliereSelectionnee.currentValue) {
      console.log('idErabliereSelectionnee changed', this.idErabliereSelectionnee);
      try {
        this.todayReminders = await this.getTodaysReminders(this.idErabliereSelectionnee);
        console.log('todayReminders', this.todayReminders);
      } catch (error) {
        console.error('Error getting today\'s reminders', error);
      }
    }
  }
  
  async ngOnInit() {
    console.log('ngOnInit called');
    if (this.idErabliereSelectionnee) {
      console.log('idErabliereSelectionnee', this.idErabliereSelectionnee);
      try {
        this.todayReminders = await this.getTodaysReminders(this.idErabliereSelectionnee);
        console.log('todayReminders', this.todayReminders);
      } catch (error) {
        console.error('Error getting today\'s reminders', error);
      }
    }
  }

  async getTodaysReminders(idErabliereSelectionnee:any, skip: number = 0, top?: number): Promise<Note[]> {
    // Get all notes
    const notes = await this.erabliereapiService.getNotes(idErabliereSelectionnee, skip, top);

    // Get today's date
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    // Filter notes whose reminder date is today
    const todayNotes = notes.filter(note => {
      const reminderDate = note.reminderDate ? new Date(note.reminderDate) : undefined;
      if (reminderDate) {
        reminderDate.setHours(0, 0, 0, 0);
        return reminderDate.getTime() === today.getTime();
      }
      return false;
    });

    console.log('todayNotes', todayNotes);
    console.log('ohohohohoho');
    return todayNotes;
  }

}
