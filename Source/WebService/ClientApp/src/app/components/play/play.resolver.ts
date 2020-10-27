import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";

@Injectable()
export class PlayResolver implements Resolve<any> {
    result: string;
    async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<any> {
        this.result = route.params["playerID"];
        return this.result;
    }
}