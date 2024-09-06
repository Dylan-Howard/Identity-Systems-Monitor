import { useState } from 'react';
import { useRecordContext, useShowContext } from 'react-admin';
import { fetchData } from '../DataFetcher/DataFetcher';
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Menu,
  MenuItem,
  Snackbar,
  Stack,
  Typography,
} from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { FormDialog } from '../Modules/UserDialogue';

const apiUrl = import.meta.env.VITE_ACTION_SERVER_URL;

const ActionDialog = (
  {
    active,
    title,
    body,
    onCopy,
    onClose,
  } : {
    active: boolean,
    title: string,
    body: string,
    onCopy: any,
    onClose: any,
  }
) => {

  const isSecureContext = navigator.clipboard !== undefined;

  return (
    <Dialog
      open={active}
      onClose={onClose}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle id="alert-dialog-title">{title}</DialogTitle>
      <DialogContent id="alert-dialog-description">
        {
          body !== undefined
          ?
            body.split('\n').map((line) => (
              <Typography
                key={line.substring(0, 10)}
                component="p"
                variant="body1"
                gutterBottom
              >
                  { line }
              </Typography>
            ))
          : ''
        }
      </DialogContent>
      <DialogActions>
        { isSecureContext
            ? <Button variant="contained" onClick={onCopy} autoFocus>Copy</Button>
            : ''
        }
        <Button variant="outlined" onClick={onClose} autoFocus={!isSecureContext}>Okay</Button>
      </DialogActions>
    </Dialog>
  );
}

export const ProfileShowActions = () => {
  const record = useRecordContext();
  const { isLoading } = useShowContext();
  const [ snackbar, setSnackbar] = useState({
    open: false,
    allowCopy: false,
    message: '',
    data: '',
  });

  const [ dialog, setDialog ] = useState({ body: '', active: false });
  
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleMenuClick = (event: any) => {
    setAnchorEl(event.currentTarget);
  };
  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleActionClick = (action: string, data?: string) => {
    let url = `${apiUrl}/users/${record.email}/${action}${data ? '/' + data : ''}`;

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
              "Hi there!\n"
              + `I've set this user's password to ${json.data.password}\n`
              + `Could you verify that they are able to sign in and have access to everything that they need?\n`
              + "Additionally, could you please assist them in completing their challenge questions and recording these answers in a secure location?\n"
              + "They may change their password using <a href=\"https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb\">these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb) and update their security questions using <a href=\"https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e\" >these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e).\n"
              + "Thank you!\n"
              + "Dylan Howard"
            ),
          });

          setDialog({
            body: (
              "Hi there!\n"
              + `I've set this user's password to ${json.data.password}\n`
              + `Could you verify that they are able to sign in and have access to everything that they need?\n`
              + "Additionally, could you please assist them in completing their challenge questions and recording these answers in a secure location?\n"
              + "They may change their password using <a href=\"https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb\">these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb) and update their security questions using <a href=\"https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e\" >these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e).\n"
              + "Thank you!\n"
              + "Dylan Howard"
            ),
            active: false,
          });
        }
        if (action === 'newpassword-hr') {
          setSnackbar({
            open: true,
            allowCopy: true,
            message: `Password set to: ${json.data.password}`,
            data: (
              "Hi there!\n"
              + `I've set this user's password to ${json.data.password}\n`
              + "They may change their password using <a href=\"https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb\">these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb) and update their security questions using <a href=\"https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e\" >these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e).\n"
              + "Thank you!\n"
              + "Dylan Howard"
            ),
          });

          setDialog({
            body: (
              "Hi there!\n"
              + `I've set this user's password to ${json.data.password}\n`
              + "They may change their password using <a href=\"https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb\">these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/b014ef89-9b34-409c-9f25-56faa317f5bb) and update their security questions using <a href=\"https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e\" >these instructions</a> (https://warrencounty.incidentiq.com/kb/articles/13b5e7be-944b-466c-9456-a409e37d993e).\n"
              + "Thank you!\n"
              + "Dylan Howard"
            ),
            active: false,
          });
        }
        if (action === 'welcome') {
          setSnackbar({
            open: true,
            allowCopy: true,
            message: json.message,
            data: (
              "Hi there!\n"
              + `I've set this user's password to ${json.data.password}\n`
              + "I've triggered our system to resend your Welcome Email. You should be able to follow these instructions to claim your account and update their password. When you're able, could you please confirm that you're able to access your account?\n"
              + "Thank you!\n"
              + "Dylan Howard"
            ),
          });
          setDialog({
            body: (
              "Hi there!\n"
              + "I've triggered our system to resend your Welcome Email. You should be able to follow these instructions to claim your account and update their password. When you're able, could you please confirm that you're able to access your account?\n"
              + "Thank you!\n"
              + "Dylan Howard"
            ),
            active: false,
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

  const showSnackbarMessage = () => {
    setDialog({ ...dialog, active: true });
  }

  const handleDialogClose = () => setDialog({ body: '', active: false });

  const handleDialogCopy = () => {
    navigator.clipboard.writeText(snackbar.data);
  }

  if (isLoading) { return null };
  return (
    <Stack direction="row" spacing={2} >
      <FormDialog submitAction={handleActionClick} />
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
        <MenuItem onClick={() => handleActionClick('newpassword')}>Generate Password (HR)</MenuItem>
        <MenuItem onClick={() => handleActionClick('welcome')}>Send Welcome Email</MenuItem>
        <MenuItem onClick={() => handleActionClick('rename')}>Rename Now</MenuItem>
      </Menu>
      <ActionDialog
        active={dialog.active}
        title="Updated User Account"
        body={dialog.body}
        onCopy={handleDialogCopy}
        onClose={handleDialogClose}
      />
      <Snackbar
        open={snackbar.open}
        autoHideDuration={3000}
        onClose={handleSnackbarClose}
        message={snackbar.message}
        action={
          snackbar.allowCopy
            ? <Button color="primary" size="small" onClick={showSnackbarMessage}>Show Details</Button>
            : null
        }
      />
    </Stack>
  )
};