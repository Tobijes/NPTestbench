import { Box, AppBar, Toolbar, IconButton, Typography, Button, TextField, InputLabel, Select, CircularProgress, Autocomplete } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import React from 'react';
import ConfigurationDropDown from './ConfigurationDropDown';
import { Link, NavLink } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';

import NorthPropulsionLogo from '../assets/north_propulsion.png'

const TopBar = () => {


  return (
    <Box >
      <AppBar position="static">
        <Toolbar>
          {/* <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
          >
            <MenuIcon />
          </IconButton> */}

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
          <Button sx={{left: 900}} 
           variant="contained"
          component={Link} to = "/summary"
           >Show Summary</Button>
        </Toolbar>
      </AppBar>
    </Box>
  );
}


export default TopBar;