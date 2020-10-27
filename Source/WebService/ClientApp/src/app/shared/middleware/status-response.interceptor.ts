import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { tap } from 'rxjs/operators';
import { StatusResponseService } from "../service/statusResponse.service";
import { StatusResponse } from "../viewmodels";

@Injectable()
export class StatusResponseInterceptor implements HttpInterceptor {
    constructor(private statusRespSvc: StatusResponseService) {

    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            tap(httpEvent => {
                if (httpEvent instanceof HttpResponse) {
                    this.statusRespSvc.statusResponse = JSON.parse(httpEvent.headers.get("Status")) as StatusResponse;
                }
            })
        );
    }
}