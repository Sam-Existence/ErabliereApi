import { Component, OnInit } from '@angular/core';
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

  constructor(private erabliereapiService: ErabliereApi) { }

  async ngOnInit() {
    const idErabliereSelectionnee: any = null; // Replace null with the actual value
    const skip: number = 0; // Replace 0 with the actual value
    const top: number | undefined = undefined; // Replace undefined with the actual value
    this.todayNotes = await this.getTodaysReminders(idErabliereSelectionnee, skip, top);
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
