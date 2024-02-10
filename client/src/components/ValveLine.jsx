import { Button, Stack, Typography } from "@mui/material";
import PropTypes from 'prop-types';
import { useState } from "react";

const ValveLine = ({ device }) => {
    const [open, setOpen] = useState(true);

    const onOpenClick = () => {
        setOpen(true);
    }
    const onCloseClick = () => {
        setOpen(false);

    }
    const onPulseClick = () => {

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
    device: PropTypes.node.isRequired,
};

export default ValveLine;