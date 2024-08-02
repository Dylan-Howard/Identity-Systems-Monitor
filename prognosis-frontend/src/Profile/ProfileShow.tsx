import {
  ShowBase,
  useRecordContext,
  TextField,
  useShowContext,
  SimpleShowLayout,
  Labeled,
  BooleanField,
  DateField,
} from 'react-admin';

import {
  Box,
  Container,
  Grid,
  Card,
  Stack,
  Chip,
  Link,
  TableContainer,
  Table,
  TableBody,
  TableHead,
  TableRow,
  TableCell,
  Typography,
  Paper,
} from "@mui/material";
import { ProfileShowActions } from './ProfileShowActions';
import { ProfilePhoto } from '../Modules/ProfilePhoto';
import { ProfileSkeleton } from '../Skeleton/ProfileSkeleton';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { ProfileShowCardActions } from './ProfileShowCardActions';
import Class from '../Types/Class';

const ProfileShowCard = ({ title, data, columns, system }: { title: string, data: any[], columns: number, system: string }) => {

  const fields = [];
  for (let i = 0; i < columns; i += 1) {
    fields.push([]);
  }
  const keys = Object.keys(data);
  const values = Object.values(data);

  let colIndex = 0;
  for (let i = 0; i < keys.length; i += 1) {
    // @ts-ignore
    fields[colIndex].push([ keys[i], `${values[i]}` ]);
    colIndex = (colIndex + 1) % columns;
  }

  return (
    <Card sx={{ p: 2, mb: 2 }}>
      <Stack direction="row" justifyContent="space-between" spacing={2}>
        <Typography variant="h2" fontSize='1.2rem' fontWeight={600} gutterBottom>{title}</Typography>
        {
          system === 'Google'
            ? <ProfileShowCardActions system={system} />
            : ''
        }
      </Stack>
      
      <Grid container spacing={2}>
        {
          fields.map((col: string[][]) =>(
            <Grid item key={`col-${col[0][0]}`} sm={6}>
              {
                col.map((fld: string[]) => (
                  <Labeled key={`fld-${fld[0]}`} label={fld[0]} fullWidth sx={{ fontSize: '1.1rem' }}>
                    <Typography sx={{ fontSize: '1rem', }}>{fld[1] ? fld[1] : 'not defined'}</Typography>
                  </Labeled>
                ))
              }
            </Grid>
          ))
        }
      </Grid>
    </Card>
  )
}

