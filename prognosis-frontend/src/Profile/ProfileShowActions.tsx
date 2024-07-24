import { useState } from 'react';
import { useRecordContext, useShowContext } from 'react-admin';
// import './UserShow.css';
import { fetchData } from '../DataFetcher/DataFetcher';
import {
  Button,
  Menu,
  MenuItem,
  Snackbar,
  Stack,
} from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { FormDialog } from '../Modules/UserDialogue';

const apiUrl = import.meta.env.VITE_ACTION_SERVER_URL;

export const ProfileShowActions = () => {
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
              "Hi there!\n\n"
              + `I've set this employee's password to ${json.data.password}\n\n`
              + `Could you verify that they are able to sign in and have access to everything that they need?\n\n`
              + "Additionally, could you please assist them in completing their challenge questions and recording these answers in a secure location?\n\n"
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