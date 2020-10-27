import { CanActivate, Router } from "@angular/router";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class CommonAuthGuard implements CanActivate {
    constructor(private router: Router) {
    }

    canActivate(route, state): boolean {
        //Don't let users access pages via url bar
        //Only access pages via in-app navigation
        if (this.router.url !== "/") {
            return true;
        } else {
            this.router.navigate(["/"]);
        }
    }
}