import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MsalService, MSAL_INSTANCE } from "@azure/msal-angular";
import { PublicClientApplication } from "@azure/msal-browser";
import { moduleMetadata } from "@storybook/angular";

export class ModuleStoryHelper{
    static getErabliereApiStoriesModuleMetadata(declarations: Array<any> = [], additionnalImports: Array<any> = []) {
        let imports: any[] = [
            HttpClientModule,
            FormsModule,
            ReactiveFormsModule,
            ...additionnalImports
        ]
        return moduleMetadata({
            declarations: declarations,
            imports: imports,
            providers: [
                { 
                    provide: MSAL_INSTANCE, 
                    useFactory: () => new PublicClientApplication({
                        auth: {
                            clientId: '',
                        }
                      })
                },
                MsalService,
            ]
        })
    }
}
