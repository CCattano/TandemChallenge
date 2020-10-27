import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BaseAPI } from "./base-api";
import { StatusResponseService } from "../service/statusResponse.service";

@Injectable()
export class TriviaAPI extends BaseAPI {
    private readonly api: string = "Trivia";
    constructor(httpClient: HttpClient, statusRespSvc: StatusResponseService) {
        super(httpClient, statusRespSvc);
    }

    public async e2eTest(): Promise<boolean> {
        const request: string = `${this.api}/E2ETest`;
        const response: boolean = await super.HttpGet<boolean>(request);
        return response;
    }
}
