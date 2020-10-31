import { Component } from "@angular/core";
import { PlayerHistory } from "../../../shared/viewmodels";

@Component({
    selector: 'app-player-history',
    templateUrl: './player-history.component.html'
})
export class PlayerHistoryComponent {
    history: PlayerHistory[] = [];
    constructor() {
        //TODO: Verify that abandoning a game causes the right data to show up here
        const history: PlayerHistory[] = window.history.state.playerHistory;
        if (![undefined, null].includes(history)) {
            this.history = history;
        }
    }
}