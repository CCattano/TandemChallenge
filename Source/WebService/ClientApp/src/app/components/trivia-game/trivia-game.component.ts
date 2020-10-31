import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { PlayerAPI } from "../../shared/api";
import { Answer, Player, PlayerAnswer, PlayerRound, QuestionDetail } from "../../shared/viewmodels";

@Component({
    selector: 'app-trivia-game',
    templateUrl: './trivia-game.component.html',
    styles: [`
        .tgc-wrong-answer {
            background: rgba(200, 35, 51, .5)
        }
        .tgc-right-answer {
            background: rgba(33, 136, 56, .5)
        }
        .tgc-active-answer {
            background: rgba(0, 105, 217, .5)
        }
    `]
})
export class TriviaGameComponent implements OnInit {
    playingAsGuest: boolean = false;
    player: Player = null;
    round: PlayerRound = null;
    questionDetails: QuestionDetail[] = [];

    resumingGame: boolean = false;
    //currentQuestion: number = 0;
    lastAnsweredQuestion: number = 0;
    selectedAnswer: number = undefined;

    clickedStart: boolean = false;
    gameOver: boolean = false;

    currentScore: number = 0;
    showResults: boolean = false;
    resultMsg: string;
    viewingHistory: boolean = false;

    nextQuestionIdx: number = 0;
    crntQuestionIdx: number = 0;

    rightAnswerCount: number = 0;
    wrongAnswerCount: number = 0;

    timeToComplete: string = undefined;

    private readonly wrongAnswerMsgs: string[] = [
        "Oof...", "F", "This ain't it chief", "RIP",
        "It's gonna be a no from me dawg",
        "You fill out TPS reports with that brain?",
        "*wrong answer buzzer sound from Family Feud™©* (use ur imagination plz)",
        "You'll get the next one (hopefully)", "Wrong answer!",
        "Better luck next time!", "Wanna borrow my Encyclopedia?",
        "As sure as God wears sandals, you did not get that question right, bud.",
        "Now ur gunna wanna have an eye, cuz that question came out meaner than spit and went like a hot dang",
        "*breathes in through teeth sharply*"
    ];

    private readonly rightAnswerMsgs: string[] = [
        "That's a Texas-sized 10-4 Good Buddy",
        "Someone had their Frosted Mini-Wheats this mornin then, eh?",
        "Never doubted ya for a sec there big shooter",
        "You signed up for Who Want To Be A Millionaire yet?",
        "Das it mang, das it",
        "Well ya wanna know what? Good job, bud, I'm proud of ya.",
        "With brains like these who need brawn amirite?",
        "Ferrdaaaa!", "Correct!"
    ]

    constructor(
        route: ActivatedRoute,
        private router: Router,
        private playerAPI: PlayerAPI
    ) {
        //Determine if playing as a guest
        this.playingAsGuest = route.routeConfig.path.includes("guest");
        if (this.playingAsGuest) {
            this.round = route.snapshot.data.trivia; //sourced from snapshot, provided by resolver
        } else {
            //Not playing as guest get player info
            this.player = window.history.state.player as Player; //sourced from state, provided by router.navigate

            //Determine if new game or prevGame
            this.round = window.history.state.prevGame as PlayerRound; //sourced from state, provided by router.navigate
            if (this.round == undefined) {
                //New game
                this.round = route.snapshot.data.trivia; //sourced from snapshot, provided by resolver
            } else {
                this.resumingGame = true;
            }
        }
        this.questionDetails = this.round.questionDetails;

        if (this.round.playerAnswers == undefined) {
            this.round.playerAnswers = [];
        }
    }

    public async ngOnInit(): Promise<void> {
        //Determine question order for new game v resume game
        if (!this.playingAsGuest && this.resumingGame) {
            //find answered questionDetails
            let answeredQuestions: QuestionDetail[] = this.questionDetails.filter(qd => this.round.playerAnswers.findIndex(pa => pa.questionID === qd.questionID) !== -1);
            //sort answered questionDetails by sequence
            answeredQuestions = answeredQuestions.sort((a, b) => b.questionSequence > a.questionSequence ? 1 : 0);
            //pop last
            const lastQuestion: QuestionDetail = answeredQuestions.pop();
            //Next question is one index after the last question
            this.crntQuestionIdx = this.questionDetails.findIndex(qd => qd.questionSequence === lastQuestion.questionSequence) + 1;
            this.nextQuestionIdx = this.crntQuestionIdx;
            //Get right wrong answer counts
            const allAnswers: Answer[] = this.questionDetails.map(qd => qd.answers).reduce((store, current) => store.concat(current));
            const answers: boolean[] = this.round.playerAnswers.map(pa => allAnswers.find(a => a.answerID === pa.answerID).isCorrect);
            this.rightAnswerCount = answers.filter(isCorrect => isCorrect).length;
            this.wrongAnswerCount = answers.filter(isCorrect => !isCorrect).length;
        }

        //shuffle answer appearance order for each question
        this.questionDetails.forEach((qd, i) => {
            qd.answers = this.shuffleArray(qd.answers);
        });
    }

