import {combineEpics, Epic} from "redux-observable";
import {catchError} from "rxjs";
import {getUsersEpic} from "./Epics/UserEpics";

export const rootEpic: Epic = (action$, store$, dependencies) =>
    combineEpics(
        getUsersEpic
    )(action$, store$, dependencies).pipe(
        catchError((error, source) => {
            return source
        })
    )
