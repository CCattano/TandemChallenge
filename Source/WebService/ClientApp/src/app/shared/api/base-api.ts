import { HttpClient } from "@angular/common/http";
import { StatusResponseService } from "../service/status-response.service";
import { StatusResponse } from "../viewmodels";

export abstract class BaseAPI {
    public get statusResponse(): StatusResponse {
        return this.statusRespSvc.statusResponse;
    }

    constructor(
        private httpClient: HttpClient,
        private statusRespSvc: StatusResponseService
    ) {
    }

    protected async HttpPost<TResponse>(request: string, payload: object, loadingMsg?: string): Promise<TResponse> {
        try {
            const response = await this.httpClient.post(request, payload).toPromise() as TResponse;
            return response;
        } catch (e) {
            console.log(e);
        }
        return null;
    }

    protected async HttpGet<TResponse>(request: string, loadingMsg?: string): Promise<TResponse> {
        const response = await this.httpClient.get(request).toPromise() as TResponse;
        return response;
    }
}