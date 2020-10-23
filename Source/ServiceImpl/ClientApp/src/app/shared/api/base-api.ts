import { HttpClient } from "@angular/common/http";
import { BaseViewModel } from "../viewmodels";

export abstract class BaseAPI {
    constructor(private httpClient: HttpClient) {
    }

    protected async HttpPost<TResponse extends BaseViewModel>(request: string, payload: object, loadingMsg?: string): Promise<TResponse> {
        const response = await this.httpClient.post(request, payload).toPromise() as TResponse;
        return response;
    }

    protected async HttpGet<TResponse extends BaseViewModel>(request: string, loadingMsg?: string): Promise<TResponse> {
        const response = await this.httpClient.get(request).toPromise() as TResponse;
        return response;
    }
}
