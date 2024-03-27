// View1.js
import { Box, Button, Container, Divider, Grid, Stack, Typography } from '@mui/material';
import React from 'react';
import RunPanel from '../components/RunPanel';
import RunProvider from '../providers/RunProvider';
import DataDiagramController from '../components/DataDiagram';
import ValvePanel from '../components/ValvePanel';

const MainPage = () => {
  return <Box>
    <Stack direction="row" spacing={16}>

      <Stack spacing={4}>
        <Stack spacing={2}>
          <Stack direction="row" justifyContent="space-between">
            <Typography variant='h4'>
              Data
            </Typography>
            <Button variant="contained" disabled={true}>Reset</Button>
          </Stack>
          <DataDiagramController />
        </Stack>
        <Stack spacing={2}>
          <Typography variant='h4'>
            Run
          </Typography>
          <RunProvider>
            <RunPanel />
          </RunProvider>
        </Stack>
      </Stack>

      <Stack spacing={2}>
        <Typography variant='h4'>
          Commands
        </Typography>
        <ValvePanel />
      </Stack>
    </Stack>
  </Box>;
};

export default MainPage;