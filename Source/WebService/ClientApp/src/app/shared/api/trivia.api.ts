import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { StatusResponseService } from "../service/status-response.service";
import { PlayerHistory } from "../viewmodels";
import { BaseAPI } from "./base-api";

@Injectable()
export class TriviaAPI extends BaseAPI {
    private readonly api: string = "Trivia";
    constructor(httpClient: HttpClient, statusRespSvc: StatusResponseService) {
        super(httpClient, statusRespSvc);
    }

    public async getTriviaRound(playerID: number): Promise<PlayerHistory> {
        const endpoint: string = `${this.api}/GetTriviaRound`;
        const queryParam: string = `playerID=${playerID}`;
        const request: string = `${endpoint}?${queryParam}`;
        const response: PlayerHistory = await super.HttpGet(request);
        return response;
    }

    public async getGuestTriviaRound(): Promise<PlayerHistory> {
        const request: string = `${this.api}/GetGuestTriviaRound`;
        const response: PlayerHistory = await super.HttpGet(request);
        return response;
    }
}