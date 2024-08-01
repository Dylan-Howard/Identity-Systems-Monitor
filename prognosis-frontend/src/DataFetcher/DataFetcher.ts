import { fetchUtils } from 'react-admin';

const actionsApiUrl = import.meta.env.VITE_ACTION_SERVER_URL;

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

export const postData = async (url: string, data: Object, options: fetchUtils.Options = {}) => {
  console.log(data);
  
  return fetchUtils.fetchJson(
    url,
    {
      method: 'POST',
      body: JSON.stringify(data),
      ...options,
    }
  );
}