    /** Let's-a go! */
    public letsAGo() { this.clickedStart = true; } //Just havin fun at this point lol

    /**
     * Submit answer for question.
     * Guest's answers are retained locally in session
     * User's answers are posted to the db
     */
    public async submitAnswer(): Promise<void> {
        const currentQuestion: QuestionDetail = this.questionDetails[this.crntQuestionIdx];
        const selectedAnswer: Answer = this.questionDetails[this.crntQuestionIdx].answers[this.selectedAnswer];
        const playerAnswer: PlayerAnswer = {
            answerID: selectedAnswer.answerID,
            questionID: currentQuestion.questionID,
            playerHistoryID: this.round.playerHistoryID
        };
        if (!this.playingAsGuest) {
            await this.playerAPI.savePlayerAnswer(playerAnswer);
        }

        this.round.playerAnswers.push(playerAnswer);
        if (selectedAnswer.isCorrect) {
            this.rightAnswerCount++;
        } else {
            this.wrongAnswerCount++;
        }
        this.setResultMsg(selectedAnswer.isCorrect);
        this.currentScore = this.getRandomNum(-100000, 100000);
        this.showResults = true;
    }

    /** Done viewing results of just-answered question.Move to next */
    async nextQuestion(): Promise<void> {
        this.selectedAnswer = undefined;
        this.nextQuestionIdx++;
        this.crntQuestionIdx++;
        if (this.crntQuestionIdx === 10) {
            await this.playerAPI.markRoundCompleted(this.round.playerHistoryID);
            this.round.completedDateTime = new Date(new Date().toUTCString());
            this.gameOver = true;
        }
        this.showResults = false;
    }

    /** Viewing old answer */
    showPrev(): void {
        this.viewingHistory = true;
        this.crntQuestionIdx--;
        this.setAnswer();
    }

    /** Moving back towards newest question to answer */
    showNext() {
        this.crntQuestionIdx++;
        if (this.crntQuestionIdx === this.nextQuestionIdx) {
            this.selectedAnswer = undefined;
            this.viewingHistory = false;
            this.showResults = false;
        } else {
            this.setAnswer();
        }
    }

    calcPercentRight(): number {
        return Math.floor((this.rightAnswerCount / 10) * 100);
    }

    getTimeSpent(): string {
        //Using a bit of memoization here cause I don't want Angular to recalc this over and over
        //Could setup a directive that takes this logic and performs it inside a dynamically generated zone that's then detatched
        //from the digest cycle, but that's a bit more arcane, and less accessible of an concept/impl than just doing some memoization
        if (this.timeToComplete == undefined) {
            const diffInMs: number = this.round.completedDateTime.valueOf() - this.round.startedDateTime.valueOf();
            let diffInHour: number = diffInMs / 1000 / 60 / 60;
            const hourAsStr: string = diffInHour.toString();
            let separator: string = hourAsStr.split("").find(char => isNaN(parseInt(char)))
            const remainingMin: number = parseFloat(`0.${hourAsStr.split(separator)[1]}`) * 60;
            const minAsStr: string = remainingMin.toString();
            separator = minAsStr.split("").find(char => isNaN(parseInt(char)))
            const remainingSec: number = parseFloat(`0.${minAsStr.split(separator)[1]}`) * 60;

            const result: string = `${Math.floor(diffInHour)}h ${Math.floor(remainingMin)}m ${Math.round(remainingSec)}s`;
            this.timeToComplete = result;
        }
        return this.timeToComplete;
    }

    /** Get answer from previously answered question */
    private setAnswer() {
        const question: QuestionDetail = this.questionDetails[this.crntQuestionIdx];
        const selectedAnswer: PlayerAnswer = this.round.playerAnswers.find(a => a.questionID === question.questionID);
        this.selectedAnswer = question.answers.findIndex(a => a.answerID === selectedAnswer.answerID);
        this.setResultMsg(question.answers.find(a => a.answerID === selectedAnswer.answerID).isCorrect);
        this.showResults = true;
    }

    /** Set message related to answer choice for question */
    private setResultMsg(choseCorrectly: boolean): void {
        let rndMsgIdx: number;
        if (choseCorrectly) {
            rndMsgIdx = this.getRandomNum(0, this.rightAnswerMsgs.length);
            this.resultMsg = this.rightAnswerMsgs[rndMsgIdx];
        } else {
            rndMsgIdx = this.getRandomNum(0, this.wrongAnswerMsgs.length);
            this.resultMsg = this.wrongAnswerMsgs[rndMsgIdx];
        }
    }

    private shuffleArray<T>(array: T[]): T[] {
        const result: T[] = [];
        const indexes: number[] = array.map((_v: T, i: number) => i);
        while (result.length !== array.length) {
            const randIndex: number = this.getRandomNum(0, indexes.length);
            result.push(array[indexes[randIndex]]);
            indexes.splice(randIndex, 1);
        }
        return result;
    }

    private getRandomNum(lower: number, upper: number): number {
        lower = Math.ceil(lower);
        upper = Math.floor(upper);
        return Math.floor(Math.random() * (upper - lower) + lower);
    }
};