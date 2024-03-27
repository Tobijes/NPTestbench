import * as React from 'react';
import Box from '@mui/material/Box'; import Grid from '@mui/material/Grid';
import ParameterList from '../components/Parameter';
import ScrollableList from '../components/ConfigurationListView';
import { Divider, Stack, Typography } from '@mui/material';
import ParameterPane from '../components/Parameter';

const ConfigurationPage = () => {

  return <Stack direction="row" spacing={8}>
    <Stack direction="column">
      <Typography variant='h6'>Configurations</Typography>
      <ScrollableList />
    </Stack>
    <ParameterPane />
  </Stack>


};

export default ConfigurationPage;