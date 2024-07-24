import { fetchUtils } from 'react-admin';

const actionsApiUrl = import.meta.env.VITE_ACTION_SERVER_URL;
const actionsAuthUsername = import.meta.env.VITE_ACTION_SERVER_AUTH_USERNAME;
const actionsAuthPassword = import.meta.env.VITE_ACTION_SERVER_AUTH_PASSWORD;

export const fetchData = async (url: string, options: fetchUtils.Options = {}) => {
  if (url.substring(0, actionsApiUrl.length) === actionsApiUrl) {
    const customHeaders = (options.headers ||
        new Headers({
            Accept: 'application/json',
        })) as Headers;
    const basicAuthString = btoa(
      `${import.meta.env.VITE_JSON_SERVER_USERNAME}:${import.meta.env.VITE_JSON_SERVER_PASSWORD}`
    );
      
    customHeaders.set('Authorization', `Basic ${basicAuthString}`);
    options.headers = customHeaders;
  }
  return fetchUtils.fetchJson(url, options);
}