import { useState, FormEvent } from 'react';
import { useRecordContext, useShowContext } from 'react-admin';
import './UserShow.css';
import { fetchData } from '../DataFetcher/DataFetcher';
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
  Menu,
  MenuItem,
  Snackbar,
  Stack,
  Switch,
  TextField,
} from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';

const apiUrl = import.meta.env.VITE_JSON_SERVER_URL;

const FormDialog = ({ submitAction }: { submitAction: Function}) => {
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

export const UserShowActions = () => {
  const record = useRecordContext();
  const { isLoading } = useShowContext();
  const [ snackbar, setSnackbar] = useState({
    open: false,
    allowCopy: false,
    message: '',
    data: '',
  });
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const handleMenuClick = (event: any) => {
    setAnchorEl(event.currentTarget);
  };
  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleActionClick = (action: string, data?: string) => {
    let url = `${apiUrl}/users/${record.id}/${action}${data ? '/' + data : ''}`;

    fetchData(url)
      .then((response) => {
        const { json } = response;
        if (!json.success) {
          setSnackbar({
            open: true,
            allowCopy: false,
            message: `Error: ${json.message}`,
            data: '',
          });
          return;
        }
        if (action === 'newpassword') {
          setSnackbar({
            open: true,
            allowCopy: true,
            message: `Password set to: ${json.data.password}`,
            data: (
              "Hi there!\n\n"
              + `I've set this employee's password to ${json.data.password} . Could you verify that they are able to sign in and have access to everything that they need?\n\n`
              + "Additionally, could you please assist them in completing their challenge questions and recording these answers on paper to store in a secure location?\n\n"
              + "Thank you!\n\n"
              + "Dylan Howard"
            ),
          });
        }
        if (action === 'welcome') {
          setSnackbar({
            open: true,
            allowCopy: true,
            message: json.message,
            data: json.data.recipient,
          });
        }
        if (action === 'rename') {
          setSnackbar({
            open: true,
            allowCopy: false,
            message: json.message,
            data: json.data.username,
          });
        }
      })
      .catch((err) => {
        setSnackbar({
          open: true,
          allowCopy: false,
          message: `Error: ${err.message}`,
          data: '',
        })
      });
    handleMenuClose();
  };

  const handleSnackbarClose = () => {
    setSnackbar({
      open: false,
      allowCopy: false,
      message: '',
      data: '',
    })
  }

  const copySnackbarToClipboard = () => {
    navigator.clipboard.writeText(snackbar.data);
  }

  if (isLoading) { return null };
  return (
    <Stack direction="row" spacing={2} >
      <FormDialog
        submitAction={handleActionClick}
      />
      <Button
        id="basic-button"
        aria-controls={open ? 'basic-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={open ? 'true' : undefined}
        onClick={(e) => handleMenuClick(e)}
        color="primary"
        variant="contained"
        endIcon={<ExpandMoreIcon />}
      >
        Support Tools
      </Button>
      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleMenuClose}
        MenuListProps={{
          'aria-labelledby': 'basic-button',
        }}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <MenuItem onClick={() => handleActionClick('newpassword')}>Generate Password</MenuItem>
        <MenuItem onClick={() => handleActionClick('welcome')}>Send Welcome Email</MenuItem>
        <MenuItem onClick={() => handleActionClick('rename')}>Rename Now</MenuItem>
      </Menu>
      <Snackbar
        open={snackbar.open}
        autoHideDuration={3000}
        onClose={handleSnackbarClose}
        message={snackbar.message}
        action={
          snackbar.allowCopy
            ? <Button color="primary" size="small" onClick={copySnackbarToClipboard}>Copy</Button>
            : null
        }
      />
    </Stack>
  )
};