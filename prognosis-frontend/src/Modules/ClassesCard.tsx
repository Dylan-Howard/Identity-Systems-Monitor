
import OneRosterClass from '../Types/OneRosterClass';

import {
  Link,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material';

export const ClassesCard = ({ classes } : { classes: OneRosterClass[] }) => (
  <TableContainer component={Paper}>
    <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
      <TableHead>
        <TableRow>
          <TableCell>Title</TableCell>
          <TableCell>Status</TableCell>
          <TableCell>Date Last Modified</TableCell>
          <TableCell>Class Type</TableCell>
          <TableCell>Class Code</TableCell>
          <TableCell>School</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {classes.map(({ id, title, status, dateLastModified, classType, classCode, school }) => (
          <TableRow
            key={id}
            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
          >
            <TableCell component="th" scope="row">
              <Link href={`./#/classes/${id}/show`}>
                {title}
              </Link>
            </TableCell>
            <TableCell>{status}</TableCell>
            <TableCell>{dateLastModified}</TableCell>
            <TableCell>{classType}</TableCell>
            <TableCell>{classCode}</TableCell>
            <TableCell>
              <Link href={`./#/organizations/${school}/show`}>
                {school}
              </Link>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  </TableContainer>
);