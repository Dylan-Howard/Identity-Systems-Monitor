import {
  BooleanField,
  BooleanInput,
  Datagrid,
  DateField,
  EmailField,
  List,
  SearchInput,
  TextField
} from 'react-admin';
import QuickFilter from '../Modules/QuickFilter';

const profileFilters = [
  <SearchInput source="q" alwaysOn />,
  <QuickFilter source="status" label="Disabled Users" defaultValue={false} />,
  <QuickFilter source="claimed" label="Unclaimed Users" defaultValue={false} />,
  <QuickFilter source="locked" label="Locked Users" defaultValue={true} />,
  <QuickFilter source="mfaMethod" label="No MFA" defaultValue={'None'} />
];

export const ProfileList = () => (
  <List filters={profileFilters}>
    <Datagrid rowClick="show">
      <EmailField source="email" label="Email" />
      <TextField source="firstName" label="First" />
      <TextField source="lastName" label="Last" />
      <BooleanField source="status" label="Status" />
      <BooleanField source="claimed" label="Claimed" />
      <BooleanField source="locked" label="Locked" />
      <TextField source="mfaMethod" label="Mfa Method" />
    </Datagrid>
  </List>
);