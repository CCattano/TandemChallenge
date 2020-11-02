import { Injectable } from "@angular/core";
import { StatusResponse } from "../viewmodels";

@Injectable()
export class StatusResponseService {
    public statusResponse: StatusResponse;
    public status: typeof Status = Status;
}
enum Status {
    success = 200
    //Add more as you need to check more
};