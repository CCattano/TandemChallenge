import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot, Router } from "@angular/router";
import { TriviaAPI } from "../../shared/api";
import { PlayerHistory, PlayerRound, Player } from "../../shared/viewmodels";

@Injectable()
export class TriviaGameResolver implements Resolve<any> {
    constructor(
        private triviaAPI: TriviaAPI,
        private router: Router
    ) {
    }

    result: PlayerHistory = null;
    async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<any> {
        const routeNavData: { prevGame?: PlayerRound, player?: Player } = this.router.getCurrentNavigation().extras.state;
        if (route.routeConfig.path.includes("guest")) {
            //playing guest game, fetch round
            const response: PlayerHistory = await this.triviaAPI.getGuestTriviaRound();
            this.result = response;

        } else if (routeNavData.prevGame == undefined) {
            //starting new game for account, fetch round
            //PrevGame does not contain data, fetch a round of trivia questions
            const playerID: number = routeNavData.player.playerID;
            const response: PlayerHistory = await this.triviaAPI.getTriviaRound(playerID);
            this.result = response;
        } else {
            //Resuming existing game, /playermenu has set game data on route already
        }
        return this.result;
    }
}