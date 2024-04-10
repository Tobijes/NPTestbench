import { Box, AppBar, Toolbar, IconButton, Stack, Button } from '@mui/material';
import React from 'react';
import ConfigurationDropDown from './ConfigurationDropDown';
import { Link, useLocation } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';
import AccountTreeIcon from '@mui/icons-material/AccountTree';
import QueryStatsIcon from '@mui/icons-material/QueryStats';

import NorthPropulsionLogo from '../assets/north_propulsion.png'
import { useDataStreamContext } from '../providers/DataStreamProvider';

const TopBar = () => {
  const dataStreamContext = useDataStreamContext();

  let location = useLocation();
  const path = location.pathname;
  return (
    <Box >
      <AppBar position="static" color={dataStreamContext.connected ? "primary" : "error"}>
        <Toolbar>
          <Stack flexGrow={1} direction="row" justifyContent="space-between"  alignItems="center">

            <Link to="/">
              <Box component="img" sx={{ height: 40 }} alt="North Propulsion Logo" src={NorthPropulsionLogo} />
            </Link>


            <Stack direction="row" spacing={2}>
              <Button component={Link} sx={{bgcolor: "primary.dark"}} to="/" variant="contained" disabled={path=="/"} startIcon={<AccountTreeIcon />}>Home</Button>
              <Button component={Link} sx={{bgcolor: "primary.dark"}} to="/summary" variant="contained" disabled={path=="/summary"} startIcon={<QueryStatsIcon />}>Summary</Button>
              <Button component={Link} sx={{bgcolor: "primary.dark"}} to="/configuration" variant="contained" disabled={path=="/configuration"} startIcon={<EditIcon />}>Configurations</Button>
            </Stack>

            <Box sx={{ ml: 8 }}>
              <ConfigurationDropDown />
            </Box>
            
          </Stack>
        </Toolbar>
      </AppBar>
    </Box>
  );
}


export default TopBar;