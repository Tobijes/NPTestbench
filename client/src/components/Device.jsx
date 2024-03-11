import React, { useState, useEffect } from 'react';
import { Box, TextField, IconButton, Container, Button, Grid } from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';
import { useConfigurationContext } from '../providers/ConfigurationProvider';
import SaveIcon from '@mui/icons-material/Save'; // Importing the Save icon

const DeviceInput = ({ parmId, paramName, paramValue }) => {
    // Local state to manage input values
    const [name, setName] = useState(paramName || '');
    const [value, setValue] = useState(paramValue || '');
    const [isModified, setIsModified] = useState(false);
    const { updateParameter, currentConfiguration } = useConfigurationContext(); // Destructure to get state and setState

    var currentparm = currentConfiguration.parameters.find(e => e.id == parmId)
    useEffect(() => {
        if (currentparm) {
            setIsModified(name !== currentparm.name || value !== currentparm.value);
        } else {
            // Handle the case where currentparm is undefined, maybe reset state
            setIsModified(false);
        }
    }, [name, value, currentparm]);

    // Handle changes to the input fields and propagate them upwards
    const handleChange = (setter) => (event) => {
        setter(event.target.value);
        console.log("this is name " + name)
        console.log("this is Paramname " + paramName)
    };



    return (
        <Box display="flex" alignItems="center" gap={2} marginBottom={2}>
            <TextField
                label="Device Name"
                variant="outlined"
                name="name"
                value={name}
                onChange={handleChange(setName)}
            />
            <TextField
                label="Start Address"
                variant="outlined"
                name="value"
                value={value}
                onChange={handleChange(setValue)}
            />
            <TextField
                label="DataType"
                variant="outlined"
                name="value"
                value={value}
                onChange={handleChange(setValue)}
            />
            <TextField
                label="DrawingID"
                variant="outlined"
                name="value"
                value={value}
                onChange={handleChange(setValue)}
            />
            <Box display="flex" flexDirection="column" alignItems="center" gap={1}>
                <IconButton onClick={() => updateParameter({ id: parmId, name: name, value: value })}>
                    <SaveIcon color={isModified ? 'primary' : 'disabled'} /> {/* Dynamically change color */}
                </IconButton>
            </Box>
        </Box>
    );
};
