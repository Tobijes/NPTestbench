import { Button, Stack, Typography } from "@mui/material";
import PropTypes from 'prop-types';
import { useState } from "react";
import { useCommandContext } from "../providers/CommandProvider";

const ValveLine = ({ device }) => {
    const [open, setOpen] = useState(true);
    const commandContext = useCommandContext();

    const onOpenClick = () => {
        commandContext.callOpen(device.id);
        setOpen(true);
    }
    const onCloseClick = () => {
        commandContext.callClose(device.id);
        setOpen(false);

    }
    const onPulseClick = () => {
        commandContext.callPulse(device.id);
    }

    return <Stack direction="row" spacing={8} justifyContent="space-between" alignItems="center">
        <Typography>{device.name}</Typography>
        <Stack direction="row" spacing={4}>
            <Button variant='contained' disabled={open} onClick={onOpenClick}>Open</Button>
            <Button variant='contained' disabled={!open} onClick={onCloseClick}>Close</Button>
            <Button variant='contained' onClick={onPulseClick}>Pulse</Button>
        </Stack>
    </Stack>
}

ValveLine.propTypes = {
    device: PropTypes.object.isRequired,
};

export default ValveLine;