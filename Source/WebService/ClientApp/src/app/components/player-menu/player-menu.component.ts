import { Component } from '@angular/core';
import { TriviaAPI } from '../../shared/api/trivia.api';
import { ActivatedRoute, Router } from '@angular/router';
import { Player } from '../../shared/viewmodels';
import { NavMenuService } from '../../shared/service';

@Component({
    selector: 'app-player-menu',
    templateUrl: './player-menu.component.html',
})
export class PlayerMenuComponent {
    player: Player
    constructor(
        route: ActivatedRoute,
        private router: Router,
        private navMenuSvc: NavMenuService
    ) {
        this.player = route.snapshot.data.player || null;
        this.navMenuSvc.setPlayerName(this.player.name);
        //TODO in resolver setup call for game history
    }

    startNewGame(): void {
        this.router.navigate([`play`], { state: { player: this.player } });
    }
}
