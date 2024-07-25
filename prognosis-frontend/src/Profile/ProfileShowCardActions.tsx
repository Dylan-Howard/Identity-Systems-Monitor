import { useRecordContext, useShowContext } from 'react-admin';
import {
  Box,
  Button,
  CircularProgress,
  Fade,
  Snackbar,
} from "@mui/material";
import { useState, useRef } from 'react';
import { fetchData } from '../DataFetcher/DataFetcher';

const apiUrl = import.meta.env.VITE_ACTION_SERVER_URL;

export const ProfileShowCardActions = ({ system } : {system: string}) => {
  const record = useRecordContext();
  const { isLoading } = useShowContext();
  const [ isActing, setIsActing ] = useState(false);
  const [ snackbar, setSnackbar] = useState({
    open: false,
    allowCopy: false,
    message: '',
    data: '',
  });

  const handleActionClick = () => {
    const targetSystem = system.replace(' ', '').toLowerCase()
    let url = `${apiUrl}/users/${record.email}/sync/${targetSystem}`;
    console.log(url);

    setIsActing(true);

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
          setIsActing(false);
          return;
        }

        setSnackbar({
          open: true,
          allowCopy: false,
          message: json.message,
          data: '',
        });
        setIsActing(false);
      })
      .catch((err) => {
        setSnackbar({
          open: true,
          allowCopy: false,
          message: `Error: ${err.message}`,
          data: '',
        })
        setIsActing(false);
      });
  };

  const handleSnackbarClose = () => {
    setSnackbar({
      open: false,
      allowCopy: false,
      message: '',
      data: '',
    })
  }

  if (isLoading) { return null };
  return (
    <Box>
      <Button
        id="basic-button"
        color="primary"
        variant="contained"
        onClick={() => handleActionClick()}
      >
        {

          isActing
            ? (
                <Fade
                  in={isActing}
                  style={{
                    transitionDelay: isActing ? '500ms' : '0ms',
                  }}
                  unmountOnExit
                >
                  <CircularProgress size={'1.6rem'} />
                </Fade>
            )
            : 'Sync Now'
        }
      </Button>
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
    </Box>
  )
};