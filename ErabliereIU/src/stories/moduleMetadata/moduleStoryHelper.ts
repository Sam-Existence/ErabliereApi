import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { MsalService, MSAL_INSTANCE } from "@azure/msal-angular";
import { PublicClientApplication } from "@azure/msal-browser";
import { moduleMetadata } from "@storybook/angular";
import { AppRoutingModule } from "src/app/app-routing.module";

export class ModuleStoryHelper{
    static getErabliereApiStoriesModuleMetadata() {
        return moduleMetadata({
            imports: [
                HttpClientModule,
                FormsModule,
                ReactiveFormsModule,
            ],
            providers: [
                { 
                    provide: MSAL_INSTANCE, 
                    useFactory: () => new PublicClientApplication({
                        auth: {
                            clientId: '',
                        }
                      })
                },
                MsalService
            ]
        })
    }
}
