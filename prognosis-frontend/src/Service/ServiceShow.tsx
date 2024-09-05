import { FunctionField, Labeled, ShowBase, SimpleShowLayout, TextField, useRecordContext, useShowContext } from 'react-admin';
import Container from '@mui/material/Container';
import { Box, Typography, Card, Grid, Stack, IconButton } from '@mui/material';
import { ClassesCard } from '../Modules/ClassesCard';
import { Copyright } from '../Modules/Copyright';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { ProfileSkeleton } from '../Skeleton/ProfileSkeleton';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { useState } from 'react';

export const ServiceShowLayout = () => {
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

  const togglePasswordVisibility = () => setShowPassword(!showPassword);
  
  return (
  <ShowBase>
    <SimpleShowLayout>
      <Container maxWidth="lg">
        <Box sx={{ mb: 2 }}>
          <ProBreadcrumbs parts={['services', record.name.toString()]}/>
        </Box>
          {/* Organization */}
          <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mb: 2 }}>{record.name}</Typography>
          <Card sx={{ p: 2, mb: 2 }}>
            <Grid container spacing={4} sx={{ mt: 0 }}>
              <Grid item sm={6}>
                <Labeled label="Organization Id" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="id" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="Name" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="name" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="serviceType" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="serviceType" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
              </Grid>
              <Grid item sm={6}>
                <Labeled label="username" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="username" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="password" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <Stack direction="row" spacing={2}>
                    <FunctionField
                      source="password"
                      render={(record) => showPassword ? record.password : '• • • • • • • • • • • • • • • •'}
                      sx={{ fontSize: '1.1rem' }}
                      />
                    <IconButton
                      aria-label="show password"
                      size="small"
                      onClick={togglePasswordVisibility}
                      sx={{ p: 0 }}
                    >
                      { showPassword ? <VisibilityOffIcon /> : <VisibilityIcon /> }
                    </IconButton>
                  </Stack>
                </Labeled>
                <Labeled label="baseUrl" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="baseUrl" sx={{ fontSize: '1.1rem' }} />
                </Labeled>
                <Labeled label="tokenUrl" fullWidth sx={{ fontSize: '1.1rem' }} >
                  <TextField source="tokenUrl" sx={{ fontSize: '1.1rem' }} />
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

export const ServiceShow = () => (
  <ShowBase>
    <ServiceShowLayout />
  </ShowBase>
);