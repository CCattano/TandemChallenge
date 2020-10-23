import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { LayoutComponent, NavMenuComponent } from './shared/components';
import { CommonAPI } from './shared/api/common.api';

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' }
];

@NgModule({
    declarations: [
        //#region PAGE COMPONENTS
        HomeComponent,
        //#endregion

        //#region SHARED COMPONENTS
        LayoutComponent,
        NavMenuComponent,
        //#endregion
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot(routes)
    ],
    providers: [
        CommonAPI
    ],
    bootstrap: [LayoutComponent]
})
export class AppModule { }
