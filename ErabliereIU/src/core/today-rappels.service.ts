import { Injectable } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Note } from 'src/model/note';

@Injectable({
    providedIn: 'root'
})
export class RappelService {
    constructor(private erabliereapiService: ErabliereApi) { }

    async getTodaysReminders(idErabliereSelectionnee:any): Promise<Note[]> {
        const todaysNote  = await this.erabliereapiService.getActiveRappelNotes(idErabliereSelectionnee);

        return todaysNote;
    }
}
