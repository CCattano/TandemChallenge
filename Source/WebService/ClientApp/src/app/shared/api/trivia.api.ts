import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { StatusResponseService } from "../service/status-response.service";
import { PlayerRound } from "../viewmodels";
import { BaseAPI } from "./base-api";

@Injectable()
export class TriviaAPI extends BaseAPI {
    private readonly api: string = "Trivia";
    constructor(httpClient: HttpClient, statusRespSvc: StatusResponseService) {
        super(httpClient, statusRespSvc);
    }

    public async getTriviaRound(playerID: number): Promise<PlayerRound> {
        const endpoint: string = `${this.api}/GetTriviaRound`;
        const queryParam: string = `playerID=${playerID}`;
        const request: string = `${endpoint}?${queryParam}`;
        const response: PlayerRound = await super.HttpGet(request);
        return response;
    }

    public async getGuestTriviaRound(): Promise<PlayerRound> {
        const request: string = `${this.api}/GetGuestTriviaRound`;
        const response: PlayerRound = await super.HttpGet(request);
        return response;
    }

    public async getExistingRound(playerHistoryID: number): Promise<PlayerRound> {
        const endpoint: string = `${this.api}/getExistingRound`;
        const queryParam: string = `playerHistoryID=${playerHistoryID}`;
        const request: string = `${endpoint}?${queryParam}`;
        const response: PlayerRound = await super.HttpGet(request);
        return response;
    }
}