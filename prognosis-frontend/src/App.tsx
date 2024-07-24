import { Admin, Resource, ListGuesser, ShowGuesser } from 'react-admin';
import { authProvider } from './authProvider';
import { dataProvider } from './dataProvider';
// import { UserList } from './User/UserList';
// import { UserShow } from './User/UserShow';
// import { RenameList } from './Rename/RenameList';
import { Dashboard } from './Dashboard';
import { ServiceList } from './Service/ServiceList';
import { ProfileList } from './Profile/ProfileList';
import { ProfileShow } from './Profile/ProfileShow';
// import { ChangeList } from './Change/ChangeList';

import CloudIcon from '@mui/icons-material/Cloud';
// import NumbersIcon from '@mui/icons-material/Numbers';
import UserIcon from '@mui/icons-material/Group';
// import SupportAgentIcon from '@mui/icons-material/SupportAgent';

import { iamTheme } from './Theme'
import { proDataProvider } from './Data/proDataProvider';

export const App = () => (
  <Admin
    authProvider={authProvider}
    // @ts-ignore 
    dataProvider={proDataProvider}
    dashboard={Dashboard}
    defaultTheme='light'
    theme={iamTheme}
  >
    <Resource
      name="services"
      list={ServiceList}
      show={ShowGuesser}
      icon={CloudIcon}
    />
    <Resource
      name="profiles"
      list={ProfileList}
      show={ProfileShow}
      icon={UserIcon}
    />
    {/* <Resource
      name="users"
      list={UserList}
      show={UserShow}
      recordRepresentation="mail"
      icon={UserIcon}
    />
    <Resource
      name="changes"
      list={ChangeList}
      icon={ChangeCircleIcon}
    />
    <Resource
      name="renames"
      list={RenameList}
      icon={AbcIcon}
    /> */}
    {/* <Resource
      name="alerts"
      list={AlertList}
    /> */}
  </Admin>
);
