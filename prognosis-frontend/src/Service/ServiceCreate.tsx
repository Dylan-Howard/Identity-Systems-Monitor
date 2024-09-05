import {
  Create,
  SimpleForm,
  BooleanInput,
  TextInput,
  DateInput,
  required,
  NumberInput,
  SelectInput,
} from 'react-admin';
import { Box, Container, Typography } from '@mui/material';
import { ProBreadcrumbs } from '../Modules/ProBreadcrumbs';
import { Copyright } from '../Modules/Copyright';

const typeChoices = [
  { id: 'oneroster', name: 'One Roster' },
  { id: 'ri_api', name: 'Rapid Identity' },
  { id: 'rest_api', name: 'Rest API' },
  { id: 'csv', name: 'CSV Import' },
];

export const ServiceCreate = () => (
  <Container maxWidth="xl" sx={{ mt: 2 }}>
    <Box sx={{ mb: 2 }}>
      <ProBreadcrumbs parts={['services', 'New Service Connection']}/>
    </Box>
    <Create>
      <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mt: 2, ml: 2 }}>New Service Connection</Typography>
      <SimpleForm>
        <TextInput source="name" validate={[required()]} />
        <SelectInput source="serviceType" choices={typeChoices} validate={[required()]} />
        <TextInput source="username" validate={[required()]} />
        <TextInput source="password" type="password" validate={[required()]} />
        <TextInput source="baseUrl" type="url" validate={[required()]} />
        <TextInput source="tokenUrl" />
      </SimpleForm>
    </Create>
    <Copyright />
  </Container>
);

export default ServiceCreate;