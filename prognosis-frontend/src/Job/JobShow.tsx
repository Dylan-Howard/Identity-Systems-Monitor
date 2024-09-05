import {
  BooleanField,
  DateField,
  Labeled,
  ReferenceField,
  ShowBase,
  SimpleShowLayout,
  TextField,
  useRecordContext,
  useShowContext,
} from 'react-admin';
import {
  Box,
  Card,
  Container,
  Grid,
  Typography,
} from '@mui/material';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { ProfileSkeleton } from '../Skeleton/ProfileSkeleton';
import { Copyright } from '../Modules/Copyright';

export const JobShowLayout = () => {
  const { isLoading } = useShowContext();
  const record = useRecordContext();

  if (isLoading) {
    return <ProfileSkeleton />;
  }

  if (!record) {
    console.log('No record yet...');
    return <ProfileSkeleton />;
  }
  
  return (
  <ShowBase>
    <SimpleShowLayout>
      <Container maxWidth="lg">
        <Box sx={{ mb: 2 }}>
          <ProBreadcrumbs parts={['jobs', record.id.toString()]}/>
        </Box>
          {/* Organization */}
          <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mb: 2 }}>{record.id}</Typography>
          <Card sx={{ p: 2, mb: 2 }}>
            <Grid container spacing={4} sx={{ mt: 0 }}>
              <Grid item sm={6}>
                <Labeled label="Prognosis Id" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="id" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="active" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <BooleanField source="active" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
              </Grid>
              <Grid item sm={6}>
                <Labeled label="startDate" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <DateField source="startDate" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="nextRunTime" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <DateField source="nextRunTime" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="frequency" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="frequency" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
              </Grid>
            </Grid>
          </Card>
          <Copyright />
        </Container>
    </SimpleShowLayout>
  </ShowBase>
  );
};

export const JobShow = () => (
  <ShowBase>
    <JobShowLayout />
  </ShowBase>
);