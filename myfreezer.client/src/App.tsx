import React from 'react';
import AppContent from './Components/AppContent'
import NavBar from './Components/NavBar'
import Login from './Components/Login'
import { createBrowserRouter, RouterProvider } from "react-router-dom";




function App() {

  return (
    <div className="App container-fluid p-0 h-100" data-bs-theme="dark">
        <AppContent/>
    </div>
  );
}

export default App;
