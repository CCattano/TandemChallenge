import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { PlayerTokenService } from "../../service/player-token.service";

@Injectable({
    providedIn: 'root'
})
export class UserDataAuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private tokenSvc: PlayerTokenService
    ) {
    }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        debugger;
        let playerID: string = route.params["playerID"] || "";
        if (playerID === "") {
            this.router.navigate(["/"]);
        } else if (Number(playerID) !== this.tokenSvc.playerID) {
            this.router.navigate(["/"]);
        } else {
            return true;
        }
    }
}