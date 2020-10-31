import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { PlayerAPI } from "../../../shared/api";
import { Player, PlayerHistory } from "../../../shared/viewmodels";

@Injectable()
export class PlayerMenuResolver implements Resolve<any> {
    constructor(private playerAPI: PlayerAPI) {
    }

    private result: {
        player: Player,
        playerHistory: PlayerHistory[]
    } = {} as { player: Player, playerHistory: PlayerHistory[] };
    async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<any> {
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
        const playerHistory: PlayerHistory[] = await this.playerAPI.getAllPlayerHistory(playerID);
        this.result.playerHistory = playerHistory;
    }
}