const ProfileShowLayout = () => {
  const { isLoading } = useShowContext();
  const record = useRecordContext();

  if (isLoading) {
    return <ProfileSkeleton />;
  }

  if (!record) {
    console.log('No record yet...');
    return <ProfileSkeleton />;
  }

  const photoUrl = record.links ? record.links.map((lnk: any) => lnk.photoUrl).pop() : '';

  const isError = {
    unclaimed: !record.claimed,
    disabled: !record.status,
    locked: record.locked,
    noMfa: record.mfaMethod === 'None',
  };

  return (
    <ShowBase>
      <SimpleShowLayout className="UserShow">
        <Container maxWidth="xl">
          <Box sx={{ mb: 2 }}>
            <ProBreadcrumbs parts={['profiles', record.email.toString()]}/>
          </Box>
          

          {/* Profile */}
          <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mb: 2 }}>{`${record.firstName} ${record.lastName}`}</Typography>
          <Card sx={{ p: 2, mb: 2 }}>
            <Stack direction="row-reverse" spacing={2}>
              <ProfileShowActions />
            </Stack>
            <Grid container spacing={4} sx={{ mt: 0 }}>
              <Grid item sm={6}>
                <ProfilePhoto url={photoUrl} firstName={record.firstName} lastName={record.lastName} />
              </Grid>
              <Grid item sm={6}>
                <Labeled label="Id" fullWidth sx={{ fontSize: '1.1rem' }}>
                  <TextField source="id" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Username" fullWidth sx={{ fontSize: '1.1rem' }}>
                  <TextField source="email" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="First Name" fullWidth sx={{ fontSize: '1.1rem' }}>
                  <TextField source="firstName" emptyText="not defined" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Last Name" fullWidth>
                  <TextField source="lastName" emptyText="not defined" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Stack direction="row" spacing={2}>
                  <Labeled label="Status" fullWidth sx={{ fontSize: '1.1rem' }}>
                    <BooleanField source="status" emptyText="not defined" sx={{ fontSize: '1.1rem' }} />
                  </Labeled>
                  {
                    isError.disabled ? <Chip color="error" size="small" label="Warning" sx={{ fontSize: '1.1rem' }} /> : ''
                  }
                </Stack>
                <Stack direction="row" spacing={2}>
                  <Labeled label="Claim Flag" sx={{ fontSize: '1.1rem' }}>
                    <BooleanField source="claimed" sx={{ fontSize: '1.1rem' }} />
                  </Labeled>
                  {
                    isError.unclaimed ? <Chip color="error" size="small" label="Warning" /> : ''
                  }
                </Stack>
                <Stack direction="row" spacing={2}>
                  <Labeled label="Locked" sx={{ fontSize: '1.1rem' }}>
                    <BooleanField source="locked" sx={{ fontSize: '1.1rem' }} />
                  </Labeled>
                  {
                    isError.locked ? <Chip color="error" size="small" label="Warning" /> : ''
                  }
                </Stack>
                <Stack direction="row" spacing={2}>
                  <Labeled label="MFA Method" sx={{ fontSize: '1.1rem' }}>
                    <TextField source="mfaMethod" sx={{ fontSize: '1.1rem' }} />
                  </Labeled>
                  {
                    isError.noMfa ? <Chip color="error" size="small" label="Warning" /> : ''
                  }
                </Stack>
                {/* <Labeled label="Primary Location" fullWidth>
                  <FunctionField
                    source="idautoPersonLocCodes"
                    render={
                      (record: {
                        idautoPersonLocCodes: string | undefined,
                        idautoPersonSchoolCodes: string | undefined,
                        idautoPersonLocNames: string | undefined,
                        idautoPersonSchoolNames: string | undefined,
                      }) => (
                        `${record.idautoPersonLocCodes || record.idautoPersonSchoolCodes} - ${record.idautoPersonLocNames || record.idautoPersonSchoolNames}`
                      )
                    }
                  />
                </Labeled> */}
                <Labeled label="Pending Rename" fullWidth sx={{ fontSize: '1.1rem' }}>
                  <TextField source="idautoPersonRenameUsername" emptyText="not defined" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Rename Flagged Date" fullWidth sx={{ fontSize: '1.1rem' }}>
                  <DateField source="idautoPersonRenameFlagDate" emptyText="not defined" sx={{ fontSize: '1.1rem' }} / >
                </Labeled>
              </Grid>
            </Grid>
          </Card>

          {/* Linked Accounts */}
          {
            record.links && record.links.length !== 0
              ? record.links.map((lnk: any) => (
                <ProfileShowCard
                  key={lnk.serviceName}
                  title={`Linked Account: ${lnk.serviceName}`}
                  data={lnk.linkedAccount}
                  columns={2}
                  system={lnk.serviceName}
                />
              ))
              : ''
          }
          {
            record.classes && record.classes.length !== 0
              ? (
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
                      {record.classes.map((cls: Class) => (
                        <TableRow
                          key={cls.id}
                          sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                          <TableCell component="th" scope="row">
                            <Link href={`./#/classes/${cls.id}/show`}>
                              {cls.title}
                            </Link>
                          </TableCell>
                          <TableCell>{cls.status}</TableCell>
                          <TableCell>{cls.dateLastModified}</TableCell>
                          <TableCell>{cls.classType}</TableCell>
                          <TableCell>{cls.classCode}</TableCell>
                          <TableCell>
                            <Link href={`./#/organizations/${cls.school}/show`}>
                              {cls.school}
                            </Link>
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                </TableContainer>
              )
              : ''
          }
        </Container>
      </SimpleShowLayout>
    </ShowBase>
  )
}

export const ProfileShow = () => (
  <ShowBase>
    <ProfileShowLayout />
  </ShowBase>
);