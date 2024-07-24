
import {
  Datagrid,
  List,
  BooleanField,
  DateField,
  FunctionField,
  TextField,
  useListContext,
  useUnselect,
} from 'react-admin';

import { fetchData } from '../DataFetcher/DataFetcher';

import { Button, Snackbar } from '@mui/material';
import AbcIcon from '@mui/icons-material/Abc';
import { useState } from 'react';

const apiUrl = import.meta.env.VITE_JSON_SERVER_URL;

const EmptyPage = () => (
  <div>
    <span>Empty</span>
  </div>
)

const RenameBulkActionButtons = ({ setSnackbar, renameAction } : { setSnackbar: Function, renameAction: Function }) => {
  const { selectedIds } = useListContext();

  const handleClick = () => {

    for (let i = 0; i < selectedIds.length; i += 1) {
      renameAction(selectedIds[i]);
    }
    setSnackbar({
      open: true,
      message: 'Renaming selected users',
    });
  }

  return (
    <>
      <Button
        color="primary"
        variant="text"
        onClick={handleClick}
        startIcon={<AbcIcon />}
      >
        Rename All
      </Button>
    </>
  )
};


export const RenameList = () => {
  const unselect = useUnselect('renames');
  const [ snackbar, setSnackbar] = useState({
    open: false,
    message: '',
  });

  const handleSnackbarClose = () => {
    setSnackbar({
      open: false,
      message: '',
    })
  }

  const handleRename = (id: string) => {
    const url = `${apiUrl}/users/${id}/rename`;
    console.log(url);

    fetchData(url)
      .then((response) => {
        if (!response.json.success) {
          throw Error(response.json.message);
        }
        unselect([ id ]);
      })
      .catch((err) => {
        console.log(err.message);
      });
  }

  const handleRenameButtonClick = (id: string, targetName: string) => {
    handleRename(id);
    setSnackbar({
      open: true,
      message: `Renaming ${targetName}`,
    });
  }

  return (
    <>
      <List empty={<EmptyPage />} perPage={25}>
        <Datagrid
          rowClick="toggleSelection"
          bulkActionButtons={
            <RenameBulkActionButtons setSnackbar={setSnackbar} renameAction={handleRename} />
          }
        >
          <TextField source="identifier" sortable={false} />
          <TextField source="mail" label="Current Username" sortable={false} />
          <TextField source="idautoPersonRenameUsername" label="Pending Username" sortable={false} />
          <DateField source="idautoPersonRenameFlagDate" label="Rename Date" sortable={false} />
          <TextField source="employeeType" label="Type" sortable={false} />
          <BooleanField source="isActive" label="Active" sortable={false} />
          <FunctionField
            source="identifier"
            label=""
            render={(record: any) => (
              <Button
                onClick={() => handleRenameButtonClick(record.id, record.mail)}
                variant="outlined"
              >
                Rename Now
              </Button>
            )}
            sortable={false}
          />
        </Datagrid>
      </List>
      <Snackbar
        open={snackbar.open}
        autoHideDuration={3000}
        onClose={handleSnackbarClose}
        message={snackbar.message}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
      />
    </>
  );
};
