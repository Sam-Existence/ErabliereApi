import { HomePage } from "cypress/pages/home.page";
import { NotesPage } from "cypress/pages/notes.page";

describe("Notes page", { testIsolation: false }, () => {
    const homePage = new HomePage();
    let notesPage: NotesPage | null = null;

    it("should be able to navigate to note page", () => {
        homePage.visit();

        notesPage = homePage.clickOnNotesButtonNavMenu();

        notesPage.getPageTitle().should('have.text', 'Notes');
    });

    it("should add note without specifing date", () => {
        if (notesPage == null) {
            throw new Error("Notes page is not initialized");
        }

        // generate random title and content
        const title = `Note title ${Math.floor(Math.random() * 1000)}`;
        const content = `Note content ${Math.floor(Math.random() * 1000)}`;

        notesPage.addNote(title, content);

        cy.wait(1000);

        // validate that note is added
        notesPage.getNoteTitle().should('have.text', title);
        notesPage.getNoteDescription().should('have.text', content);
    });

    it("should click on cancel button", () => {
        if (notesPage == null) {
            throw new Error("Notes page is not initialized");
        }

        notesPage.getCancelButton().click();
    });

    it("should add note with specific date", () => {
        if (notesPage == null) {
            throw new Error("Notes page is not initialized");
        }

        // generate random title and content
        const title = `Note title ${Math.floor(Math.random() * 1000)}`;
        const content = `Note content ${Math.floor(Math.random() * 1000)}`;
        const date = new Date();

        notesPage.addNote(title, content, toISO8601(date));

        cy.wait(1000);

        // validate that note is added
        notesPage.getNoteTitle().should('have.text', title);
        notesPage.getNoteDescription().should('have.text', content);
        notesPage.getNoteDate().should('have.text', toISO8601(date));
    });

    function toISO8601(date: Date): string {
        // format a date using the iso 8601 standard
        // the date should have the offset in the format +00:00
        // the date should be in the format YYYY-MM-DD
        // the date should be in the format HH:mm:ss

        const offset = date.getTimezoneOffset();
        const hours = Math.floor(offset / 60);
        const minutes = offset % 60;

        const offsetString = `${hours <= 0 ? "+" : "-"}${pad2(Math.abs(hours))}:${minutes < 10 ? "0" : ""}${minutes}`;

        // format the date, mounth, date, hours, minutes and seconds should be 2 digits
        const dateString = `${date.getFullYear()}-${pad2(date.getMonth() + 1)}-${pad2(date.getDate())}`;
        const timeString = `${pad2(date.getHours())}:${pad2(date.getMinutes())}:${pad2(date.getSeconds())}`;

        return `${dateString}T${timeString}${offsetString}`;
    }

    function pad2(number: number) {
        return (number < 10 ? '0' : '') + number;
    }
});