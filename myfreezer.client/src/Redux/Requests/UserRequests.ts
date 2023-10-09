import {map, Observable} from "rxjs";
import {User} from "../../Models/User";
import {GetAjaxObservable} from "./LoginRequests";


interface GraphQLUsers {
    user: {
        users: User[]
    }
}

export function RequestUsers(): Observable<User[]> {
    return GetAjaxObservable<GraphQLUsers>(
        `query {
                  user{
                    users{
                      id
                      login
                      password
                      registrationDate
                    }
                  }
                }`, {})
        .pipe(
            map(res => {
                if (res.response.errors) {
                    console.error(JSON.stringify(res.response.errors))
                    throw "error"
                }
                return res.response.data.user.users
            })
        )
}