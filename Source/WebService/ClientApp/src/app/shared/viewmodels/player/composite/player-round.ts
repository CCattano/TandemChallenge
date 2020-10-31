import { PlayerAnswer, PlayerHistory, QuestionDetail } from "../..";

export class PlayerRound extends PlayerHistory {
    public questionDetails: QuestionDetail[];
    public playerAnswers: PlayerAnswer[];
}