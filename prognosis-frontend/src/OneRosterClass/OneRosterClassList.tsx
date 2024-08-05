import { BooleanField, Datagrid, DateField, FunctionField, List, NumberField, TextField } from 'react-admin';
import OneRosterClass from '../Types/OneRosterClass';
import Link from '@mui/material/Link';

export const OneRosterClassList = () => (
  <List>
    <Datagrid rowClick="show">
      <TextField source="title" />
      <TextField source="identifier" />
      <BooleanField source="status" />
      <DateField source="dateLastModified" />
      <TextField source="classType" />
      <TextField source="classCode" />
      <TextField source="location" />
      <FunctionField
        label="Organization"
        source="orgSourcedId"
        render={(record: OneRosterClass) => (
          <Link href={`#/organizations/${record.school}/show`}>{record.organization}</Link>
        )}
      />
      <NumberField source="enrollmentCount" />
    </Datagrid>
  </List>
);