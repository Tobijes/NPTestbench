import React from 'react';
import DataStreamProvider from './providers/DataStreamProvider';
import './App.css'
import SendBox from './components/SendBox';
import TopBar from './components/TopBar';
import { Box, ScopedCssBaseline } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';
import ConfigurationProvider from './providers/ConfigurationProvider';
import Diagram from './components/Diagram';

function App() {

  return (
    <React.Fragment>
      <CssBaseline />
      <ConfigurationProvider>
      <DataStreamProvider>
        <Box>
          <TopBar />
          <Diagram />
          <h1>SignalR Demo</h1>
          <SendBox />
        </Box>
      </DataStreamProvider>
      </ConfigurationProvider>
    </React.Fragment>
  );
}

export default App
