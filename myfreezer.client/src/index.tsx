import ReactDOM from 'react-dom/client';
import App from './App';
import {Provider} from "react-redux";
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import React from "react";

import NavBar from './Components/NavBar'
import Login from './Components/Login'
import AppContent from "./Components/AppContent";
import store from "./Redux/store";

const router = createBrowserRouter([
    {
        element: <NavBar/>,
        children: [
            {
                path: "/",
                element: <AppContent/>
            },
            {
                path: "/login",
                element: <Login/>
            }
        ]
    }
])

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <Provider store={store}>
        <RouterProvider router={router}/>
    </Provider>
);
