import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { tap, first } from 'rxjs/operators';
import { StatusResponseService } from "../../service";
import { StatusResponse } from "../../viewmodels";

@Injectable()
export class StatusResponseInterceptor implements HttpInterceptor {
    constructor(private statusRespSvc: StatusResponseService) {

    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            tap(httpEvent => {
                if (httpEvent instanceof HttpResponse) {
                    //Header's obj when parsed has properties in PascalCase
                    //Need to massage to camelCase to work with in the client side
                    const pascalStatusResp: any = JSON.parse(httpEvent.headers.get("Status"));
                    const camelStatusRep: any = this.processPascalObj(pascalStatusResp);
                    //Need to do the same for the StatusDesc objs if any
                    if (![undefined, null, []].includes((camelStatusRep as StatusResponse).statusDetails)) {
                        (camelStatusRep as StatusResponse).statusDetails =
                            (camelStatusRep as StatusResponse).statusDetails.map(pascalStatusDetail => {
                                const camelStatusDetail: any = this.processPascalObj(pascalStatusDetail);
                                return camelStatusDetail;
                            });
                    }
                    this.statusRespSvc.statusResponse = camelStatusRep as StatusResponse;
                }
            })
        );
    }

    private processPascalObj(pascalObj: object): object {
        let camelObj: any = {};
        Object.keys(pascalObj).forEach(pascalKey => {
            const value: any = pascalObj[pascalKey];
            const camelKey: string = this.getCamelCaseKey(pascalKey);
            camelObj[camelKey] = value;
        });
        return camelObj;
    }

    private getCamelCaseKey(pascalKey: string): string {
        const firstLetter: string = pascalKey.substring(0, 1);
        const restOfKey: string = pascalKey.substring(1, pascalKey.length);
        return firstLetter.toLowerCase() + restOfKey
    }
}