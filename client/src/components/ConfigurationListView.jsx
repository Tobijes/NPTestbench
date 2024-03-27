import * as React from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import Paper from '@mui/material/Paper';
import { useConfigurationContext } from '../providers/ConfigurationProvider';
import { Chip, Stack, Typography } from '@mui/material';

export default function ScrollableListWithMUI() {
    const { configs, currentConfiguration, setCurrentConfiguration, getConfigById, activeConfiguration } = useConfigurationContext();

    // console.log("current config " + JSON.stringify(configs))
    const handleItemClick = (config) => {
        getConfigById(config.id)
        // console.log("this is selected config:", JSON.stringify(config))

        setCurrentConfiguration(config);
        // console.log(`Item with ID ${config.id} was clicked`);
    };

    return (
        <Paper
            sx={{
                minWidth: 400,
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
                                color: currentConfiguration.id === item.id ? 'white' : 'inherit',
                                bgcolor: currentConfiguration.id === item.id ? 'primary.main' : 'inherit',
                                '&:hover': {
                                    bgcolor: 'primary.light'
                                }
                            }}>
                            <Stack direction="row" alignItems="center" spacing={1}>
                                <Typography variant="body">{item.name} </Typography>
                                {activeConfiguration.id === item.id ? <Chip label="Active" color="success" variant="outlined" sx={{height: 24}}/> : null}
                            </Stack>
                        </ListItemButton>
                    </ListItem>
                ))}
            </List>
        </Paper>
    );
}