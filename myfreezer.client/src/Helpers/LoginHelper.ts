import {deleteCookie, getCookie} from "./CookieHelper";
import {JWTTokenType} from "../Models/JWTTokenType";

export function DeleteAllCookies(){
    deleteCookie("refreshToken");
    deleteCookie("accessToken");
    deleteCookie("userId");
}

export function IsTimeToRefreshAccessToken() {
    const accessTokenStr = getCookie("accessToken");
    if (accessTokenStr === null || getCookie("refreshToken") === null) {
        return true;
    }
    const accessToken = JSON.parse(accessTokenStr) as JWTTokenType;
    const nowInUNIX = (new Date()).getTime();
    return accessToken.expiredAtUNIX! - nowInUNIX < 2000;
}