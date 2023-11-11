import { HttpClientModule } from "@angular/common/http";
import { importProvidersFrom } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MsalService, MSAL_INSTANCE } from "@azure/msal-angular";
import { PublicClientApplication } from "@azure/msal-browser";
import { applicationConfig, moduleMetadata } from "@storybook/angular";
import { AppModule } from "src/app/app.module";

export class ModuleStoryHelper{
    static getErabliereApiStoriesModuleMetadata(declarations: Array<any> = [], additionnalImports: Array<any> = []) {

        return applicationConfig({
            providers: [importProvidersFrom(AppModule)]
        });
        
    }
}
