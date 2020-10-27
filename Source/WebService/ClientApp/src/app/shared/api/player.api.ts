import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BaseAPI } from "./base-api";
import { StatusResponseService } from "../service/statusResponse.service";
import { NewPlayer } from "../viewmodels";

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

    public async createAccount(username: string, password: string): Promise<NewPlayer> {
        const path: string = `${this.api}/CreateAccount`;
        const queryParam: string = `username=${username}&password=${password}`;
        const request: string = `${path}?${queryParam}`;
        const response: NewPlayer = await super.HttpPost<NewPlayer>(request, null);
        return response;
    }
}
