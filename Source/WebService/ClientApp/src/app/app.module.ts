import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule, Provider } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent, MainMenuComponent, PlayerMenuComponent, TriviaGameComponent, PlayerHistoryComponent } from './components';
import { PlayerMenuResolver } from './components/player/menu/player-menu.resolver';
import { TriviaGameResolver } from './components/trivia-game/trivia-game.resolver';
import { PlayerAPI, TriviaAPI } from './shared/api';
import { LayoutComponent, NavMenuComponent } from './shared/components';
import { AppInitializer } from './shared/middleware';
import { CommonAuthGuard } from './shared/middleware/common.authguard';
import { StatusResponseInterceptor } from './shared/middleware/interceptors/status-response.interceptor';
import { TandemTokenInterceptor } from './shared/middleware/interceptors/tandem-token.interceptor';
import { NavMenuService } from './shared/service';
import { PlayerTokenService } from './shared/service/player-token.service';
import { StatusResponseService } from './shared/service/status-response.service';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';


const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: "mainmenu" },
    { path: 'mainmenu', component: MainMenuComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
    { path: 'account/create', component: AccountComponent, pathMatch: 'full' },
    { path: 'account/login', component: AccountComponent, pathMatch: 'full' },
    { path: 'player/menu/:playerID', component: PlayerMenuComponent, resolve: { playerDetails: PlayerMenuResolver } },
    { path: 'player/history', component: PlayerHistoryComponent },
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
        //APIs
        TriviaAPI,
        PlayerAPI,
        //Resolvers
        PlayerMenuResolver,
        TriviaGameResolver,
        //AuthGuards
        CommonAuthGuard,
        //State Machine Services
        StatusResponseService,
        PlayerTokenService,
        NavMenuService,
        //Interceptors
        httpInterceptors,
        //Initializers
        AppInitializer,
        appInitializers
    ],
    bootstrap: [LayoutComponent]
})
export class AppModule { }
