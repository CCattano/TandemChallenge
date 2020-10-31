import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { TriviaAPI } from "../../../shared/api";
import { Player, PlayerHistory, PlayerRound } from "../../../shared/viewmodels";

@Injectable()
export class PlayerHistoryResolver implements Resolve<any> {
    private result: PlayerRound[] = [];

    constructor(
        private router: Router,
        private triviaAPI: TriviaAPI
    ) {
    }

    async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<any> {
        this.result = [];
        const history: PlayerHistory[] = this.router.getCurrentNavigation().extras.state.playerHistory;

        const tasks: Promise<void>[] = [];

        history.forEach(h => tasks.push(this.getCompletedRound(h.playerHistoryID)));

        await Promise.all(tasks);

        return this.result;
    }

    private async getCompletedRound(playerHistoryID: number): Promise<void> {
        const response: PlayerRound = await this.triviaAPI.getExistingRound(playerHistoryID);
        //response.startedDateTime and response.completedDateTime comes as string from .NET, parse to actual Date
        //Tried doing this through get/set, wasn't having any luck, would ideally centralize this process somehow or sort out .NET
        response.startedDateTime = new Date(response.startedDateTime);
        if (![null, undefined].includes(response.completedDateTime)) {
            response.completedDateTime = new Date(response.completedDateTime);
        }
        this.result.push(response);
    }
}