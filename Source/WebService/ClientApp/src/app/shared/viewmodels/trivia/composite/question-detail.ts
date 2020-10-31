import { Answer, Question } from "../..";

export class QuestionDetail extends Question {
    public answers: Answer[];
    public questionSequence: number;
}