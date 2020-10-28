import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule, Provider } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent, MainMenuComponent, PlayComponent } from './components';
import { PlayResolver } from './components/player-menu/player-menu.resolver';
import { PlayerAPI, TriviaAPI } from './shared/api';
import { LayoutComponent, NavMenuComponent } from './shared/components';
import { AppInitializer } from './shared/middleware';
import { CommonAuthGuard, UrlNavAuthGuard, UserDataAuthGuard } from './shared/middleware/authguards';
import { StatusResponseInterceptor } from './shared/middleware/interceptors/status-response.interceptor';
import { TandemTokenInterceptor } from './shared/middleware/interceptors/tandem-token.interceptor';
import { PlayerTokenService } from './shared/service/player-token.service';
import { StatusResponseService } from './shared/service/status-response.service';

const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: "mainmenu" },
    { path: 'mainmenu', component: MainMenuComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
    { path: 'account/create', component: AccountComponent, pathMatch: 'full', canActivate: [UrlNavAuthGuard] },
    { path: 'account/login', component: AccountComponent, pathMatch: 'full', canActivate: [UrlNavAuthGuard] },
    { path: 'play/guest', component: PlayComponent, pathMatch: 'full', canActivate: [UrlNavAuthGuard] },
    {
        path: 'play/:playerID', component: PlayComponent,
        resolve: { player: PlayResolver }, canActivate: [UserDataAuthGuard]
    }
];

const httpInterceptors: Provider = [
    { provide: HTTP_INTERCEPTORS, useClass: StatusResponseInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: TandemTokenInterceptor, multi: true }
];

export function RunInitialization(appInitilaizer: AppInitializer): () => void {
    return () => appInitilaizer.initialize();
}
const appInitializers: Provider = [
    { provide: APP_INITIALIZER, useFactory: RunInitialization, deps: [AppInitializer], multi: true }
];

@NgModule({
    declarations: [
        //#region PAGE COMPONENTS
        AccountComponent,
        PlayComponent,
        MainMenuComponent,
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
        PlayerTokenService,
        //Interceptors
        httpInterceptors,
        //Initializers
        AppInitializer,
        appInitializers
    ],
    bootstrap: [LayoutComponent]
})
export class AppModule { }
