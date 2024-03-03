import React, { useState, useEffect } from 'react';
import { Box, TextField, IconButton, Container, Button, Grid } from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';
import { useConfigurationContext } from '../providers/ConfigurationProvider';
import SaveIcon from '@mui/icons-material/Save'; // Importing the Save icon


const ParameterInput = ({ parmId, onRemove, onParamChange, paramName, paramValue }) => {
    // Local state to manage input values
    const [name, setName] = useState(paramName || '');
    const [value, setValue] = useState(paramValue || '');
    const [isModified, setIsModified] = useState(false);
    const { updateParameter } = useConfigurationContext(); // Destructure to get state and setState


    useEffect(() => {
        setIsModified(name !== paramName || value !== paramValue);
    }, [name, value, paramName, paramValue]);

    // Handle changes to the input fields and propagate them upwards
    const handleChange = (setter) => (event) => {
        setter(event.target.value);
        console.log("this was paramId " + parmId)
        //onParamChange({ id: paramid, name: event.target.name, value: event.target.value });
    };



    return (
        <Box display="flex" alignItems="center" gap={2} marginBottom={2}>
            <TextField
                label="Parameter Name"
                variant="outlined"
                name="name"
                value={name}
                onChange={handleChange(setName)}
            />
            <TextField
                label="Value"
                variant="outlined"
                name="value"
                value={value}
                onChange={handleChange(setValue)}
            />
            <Box display="flex" flexDirection="column" alignItems="center" gap={1}>
                <IconButton onClick={onRemove} color="error">
                    <HighlightOffIcon />
                </IconButton>
                <IconButton onClick={() => updateParameter({ id: parmId, name: name, value: value })}>
                    <SaveIcon color={isModified ? 'primary' : 'disabled'} /> {/* Dynamically change color */}
                </IconButton>
            </Box>
        </Box>
    );
};

const ParameterList = () => {
    const { activeConfiguration, deleteParameter } = useConfigurationContext(); // Destructure to get state and setState
    console.log("state" + JSON.stringify(activeConfiguration))
    //why is state null on refresh? rewrite when figured out
    const [localParameters, setLocalParameters] = useState(activeConfiguration.parameters.map(param => ({
        id: param.id || Date.now(),
        name: param.name || '',
        value: param.value || ''
    })))

    const addParameter = () => {
        console.log("this is add State: " + JSON.stringify(activeConfiguration))
        setLocalParameters([...localParameters, { id: Date.now(), name: '', value: '' }]);
    };

    const removeParameter = (id) => {
        deleteParameter(id);
    };

    const handleParamChange = (id, data) => {
        /* setLocalParameters(localParameters.map(param => {
             if (param.id === id) {
                 return { ...param, [data.name]: data.value };
             }
             return param;
         }));*/
    };

    return (
        <Container>
            <Box display="flex" flexWrap="wrap" alignItems="center" marginBottom={2} >
                <h3>Parameters</h3>
                <Box marginLeft={2}>
                    <IconButton onClick={addParameter} color="primary">
                        <AddCircleOutlineIcon />
                    </IconButton>
                </Box>
            </Box>
            <Grid container spacing={2}>
                {localParameters.map((param) => (
                    <Grid item xs={12} sm={12} md={4} key={param.id}>
                        <ParameterInput
                            parmId={param.id}
                            paramName={param.name}
                            paramValue={param.value}
                            onParamChange={(data) => handleParamChange(param.id, data)}
                            onRemove={() => removeParameter(param.id)}
                        />
                    </Grid>
                ))}
            </Grid>
        </Container>

    );
};

export default ParameterList;