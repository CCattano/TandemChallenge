import { Component } from '@angular/core';
import { TriviaAPI } from '../../shared/api/trivia.api';
import { ActivatedRoute } from '@angular/router';
import { Player } from '../../shared/viewmodels';

@Component({
    selector: 'app-player-menu',
    templateUrl: './player-menu.component.html',
})
export class PlayComponent {
    player: Player
    playingAsGuest: boolean;
    constructor(route: ActivatedRoute) {
        this.player = route.snapshot.data.player || null;
        this.playingAsGuest = this.player === null;
    }
}
