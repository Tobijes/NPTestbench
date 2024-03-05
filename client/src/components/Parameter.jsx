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
    const { currentConfiguration, deleteParameter, addParameter } = useConfigurationContext(); // Destructure to get state and setState
    console.log("state" + JSON.stringify(currentConfiguration))



    const addParameterLocal = () => {
        console.log("this is add State: " + JSON.stringify(currentConfiguration))
        addParameter()
        //setLocalParameters([...localParameters, { id: Date.now(), name: '', value: '' }]);
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
                    <IconButton onClick={addParameterLocal} color="primary">
                        <AddCircleOutlineIcon />
                    </IconButton>
                </Box>
            </Box>
            <Grid container spacing={2}>
                {currentConfiguration.parameters.map((param) => (
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