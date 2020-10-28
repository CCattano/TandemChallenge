import { Component } from '@angular/core';
import { PlayerTokenService } from '../../service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
    get loggedIn(): boolean {
        return this.tokenSvc.haveToken;
    }
    isExpanded: boolean = false;

    constructor(
        private tokenSvc: PlayerTokenService,
        private router: Router
    ) {
    }

    collapse(): void {
        this.isExpanded = false;
    }

    toggle(): void {
        this.isExpanded = !this.isExpanded;
    }

    logout(): void {
        this.tokenSvc.removeToken();
        this.router.navigate(["/"]);
    }
}
