import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { PlayerTokenService } from "../../service/player-token.service";

@Injectable({
    providedIn: 'root'
})
export class UrlNavAuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private tokenSvc: PlayerTokenService
    ) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        debugger;
        //The router's url will be equal to "/" on app intialization
        //So if a user tried to enter a page straight from the url bar
        //They are redirected to the main menu to begin the app flow
        if (this.router.url === "/") {
            this.router.navigateByUrl("mainmenu");
            return false;
        } else {
            return true;
        }
    }
}