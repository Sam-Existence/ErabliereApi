describe('Visits the web app', () => {
  it('visit the app', () => {
    cy.login()
    cy.visit('/')
    cy.contains('Érablière IU')
  });

  it('We are in a connected session. No authentication ask.', () => {
    cy.get("#login-button").should("not.exist");
    cy.get("#logout-button").should("exist");
  });
});