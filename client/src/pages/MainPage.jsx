// View1.js
import { Box, Button, Container, Divider, Grid, Stack, Typography } from '@mui/material';
import React from 'react';
import Diagram from '../components/Diagram';
import ValveLine from '../components/ValveLine';

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
        <Box>
          <Typography variant='h4'>
            Run
          </Typography>
          <Button variant='contained'>Start Run</Button>
        </Box>
      </Stack>

      <Stack spacing={2}>
        <Typography variant='h4'>
          Commands
        </Typography>
        <Stack spacing={2} divider={<Divider orientation="horizontal" flexItem />}>
          <ValveLine device={{name: "Valve 1"}}/>
          <ValveLine device={{name: "Valve 2"}}/>
        </Stack>
      </Stack>
    </Stack>
  </Box>;
};

export default MainPage;