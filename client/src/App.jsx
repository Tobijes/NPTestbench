import React from 'react';
import DataStreamProvider from './providers/DataStreamProvider';
import './App.css'
import SendBox from './components/SendBox';
import TopBar from './components/TopBar';
import { Box, ScopedCssBaseline } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';

function App() {

  return (
    <React.Fragment>
      <CssBaseline />
      <DataStreamProvider>
        <Box>
          <TopBar />
          <h1>SignalR Demo</h1>
          <SendBox />
        </Box>
      </DataStreamProvider>
    </React.Fragment>

  );
}

export default App
