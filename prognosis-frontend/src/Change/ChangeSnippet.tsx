import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

export const ChangeSnippet = (
  { data }: { data: { user: string, currentId: string, formerId: string, timestamp: string }[] }
) => {
  return (
    <Paper sx={{ p: 2 }}>
      <Typography>Recent StateId Changes</Typography>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Username</TableCell>
            <TableCell>Date Modified</TableCell>
            <TableCell>Current Id</TableCell>
            <TableCell>Former Id</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {
            data.map(({ user, currentId, formerId, timestamp}) => (
              <TableRow key={user}>
                <TableCell>{user}</TableCell>
                <TableCell align="right">{timestamp}</TableCell>
                <TableCell>
                  <Link color="primary" href={`#/users/${user}/show`}>
                    {currentId}
                  </Link>
                </TableCell>
                <TableCell>
                  {formerId}
                </TableCell>
              </TableRow>
            ))
          }
        </TableBody>
      </Table>
      <Link color="primary" href="#/changes" sx={{ mt: 3 }}>
        See more changes
      </Link>
    </Paper>
  )
};