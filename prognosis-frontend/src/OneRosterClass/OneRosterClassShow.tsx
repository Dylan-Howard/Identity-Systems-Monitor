import {
  BooleanField,
  DateField,
  Labeled,
  NumberField,
  ReferenceField,
  ReferenceOneField,
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
  Link,
  Stack,
  Typography,
} from '@mui/material';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { ProfileSkeleton } from '../Skeleton/ProfileSkeleton';
import { ClassesCard } from '../Modules/ClassesCard';
import { Copyright } from '../Modules/Copyright';
import EnrollmentsCard from '../Modules/EnrollmentsCard';

export const OneRosterClassLayout = () => {
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
      <Container maxWidth="lg">
        <Box sx={{ mb: 2 }}>
          <ProBreadcrumbs parts={['classes', record.title.toString()]}/>
        </Box>
          {/* Organization */}
          <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mb: 2 }}>{record.title}</Typography>
          <Card sx={{ p: 2, mb: 2 }}>
            <Grid container spacing={4} sx={{ mt: 0 }}>
              <Grid item sm={6}>
                <Labeled label="Prognosis Id" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="id" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Status" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <BooleanField source="status" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Date Last Modified" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <DateField source="dateLastModified" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Title" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="title" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Identifier" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="identifier" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
              </Grid>
              <Grid item sm={6}>
                <Labeled label="Enrollments" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <NumberField source="enrollmentCount" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Class Type" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="classType" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Class Code" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="classCode" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Location" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="location sx={{ fontSize: '1.1rem' }}" />
                </Labeled>
                <Labeled label="Organization" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <Link href={`#/organizations/${record.school}/show`}>
                    <TextField source="organization" sx={{ fontSize: '1.1rem' }} />
                  </Link>
                </Labeled>
              </Grid>
            </Grid>
          </Card>
          {
            record.enrollments && record.enrollments.length !== 0
              ? <Box component="section">
                  <Typography variant="h2" sx={{ fontSize: 32, fontWeight: 600, mb: 2 }}>Teachers</Typography>
                  <EnrollmentsCard enrollments={record.enrollments} role="teacher" />
                </Box>
              : ''
          }
          {
            record.enrollments && record.enrollments.length !== 0
              ? <Box component="section">
                  <Typography variant="h2" sx={{ fontSize: 32, fontWeight: 600, mb: 2 }}>Students</Typography>
                  <EnrollmentsCard enrollments={record.enrollments} role="student" />
                </Box>
              : ''
          }
          <Copyright />
        </Container>
    </SimpleShowLayout>
  </ShowBase>
  );
};

export const OneRosterClass = () => (
  <ShowBase>
    <OneRosterClassLayout />
  </ShowBase>
);