import { Box, AppBar, Toolbar, IconButton, Typography, Button, TextField, InputLabel, Select, CircularProgress, Autocomplete } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import React from 'react';
import ConfigurationDropDown from './ConfigurationDropDown';

const TopBar = () => {


  return (
    <Box >
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" component="div">
            News
          </Typography>
          <ConfigurationDropDown/>
          
        </Toolbar>
      </AppBar>
    </Box>
  );
}


export default TopBar;