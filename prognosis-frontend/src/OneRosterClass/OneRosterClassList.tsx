import {
  BooleanField,
  Datagrid,
  DateField,
  FunctionField,
  List,
  NumberField,
  SearchInput,
  TextField
} from 'react-admin';
import Container from '@mui/material/Container'
import OneRosterClass from '../Types/OneRosterClass';
import Link from '@mui/material/Link';
import { Copyright } from '../Modules/Copyright';

const classFilters = [
  <SearchInput source="q" alwaysOn />,
];

export const OneRosterClassList = () => (
  <Container maxWidth="lg">
    <List filters={classFilters}>
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
    <Copyright />
  </Container>
);