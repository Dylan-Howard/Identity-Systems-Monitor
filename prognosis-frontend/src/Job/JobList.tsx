import {
  BooleanField,
  Datagrid,
  DateField,
  FunctionField,
  List,
  TextField,
} from 'react-admin';
import Container from '@mui/material/Container';
import Link from '@mui/material/Link';
import { Copyright } from '../Modules/Copyright';

export const JobList = () => (
  <Container maxWidth="lg">
    <List>
      <Datagrid rowClick="show">
        <FunctionField
          label="Service"
          source="serviceId"
          render={(record: any) => (
            <Link href={`#/services/${record.serviceId}/show`}>{record.serviceName}</Link>
          )}
        />
        <DateField source="startDate" />
        <DateField source="nextRunTime" />
        <TextField source="frequency" />
        <BooleanField source="active" />
      </Datagrid>
    </List>
    <Copyright />
  </Container>
);