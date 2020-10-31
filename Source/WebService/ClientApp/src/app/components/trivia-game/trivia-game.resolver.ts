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
        this.result = null;

        const routeNavData: { prevGame?: PlayerRound, player?: Player } = this.router.getCurrentNavigation().extras.state;
        if (route.routeConfig.path.includes("guest")) {
            //playing guest game, fetch round
            const response: PlayerHistory = await this.triviaAPI.getGuestTriviaRound();
            //response.startedDateTime comes as string from .NET, parse to actual Date
            //Tried doing this through get/set, wasn't having any luck, would ideally centralize this process somehow or sort out .NET
            response.startedDateTime = new Date(response.startedDateTime);
            this.result = response;

        } else if (routeNavData.prevGame == undefined) {
            //starting new game for account, fetch round
            //PrevGame does not contain data, fetch a round of trivia questions
            const playerID: number = routeNavData.player.playerID;
            const response: PlayerHistory = await this.triviaAPI.getTriviaRound(playerID);
            //response.startedDateTime comes as string from .NET, parse to actual Date
            //Tried doing this through get/set, wasn't having any luck, would ideally centralize this process somehow or sort out .NET
            response.startedDateTime = new Date(response.startedDateTime);
            this.result = response;
        } else {
            //Resuming existing game, /player/menu has set game data on route already
        }
        return this.result;
    }
}