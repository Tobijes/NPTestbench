import { Box, AppBar, Toolbar, IconButton } from '@mui/material';
import React from 'react';
import ConfigurationDropDown from './ConfigurationDropDown';
import { Link } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';

import NorthPropulsionLogo from '../assets/north_propulsion.png'
import { useDataStreamContext } from '../providers/DataStreamProvider';

const TopBar = () => {
  const dataStreamContext = useDataStreamContext();

  return (
    <Box >
      <AppBar position="static" color={dataStreamContext.connected ? "primary" : "error"}>
        <Toolbar>
          <Link to="/">
            <Box
              component="img"
              sx={{
                height: 40,
              }}
              alt="North Propulsion Logo"
              src={NorthPropulsionLogo}
            />
          </Link>

          <Box sx={{ ml: 8 }}>
            <ConfigurationDropDown />
          </Box>
          <IconButton
            component={Link} to="/configuration"
            size="large"
            edge="start"
            color="inherit"
            aria-label="edit"
            sx={{ ml: 2 }}
          >
            <EditIcon />
          </IconButton>
        </Toolbar>
      </AppBar>
    </Box>
  );
}


export default TopBar;