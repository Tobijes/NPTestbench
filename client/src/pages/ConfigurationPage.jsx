import * as React from 'react';
 import ScrollableList from '../components/ConfigurationListView';
import { Stack, Typography } from '@mui/material';
import ConfigurationPane from '../components/ConfigurationPane';

const ConfigurationPage = () => {

  return <Stack direction="row" spacing={8}>
    <Stack direction="column">
      <Typography variant='h6'>Configurations</Typography>
      <ScrollableList />
    </Stack>
    <ConfigurationPane />
  </Stack>


};

export default ConfigurationPage;