import { useState, FormEvent } from 'react';
// import './UserShow.css';
import {
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  FormGroup,
  FormControlLabel,
  IconButton,
  Stack,
  Switch,
  TextField,
} from "@mui/material";
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';

export function FormDialog ({ submitAction }: { submitAction: Function}) {
  const [open, setOpen] = useState(false);
  const [inputVisible, setInputVisible] = useState(false);
  const [useInputPassword, setUseInputPassword] = useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const toggleUseInputPassword = () => {
    setUseInputPassword(!useInputPassword);
  }

  const toggleVisibility = () => {
    setInputVisible(!inputVisible);
  }

  return (
    <>
      <Button variant="outlined" onClick={handleClickOpen}>
        Set Password
      </Button>
      <Dialog
        open={open}
        onClose={handleClose}
        PaperProps={{
          component: 'form',
          onSubmit: (event: FormEvent<HTMLFormElement>) => {
            event.preventDefault();
            const formData = new FormData(event.currentTarget);
            const formJson = Object.fromEntries((formData as any).entries());
            submitAction(
              'newpassword',
              useInputPassword ? formJson.password : ''
            );
            handleClose();
          },
        }}
        maxWidth="xs"
      >
        <DialogTitle>Generate New Password</DialogTitle>
        <DialogContent>
          <DialogContentText>
            To use your own password, please enter the password here. We will
            make sure that this password is valid.
          </DialogContentText>
          <FormGroup>
            <FormControlLabel
              control={
                <Switch
                  checked={useInputPassword}
                  onClick={toggleUseInputPassword}
                />
              }
              label="Use your own password"
            />
          </FormGroup>
          {
            useInputPassword
              ? (
                <Stack direction="row">
                  <TextField
                    autoFocus
                    margin="dense"
                    id="password"
                    name="password"
                    label="New Password"
                    type={ inputVisible ? 'text' : 'password'}
                    fullWidth
                    variant="standard"
                  />
                  <IconButton
                    type="button"
                    sx={{ p: '10px' }}
                    aria-label="toggle-visibility"
                    onClick={toggleVisibility}
                  >
                    { inputVisible ? <VisibilityIcon /> : <VisibilityOffIcon />}
                  </IconButton>
                </Stack>
              )
              : ''
          }
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button type="submit" variant="contained">
            {
              useInputPassword
                ? 'Set Password'
                : 'Generate Password'
            }
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};