import { combineDataProviders } from 'react-admin';
import { userDataProvider } from './User/userDataProvider';
import { changeDataProvider } from './Change/changeDataProvider';
import { proDataProvider } from './Data/proDataProvider';

export const dataProvider = combineDataProviders((resource: string) => {
  switch (resource) {
    case 'services':
      return proDataProvider;
    case 'profiles':
      return proDataProvider;
    case 'totals':
      return proDataProvider;
    case 'agents':
      return proDataProvider;
    case 'users':
      return userDataProvider;
    case 'changes':
      return changeDataProvider;
      case 'renames':
        return userDataProvider;
    default:
      throw new Error(`Unknown resource: ${resource}`);
  }
});
