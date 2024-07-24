import { BooleanField, Datagrid, DateField, List, TextField, ReferenceField } from 'react-admin';

export const JobList = () => (
    <List>
        <Datagrid rowClick="show">
            <TextField source="id" />
            <TextField source="serviceId" />
            <DateField source="startDate" />
            <DateField source="nextRunTime" />
            <DateField source="frequency" />
            <BooleanField source="active" />
        </Datagrid>
    </List>
);