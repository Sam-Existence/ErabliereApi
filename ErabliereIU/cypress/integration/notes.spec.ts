import { HomePage } from "cypress/pages/home.page";
import { NotesPage } from "cypress/pages/notes.page";

describe("Notes page", () => {
    const homePage = new HomePage();
    let notesPage: NotesPage | null = null;

    it("should be able to navigate to note page", () => {
        homePage.visit();

        notesPage = homePage.clickOnNotesButtonNavMenu();

        notesPage.getPageTitle().should('have.text', 'Notes');
    });

    it("should add note", () => {
        if (notesPage == null) {
            throw new Error("Notes page is not initialized");
        }

        notesPage.addNote("This is a note title", "This is a note content");
    });
});