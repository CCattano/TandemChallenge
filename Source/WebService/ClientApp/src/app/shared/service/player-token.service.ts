import { Injectable } from "@angular/core";

@Injectable()
export class PlayerTokenService {
    private _playerID: number;
    private _tokenTTL: Date;
    private _token: string;

    public get playerID(): number {
        return this._playerID;
    }
    public get tokenTTL(): Date {
        return this._tokenTTL;
    }
    public get token(): string {
        return this._token;
    }
    public haveToken: boolean = false;

    public setToken(token: string, writeToCookie: boolean = false): void {
        this._token = token;
        this.haveToken = true;

        //Decode provided token
        this.decodeToken(token);

        //Set token in cookie
        if (writeToCookie) {
            this.writeToCookie();
        }
    }

    public removeToken(): void {
        this.writeToCookie(true);
        this.haveToken = false;
        this._token = null;
        this._playerID = 0;
        this._tokenTTL = null;
    }

    private writeToCookie(expire: boolean = false) {
        let cookieName: string = `TandemTriviaToken=${this._token}`;
        const cookieExpiration: string =
            `;expires=${expire ? new Date().toUTCString() : this._tokenTTL.toUTCString()}`;
        const cookiePath: string = ";path=/";
        const cookieSameSitePolicy: string = ";samesite=lax";
        const cookie: string = `${cookieName}${cookieExpiration}${cookiePath}${cookieSameSitePolicy}`;
        document.cookie = cookie;
    }

    private decodeToken(token: string): void {
        //Token anatomy is "encodedHeader.encodedBody.encodedSignature"
        const encodedTokenBody = token.split(".")[1];
        const decodedTokenBody: string = this.base64UrlDecode(encodedTokenBody);

        const tokenObj: { PlayerID: number, ExpirationDateTime: Date } = JSON.parse(decodedTokenBody);

        this._playerID = tokenObj.PlayerID;
        this._tokenTTL = new Date(tokenObj.ExpirationDateTime);
    }

    private base64UrlDecode(token: string) {
        // Replace non-url compatible chars with base64 standard chars
        token = token
            .replace(/-/g, '+')
            .replace(/_/g, '/');

        // Pad out with standard base64 required padding characters
        var pad = token.length % 4;
        if (pad) {
            if (pad === 1) {
                throw new Error('InvalidLengthError: token base64url string is the wrong length to determine padding');
            }
            token += new Array(5 - pad).join('=');
        }

        const decodedToken: string = atob(token);
        return decodedToken;
    }
}