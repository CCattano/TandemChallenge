import { Component } from "@angular/core";
import { PlayerAPI } from "../../../shared/api";
import { ActivatedRoute, Router } from "@angular/router";
import { PlayerTokenService, StatusResponseService } from "../../../shared/service";

@Component({
    selector: 'app-player-acct',
    templateUrl: './player-account.component.html'
})
export class PlayerAccountComponent {
    playerID: number;

    //#region SHARED VARS
    changeUsername: boolean = false;
    changePassword: boolean = false;
    updateSuccess: boolean;
    updateErrMsg: string;
    displayErr: boolean;
    //#endregion

    //#region USERNAME VARS
    username: string;
    usernameDebounceTimerID: number;
    shouldDisplayAvailability: boolean;
    usernameAvailable: boolean;
    lastCheckedUsername: string;
    //#endregion

    //#region PW VARS
    crntPassword: string;
    newPassword: string;
    repeatNewPassword: string;
    newPasswordMatch: boolean;
    shouldCompareNewPw: boolean;
    private pwDebounceTimerID: number;
    //#endregion

    constructor(
        route: ActivatedRoute,
        private router: Router,
        private playerAPI: PlayerAPI,
        private tokenSvc: PlayerTokenService,
        private statusRespSvc: StatusResponseService
    ) {
        this.playerID = parseInt(route.snapshot.params["playerID"]);
    }

    public toggleOpts(forUsername: boolean): void {
        if (forUsername) {
            this.changeUsername = true;
            this.changePassword = false;
        } else {
            this.changePassword = true;
            this.changeUsername = false;
        }
    }

    goBack(): void {
        if (this.changeUsername || this.changePassword) {
            //Reset shared vars
            this.changeUsername = this.changePassword = this.updateSuccess = this.displayErr = false;
            this.updateErrMsg = undefined;
            //Reset username vars
            this.username = this.lastCheckedUsername = this.usernameDebounceTimerID = undefined;
            this.shouldDisplayAvailability = this.usernameAvailable = false;
            //Reset password vars
            this.crntPassword = this.newPassword = this.repeatNewPassword = this.pwDebounceTimerID = undefined;
            this.newPasswordMatch = this.shouldCompareNewPw = false;
        } else {
            this.router.navigate(["/mainmenu"]);
        }
    }

    public updateDisabled(): boolean {
        let shouldDisable: boolean;
        const nullEmptyUndefined = [undefined, null, ""];
        if (this.changeUsername) {
            shouldDisable = nullEmptyUndefined.includes(this.username) || !this.usernameAvailable;
        } else {
            shouldDisable = nullEmptyUndefined.includes(this.crntPassword)
                || nullEmptyUndefined.includes(this.newPassword)
                || nullEmptyUndefined.includes(this.repeatNewPassword)
                || !this.newPasswordMatch;
        }
        return shouldDisable;
    }

    public async update(): Promise<void> {
        if (this.changeUsername) {
            const newToken: string = await this.playerAPI.changeUsername(this.playerID, this.username);
            if (![undefined, null, ""].includes(newToken)) {
                //Received token from successful call, set token and go back to main menu
                this.tokenSvc.setToken(newToken, true);
                this.updateSuccess = true;
                window.setTimeout(() => {
                    this.router.navigate(["/mainmenu"]);
                }, 2000);
            } else {
                //Did not receive token, something went wrong
                this.trySetError();
            }
        } else {
            await this.playerAPI.changePassword(this.playerID, this.crntPassword, this.newPassword);
            if (this.statusRespSvc.statusResponse.statusCode !== this.statusRespSvc.status.success) {
                //StatusResponse does not indicate success, try to set provided error details
                this.trySetError();
            } else {
                //Call was successful go back to main menu
                this.updateSuccess = true;
                window.setTimeout(() => {
                    this.router.navigate(["/mainmenu"]);
                }, 3000);
            }
        }
    }

    //#region USERNAME METHODS

    public usernameCheck(username: string): void {
        this.username = username;
        if (this.usernameDebounceTimerID != undefined) {
            window.clearTimeout(this.usernameDebounceTimerID);
        }
        this.usernameDebounceTimerID = window.setTimeout(async () => {
            await this.checkUsername();
        }, 333);
    }

    private async checkUsername(): Promise<void> {
        if ([undefined, null, ""].includes(this.username)) {
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

    //#endregion

    //#region PASSWORD METHODS

    public comparePasswords(repeatPw: string): void {
        this.repeatNewPassword = repeatPw;
        if (this.pwDebounceTimerID != undefined) {
            window.clearTimeout(this.pwDebounceTimerID);
        }
        this.pwDebounceTimerID = window.setTimeout(() => {
            this.shouldCompareNewPw = ![null, undefined, ""].includes(this.repeatNewPassword);
            this.newPasswordMatch = this.shouldCompareNewPw
                ? this.repeatNewPassword == this.newPassword
                : false;
        }, 333);
    }

    //#endregion

    //HELPER METHODS
    private trySetError(): void {
        this.displayErr = true;
        if (
            this.statusRespSvc.statusResponse.statusDetails != undefined
            && this.statusRespSvc.statusResponse.statusDetails.length > 0
        ) {
            this.updateErrMsg = this.statusRespSvc.statusResponse.statusDetails[0].desc;
        } else {
            this.updateErrMsg = "An unknown error has occurred. Please try again.";
        }
    }
}