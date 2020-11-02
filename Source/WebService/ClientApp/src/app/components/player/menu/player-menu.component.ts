import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { TriviaAPI } from '../../../shared/api/trivia.api';
import { ActivatedRoute, Router } from '@angular/router';
import { Player, PlayerHistory, PlayerRound } from '../../../shared/viewmodels';
import { NavMenuService } from '../../../shared/service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlayerAPI } from '../../../shared/api';
import { last } from 'rxjs/operators';

@Component({
    selector: 'app-player-menu',
    templateUrl: './player-menu.component.html',
    styles: [`
        .pmc-disabled-card {
            background: rgba(127, 127, 127, 0.25);
            mix-blend-mode: multiply;
        }
    `]
})
export class PlayerMenuComponent implements OnInit {
    player: Player;
    history: PlayerHistory[] = [];
    incompleteRound: PlayerRound = null;

    @ViewChild("inProgressModal", { static: true }) inProgressModal;

    constructor(
        route: ActivatedRoute,
        private router: Router,
        private navMenuSvc: NavMenuService,
        private triviaAPI: TriviaAPI,
        private playerAPI: PlayerAPI,
        private modalSvc: NgbModal
    ) {
        const playerDetails: {
            player: Player,
            playerHistory: PlayerHistory[]
        } = route.snapshot.data.playerDetails;

        this.player = playerDetails.player;
        if (![null, undefined, []].includes(playerDetails.playerHistory)) {
            this.history = playerDetails.playerHistory;
        }
        this.navMenuSvc.setPlayerName(this.player.name);
    }

    async ngOnInit(): Promise<void> {
        const lastGame: PlayerHistory = this.history.length > 0
            ? this.history[this.history.length - 1]
            : undefined;
        if (lastGame != undefined && [undefined, null].includes(lastGame.completedDateTime)) {
            //Last game was not finished, grab its data
            this.incompleteRound =
                await this.triviaAPI.getExistingRound(lastGame.playerHistoryID);
        }
    }

    async startNewGame(): Promise<void> {
        if (this.incompleteRound !== null) {
            const startNewGame: boolean = await this.modalSvc.open(this.inProgressModal, {
                size: "lg", keyboard: false
            }).result;
            if (startNewGame) {
                await this.playerAPI.markRoundCompleted(this.incompleteRound.playerHistoryID);
            }
        }
        this.router.navigate(["play"], { state: { player: this.player } });
    }

    resumeGame(): void {
        this.router.navigate(["play"], { state: { player: this.player, prevGame: this.incompleteRound } });
    }

    viewHistory(): void {
        this.router.navigate(["player/history"], { state: { playerHistory: this.history } });
    }

    manageAccount(): void {
        this.router.navigate([`player/account/${this.player.playerID}`]);
    }

    getLastPlayTime(): string {
        if (this.history.length === 0) {
            return new Date().toLocaleString();
        } else {
            const lastGame = this.history[this.history.length - 1];
            if ([null, undefined].includes(lastGame.completedDateTime)) {
                return new Date(lastGame.startedDateTime).toLocaleString();
            } else {
                return new Date(lastGame.completedDateTime).toLocaleString();
            }
        }
    }
}