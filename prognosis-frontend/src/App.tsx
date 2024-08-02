import { Admin, Resource, ListGuesser, ShowGuesser } from 'react-admin';
import { authProvider } from './authProvider';
import { dataProvider } from './dataProvider';
import { Dashboard } from './Dashboard';
import { ServiceList } from './Service/ServiceList';
import { ProfileList } from './Profile/ProfileList';
import { ProfileShow } from './Profile/ProfileShow';
import { OrganizationShow } from './Organization/OrganizationShow';

import CloudIcon from '@mui/icons-material/Cloud';
import UserIcon from '@mui/icons-material/Group';
import TaskIcon from '@mui/icons-material/Task';
import WorkIcon from '@mui/icons-material/Work';
import ClassIcon from '@mui/icons-material/Class';
import BusinessIcon from '@mui/icons-material/Business';
import SupportAgentIcon from '@mui/icons-material/SupportAgent';

import { iamTheme } from './Theme'
import { JobList } from './Job/JobList';
import { TaskList } from './Task/TaskList';
import { JobCreate } from './Job/JobCreate';
import { OneRosterClass } from './OneRosterClass/OneRosterClassShow';
import { OrganizationList } from './Organization/OrganizationList';

export const App = () => (
  <Admin
    authProvider={authProvider}
    // @ts-ignore 
    dataProvider={dataProvider}
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
    <Resource
      name="organizations"
      list={OrganizationList}
      show={OrganizationShow}
      icon={BusinessIcon}
    />
    <Resource
      name="classes"
      list={ListGuesser}
      show={OneRosterClass}
      icon={ClassIcon}
    />
    <Resource
      name="jobs"
      create={JobCreate}
      list={JobList}
      show={ShowGuesser}
      icon={WorkIcon}
    />
    <Resource
      name="tasks"
      list={TaskList}
      show={ShowGuesser}
      icon={TaskIcon}
    />
    <Resource
      name="agents"
      list={ListGuesser}
      show={ShowGuesser}
      icon={SupportAgentIcon}
    />
  </Admin>
);
