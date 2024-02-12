import { Button, Stack, Typography } from "@mui/material";
import PropTypes from 'prop-types';
import { useState } from "react";
import { useRunContext } from "../providers/RunProvider";
import { useDataStreamContext } from "../providers/DataStreamProvider";

const RunPanel = () => {
  const dataStreamContext = useDataStreamContext();
  const runContext = useRunContext();

  const onStartClick = () => {
    runContext.callStart();
  }

  const onStopClick = () => {
    runContext.callStop();
  }
  const isRunning = dataStreamContext.runId !== undefined && dataStreamContext.runId !== null;

  return <Stack spacing={1}>
    {isRunning && <Typography>Running! Run ID: #{dataStreamContext.runId}</Typography>}
    <Stack spacing={1} direction={"row"}>
      <Button variant='contained' onClick={onStartClick} disabled={isRunning}>Start Run</Button>
      <Button variant='contained' onClick={onStopClick} disabled={!isRunning}>Stop Run</Button>
    </Stack>
  </Stack>
}

RunPanel.propTypes = {

};

export default RunPanel;