import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PlayerTokenService } from "../../service";
import { Observable } from "rxjs";


@Injectable()
export class TandemTokenInterceptor implements HttpInterceptor {
    constructor(private tokenSvc: PlayerTokenService) {
    }

    public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let httpReq: HttpRequest<any> = req.clone();
        if (this.tokenSvc.haveToken) {
            httpReq = req.clone({
                headers: req.headers.append("TandemTriviaToken", this.tokenSvc.token)
            });
        }
        return next.handle(httpReq);
    }
}