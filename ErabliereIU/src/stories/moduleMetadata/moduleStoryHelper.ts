import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { importProvidersFrom } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { applicationConfig, moduleMetadata } from "@storybook/angular";
import { NgChartsModule } from "ng2-charts";
import { NgxMaskDirective, NgxMaskPipe } from "ngx-mask";
import { AppRoutingModule } from "src/app/app-routing.module";
import { AppModule } from "src/app/app.module";

export class ModuleStoryHelper{
    static getErabliereApiStoriesModuleMetadata(declarations: Array<any> = [], additionnalImports: Array<any> = []) {
        return moduleMetadata({
            declarations: declarations,
            imports: [
                CommonModule,
                NgChartsModule,
                //AppRoutingModule,
                HttpClientModule,
                ReactiveFormsModule,
                FormsModule,
                NgxMaskDirective, 
                NgxMaskPipe
            ],
        });
    }

    static getErabliereApiStoriesApplicationConfig() {
        return applicationConfig({
            providers: [importProvidersFrom(AppModule)]
        });
    }
}
