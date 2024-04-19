import { Injectable } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Note } from 'src/model/note';

@Injectable({
    providedIn: 'root'
})
export class RappelService {
    constructor(private erabliereapiService: ErabliereApi) { }

    async getTodaysReminders(idErabliereSelectionnee:any, skip: number = 0, top?: number): Promise<Note[]> {
        const notes = await this.erabliereapiService.getNotes(idErabliereSelectionnee, skip, top);

        const today = new Date();
        today.setHours(0, 0, 0, 0);

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
