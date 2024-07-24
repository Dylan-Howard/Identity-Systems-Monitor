
import { Datagrid, List, TextField, ReferenceField, DateInput, minValue, maxValue, SelectInput, DateField } from 'react-admin';
import { calcMinMaxDateRanges } from './DateCalculator';

const sourceSystems = [
  // { id: 'admin', name: 'Admin' },
  { id: 'ri', name: 'Rapid Identity' },
  { id: 'sis', name: 'Infinite Campus' },
  { id: 'hr', name: 'Munis' }
];

const { minDate, maxDate } = calcMinMaxDateRanges(new Date())

const changesFilters = [
  
  <SelectInput source="sourceSystem" label="Source System" choices={sourceSystems} alwaysOn key='sourceInput' />,
  <DateInput
    source="minDate"
    label="Earliest Change Date"
    validate={minValue(minDate), maxValue(maxDate)}
    alwaysOn
    key='dateInput'
  />
  // <ReferenceInput source="userId" label="User" reference="users" />,
];

export const ChangeList = () => (
  <List filters={changesFilters}>
    <Datagrid rowClick="show" bulkActionButtons={false}>
      {/* <TextField source="sourcedId" label="Id" /> */}
      <ReferenceField source="idautoID" label="User" reference="users" link="show" />
      {/* <EmailField source="email" label="Username" /> */}
      <TextField source="currentStateId" label="New Value" />
      <TextField source="formerStateId" label="Former Value" />
      {/* <DateField source="startDate" label="Start Date" showTime /> */}
      <DateField source="endDate" label="Change Date" showTime />
    </Datagrid>
  </List>
);
