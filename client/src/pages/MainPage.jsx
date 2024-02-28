// View1.js
import { Box, Button, Container, Divider, Grid, Stack, Typography } from '@mui/material';
import React from 'react';
import Diagram from '../components/Diagram';
import ValveLine from '../components/ValveLine';
import RunPanel from '../components/RunPanel';
import RunProvider from '../providers/RunProvider';
import CommandProvider from '../providers/CommandProvider';

const MainPage = () => {
  return <Box>
    <Stack direction="row" spacing={16}>

      <Stack spacing={4}>
        <Stack spacing={2}>
          <Stack direction="row" justifyContent="space-between">
            <Typography variant='h4'>
              Data
            </Typography>
            <Button variant="contained">Reset</Button>
          </Stack>
          <Diagram />
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
        <CommandProvider>
          <Stack spacing={2} divider={<Divider orientation="horizontal" flexItem />}>
            {/* TODO: Hardcoded ids.  */}
            <ValveLine device={{ name: "Valve 1", id: 3}} /> 
            <ValveLine device={{ name: "Valve 2", id: 4}} />
          </Stack>
        </CommandProvider>
      </Stack>
    </Stack>
  </Box>;
};

export default MainPage;