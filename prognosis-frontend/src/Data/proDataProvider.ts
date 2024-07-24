
import { stringify } from 'query-string';
import { fetchData } from '../DataFetcher/DataFetcher';
// import { devAgents, devProfiles, devServices, devTotals } from './devData';

const apiUrl = import.meta.env.VITE_JSON_SERVER_URL;

export const proDataProvider = {
  getList: async (
    resource: string,
    params: {
      filter: {
        q?: string,
        status?: string,
        locked?: string,
        claimed?: string,
        mfaMethod?: string,
      },
      pagination: {
        page: number,
        perPage: number
      },
      sort: {
        field: string,
        order: string
      }
    }
  ) => {
    const { filter } = params;
    const { page, perPage } = params.pagination;
    const { field, order } = params.sort;

    let fetchFilter = '*';
    if (filter.q) {
      fetchFilter = `q=${filter.q}`;
    }
    if (filter.status !== undefined) {
      fetchFilter = `status=${filter.status}`;
    }
    if (filter.locked !== undefined) {
      fetchFilter = `locked=${filter.locked}`;
    }
    if (filter.claimed !== undefined) {
      fetchFilter = `claimed=${filter.claimed}`;
    }
    if (filter.mfaMethod) {
      fetchFilter = `mfaMethod=${filter.mfaMethod}`;
    }

    const query = {
      sort: field,
      order: order,
      startIndex: JSON.stringify((page - 1) * perPage),
      endIndex: JSON.stringify(page * perPage - 1),
    };
    const url = `${apiUrl}/${resource}?${stringify(query)}&${fetchFilter}`;

    console.log(params);
    console.log(url);
    
    const { json } = await fetchData(url);

    return {
      data: json.data,
      total: json.total,
    };
  },
  getOne: async (resource: string, params: any) => {
    const url = `${apiUrl}/${resource}/${params.id}`;
    const { json } = await fetchData(url);
    return {
      data: json,
    };
  },

  getMany: async (resource: string, params: any) => {
    const query = { filter: params };

    return {
      data: [],
      total: 0,
    };
  },

  getManyReference: async (resource: string, params: any) => {
    console.log(`${resource}, ${params}`);
    return {
      data: [],
      total: 0,
    };
  },

  create: async (resource: string, params: string) => {
    console.log(`${resource}, ${params}`);
    return { data: [] };
  },

  update: async (resource: string, params: { id: string }) => {
    console.log(`${resource}, ${params}`);
    return { data: new Object() };
  },

  updateMany: async (resource: string, params: { ids: string[] }) => {
    console.log(`${resource}, ${params}`);
    return { data: [{}] };
  },

  delete: async (resource: string, params: { id: string }) => {
    console.log(`${resource}, ${params}`);
    return { data: [{}] };
  },

  deleteMany: async (resource: string, params: { ids: string[] }) => {
    console.log(`${resource}, ${params}`);
    return { data: [{}] };
  },
};
