import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BaseAPI } from "./base-api";
import { StatusResponseService } from "../service/status-response.service";
import { Player, PlayerAnswer } from "../viewmodels";

@Injectable()
export class PlayerAPI extends BaseAPI {
    private readonly api: string = "Player";
    constructor(httpClient: HttpClient, statusRespSvc: StatusResponseService) {
        super(httpClient, statusRespSvc);
    }

    public async playerNameIsAvailable(playerName: string): Promise<boolean> {
        const path: string = `${this.api}/PlayerNameIsAvailable`;
        const queryParam: string = `playerName=${playerName}`;
        const request: string = `${path}?${queryParam}`;
        const response: boolean = await super.HttpGet<boolean>(request);
        return response;
    }

    public async createAccount(username: string, password: string): Promise<string> {
        const path: string = `${this.api}/CreateAccount`;
        const queryParam: string = `username=${username}&password=${password}`;
        const request: string = `${path}?${queryParam}`;
        const response: string = await super.HttpPost<string>(request, null);
        return response;
    }

    public async loginToAccount(username: string, password: string): Promise<string> {
        const path: string = `${this.api}/Login`;
        const queryParam: string = `username=${username}&password=${password}`;
        const request: string = `${path}?${queryParam}`;
        const response: string = await super.HttpPost<string>(request, null);
        return response;
    }

    public async getPlayerByPlayerID(playerID: string): Promise<Player> {
        const path: string = `${this.api}/GetPlayerByID`;
        const queryParam: string = `playerID=${playerID}`;
        const request: string = `${path}?${queryParam}`;
        const response: Player = await super.HttpGet(request);
        return response;
    }
}