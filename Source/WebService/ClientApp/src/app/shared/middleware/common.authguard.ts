import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { PlayerTokenService } from "../service/player-token.service";

@Injectable({
    providedIn: 'root'
})
export class CommonAuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private tokenSvc: PlayerTokenService
    ) {
    }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        //Check if token exists
        if (this.tokenSvc.haveToken) {
            if (new Date() > this.tokenSvc.tokenTTL) {
                this.router.navigateByUrl("account/login");
            } else {
                this.router.navigateByUrl(`player/menu/${this.tokenSvc.playerID}`);
            }
        } else {
            if (state.url == "/mainmenu") {
                return true;
            } else {
                this.router.navigateByUrl("mainmenu");
            }
        }
        return false;
    }
}