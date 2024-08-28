import {
  Datagrid,
  List,
  BooleanField,
  DateField,
  TextField,
  SearchInput,
} from 'react-admin';
import QuickFilter from '../Modules/QuickFilter';

const taskFilters = [
  <SearchInput source="q" alwaysOn
    // sx={{
    //   '&:focus': { width: 400 },
    //   '&:active': { width: 400 },
    //   '&:target': { width: 400 },
    // }}
  />,
  <QuickFilter source="active" label="Active Tasks" defaultValue={true} />,
];

export const TaskList = () => (
  <List filters={taskFilters} >
    <Datagrid rowClick="show">
      <TextField source="serviceName" />
      <DateField source="startTime" />
      <DateField source="endTime" />
      <TextField source="notes" />
      <BooleanField source="active" />
    </Datagrid>
  </List>
);