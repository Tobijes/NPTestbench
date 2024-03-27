import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { useConfigurationContext } from '../providers/ConfigurationProvider';

export default function ConfigurationDropDown() {
    const { activeConfiguration, setActiveConfiguration, configs} = useConfigurationContext(); // Destructure to get state and setState
    const handleChange = (event) => {
        const selectedConfigId = event.target.value;     
        const selectedConfig = configs.find(config => config.id.toString() == selectedConfigId);
        if (selectedConfig) {
            setActiveConfiguration(selectedConfig);
        }
    };
    return (
        <Box sx={{ minWidth: 250 }}>
            <FormControl fullWidth>
                <InputLabel id="configuration-select-label" sx={{color: "white"}}>Active Configuration</InputLabel>
                <Select
                    labelId="configuration-select-label"
                    id="demo-simple-select"
                    value={activeConfiguration ? activeConfiguration.id : ''}
                    label="Configuration"
                    onChange={handleChange}
                    sx={{height: 40, color:"white"}} 
                >
                    {/* Dynamically create MenuItems from your context state */}
                    {configs && configs.map((item, index) => (
                        <MenuItem key={index} value={item.id}>{item.name}</MenuItem>
                    ))}
                </Select>
            </FormControl>
        </Box>
    );
}
