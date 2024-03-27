import React, { useState, useEffect } from 'react';
import { Box, TextField, IconButton, Button, Grid, Stack, Typography } from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';
import { useConfigurationContext } from '../providers/ConfigurationProvider';
import SaveIcon from '@mui/icons-material/Save'; // Importing the Save icon


const ParameterInput = ({ parmId, paramName, paramValue }) => {
    // Local state to manage input values
    const [name, setName] = useState(paramName || '');
    const [value, setValue] = useState(paramValue || '');
    const [isModified, setIsModified] = useState(false);
    const { updateParameter, deleteParameter, currentConfiguration } = useConfigurationContext(); // Destructure to get state and setState

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
        <Stack direction="row" spacing={2} alignItems="center">
            <Box>
                <IconButton onClick={() => deleteParameter(parmId)} color="error">
                    <HighlightOffIcon />
                </IconButton>
            </Box>
            <TextField
                label="Parameter Name"
                variant="outlined"
                name="name"
                value={name}
                onChange={handleChange(setName)}
                fullWidth={true}
            />
            <TextField
                label="Value"
                variant="outlined"
                name="value"
                value={value}
                onChange={handleChange(setValue)}
                fullWidth={false}
            />
            <Box>
                <IconButton onClick={() => updateParameter({ id: parmId, name: name, value: value })} disabled={!isModified}>
                    <SaveIcon color={isModified ? 'primary' : 'disabled'} />
                </IconButton>
            </Box>
        </Stack>
    );
};

const ParameterPane = () => {
    const { currentConfiguration, addParameter } = useConfigurationContext();

    if (currentConfiguration == null) {
        return <Box></Box>
    }

    return <Stack direction="column" spacing={2}>
    <Stack direction="row" justifyContent="space-between">
        <Typography variant='h6'>Parameters</Typography>
        <Button variant="contained" startIcon={<AddCircleOutlineIcon />} onClick={addParameter}>Add parameter</Button>
    </Stack>

    <Grid container rowSpacing={4} columnSpacing={8} sx={{ marginLeft: "-64px !important" }}> {/* Alignment must be bug in grid impl. This fixes for now */}
        {currentConfiguration.parameters.map((param) => (
            <Grid item md={12} lg={6} key={param.id} > {/*xs={12} sm={12} md={12} lg={4}*/}
                <ParameterInput
                    parmId={param.id}
                    paramName={param.name}
                    paramValue={param.value}
                />
            </Grid>
        ))}
    </Grid>
</Stack>

}

export default ParameterPane;