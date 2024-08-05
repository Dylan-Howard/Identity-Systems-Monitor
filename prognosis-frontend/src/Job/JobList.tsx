import {
  BooleanField,
  Datagrid,
  DateField,
  FunctionField,
  List,
  TextField,
} from 'react-admin';
import Link from '@mui/material/Link';

export const JobList = () => (
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
);