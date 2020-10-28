import { Router } from "@angular/router";
import { Injectable, Injector } from "@angular/core";
import { PlayerTokenService } from "../service/player-token.service";

@Injectable()
export class AppInitializer {
    private tokenSvc: PlayerTokenService;
    constructor(injector: Injector) {
        this.tokenSvc = injector.get(PlayerTokenService);
    }

    public initialize(): void {
        const token: string = document.cookie
            .split(";")
            .find(row => row.startsWith("TandemTriviaToken"));

        if (![undefined, null, ""].includes(token)) {
            this.tokenSvc.setToken(token);
        }
    }
}