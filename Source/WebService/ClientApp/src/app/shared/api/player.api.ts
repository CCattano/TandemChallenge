import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BaseAPI } from "./base-api";
import { StatusResponseService } from "../service/status-response.service";
import { Player, PlayerAnswer, PlayerHistory } from "../viewmodels";

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

    public async getPlayerByPlayerID(playerID: number): Promise<Player> {
        const path: string = `${this.api}/GetPlayerByID`;
        const queryParam: string = `playerID=${playerID}`;
        const request: string = `${path}?${queryParam}`;
        const response: Player = await super.HttpGet(request);
        return response;
    }

    public async savePlayerAnswer(playerAnswer: PlayerAnswer): Promise<void> {
        const request: string = `${this.api}/SavePlayerAnswer`;
        await super.HttpPost(request, playerAnswer);
    }

    public async markRoundCompleted(playerHistoryID: number): Promise<void> {
        const path: string = `${this.api}/MarkRoundCompleted`;
        const queryParam: string = `playerHistoryID=${playerHistoryID}`;
        const request: string = `${path}?${queryParam}`;
        await super.HttpPost(request, null);
    }

    public async getAllPlayerHistory(playerID: number): Promise<PlayerHistory[]> {
        const path: string = `${this.api}/GetPlayerHistory`;
        const queryParam: string = `playerID=${playerID}`;
        const request: string = `${path}?${queryParam}`;
        const response: PlayerHistory[] = await super.HttpGet(request);
        return response;
    }

    public async changeUsername(playerID: number, newUsername: string): Promise<string> {
        const path: string = `${this.api}/ChangeUsername/${playerID}`;
        const queryParam: string = `newUsername=${newUsername}`;
        const request: string = `${path}?${queryParam}`;
        const response: string = await super.HttpPost<string>(request, null);
        return response;
    }

    public async changePassword(playerID: number, currentPassword: string, newPassword: string): Promise<void> {
        const path: string = `${this.api}/ChangePassword/${playerID}`;
        const queryParams: string = `currentPassword=${currentPassword}&newPassword=${newPassword}`;
        const request: string = `${path}?${queryParams}`;
        await super.HttpPost<string>(request, null);
    }
}