export class FormUtil {
    static typeText(text: string, selector: string, formControl: string, timeout: number = 10000): void {
        cy.get(selector, { timeout: timeout }).then(element => {
            cy.wrap(element)
                .find('input[formcontrolname="' + formControl + '"]')
                .type(text);
        });
    }
    static clickButton(selector: string, buttonId: string, timeout: number = 10000): void {
        cy.get(selector, { timeout: timeout }).then(noteComponent => {
            cy.wrap(noteComponent)
                .find('button[id="' + buttonId + '"]')
                .click();
        });
    }
}