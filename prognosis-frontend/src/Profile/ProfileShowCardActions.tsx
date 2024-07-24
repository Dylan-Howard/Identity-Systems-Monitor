import { useShowContext } from 'react-admin';
import {
  Button,
  Stack,
} from "@mui/material";

export const ProfileShowCardActions = () => {
  const { isLoading } = useShowContext();

  if (isLoading) { return null };
  return (
    <Stack direction="row" spacing={2} >
      <Button
        id="basic-button"
        color="primary"
        variant="contained"
      >
        Force Sync
      </Button>
    </Stack>
  )
};