import * as React from 'react';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import EditIcon from '@mui/icons-material/Edit';
import { useConfigurationContext } from '../providers/ConfigurationProvider';

const RenameDialog = () => {
  const { currentConfiguration, renameConfiguration } = useConfigurationContext();
  const [open, setOpen] = React.useState(false);
  const [name, setName] = React.useState("Error: notset");

  const handleClickOpen = () => {
    setName(currentConfiguration.name)
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleSave = async () => {
    await renameConfiguration(name)
    handleClose();
  }

  const handleTextChange = (e) => {
    setName(e.target.value)
  }

  const saveDisabled = currentConfiguration.name === name || name.length == 0;

  return (
    <React.Fragment>
      <Button variant="contained" startIcon={<EditIcon />} onClick={handleClickOpen}>Rename</Button>
      <Dialog
        open={open}
        onClose={handleClose}
        fullWidth={true}
      >
        <DialogTitle>Change Configuration Name</DialogTitle>
        <DialogContent>
          {/* <DialogContentText>
              To subscribe to this website, please enter your email address here. We
              will send updates occasionally.
            </DialogContentText> */}
          <TextField
            autoFocus
            margin="dense"
            id="name"
            name="name"
            label="Configuration name"
            type="text"
            fullWidth
            variant="outlined"
            value={name}
            onChange={handleTextChange}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button onClick={handleSave} disabled={saveDisabled}>Save</Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
}

export default RenameDialog