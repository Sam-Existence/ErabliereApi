import { HomePage } from "cypress/pages/home.page";

describe("Notes page", () => {
    const homePage = new HomePage();

    it("should be able to add note", () => {
        homePage.visit();

        var notesPage = homePage.clickOnNotesButtonNavMenu();

        notesPage.getPageTitle().should('have.text', 'Notes');

        notesPage.addNote("This is a note title", "This is a note content");
    });
});