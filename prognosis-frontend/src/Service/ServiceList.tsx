import { Datagrid, List, TextField } from 'react-admin';

export const ServiceList = () => (
  <List>
    <Datagrid rowClick="show">
      <TextField source="id" />
      <TextField source="name" />
      <TextField source="serviceType" />
    </Datagrid>
  </List>
);