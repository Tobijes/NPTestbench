import * as React from 'react';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import Paper from '@mui/material/Paper';
import { useConfigurationContext } from '../providers/ConfigurationProvider';

export default function ScrollableListWithMUI() {
    const {configs, currentConfiguration, setCurrentConfiguration, getConfigById, createConfiguration } = useConfigurationContext(); 

    const createNewItem = () => {
        createConfiguration("New Config Name")
    };
    console.log("current config " + JSON.stringify(configs))
    const handleItemClick = (config) => {
        getConfigById(config.id)
        console.log("this is selected config:", JSON.stringify(config))

        setCurrentConfiguration(config);
        console.log(`Item with ID ${config.id} was clicked`);
    };

    return (
        <div>
            <Button variant="contained" onClick={createNewItem}>
                Create new Item
            </Button>
            <Paper
                sx={{
                    maxHeight: 400,
                    overflow: 'auto',
                    mt: 2,
                    border: '1px solid rgba(0, 0, 0, 0.12)',
                    '& ul': { padding: 0 },
                }}
            >
                <List>
                    {configs.map((item) => (
                        <ListItem key={item.id} disablePadding>
                            <ListItemButton onClick={() => handleItemClick(item)}
                                sx={{
                                    bgcolor: currentConfiguration.id === item.id ? 'primary.main' : 'inherit', 
                                    '&:hover': {
                                        bgcolor: 'primary.light' 
                                    }
                                }}>
                                
                                {item.name + ((currentConfiguration.id === item.id) ? " - (Selected)" : "")}
                            </ListItemButton>
                        </ListItem>
                    ))}
                </List>
            </Paper>
        </div>
    );
}