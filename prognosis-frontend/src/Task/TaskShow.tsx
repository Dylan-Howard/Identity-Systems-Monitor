import { BooleanField, DateField, FunctionField, Labeled, ReferenceField, ShowBase, SimpleShowLayout, TextField, useRecordContext, useShowContext } from 'react-admin';
import Container from '@mui/material/Container';
import { Box, Typography, Card, Grid, Stack, IconButton, Link } from '@mui/material';
import { ClassesCard } from '../Modules/ClassesCard';
import { Copyright } from '../Modules/Copyright';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { ProfileSkeleton } from '../Skeleton/ProfileSkeleton';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { useState } from 'react';

export const TaskShowLayout = () => {
  const { isLoading } = useShowContext();
  const record = useRecordContext();
  const [showPassword, setShowPassword] = useState(false);

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
          <ProBreadcrumbs parts={['tasks', record.id.toString()]}/>
        </Box>
          {/* Task */}
          <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mb: 2 }}>{record.id}</Typography>
          <Card sx={{ p: 2, mb: 2 }}>
            <Grid container spacing={4} sx={{ mt: 0 }}>
              <Grid item sm={6}>
                
                <BooleanField source="" />
                <Labeled label="Task Id" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="id" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Job Id" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <Link href={`./#/jobs/${record.jobId.toString()}/show`}>
                    <TextField source="jobId" sx={{ fontSize: '1.1rem' }} />
                  </Link>
                </Labeled>
                <Labeled label="active" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <BooleanField source="active" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
              </Grid>
              <Grid item sm={6}>
                <Labeled label="startTime" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <DateField source="startTime" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="endTime" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <DateField source="endTime" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="notes" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="notes" sx={{ fontSize: '1.1rem' }} />
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

export const TaskShow = () => (
  <ShowBase>
    <TaskShowLayout />
  </ShowBase>
);