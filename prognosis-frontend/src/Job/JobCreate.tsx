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

const serviceChoices = [
  { id: '1a85c3f5-5392-476e-ac43-2c7d2422dee0', name: 'Microsoft Active Directory' },
  { id: '0ad37c88-6dea-43a5-88b4-32e802c23ee4', name: 'Microsoft Entra Id' },
  { id: 'cdaa055c-2c3d-490e-b145-72f804d95f6e', name: 'Rapid Idenity' },
  { id: '8cd563c4-b1f8-4c00-9990-8a3c9a190944', name: 'Google Workspace' },
]

export const JobCreate = () => (
  <Container maxWidth="xl" sx={{ mt: 2 }}>
    <Box sx={{ mb: 2 }}>
      <ProBreadcrumbs parts={['jobs', 'New Scheduled Job']}/>
    </Box>
    <Create>
      <Typography variant="h1" fontSize={48} fontWeight={600} sx={{ mt: 2, ml: 2 }}>New Scheduled Job</Typography>
      <SimpleForm>
        <SelectInput source="serviceId" choices={serviceChoices} validate={[required()]} />
        <DateInput source="startDate" defaultValue={new Date()} parse={(dat) => dat.toJSON()} />
        <DateInput source="nextRuntime" defaultValue={new Date()} parse={(dat) => dat.toJSON()} />
        <TextInput source="frequency" defaultValue={'60'} />
        <BooleanInput source="active"></BooleanInput>
      </SimpleForm>
    </Create>
  </Container>
);
// {
//   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//   "serviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//   "startDate": "2024-07-24T19:11:08.216Z",
//   "nextRunTime": "2024-07-24T19:11:08.216Z",
//   "frequency": "string",
//   "active": true
// }