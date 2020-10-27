import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, Provider } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent, PlayComponent } from './components';
import { PlayResolver } from './components/play/play.resolver';
import { PlayerAPI, TriviaAPI } from './shared/api';
import { LayoutComponent, NavMenuComponent } from './shared/components';
import { CommonAuthGuard, StatusResponseInterceptor } from './shared/middleware';
import { StatusResponseService } from './shared/service/statusResponse.service';

const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'account' },
    { path: 'account', component: AccountComponent, pathMatch: 'full' },
    {
        path: 'play/:playerID',
        component: PlayComponent,
        resolve: {
            player: PlayResolver
        },
        canActivate: [CommonAuthGuard]
    }
];

const httpInterceptors: Provider = [
    { provide: HTTP_INTERCEPTORS, useClass: StatusResponseInterceptor, multi: true }
];

@NgModule({
    declarations: [
        //#region PAGE COMPONENTS
        AccountComponent,
        PlayComponent,
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
        //APIs
        TriviaAPI,
        PlayerAPI,
        //Resolvers
        PlayResolver,
        //AuthGuards
        CommonAuthGuard,
        //State Machine Services
        StatusResponseService,
        //Interceptors
        httpInterceptors
    ],
    bootstrap: [LayoutComponent]
})
export class AppModule { }
