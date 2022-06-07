export class NotesPage {
    getPageTitle(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.get('note.col-lg-10 > h3');
    }

    getAddButton(): Cypress.Chainable<JQuery<HTMLElement>> {
        var addButton = cy.get('#addNoteButton');
        return addButton;
    }

    enterNoteTitle(value: string): void {
        cy.get("note", { timeout: 10000 }).then(noteComponent => {
            cy.wrap(noteComponent)
              .find('input[formcontrolname="title"]')
              .type(value);
        });
    }

    enterNoteDescription(value: string): void {
        cy.get("note", { timeout: 10000 }).then(noteComponent => {
            cy.wrap(noteComponent)
              .find('input[formcontrolname="text"]')
              .type(value);
        });
    }

    addNote(title: string, content: string): NotesPage {
        this.getAddButton()
            .click();

        this.enterNoteTitle(title);
        this.enterNoteDescription(content);

        return this;
    }
}