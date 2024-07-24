import { fetchUtils } from 'react-admin';
import { stringify } from 'query-string';
import { getMinDate, getDateDifference } from './DateCalculator';

const apiUrl = import.meta.env.VITE_JSON_SERVER_URL;

const httpClient = (url: string, options: fetchUtils.Options = {}) => {
  const customHeaders = (options.headers ||
      new Headers({
          Accept: 'application/json',
      })) as Headers;
  
  const basicAuthString = btoa(
    `${import.meta.env.VITE_JSON_SERVER_USERNAME}:${import.meta.env.VITE_JSON_SERVER_PASSWORD}`
  );

  customHeaders.set('Authorization', `Basic ${basicAuthString}`);
  options.headers = customHeaders;


  return fetchUtils.fetchJson(url, options);
}

// Prepares filter by resource type
const prepLdapFilter = (resource: string, params: any) => {
  const filterFields = [];

  if (params.userType) {
    filterFields.push(`(employeeType=${params.userType})`);
  }
  if (params.idautoPersonHRID) {
    filterFields.push(`(idautoPersonHRID=${params.idautoPersonHRID})`);
  }
  if (params.givenName) {
    filterFields.push(`(givenName=${params.givenName})`);
  }

  if (filterFields.length) {
    filterFields.splice(0,0,'(&');
    filterFields.push(')');
  } else {
    filterFields.push('(idautoPersonHRID=*)');
  }
  
  return filterFields.join('');
}

export const changeDataProvider = {
  getList: async (resource, params) => {
    let { sourceSystem, minDate, attribute } = params.filter;
    // console.log(params.filter);
    const dateRange = minDate ? getDateDifference(minDate) : 90;
    
    if (!sourceSystem) {
      sourceSystem = 'sis'
    }
    if (!attribute) {
      attribute = 'id';
    }
    
    const url = `${apiUrl}/${resource}/${sourceSystem}/${attribute}?dayRange=${dateRange}`;
    
    const { json } = await httpClient(url);
    const { data, range } = json;

    console.log(data);

    return {
      data,
      total: range,
    };
  },

  getOne: async (resource, params) => {
    const url = `${apiUrl}/${resource}/${params.id}`;

    const { json } = await httpClient(url);
    const { data } = json;

    return { data };
  },

  getMany: async (resource, params) => {
        const query = {
            filter: JSON.stringify({ ids: params.ids }),
        };
        const url = `${apiUrl}/${resource}?${stringify(query)}`;
        const { json } = await httpClient(url);
        return { data: json };
  },

  
  getManyReference: async (resource, params) => {
    // const { page, perPage } = params.pagination;
    // const { field, order } = params.sort;
    // const query = {
    //   sort: JSON.stringify([field, order]),
    //   range: JSON.stringify([(page - 1) * perPage, page * perPage - 1]),
    //   filter: JSON.stringify({
    //      ...params.filter,
    //      [params.target]: params.id,
    //   }),
    // };
    // const url = `${apiUrl}/${resource}?${stringify(query)}`;
    // const { json, headers } = await fetchData(url);
    // return {
    //   data: json,
    //   total: parseInt(headers.get('content-range').split('/').pop(), 10),
    // };
    return {
      data: [],
      total: 0,
    };
  },

  create: async (resource, params) => {
    // const { json } = await fetchData(`${apiUrl}/${resource}`, {
    //   method: 'POST',
    //   body: JSON.stringify(params.data),
    // })
    return { data: [] };
  },

  update: async (resource, params) => {
    return { data: [] };
  },

  updateMany: async (resource, params) => {
    // const query = {
    //   filter: JSON.stringify({ id: params.ids}),
    // };
    // const url = `${apiUrl}/${resource}?${stringify(query)}`;
    // const { json } = await fetchData(url, {
    //   method: 'PUT',
    //   body: JSON.stringify(params.data),
    // })
    return { data: [] };
  },

  delete: async (resource, params) => {
    // const url = `${apiUrl}/${resource}/${params.id}`;
    // const { json } = await fetchData(url, {
    //   method: 'DELETE',
    // });
    return { data: [] };
  },

  deleteMany: async (resource, params) => {
    // const query = {
    //   filter: JSON.stringify({ id: params.ids}),
    // };
    // const url = `${apiUrl}/${resource}?${stringify(query)}`;
    // const { json } = await fetchData(url, {
    //     method: 'DELETE',
    //     body: JSON.stringify(params.data),
    // });
    return { data: [] };
  },
};
