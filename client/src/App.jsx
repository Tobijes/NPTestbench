import React from 'react';
import DataStreamProvider from './providers/DataStreamProvider';
import './App.css'
import TopBar from './components/TopBar';
import { Box, ScopedCssBaseline } from '@mui/material';
import { createBrowserRouter, createRoutesFromElements, Outlet, Route, RouterProvider, Routes } from 'react-router-dom';
import CssBaseline from '@mui/material/CssBaseline';
import ConfigurationProvider from './providers/ConfigurationProvider';
import MainPage from './pages/MainPage';
import ConfigurationPage from './pages/ConfigurationPage';
import RootPage from './pages/Root';
import SummaryPage from './pages/SummaryPage';

export const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="/" element={<RootPage />}>
        <Route path="/" element={<MainPage />} />
        <Route path="/summary" element={<SummaryPage />} />
        <Route path="/configuration" element={<ConfigurationPage />} />
      </Route>
    </>
  )
);

function App() {

  return (
    <React.Fragment>
      <CssBaseline />
      <ConfigurationProvider>
        <DataStreamProvider>
          <RouterProvider router={router} />
        </DataStreamProvider>
      </ConfigurationProvider>
    </React.Fragment>
  );
}

export default App
