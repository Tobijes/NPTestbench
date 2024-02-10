// View1.js
import { Box } from '@mui/material';
import React from 'react';
import TopBar from '../components/TopBar';
import { Outlet } from 'react-router-dom';

const RootPage = () => {
  return <Box>
     <TopBar />
     <Box sx={{padding: 4}}>
        <Outlet />

     </Box>
  </Box>;
};

export default RootPage;