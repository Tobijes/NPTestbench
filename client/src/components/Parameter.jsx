import React, { useState } from 'react';
import { Box, TextField, IconButton, Container, Button, Grid } from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';


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
    const [parameters, setParameters] = useState([{ id: Date.now(), name: '', value: '' }]);

    const addParameter = () => {
        setParameters([...parameters, { id: Date.now(), name: '', value: '' }]);
    };

    const removeParameter = (id) => {
        setParameters(parameters.filter((param) => param.id !== id));
    };

    const handleParamChange = (id, data) => {
        setParameters(parameters.map(param => {
            if (param.id === id) {
                return { ...param, [data.name]: data.value };
            }
            return param;
        }));
    };

    const saveParameters = () => {
        // Here you would handle saving the parameters, e.g., sending them to a backend
        console.log('Saving parameters:', parameters);
    };

    return (
        <Container>
            <Box display="flex" flexWrap="wrap" alignItems="center" marginBottom={2} >
                <h3>Parameters</h3>
                <Box marginLeft={2}>
                    <IconButton onClick={addParameter} color="primary" >
                        <AddCircleOutlineIcon />
                    </IconButton>
                </Box>
            </Box>
            <Grid container spacing={2}>
                {parameters.map((param) => (
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