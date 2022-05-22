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
            if (isAzureAD(config)) {
                // Check if there is already a token in local storage
                // var token = localStorage.getItem("adal.idtoken");

                // if (token) {
                //     // Check if the token is still valid
                //     var expiresOn = localStorage.getItem(`adal.expiration.key${Cypress.env("clientId")}`) ?? "";
                //     var now = new Date();
                //     var expiresOnDate = new Date(parseInt(expiresOn));
                //     if (now < expiresOnDate) {
                //         cy.log("Token is still valid, expire at " + expiresOnDate);
                //         return;
                //     }
                // }

                cy.request({
                    method: "POST",
                    url: `https://login.microsoftonline.com/${config.tenantId}/oauth2/token`,
                    form: true,
                    body: {
                        grant_type: "client_credentials",
                        client_id: Cypress.env("clientId"),
                        client_secret: Cypress.env("clientSecret"),
                        scope: config.scopes
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
                cy.request({
                    method: "POST",
                    url: `${config.stsAuthority}/connect/token`,
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded",
                        "Authorization": "Basic cmFzcGJlcnJ5bG9jYWw6c2VjcmV0"
                    },
                    form: true,
                    body: "grant_type=client_credentials&scope=erabliereapi"
                }).then(res => {
                    sessionStorage.setItem('oidc.user:https://192.168.0.110:5005:erabliereiu', JSON.stringify(res.body));
                })
            }
        }
    });
});

function isAzureAD(config: any) {
    return config.tenantId != undefined && config.tenantId?.length > 1;
}

