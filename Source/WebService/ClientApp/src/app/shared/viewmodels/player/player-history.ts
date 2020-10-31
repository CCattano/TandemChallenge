import { Answer, PlayerAnswer, Question } from "..";

export class PlayerHistory {
    public playerHistoryID: number;
    public playerID: number;
    public roundNumber: number;
    public startedDateTime: Date;
    public completedDateTime?: Date;
}