
import { ClassEnrollment } from '../Types/OneRosterClass';

import {
  Link,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material';

export const EnrollmentsCard = ({ enrollments, role } : { enrollments: ClassEnrollment[], role: string }) => {

  const listItems = enrollments.filter((erl) => erl.role == role);

  if (listItems.length === 0) {
    return <Typography variant="body1" sx={{ mb: 2 }}>No {role} enrollments found</Typography>
  }

  return (
    <TableContainer component={Paper} sx={{ mb: 2 }}>
      <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
        <TableHead>
          <TableRow>
            <TableCell>Username</TableCell>
            <TableCell>Role</TableCell>
            <TableCell>Start Date</TableCell>
            <TableCell>End Date</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {listItems.map(({ userSourcedId, username, role, primary, beginDate, endDate }) => (
            <TableRow
              key={userSourcedId}
              sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
            >
              <TableCell component="th" scope="row">
                <Link href={`./#/profiles/${userSourcedId}/show`}>
                  {username}
                </Link>
              </TableCell>
              <TableCell>
                { `${primary ? 'Primary ' : ''}${role}` }
              </TableCell>
              <TableCell>{beginDate?.toLocaleDateString() || 'None'}</TableCell>
              <TableCell>{endDate?.toLocaleDateString() || 'None'}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

export default EnrollmentsCard;