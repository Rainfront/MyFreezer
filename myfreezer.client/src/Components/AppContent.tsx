import {Button} from "@mui/material"
import {useDispatch, useSelector} from "react-redux";
import {getUsers} from "../Redux/Epics/UserEpics";
import {RootState} from "../Redux/store";
import {User} from "../Models/User";
import {useEffect} from "react";
import {RequestUsers} from "../Redux/Requests/UserRequests";
import {DeleteAllCookies} from "../Helpers/LoginHelper";

export default function AppContent() {
    const dispatch = useDispatch();
    const userList = useSelector((state: RootState) => state.users.Users);
    console.log(userList)
    const handleClick = () => {
        dispatch(getUsers());
        
    }
    useEffect(() => {

    })
    return (<>
        <h2>MAIN PAGE</h2>
        <Button variant={"outlined"} color={"success"} onClick={DeleteAllCookies}>logout</Button>
        <Button variant={"outlined"} color={"success"} onClick={handleClick}>Get Users</Button>
        {userList ? userList.map((user: User) => {
            return (
                <>
                    <p>Id - {user.id}</p>
                    <p>Name - {user.login}</p>
                </>
            )
        }) : <></>}
    </>)
}