import { Button, Chip, Stack, Typography } from "@mui/material";
import PropTypes from 'prop-types';
import { useState } from "react";
import { useCommandContext } from "../providers/CommandProvider";
import { useDataStreamContext } from "../providers/DataStreamProvider";

const ValveLine = ({ device }) => {
    
    const dataStreamContext = useDataStreamContext();
    const commandContext = useCommandContext();
    
    const onOpenClick = () => {
        commandContext.callOpen(device.id);
    }
    const onCloseClick = () => {
        commandContext.callClose(device.id);
    }
    const onPulseClick = () => {
        commandContext.callPulse(device.id);
    }
    

    let open = undefined;
    if (dataStreamContext.deviceStates != null && device.id in dataStreamContext.deviceStates) {
        const value = dataStreamContext.deviceStates[device.id].value;
        open = value > 0;
    }


    return <Stack direction="row" spacing={4} justifyContent="space-between" alignItems="center">
        <Stack direction="row" spacing={2} alignItems="center">
            <Typography>{device.name}</Typography>
            <Chip label={open ? "Open" : "Closed"} color={open ? "success" : "error"} variant="outlined"/>

        </Stack>
        <Stack direction="row" spacing={4}>
            <Button variant='contained' disabled={open || open == undefined } onClick={onOpenClick}>Open</Button>
            <Button variant='contained' disabled={!open || open == undefined} onClick={onCloseClick}>Close</Button>
            <Button variant='contained' onClick={onPulseClick}>Pulse</Button>
        </Stack>
    </Stack>
}

ValveLine.propTypes = {
    device: PropTypes.object.isRequired,
};

export default ValveLine;