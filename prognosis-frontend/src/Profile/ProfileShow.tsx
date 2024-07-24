import {
  ShowBase,
  useRecordContext,
  TextField,
  useShowContext,
  SimpleShowLayout,
  Labeled,
  BooleanField,
  DateField,
  FunctionField,
} from 'react-admin';

import {
  Box,
  Container,
  Grid,
  Card,
  Stack,
  Chip,
  Typography,
} from "@mui/material";
// import WarningIcon from '@mui/icons-material/Warning';
import { ProfileShowActions } from './ProfileShowActions';
import { ProfilePhoto } from '../Modules/ProfilePhoto';
import { ProfileSkeleton } from '../Skeleton/ProfileSkeleton';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { ProfileShowCardActions } from './ProfileShowCardActions';

const ProfileShowCard = ({ title, data, columns }: { title: string, data: any[], columns: number }) => {

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
        <Typography variant="h2" fontSize={18} fontWeight={600} gutterBottom>{title}</Typography>
        <ProfileShowCardActions />
      </Stack>
      
      <Grid container spacing={2}>
        {
          fields.map((col: string[][]) =>(
            <Grid item key={`col-${col[0][0]}`} sm={6}>
              {
                col.map((fld: string[]) => (
                  <Labeled key={`fld-${fld[0]}`} label={fld[0]} fullWidth>
                    <Typography sx={{ fontSize: '0.875rem', }}>{fld[1] ? fld[1] : 'not defined'}</Typography>
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
    claimFlag: !record.status,
    isActive: !record.status,
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
                <Labeled label="Id" fullWidth>
                  <TextField source="id" />
                </Labeled>
                <Labeled label="Username" fullWidth>
                  <TextField source="email" />
                </Labeled>
                <Labeled label="First Name" fullWidth>
                  <TextField source="firstName" emptyText="not defined" />
                </Labeled>
                <Labeled label="Last Name" fullWidth>
                  <TextField source="lastName" emptyText="not defined" />
                </Labeled>
                <Stack direction="row" spacing={2}>
                  <Labeled label="Status" fullWidth>
                    <BooleanField source="status" emptyText="not defined" />
                  </Labeled>
                  {
                    isError.isActive ? <Chip color="error" size="small" label="Warning" /> : ''
                  }
                </Stack>
                <Stack direction="row" spacing={2}>
                  <Labeled label="Claim Flag">
                    <BooleanField source="idautoPersonClaimFlag" emptyText="not defined" />
                  </Labeled>
                  {
                    isError.claimFlag ? <Chip color="error" size="small" label="Warning" /> : ''
                  }
                </Stack>
                <Labeled label="Primary Location" fullWidth>
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
                </Labeled>
                <Labeled label="Pending Rename" fullWidth>
                  <TextField source="idautoPersonRenameUsername" emptyText="not defined" />
                </Labeled>
                <Labeled label="Rename Flagged Date" fullWidth>
                  <DateField source="idautoPersonRenameFlagDate" emptyText="not defined" / >
                </Labeled>
              </Grid>
            </Grid>
          </Card>

          {/* Linked Accounts */}
          {
            record.links && record.links.length !== 0
              ? record.links.map((lnk: any) => (
                <ProfileShowCard key={lnk.serviceName} title={`Linked Account: ${lnk.serviceName}`} data={lnk.linkedAccount} columns={2} />
              ))
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