import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { PlayerAPI } from "../../shared/api";
import { Player } from "../../shared/viewmodels";

@Injectable()
export class PlayResolver implements Resolve<any> {
    constructor(private playerAPI: PlayerAPI) {
    }

    result: Player = null;
    async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<any> {
        const playerID: string = route.params["playerID"] || "";
        if (playerID === "") {
            return this.result;
        }

        const player: Player = await this.playerAPI.getPlayerByPlayerID(playerID);
        this.result = player;

        return this.result;
    }
}