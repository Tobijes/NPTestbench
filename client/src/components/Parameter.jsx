import React, { useState } from 'react';
import { Box, TextField, IconButton, Container, Button } from '@mui/material';
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
        <Container maxWidth="sm">
            <Box display="flex" flexWrap="wrap" justifyContent="space-between" alignItems="center" marginBottom={2} marginRight={10}>
                <h3>Parameters</h3>
                <IconButton onClick={addParameter} color="primary">
                    <AddCircleOutlineIcon />
                </IconButton>
            </Box>
            {parameters.map((param) => (
                <ParameterInput
                    key={param.id}
                    paramName={param.name}
                    paramValue={param.value}
                    onParamChange={(data) => handleParamChange(param.id, data)}
                    onRemove={() => removeParameter(param.id)}
                />
            ))}
            <Button onClick={saveParameters} color="primary" variant="contained">
                Save All
            </Button>
        </Container>
    );
};

export default ParameterList;