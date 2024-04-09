import { Button, Divider, Stack, Typography } from "@mui/material";
import ValveLine from '../components/ValveLine';
import PropTypes from 'prop-types';
import { useState } from "react";

import CommandProvider from '../providers/CommandProvider';
import { useDataStreamContext } from "../providers/DataStreamProvider";
import { useConfigurationContext } from "../providers/ConfigurationProvider";

const ValvePanel = () => {
    const dataStreamContext = useDataStreamContext();
    const { activeConfiguration } = useConfigurationContext();
    
    let valveLines = [];
    if (activeConfiguration != null) {
        const writables = activeConfiguration.devices.filter(d => d.deviceChannels.filter(dc => !dc.isRead).length > 0);
        valveLines = writables.map(device => <ValveLine key={device.id} device={device} />)
    }

    return <CommandProvider>
        <Stack spacing={2} divider={<Divider orientation="horizontal" flexItem />}>
            {valveLines}
        </Stack>
    </CommandProvider>
}



export default ValvePanel;