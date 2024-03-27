import React, {  } from 'react';
import { Box, Button, Stack, Typography, Divider } from '@mui/material';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import EditIcon from '@mui/icons-material/Edit';
import { useConfigurationContext } from '../providers/ConfigurationProvider';
import ParameterPane from './Parameter';
import RenameDialog from './RenameDialog';


const ConfigurationPane = () => {
    const { currentConfiguration, cloneConfiguration } = useConfigurationContext();

    if (currentConfiguration == null) {
        return <Box></Box>
    }


    return <Stack direction="column" spacing={4} flexGrow={1}>
        <Stack direction="row" justifyContent="space-between" alignItems="center" >
            <Stack direction='column'>
                <Typography variant='body'>Configuration:</Typography>
                <Typography variant='h6'>{currentConfiguration.name}</Typography>
            </Stack>
            <Stack direction="row" spacing={2} >
                <RenameDialog />
                <Button variant="contained" startIcon={<ContentCopyIcon />} onClick={cloneConfiguration}>Duplicate</Button>
            </Stack>
        </Stack>

        <Divider />

        <ParameterPane />
       
    </Stack>
}

export default ConfigurationPane;