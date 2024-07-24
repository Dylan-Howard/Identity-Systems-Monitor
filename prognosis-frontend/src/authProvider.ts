import { AuthProvider } from "react-admin";
import { devAgents } from "./Data/devData";

export const authProvider: AuthProvider = {
  // send username and password to the auth server and get back credentials
  login: ({ username, password } : { username: string, password: string }) => {
    if (!username || !password === undefined) {
      return Promise.reject();
    }

    const user = devAgents.find((agt) => agt.username === username && agt.password === password);

    if (!user) {
      return Promise.reject();
    }

    localStorage.setItem('username', username);
    return Promise.resolve()
  },
  // when the dataProvider returns an error, check if this is an authentication error
  checkError: (error: any) => {
    console.log(error);
    return Promise.resolve()
  },
  // when the user navigates, make sure that their credentials are still valid
  checkAuth: () => (
    localStorage.getItem('username') ? Promise.resolve() : Promise.reject()
  ),
  // remove local credentials and notify the auth server that the user logged out
  logout: () => {
    localStorage.removeItem('username');
    return Promise.resolve();
  },
  // get the user's profile
  getIdentity: () => (Promise.resolve({ id: ''})),
  // get the user permissions (optional)
  getPermissions: () => (Promise.resolve()),
};