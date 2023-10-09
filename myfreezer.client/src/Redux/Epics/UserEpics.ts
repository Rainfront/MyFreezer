import {Epic, ofType} from "redux-observable";
import {map, mergeMap} from "rxjs";
import {RequestUsers} from "../Requests/UserRequests";
import {getUserList} from "../Slices/userSlice";
import {User} from "../../Models/User";


export const getUsers = () => ({type: "getUsers"});
export const getUsersEpic : Epic = action$ => action$.pipe(
    ofType("getUsers"),
    mergeMap(()=> RequestUsers().pipe(
        map((res:User[]) => getUserList(res))
    ))
)