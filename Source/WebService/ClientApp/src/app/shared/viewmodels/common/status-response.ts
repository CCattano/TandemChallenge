import { StatusDetail } from "./status-detail";

export class StatusResponse {
    public statusCode: number;
    public statusDesc: string;
    public statusDetails: StatusDetail[];
}