import {
  BooleanField,
  DateField,
  Labeled,
  Show,
  ShowBase,
  SimpleShowLayout,
  TextField,
  useRecordContext,
  useShowContext,
} from 'react-admin';
import {
  Box,
  Card,
  Chip,
  Container,
  Grid,
  Stack,
  Typography,
} from '@mui/material';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { ProfileSkeleton } from '../Skeleton/ProfileSkeleton';
import { ClassesCard } from '../Modules/ClassesCard';
import { Copyright } from '../Modules/Copyright';

export const OrganizationShowLayout = () => {
  const { isLoading } = useShowContext();
  const record = useRecordContext();

  if (isLoading) {
    return <ProfileSkeleton />;
  }

  if (!record) {
    console.log('No record yet...');
    return <ProfileSkeleton />;
  }

  const isError = {
    noClasses: false,
  }
  
  return (
  <ShowBase>
    <SimpleShowLayout>
      <Container maxWidth="xl">
        <Box sx={{ mb: 2 }}>
          <ProBreadcrumbs parts={['organizations', record.id.toString()]}/>
        </Box>
          {/* Organization */}
          <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mb: 2 }}>{record.name}</Typography>
          <Card sx={{ p: 2, mb: 2 }}>
            <Grid container spacing={4} sx={{ mt: 0 }}>
              <Grid item sm={6}>
                <Labeled label="Prognosis Id" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="id" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Date Last Modified" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <DateField source="dateLastModified" />
                </Labeled>
                <Labeled label="Name" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="name" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Identifier" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="identifier" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Type" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="type" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
              </Grid>
              <Grid item sm={6}>
                <Labeled label="Address" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="address" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="City" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="city" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="State" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="state" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Zip" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="zip" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
              </Grid>
              {/* <Stack direction="row" spacing={2}>
                  <Labeled label="Status" fullWidth sx={{ fontSize: '1.1rem' }} >
                    <BooleanField source="status" emptyText="not defined" />
                  </Labeled>
                  {
                    // isError.disabled ? <Chip color="error" size="small" label="Warning" /> : ''
                  }
                </Stack> */}
            </Grid>
          </Card>
          {
            record.classes && record.classes.length !== 0
              ? <ClassesCard classes={record.classes} />
              : ''
          }
          <Copyright />
        </Container>
    </SimpleShowLayout>
  </ShowBase>
  );
};

export const OrganizationShow = () => (
  <ShowBase>
    <OrganizationShowLayout />
  </ShowBase>
);