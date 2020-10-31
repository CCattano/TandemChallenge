import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { UtilityService } from "../../../shared/service/utility.service";
import { Answer, PlayerAnswer, PlayerRound, QuestionDetail } from "../../../shared/viewmodels";

@Component({
    selector: 'app-player-history',
    templateUrl: './player-history.component.html',
    styles: [`
        .phc-wrong-answer {
            background: rgba(200, 35, 51, .5)
        }
        .phc-right-answer {
            background: rgba(33, 136, 56, .5)
        }
        .phc-active-answer {
            background: rgba(0, 105, 217, .5)
        }
    `]
})

export class PlayerHistoryComponent implements OnInit {
    rounds: PlayerRound[] = [];
    completionTimes: string[] = [];
    cardColors: { index: number, style: string }[] = [];
    viewingAnswers: boolean = false;
    viewRoundIdx: number;
    crntQuestionIdx: number;
    chirp: string;
    private readonly chirps: string[] = [
        "Surprised to see you here, all things considered...",
        "Oh... So you care about this round now, huh?",
        "Ya know.. youse gotta lotta nerve showin up round these parts",
        "Still waitin for that apology, bud",
        "I'd say let's let bygones be byonges, but then I'd have no reason to TP your house",
        "Too little too late mate, too little too late",
        "Bad gas travels fast in a small town, that's all I know",
        "I knew you'd come crawlin back",
        "I'd say it's great to see you, but it's fine. ...It's just fine.",
        "Look, I forgive ya for what youse done, let's just be friends again"
    ];

    constructor(
        route: ActivatedRoute,
        private utilSvc: UtilityService
    ) {
        //TODO: Verify that abandoning a game causes the right data to show up here
        const rounds: PlayerRound[] = route.snapshot.data.playerRounds;
        if (![undefined, null].includes(rounds)) {
            this.rounds = rounds.sort((a, b) => a.playerHistoryID < b.playerHistoryID ? -1 : 1);
        }
        this.completionTimes = [].fill(undefined, 0, this.rounds.length);
    }

    ngOnInit(): void {
        const allAnswers: Answer[] = [];
        const that: this = this;
        this.rounds.forEach(round => {
            round.questionDetails.forEach(qd => {
                qd.answers = that.utilSvc.shuffleArray(qd.answers); //shuffling while we happen to be here
                qd.answers.forEach(a => {
                    if (allAnswers.find(aa => aa.answerID === a.answerID) == undefined) {
                        allAnswers.push(a);
                    }
                });
            });
        });

        this.rounds.forEach(round => {
            const answers: boolean[] = round.playerAnswers.map(pa => allAnswers.find(a => a.answerID === pa.answerID).isCorrect);
            const isIncomplete: boolean = round.playerAnswers.length < 10;
            round["isIncomplete"] = isIncomplete;
            round["rightAnswerCount"] = answers.filter(isCorrect => isCorrect).length;
            round["wrongAnswerCount"] = answers.filter(isCorrect => !isCorrect).length;
            round["timeToComplete"] = isIncomplete && [null, undefined].includes(round.completedDateTime)
                ? "Not Yet Finished"
                : this.utilSvc.calcRoundTime(round)
        });
    }

    //#region ANSWER REVIEW METHODS
    public viewAnswers(index: number): void {
        this.viewRoundIdx = index;
        this.crntQuestionIdx = 0;
        this.viewingAnswers = true;
        this.trySetChirp();
    }

    public isPlayerAnswer(index: number): boolean {
        const crntRound: PlayerRound = this.rounds[this.viewRoundIdx];
        const crntQuestion: QuestionDetail = crntRound.questionDetails[this.crntQuestionIdx];
        const playerAnswer: PlayerAnswer = crntRound.playerAnswers.find(pa => pa.questionID === crntQuestion.questionID);
        return playerAnswer.answerID === crntQuestion.answers[index].answerID;
    }

    public showDiffAnswer(showingNext: boolean): void {
        if (showingNext)
            this.crntQuestionIdx++;
        else
            this.crntQuestionIdx--;
        this.trySetChirp();
    }

    public closeReview(): void {
        this.viewRoundIdx = this.crntQuestionIdx = undefined;
        this.viewingAnswers = false;
    }

    private trySetChirp(): void {
        if ((this.rounds[this.viewRoundIdx] as any).isIncomplete as boolean) {
            if (this.crntQuestionIdx === this.rounds[this.viewRoundIdx].playerAnswers.length - 1) {
                this.chirp = this.chirps[this.chirps.length - 1];
            } else {
                const chirpIndex: number = this.utilSvc.getRandomNum(0, this.chirps.length - 1);
                this.chirp = this.chirps[chirpIndex];
            }
        }
    }

    //#endregion

    //UI Helper method
    public getCardColor(index: number): string {
        if (this.cardColors.find(cc => cc.index === index) != undefined) {
            return this.cardColors[index].style;
        }
        let style: string;
        const workingIndex: number = index + 1;
        if (workingIndex % 4 === 0) {
            style = "primary"
        } else {
            let idx: number = workingIndex
            while (++idx % 4 !== 0);
            const pos: number = 4 - (idx - workingIndex);
            switch (pos) {
                case 1: style = "success"; break;
                case 2: style = "danger"; break;
                case 3: style = "warning"; break;
            }
        }
        this.cardColors.push({ index: index, style: style });
        return style;
    }
}