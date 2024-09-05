import { Datagrid, List, TextField } from 'react-admin';
import Container from '@mui/material/Container';
import { Copyright } from '../Modules/Copyright';

export const ServiceList = () => (
  <Container maxWidth="lg">
    <List>
      <Datagrid rowClick="show">
        <TextField source="name" />
        <TextField source="serviceType" />
      </Datagrid>
    </List>
    <Copyright />
  </Container>
);