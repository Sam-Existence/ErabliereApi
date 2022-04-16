declare namespace Cypress {
    interface Chainable {
        login(): void;
    }
}

Cypress.Commands.add('login', () => {
    cy.request({
        method: "GET",
        url: "/assets/config/oauth-oidc.json"
    }).then(body => {
        const config = body.body;

        if (config.authEnable) {

            if (config.tenantId != undefined && config.tenantId?.length > 1) {
                cy.request({
                    method: "POST",
                    url: `https://login.microsoftonline.com/${config.tenantId}/oauth2/token`,
                    form: true,
                    body: {
                    grant_type: "client_credentials",
                    client_id: Cypress.env("clientId"),
                    client_secret: Cypress.env("clientSecret"),
                    },
                }).then(response => {
                    const ADALToken = response.body.access_token;
                    const expiresOn = response.body.expires_on;
            
                    localStorage.setItem("adal.token.keys", `${Cypress.env("clientId")}|`);
                    localStorage.setItem(`adal.access.token.key${Cypress.env("clientId")}`, ADALToken);
                    localStorage.setItem(`adal.expiration.key${Cypress.env("clientId")}`, expiresOn);
                    localStorage.setItem("adal.idtoken", ADALToken);
                });
            }
            else {
                // TODO: Authenticate using identity server
            }
        }
    });
});