import {createEpicMiddleware} from "redux-observable";
import {configureStore} from "@reduxjs/toolkit";
import UserReducer from './Slices/userSlice'
import {useDispatch} from "react-redux";
import {rootEpic} from "./rootEpic";

const epicMiddleware = createEpicMiddleware();

const store = configureStore({
    reducer: {
        users: UserReducer
    },
    middleware: [epicMiddleware]
});

epicMiddleware.run(rootEpic);

export type RootState = ReturnType<typeof store.getState>;
export type Dispatch = ReturnType<typeof useDispatch>;

export default store;