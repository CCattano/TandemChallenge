import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationStart } from '@angular/router';
import { NavMenuService, PlayerTokenService } from '../../service';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

    private greetings: string[] = [
        "Welcome", "Hey there", "Well howdy", "Ayyyy waddup",
        "Long time no see", "Welcome to the Matrix"
    ];

    private shouldGetGreeting: boolean = false;
    private currentGreeting: string;

    public get loggedIn(): boolean {
        return this.tokenSvc.haveToken;
    }
    public isExpanded: boolean = false;

    constructor(
        private tokenSvc: PlayerTokenService,
        private navMenuSvc: NavMenuService,
        private route: ActivatedRoute,
        private router: Router
    ) {
    }

    public ngOnInit(): void {
        this.router.events.subscribe(evt => {
            if (evt instanceof NavigationStart) {
                //guarantees that a new greeting is fetched only when nav'ing to /playermenu
                //otherwise the previously fetched greeting is retained while on other screens
                this.shouldGetGreeting = (evt as NavigationStart).url.includes("playermenu");
            }
        })
    }

    public collapse(): void {
        this.isExpanded = false;
    }

    public toggle(): void {
        this.isExpanded = !this.isExpanded;
    }

    public logout(): void {
        this.tokenSvc.removeToken();
        this.router.navigate(["/"]);
    }

    //only gets called from the view if a playerName is set in the NavMenuSvc
    public getGreeting(): string {
        if (this.shouldGetGreeting) {
            const upper: number = this.greetings.length;
            const lower: number = 0;
            const index: number = Math.floor(Math.random() * (upper - lower) + lower);
            this.currentGreeting = this.greetings[index];
            this.shouldGetGreeting = false;
        }
        return this.currentGreeting;
    }
}
