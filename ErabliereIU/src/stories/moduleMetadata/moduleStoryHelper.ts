import { provideHttpClient } from "@angular/common/http";
import { provideRouter } from "@angular/router";
import { MSAL_INSTANCE, MsalService } from "@azure/msal-angular";
import { applicationConfig } from "@storybook/angular";
import { provideNgxMask } from "ngx-mask";
import { MSALInstanceFactory } from "src/app/app.module";
import { EnvironmentService } from "src/environments/environment.service";

export class ModuleStoryHelper{
    static getErabliereApiStoriesApplicationConfig() {
        return applicationConfig({
            providers: [
                provideRouter([]),
                provideHttpClient(), 
                provideNgxMask(),
                MsalService,
                {
                  provide: MSAL_INSTANCE,
                  useFactory: MSALInstanceFactory,
                  deps: [EnvironmentService]
                }
            ]
        });
    }
}
