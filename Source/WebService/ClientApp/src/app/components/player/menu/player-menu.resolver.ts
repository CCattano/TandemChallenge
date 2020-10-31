import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { PlayerAPI } from "../../../shared/api";
import { Player, PlayerHistory } from "../../../shared/viewmodels";

@Injectable()
export class PlayerMenuResolver implements Resolve<any> {
    constructor(private playerAPI: PlayerAPI) {
    }

    private result: { player: Player, playerHistory: PlayerHistory[] };

    async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<any> {
        this.result = {} as { player: Player, playerHistory: PlayerHistory[] };

        const playerID: number = parseInt(route.params["playerID"]);

        const tasks: Promise<void>[] = [
            this.getPlayer(playerID),
            this.getPlayerHistory(playerID)
        ];

        await Promise.all(tasks);

        return this.result;
    }

    private async getPlayer(playerID: number): Promise<void> {
        const player: Player = await this.playerAPI.getPlayerByPlayerID(playerID);
        this.result.player = player;
    }

    private async getPlayerHistory(playerID: number): Promise<void> {
        const response: PlayerHistory[] = await this.playerAPI.getAllPlayerHistory(playerID);
        //response.startedDateTime and response.completedDateTime comes as string from .NET, parse to actual Date
        //Tried doing this through get/set, wasn't having any luck, would ideally centralize this process somehow or sort out .NET
        response.forEach(ph => {
            ph.startedDateTime = new Date(ph.startedDateTime);
            if (![null, undefined].includes(ph.completedDateTime)) {
                ph.completedDateTime = new Date(ph.completedDateTime);
            }
        });
        this.result.playerHistory = response;
    }
}