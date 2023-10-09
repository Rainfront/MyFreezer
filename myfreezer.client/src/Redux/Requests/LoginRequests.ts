import {ajax} from "rxjs/internal/ajax/ajax";
import {JWTTokenType} from "../../Models/JWTTokenType";
import {catchError, map, mergeMap, Observable} from "rxjs"
import {getCookie, setCookie} from "../../Helpers/CookieHelper";
import {DeleteAllCookies, IsTimeToRefreshAccessToken} from "../../Helpers/LoginHelper";

const url = require("../../../package.json")["url-back"] 

export type LoginResponseType = {
    data: {
        login: {
            accessToken: JWTTokenType,
            refreshToken: JWTTokenType,
            userId: number
        }
    },
    errors: [
        {
            message: string
        }
    ]
}

export function ajaxForLogin(variables: {}) {
    return ajax<LoginResponseType>({
        url: url + "-login",
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify({
            query: `query($login:LoginInputTypeGraphQL!){
                      login(loginInfo:$login){
                        userId
                        accessToken{
                          token
                          issuedAt
                          expiredAt
                        }
                        refreshToken{
                          token
                          issuedAt
                          expiredAt
                        }
                      }
                    }`,
            variables: variables
        }),
        withCredentials: true
    }).pipe(
        map(value => {
            let completeResponse = value.response;
            let loginData = completeResponse.data.login;

            if (completeResponse.errors)
                throw completeResponse.errors

            const accessTokenTransfer: JWTTokenType = {
                token: loginData.accessToken.token,
                issuedAtUNIX: new Date(loginData.accessToken.issuedAt!).getTime(),
                expiredAtUNIX: new Date(loginData.accessToken.expiredAt!).getTime()
            };
            setCookie("accessToken", JSON.stringify(accessTokenTransfer), {
                expires: accessTokenTransfer.expiredAtUNIX! / 1000,
                path: "/"
            });


            const refreshTokenTransfer: JWTTokenType = {
                token: loginData.refreshToken.token,
                issuedAtUNIX: new Date(loginData.refreshToken.issuedAt!).getTime(),
                expiredAtUNIX: new Date(loginData.refreshToken.expiredAt!).getTime()
            };
            setCookie("refreshTokenTransfer", JSON.stringify(refreshTokenTransfer), {
                expires: refreshTokenTransfer.expiredAtUNIX! / 1000,
                path: "/"
            });

            const userId = loginData.userId.toString();
            setCookie("userId", userId, {
                expires: refreshTokenTransfer.expiredAtUNIX! / 1000,
                path: "/"
            })
        }),
        catchError(error => {
            throw error
        })
    )
}

export function ajaxForRefresh(refreshToken: string) {
    return ajax<LoginResponseType>({
        url: url + "-login",
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json",
            "Authorization": "Bearer " + refreshToken
        },
        body: JSON.stringify({
            query: `query{
                        refreshTokens{
                          userId
                        accessToken{
                          token
                          issuedAt
                          expiredAt
                        }
                        refreshToken{
                          token
                          issuedAt
                          expiredAt
                        }
                        }
                    }`,
            variables: null
        }),
        withCredentials: true
    }).pipe(
        map(value => {
            let completeResponse = value.response;
            let loginData = completeResponse.data.login;

            if (completeResponse.errors) {
                DeleteAllCookies()
                throw completeResponse.errors
            }
            const accessTokenTransfer: JWTTokenType = {
                token: loginData.accessToken.token,
                issuedAtUNIX: new Date(loginData.accessToken.issuedAt!).getTime(),
                expiredAtUNIX: new Date(loginData.accessToken.expiredAt!).getTime()
            };
            setCookie("accessToken", JSON.stringify(accessTokenTransfer), {
                expires: accessTokenTransfer.expiredAtUNIX! / 1000,
                path: "/"
            });


            const refreshTokenTransfer: JWTTokenType = {
                token: loginData.refreshToken.token,
                issuedAtUNIX: new Date(loginData.refreshToken.issuedAt!).getTime(),
                expiredAtUNIX: new Date(loginData.refreshToken.expiredAt!).getTime()
            };
            setCookie("refreshTokenTransfer", JSON.stringify(refreshTokenTransfer), {
                expires: refreshTokenTransfer.expiredAtUNIX! / 1000,
                path: "/"
            });

            const userId = loginData.userId.toString();
            setCookie("userId", userId, {
                expires: refreshTokenTransfer.expiredAtUNIX! / 1000,
                path: "/"
            })
        }),
        catchError(error => {
            throw error
        })
    )


}
export type response<T = any> = {
    data:T,
    errors?:any
}
export function ajaxForLogout(token: string) {
    return ajax({
        url: url + "-login",
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'Accept': "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({
            query: `query{
                        logout
                      }`,
        }),
        withCredentials: true,
    }).pipe(
        map((res: any): void => {

            if (res.response.errors) {
                console.error(JSON.stringify(res.response.errors))
                throw "error"
            }

            return res;
        }),
        catchError((error) => {
            throw error
        })
    );
}

export function GetAjaxObservable<T>(query: string, variables: {}, withCredentials = false) {
    if (IsTimeToRefreshAccessToken()) {
        const refreshTokenStr = getCookie("refreshToken");
        const refreshToken = JSON.parse(refreshTokenStr) as JWTTokenType;
        ajaxForRefresh(refreshToken.token);
        console.log("*****")
    }
    console.log("aaaaaaaaaaaaaa")
    
    return new Observable<void>(subscriber => subscriber.next()).pipe(
        map(()=>{console.log("inside of requesr")}),
        mergeMap(()=> {
            const accessToken= JSON.parse(getCookie("accessToken")) as JWTTokenType;
            return ajax<response<T>>({
                url: url,
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + accessToken.token
                },
                body: JSON.stringify({
                    query,
                    variables
                }),
                withCredentials: withCredentials
            })
        }),
        catchError(error=>{
            throw error;
        })
    )
}