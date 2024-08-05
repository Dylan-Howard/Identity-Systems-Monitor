import {
  BooleanField,
  Datagrid,
  DateField,
  FunctionField,
  List,
  SearchInput,
  TextField,
} from 'react-admin';
import QuickFilter from '../Modules/QuickFilter';

const organizationsFilters = [
  <SearchInput source="q" alwaysOn />,
  // <QuickFilter source="q" label="Elementary School" defaultValue="elementary" />,
  // <QuickFilter source="q" label="Middle School" defaultValue="middle" />,
  // <QuickFilter source="q" label="High School" defaultValue="high" />,
];

export const OrganizationList = () => (
  <List filters={organizationsFilters}>
    <Datagrid rowClick="show">
      <TextField source="name" />
      <BooleanField source="status" />
      <DateField source="dateLastModified" />
      <TextField source="type" />
      <FunctionField
        source="address"
        render={(record: any) => `${record.address} ${record.city}, ${record.state} ${record.zip}`}
      />
    </Datagrid>
  </List>
);