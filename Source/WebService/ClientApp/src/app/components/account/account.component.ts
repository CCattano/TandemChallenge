import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PlayerAPI } from '../../shared/api';
import { StatusResponseService } from '../../shared/service';
import { PlayerTokenService } from '../../shared/service/player-token.service';

@Component({
    selector: 'app-acct',
    templateUrl: './account.component.html',
})
export class AccountComponent {
    forLogin: boolean = false;

    username: string;
    password: string = "";
    errorMsg: string;
    displayErr: boolean;

    //#region NEW ACCOUNT VARS
    repeatPassword: string;
    shouldDisplayAvailability: boolean = false;
    usernameAvailable: boolean;
    private lastCheckedUsername: string;
    shouldComparePw: boolean = false;
    passwordsMatch: boolean = false;
    private pwDebounceTimerID: number;
    private usernameDebounceTimerID: number;
    //#endregion

    constructor(
        private playerAPI: PlayerAPI,
        private router: Router,
        private tokenSvc: PlayerTokenService,
        private statusRespSvc: StatusResponseService
    ) {
        this.forLogin = this.router.url.includes("login");
    }

    //#region LOGIN METHODS
    public async login(): Promise<void> {
        const playerToken: string = await this.playerAPI.loginToAccount(this.username, this.password);
        if (![undefined, null, ""].includes(playerToken)) {
            //Received token from successful call, set token and go to main menu
            this.tokenSvc.setToken(playerToken, true);
            this.navigateAfterToken(playerToken);
        } else {
            //Did not receive token, something went wrong
            this.trySetError();
        }
    }
    //#endregion

    //#region NEW ACCOUNT METHODS

    //API METHODS
    private async checkUsername(): Promise<void> {
        if ([undefined, null, ""].findIndex(v => v === this.username) !== -1) {
            this.shouldDisplayAvailability = false;
            this.usernameAvailable = false;
            return;
        } else if (this.lastCheckedUsername === this.username) {
            return;
        }
        //Set these for the UI while we're performing the check
        this.shouldDisplayAvailability = false;
        this.usernameAvailable = false;

        //On next execution cycle begin check
        window.setTimeout(async () => {
            this.usernameAvailable = await this.playerAPI.playerNameIsAvailable(this.username);
            this.shouldDisplayAvailability = true;
            this.lastCheckedUsername = `${this.username}`; //Set by value not by reference
        }, 0);
    }

    public async createAccount(): Promise<void> {
        const playerToken: string = await this.playerAPI.createAccount(this.username, this.password);
        this.navigateAfterToken(playerToken);
    }

    //UI HELPER METHODS
    public usernameCheck(username: string): void {
        this.username = username;
        if (this.usernameDebounceTimerID != undefined) {
            window.clearTimeout(this.usernameDebounceTimerID);
        }
        this.usernameDebounceTimerID = window.setTimeout(async () => {
            await this.checkUsername();
        }, 333);
    }

    public comparePasswords(repeatPw: string): void {
        this.repeatPassword = repeatPw;
        if (this.pwDebounceTimerID != undefined) {
            window.clearTimeout(this.pwDebounceTimerID);
        }
        this.pwDebounceTimerID = window.setTimeout(() => {
            this.shouldComparePw = this.repeatPassword != undefined && this.repeatPassword !== null && this.repeatPassword !== "";
            this.passwordsMatch = this.shouldComparePw
                ? this.repeatPassword == this.password
                : false;
        }, 333);
    }

    public submitDisabled(): boolean {
        //Username must be provided and available
        //Password must be provided
        //Verify password must match initial password
        const nullEmptyUndefined: any[] = [undefined, null, ""];
        if (
            nullEmptyUndefined.findIndex(v => v === this.username) !== -1
            || nullEmptyUndefined.findIndex(v => v === this.password) !== -1
            || nullEmptyUndefined.findIndex(v => v === this.repeatPassword) !== -1
            || !this.passwordsMatch
            || !this.usernameAvailable
        ) {
            return true;
        } else {
            return false;
        }
    }

    //#endregion

    //Shared methods
    private navigateAfterToken(token: string) {
        this.tokenSvc.setToken(token, true);
        this.router.navigateByUrl(`player/menu/${this.tokenSvc.playerID}`);
    }
    private trySetError(): void {
        this.displayErr = true;
        if (
            this.statusRespSvc.statusResponse.statusDetails != undefined
            && this.statusRespSvc.statusResponse.statusDetails.length > 0
        ) {
            this.errorMsg = this.statusRespSvc.statusResponse.statusDetails[0].desc;
        } else {
            this.errorMsg = "An unknown error has occurred. Please try again.";
        }
    }
}