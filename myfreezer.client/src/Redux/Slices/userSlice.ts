import {User} from "../../Models/User";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";

export interface usersState {
    Users: User[],
    CurrentUser: User
};

const initialState:usersState = {
    Users: [],
    CurrentUser: {} as User
};
export const userSlice = createSlice({
    name: "users",
    initialState,
    reducers: {
        getUserList: (state, action:PayloadAction<User[]>) => {
            console.log("2222")
            state.Users = action.payload;
        },
        getUser: (state, action) => {
            state.CurrentUser = action.payload;
        }
    }
})

export const {
    getUserList,
    getUser
} = userSlice.actions;
export default userSlice.reducer;