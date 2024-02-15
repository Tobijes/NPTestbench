import React, { useState } from 'react';
import { Box, TextField, IconButton, Container, Button, Grid } from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';
import { useConfigurationContext } from '../providers/ConfigurationProvider';


const ParameterInput = ({ onRemove, onParamChange, paramName, paramValue }) => {
    // Local state to manage input values
    const [name, setName] = useState(paramName || '');
    const [value, setValue] = useState(paramValue || '');
    // Handle changes to the input fields and propagate them upwards
    const handleChange = (setter) => (event) => {
        setter(event.target.value);
        onParamChange({ name: event.target.name, value: event.target.value });
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
            <IconButton onClick={onRemove} color="error">
                <HighlightOffIcon />
            </IconButton>
        </Box>
    );
};

const ParameterList = () => {
    const { state, updateParameters } = useConfigurationContext(); // Destructure to get state and setState
    console.log("state" + JSON.stringify(state))
    //why is state null on refresh? rewrite when figured out
    const [localParameters, setLocalParameters] = useState(state.parameters.map(param => ({
        id: param.id || Date.now(),
        name: param.name || '',
        value: param.value || ''
    })))

    const isDisabled = state.parameters.length !== localParameters.length

    const addParameter = () => {
        console.log("this is add State: " + JSON.stringify(state))
        console.log("isDisabled" + isDisabled)
        if (!isDisabled)
            setLocalParameters([...localParameters, { id: Date.now(), name: '', value: '' }]);
    };

    const removeParameter = (id) => {
        setLocalParameters(localParameters.filter((param) => param.id !== id));
    };

    const handleParamChange = (id, data) => {
        setLocalParameters(localParameters.map(param => {
            if (param.id === id) {
                return { ...param, [data.name]: data.value };
            }
            return param;
        }));
    };
    const saveParameters = () => {

        console.log('Saving parameters:', localParameters);
        updateParameters(localParameters)
    };

    return (
        <Container>
            <Box display="flex" flexWrap="wrap" alignItems="center" marginBottom={2} >
                <h3>Parameters</h3>
                <Box marginLeft={2}>
                    <IconButton onClick={addParameter} color="primary" disabled={isDisabled}>
                        <AddCircleOutlineIcon />
                    </IconButton>
                </Box>
            </Box>
            <Grid container spacing={2}>
                {localParameters.map((param) => (
                    <Grid item xs={12} sm={12} md={4} key={param.id}>
                        <ParameterInput
                            paramName={param.name}
                            paramValue={param.value}
                            onParamChange={(data) => handleParamChange(param.id, data)}
                            onRemove={() => removeParameter(param.id)}
                        />
                    </Grid>
                ))}
            </Grid>
            <Box display="flex" justifyContent="flex-end" marginTop={2}>
                <Button onClick={saveParameters} color="primary" variant="contained">
                    Save All
                </Button>
            </Box>

        </Container>

    );
};

export default ParameterList;