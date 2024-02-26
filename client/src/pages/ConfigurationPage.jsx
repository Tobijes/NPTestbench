import * as React from 'react';
import Box from '@mui/material/Box';import Grid from '@mui/material/Grid';
import ParameterList from '../components/Parameter';
import ScrollableList from '../components/ConfigurationListView';

const ConfigurationPage = () => {
  return <Grid container spacing={2}>
    <Grid item xs={2}>
      <ScrollableList/>
    </Grid>
    <Grid item xs={10}>
      <Box>
        <ParameterList />
      </Box>
    </Grid>
    <Grid item xs={4}>
   
    </Grid>
    <Grid item xs={8}>
      
    </Grid>
  </Grid>


};

export default ConfigurationPage;