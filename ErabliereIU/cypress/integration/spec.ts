import { HomePage } from "cypress/pages/home.page";

describe('Visits the web app', () => {
  const homePage = new HomePage();

  it('visit the app', () => {
    homePage.visit();
    cy.contains('Érablière IU')
  });

  it('We are in a connected session. No authentication ask.', () => {
    cy.get("#login-button").should("not.exist");
    cy.get("#logout-button").should("exist");
  });
});