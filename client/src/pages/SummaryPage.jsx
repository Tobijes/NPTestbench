import * as React from 'react';
import { Stack, Typography } from '@mui/material';
import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import { Label } from '@mui/icons-material';
const SummaryPage = () => {

    return <Stack direction="column" spacing={3}>
        <Typography variant='h6'>Summary of Last Run</Typography>
        <Stack direction="row" spacing={25}>

            <Typography>xs=8</Typography>
            <Typography>xs=8</Typography>
            <Typography>xs=8</Typography>
            <Typography>xs=8</Typography>

        </Stack>
        <Stack direction="row" spacing={25}>
            <Typography>xs=8</Typography>
            <Typography>xs=8</Typography>
            <Typography>xs=8</Typography>
            <Typography>xs=8</Typography>
        </Stack>

    </Stack>


};

export default SummaryPage;