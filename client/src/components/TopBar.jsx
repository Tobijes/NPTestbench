import { Box, AppBar, Toolbar, IconButton, Typography, Button, TextField, InputLabel, Select, CircularProgress, Autocomplete } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import React from 'react';
import ConfigurationDropDown from './ConfigurationDropDown';
import { Link, NavLink } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';

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

          <Typography variant="h6" component={Link} sx={{ textDecoration: 'none', fontWeight:"bold", color:"inherit" }} >
            NPTestbench
          </Typography>

          <Box sx={{ ml: 2 }}>
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