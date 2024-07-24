import { Datagrid, List, TextField, EmailField, TextInput, SelectInput, BooleanField } from 'react-admin';

const userTypes = [
  // { id: 'admin', name: 'Admin' },
  { id: 'Staff', name: 'Employee' },
  { id: 'Sponsored', name: 'Sponsored' },
  { id: 'Student', name: 'Student' },
];

const userFilters = [
  <TextInput
    key="mail"
    source="mail"
    label="Email"
    sx={{//marginRight: '1em',
      '& .MuiFilledInput-input': {
          width: '400px',
      },
    }}
    alwaysOn
  />,
  <SelectInput
    key="userType"
    source="userType"
    choices={userTypes}
    sx={{//marginRight: '1em',
      '& .MuiFilledInput-input': {
          width: '8rem',
      },
    }}
  />,
  <SelectInput
    key="isActive"
    source="isActive"
    choices={[
      {id: 'true', name: 'Active'},
      {id: 'false', name: 'Inactive'}
    ]}
    sx={{//marginRight: '1em',
      '& .MuiFilledInput-input': {
          width: '8rem',
      },
    }}
  />,
  <TextInput
    key="idautoPersonHRID"
    source="idautoPersonHRID"
    label="EmployeeId"
    sx={{//marginRight: '1em',
      '& .MuiFilledInput-input': {
          width: '8rem',
      },
    }}
    alwaysOn
  />,
  <TextInput
    key="givenName"
    source="givenName"
    label="Given Name"
    sx={{//marginRight: '1em',
      '& .MuiFilledInput-input': {
          width: '7rem',
      },
    }}
  />,
  <TextInput
    key="familyName"
    source="familyName"
    label="Family Name"
    sx={{//marginRight: '1em',
      '& .MuiFilledInput-input': {
          width: '7rem',
      },
    }}
  />,
];

export const UserList = () => (
  <List filters={userFilters}>
    <Datagrid rowClick="show" bulkActionButtons={false}>
      <TextField source="identifier" />
      <TextField source="givenName" label="First Name"  />
      <TextField source="sn" label="Last Name" />
      <EmailField source="mail" label="Email" />
      <TextField source="employeeType" label="Type" />
      <BooleanField source="isActive" label="Active" />
      <BooleanField source="idautoPersonClaimFlag" label="Claimed" />
    </Datagrid>
  </List>
);
