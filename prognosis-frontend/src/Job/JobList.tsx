import { BooleanField, Datagrid, DateField, List, TextField } from 'react-admin';

export const JobList = () => (
    <List>
        <Datagrid rowClick="show">
            <TextField source="id" />
            <TextField source="serviceId" />
            <DateField source="startDate" />
            <DateField source="nextRunTime" />
            <TextField source="frequency" />
            <BooleanField source="active" />
        </Datagrid>
    </List>
);