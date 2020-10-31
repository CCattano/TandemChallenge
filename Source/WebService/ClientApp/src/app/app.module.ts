//#region IMPORTS
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule, Provider } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { AccountComponent, MainMenuComponent, PlayerHistoryComponent, PlayerMenuComponent, TriviaGameComponent } from './components';
import { PlayerHistoryResolver } from './components/player/history/player-history.resolver';
import { PlayerMenuResolver } from './components/player/menu/player-menu.resolver';
import { TriviaGameResolver } from './components/trivia-game/trivia-game.resolver';
import { PlayerAPI, TriviaAPI } from './shared/api';
import { LayoutComponent, NavMenuComponent } from './shared/components';
import { AppInitializer, CommonAuthGuard, StatusResponseInterceptor, TandemTokenInterceptor } from './shared/middleware';
import { NavMenuService, PlayerTokenService, StatusResponseService, UtilityService } from './shared/service';
//#endregion

const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: "mainmenu" },
    { path: 'mainmenu', component: MainMenuComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
    { path: 'account/create', component: AccountComponent, pathMatch: 'full' },
    { path: 'account/login', component: AccountComponent, pathMatch: 'full' },
    { path: 'player/menu/:playerID', component: PlayerMenuComponent, resolve: { playerDetails: PlayerMenuResolver } },
    { path: 'player/history', component: PlayerHistoryComponent, resolve: { playerRounds: PlayerHistoryResolver } },
    { path: 'play', component: TriviaGameComponent, pathMatch: 'full', resolve: { trivia: TriviaGameResolver } },
    { path: 'play/guest', component: TriviaGameComponent, pathMatch: 'full', resolve: { trivia: TriviaGameResolver } },
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
        PlayerMenuComponent,
        MainMenuComponent,
        TriviaGameComponent,
        PlayerHistoryComponent,
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
        RouterModule.forRoot(routes),
        NgbModalModule
    ],
    providers: [
        //#region APIs
        TriviaAPI,
        PlayerAPI,
        //#endregion

        //#region Resolvers
        PlayerMenuResolver,
        TriviaGameResolver,
        PlayerHistoryResolver,
        //#endregion

        //#region AuthGuards
        CommonAuthGuard,
        //#endregion

        //#region State Machine Services
        StatusResponseService,
        PlayerTokenService,
        NavMenuService,
        //#endregion

        //#region Misc Services
        UtilityService,
        //#endregion

        //#region Interceptors
        httpInterceptors,
        //#endregion

        //#region Initializers
        AppInitializer,
        appInitializers
        //#endregion
    ],
    bootstrap: [LayoutComponent]
})
export class AppModule { }
