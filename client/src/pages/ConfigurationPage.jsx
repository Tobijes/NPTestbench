import * as React from 'react';
import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import ParameterList from '../components/Parameter';
const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary,
}));


const ConfigurationPage = () => {
    return  <Grid container spacing={2}>
    <Grid item xs={6}>
      <Item>xs=8</Item>
    </Grid>
    <Grid item xs={6}>
      <Box>
          <ParameterList/>
      </Box>
    </Grid>
    <Grid item xs={4}>
      <Item>xs=4</Item>
    </Grid>
    <Grid item xs={8}>
      <Item>xs=8</Item>
    </Grid>
  </Grid>
  
  
  
  //<div>Content for ConfigurationPage</div>;
};

export default ConfigurationPage;