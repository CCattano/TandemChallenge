import { StatusDetail } from "./StatusDetail";

export class StatusResponse {
    public statusCode: number;
    public statusDesc: string;
    public statusDetail: StatusDetail[];
}