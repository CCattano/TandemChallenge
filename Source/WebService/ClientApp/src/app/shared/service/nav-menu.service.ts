import { Injectable } from "@angular/core";

@Injectable()
export class NavMenuService {
    private _playerName: string;
    private _nameSet: boolean = false;

    constructor() {
    }

    public get playerName(): string {
        return this._playerName;
    }

    public get nameSet(): boolean {
        return this._nameSet;
    }

    public setPlayerName(playerName: string): void {
        this._playerName = playerName;
        this._nameSet = true;
    }
}