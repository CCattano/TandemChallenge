import { Component } from '@angular/core';
import { TriviaAPI } from '../../shared/api/trivia.api';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-play',
    templateUrl: './play.component.html',
})
export class PlayComponent {
    playerID: string;
    constructor(route: ActivatedRoute) {
        this.playerID = route.snapshot.data.player;
    }
}
