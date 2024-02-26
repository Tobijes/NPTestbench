import * as React from 'react';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import Paper from '@mui/material/Paper';
import { useConfigurationContext } from '../providers/ConfigurationProvider';

export default function ScrollableListWithMUI() {
   /* const [items, setItems] = React.useState([
        { id: 1, text: 'Item 1' },
        { id: 2, text: 'Item 2' },
        { id: 3, text: 'Item 3' }
    ]);*/
    const { activeConfiguration, configs, setCurrentConfiguration } = useConfigurationContext(); // Destructure to get state and setState


    const createNewItem = () => {
        const newItem = {
            id: configs.length + 1,
            text: `Item ${configs.length + 1}`
        };
        //setItems([...configs, newItem]);
    };
    console.log("active config " + activeConfiguration.id)
    const handleItemClick = (id) => {
        // This is where you would load your state based on the clicked item's ID
        setCurrentConfiguration(id);
        // For demonstration, we'll just log the ID to the console
        console.log(`Item with ID ${id} was clicked`);
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
                            <ListItemButton onClick={() => handleItemClick(item.id)}>
                                {item.name + ((activeConfiguration.id === item.id) ? " - (Active)" : "")}
                            </ListItemButton>
                        </ListItem>
                    ))}
                </List>
            </Paper>
        </div>
    );
}