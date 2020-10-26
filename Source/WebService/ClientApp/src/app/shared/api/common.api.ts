import { Injectable } from "@angular/core";
import { BaseAPI } from "./base-api";
import { HttpClient } from "@angular/common/http";
import { BaseViewModel } from "../viewmodels";

@Injectable()
export class CommonAPI extends BaseAPI {
    private readonly api: string = "Common";
    constructor(httpClient: HttpClient) {
        super(httpClient);
    }

    public async e2eTest(): Promise<BaseViewModel> {
        const request: string = `${this.api}/E2ETest`;
        const response: BaseViewModel = await super.HttpGet<BaseViewModel>(request);
        return response;
    }
}
