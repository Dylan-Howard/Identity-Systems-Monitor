import {
  Datagrid,
  List,
  BooleanField,
  DateField,
  TextField,
} from 'react-admin';

export const TaskList = () => (
  <List>
    <Datagrid rowClick="show">
      <TextField source="id" />
      <TextField source="jobId" />
      <DateField source="jobId" />
      <DateField source="jobId" />
      <TextField source="jobId" />
      <BooleanField source="active" />
    </Datagrid>
  </List>
);