import { HomePage } from "../pages/home.page";

describe('Visits the web app', { testIsolation: false }, () => {
  const homePage = new HomePage();

  it('visit the app', () => {
    homePage.visit();
    cy.contains('Érablière IU')
  });
  
});