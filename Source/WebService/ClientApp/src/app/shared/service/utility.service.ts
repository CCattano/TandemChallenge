import { Injectable } from "@angular/core";
import { PlayerRound } from "../viewmodels";

@Injectable()
export class UtilityService {
    constructor() {
    }

    public shuffleArray<T>(array: T[]): T[] {
        const result: T[] = [];
        const indexes: number[] = array.map((_v: T, i: number) => i);
        while (result.length !== array.length) {
            const randIndex: number = this.getRandomNum(0, indexes.length);
            result.push(array[indexes[randIndex]]);
            indexes.splice(randIndex, 1);
        }
        return result;
    }

    public getRandomNum(lower: number, upper: number): number {
        lower = Math.ceil(lower);
        upper = Math.floor(upper);
        return Math.floor(Math.random() * (upper - lower) + lower);
    }

    public calcRoundTime(round: PlayerRound): string {
        const diffInMs: number = round.completedDateTime.valueOf() - round.startedDateTime.valueOf();

        let diffInHour: number = diffInMs / 1000 / 60 / 60;
        const hourAsStr: string = diffInHour.toString();
        let separator: string = hourAsStr.split("").find(char => isNaN(parseInt(char)))

        const remainingMin: number = parseFloat(`0.${hourAsStr.split(separator)[1]}`) * 60;
        const minAsStr: string = remainingMin.toString();
        separator = minAsStr.split("").find(char => isNaN(parseInt(char)))

        const remainingSec: number = parseFloat(`0.${minAsStr.split(separator)[1]}`) * 60;

        const result: string = `${Math.floor(diffInHour)}h ${Math.floor(remainingMin)}m ${Math.round(remainingSec)}s`;
        return result
    }
}