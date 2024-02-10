import React, { useState } from 'react';
import { Box, TextField, IconButton, Container } from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';

const ParameterInput = ({ onRemove }) => (
    <Box display="flex" alignItems="center" gap={2} marginBottom={2}>
        <TextField label="Parameter Name" variant="outlined" />
        <TextField label="Value" variant="outlined" />
        <IconButton onClick={onRemove} color="error">
            <HighlightOffIcon />
        </IconButton>
    </Box>
);

const ParameterList = () => {
    const [parameters, setParameters] = useState([{ id: Date.now() }]);

    const addParameter = () => {
        setParameters([...parameters, { id: Date.now() }]);
    };

    const removeParameter = id => {
        setParameters(parameters.filter(param => param.id !== id));
    };

    return (
        <Container maxWidth="sm">
            <Box display="flex" justifyContent="space-between" alignItems="center" marginBottom={2}>
                <h3>Parameter</h3>
                <IconButton onClick={addParameter} color="primary">
                    <AddCircleOutlineIcon />
                </IconButton>
            </Box>
            {parameters.map(param => (
                <ParameterInput key={param.id} onRemove={() => removeParameter(param.id)} />
            ))}
        </Container>
    );
};

export default ParameterList;