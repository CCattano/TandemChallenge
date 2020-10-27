import { Component, OnInit } from '@angular/core';
import { PlayerAPI } from '../../shared/api';
import { NewPlayer } from '../../shared/viewmodels';
import { Router } from '@angular/router';

@Component({
    selector: 'app-acct',
    templateUrl: './account.component.html',
})
export class AccountComponent implements OnInit {
    loggedIn: boolean = false;

    username: string;
    password: string = "";

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

    constructor(private playerAPI: PlayerAPI, private router: Router) {

    }

    public ngOnInit(): void {
        //TODO Read cookies to look for LogIn Token
        //Cookie to contain login token, user ID, expire time
        //Send token from cookie to server to see if it matched token on player model
        //Verify token is still valid
        //Return true from server if can proceed to Play screen
        //Pass userID to play screen to pull user data to prompt if a game should be started
    }

    public navTest(): void {
        const id: number = 1234;
        this.router.navigate(["play", id]);
    }

    //#region NEW ACCOUNT METHODS

    //API METHODS
    private async checkUsername(): Promise<void> {
        if ([undefined, null, ""].findIndex(v => v === this.username) !== -1) {
            console.log("not checking, no name to check");
            this.shouldDisplayAvailability = false;
            this.usernameAvailable = false;
            return;
        } else if (this.lastCheckedUsername === this.username) {
            console.log("not checking, same as last checked");
            return;
        }
        console.log("checking");
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
        const newPlayer: NewPlayer = await this.playerAPI.createAccount(this.username, this.password);
        const cookieName: string = `TandemTriviaToken=${newPlayer.loginToken};`;
        const cookieExpiration: string = `expires=${newPlayer.loginTokenExpireDateTime};`
        const cookiePath: string = "path=/";
        document.cookie = `${cookieName} ${cookieExpiration} ${cookiePath}`;
        this.router.navigateByUrl(`play/${newPlayer.playerID}`);
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
}