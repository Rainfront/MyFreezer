import React from 'react';
import AppContent from './Components/AppContent'
import { createBrowserRouter, RouterProvider } from "react-router-dom";

const router = createBrowserRouter([
    {
        path: "/",
        element: <div>Hello</div>
    },
    {
        path: "/login",
        element: <Login/>
    }
])

function App() {

  return (
    <div className="App container-fluid p-0 h-100" data-bs-theme="dark">
        <AppContent/>
    </div>
  );
}

export default App;
