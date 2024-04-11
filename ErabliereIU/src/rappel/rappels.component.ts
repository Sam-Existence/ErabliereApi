import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Note } from 'src/model/note';
import { ErabliereApi } from 'src/core/erabliereapi.service';

@Component({
  selector: 'app-rappels',
  standalone: true,
  imports: [],
  templateUrl: './rappels.component.html'
})
export class RappelsComponent implements OnInit {
  todayNotes: Note[] = [];

  constructor(private erabliereapiService: ErabliereApi, private _route: ActivatedRoute) { }

  async ngOnInit() {
    this._route.paramMap.subscribe(params => {
      const idErabliereSelectionnee = params.get('idErabliereSelectionnee');
      if (idErabliereSelectionnee) {
        this.getTodaysReminders(idErabliereSelectionnee).then(notes => this.todayNotes = notes);
      }
    });
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

    return todayNotes;
  }

}
