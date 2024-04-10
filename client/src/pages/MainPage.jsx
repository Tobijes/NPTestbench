// View1.js
import { Box, Button, Container, Divider, Grid, Stack, Typography } from '@mui/material';
import React from 'react';
import RunPanel from '../components/RunPanel';
import RunProvider from '../providers/RunProvider';
import DataDiagramController from '../components/DataDiagram';
import ValvePanel from '../components/ValvePanel';

const MainPage = () => {
  return <Box>
    <Stack direction="row" spacing={16} justifyContent="space-between" >
      <Stack direction="column" spacing={0} flexGrow={1}>
        <DataDiagramController />
        <Stack direction="row" spacing={0}>
          <Button variant="contained" disabled={true}>Reset limits</Button>
        </Stack>
      </Stack>

      <Stack spacing={4} >
        <Stack spacing={2}>
          <Typography variant='h4'>
            Commands
          </Typography>
          <ValvePanel />
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
    </Stack>
  </Box>;
};

export default MainPage;