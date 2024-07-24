// import { fetchUtils } from 'react-admin';
import { stringify } from 'query-string';
import { fetchData } from '../DataFetcher/DataFetcher';

import { User, RIUser, RIUserDetails } from './User';

const apiUrl = import.meta.env.VITE_JSON_SERVER_URL;

/* Prepares filter by resource type */
const prepLdapFilter = (filter: any) => {
  const filterFields = [];

  if (filter.ids) {
    filter.ids.forEach((id: string) => filterFields.push(`(idautoID=${id})`));
  }
  if (filter.userType) {
    filterFields.push(`(employeeType=${filter.userType})`);
  }
  if (filter.isActive !== undefined) {
    filter.isActive == 'true'
      ? filterFields.push(`(!(idautoDisabled=*))`)
      : filterFields.push(`(idautoDisabled=*)`)
    console.log(filter.isActive);
  }
  if (filter.idautoPersonHRID) {
    filterFields.push(`(idautoPersonHRID=${filter.idautoPersonHRID})`);
  }
  if (filter.givenName) {
    filterFields.push(`(givenName=${filter.givenName}*)`);
  }
  if (filter.familyName) {
    filterFields.push(`(sn=${filter.familyName}*)`);
  }
  if (filter.mail) {
    filterFields.push(`(mail=${filter.mail}*)`);
  }

  if (filterFields.length) {
    const conjunction = filter.ids ? '(|' : '(&';

    filterFields.splice(0,0,conjunction);
    filterFields.push(')');
    return filterFields.join('');
  }
  
  return '(employeeType=*)';
};

const formatLDAPDate = (timestamp: string) => (
  timestamp.substring(0, 4) + '-'
    + timestamp.substring(4, 6) + '-'
    + timestamp.substring(6, 8) + 'T'
    + timestamp.substring(8, 10) + ':'
    + timestamp.substring(10, 12) + ':'
    + timestamp.substring(12)
);

const mapBasicUserAttributes = (usr: RIUser) : User => {
  let identifier = '';
  if (usr.employeeType === "Student") {
    identifier = usr.idautoPersonStuID ? usr.idautoPersonStuID.toString() : 'no id';
  } else {
    identifier = usr.idautoPersonHRID ? usr.idautoPersonHRID.toString() : 'no id';
  }

  return ({
    employeeType: usr.employeeType,
    givenName: usr.givenName,
    sn: usr.sn,
    id: usr.id,
    challengeSetTimestamp: usr.idautoChallengeSetTimestamp,
    isActive: !usr.idautoDisabled,
    idautoPersonClaimFlag: usr.idautoPersonClaimFlag === "TRUE",
    idautoPersonRenameFlagDate: usr.idautoPersonRenameFlagDate,
    idautoPersonRenameUsername: usr.idautoPersonRenameUsername,
    identifier: identifier,
    mail: usr.mail,
    passwordLastSet: usr.passwordLastSet,
  })
};

const mapUserAttributes = (usr: RIUserDetails) => ({
  data: {
    id: usr.id,
    isClaimed: !!usr.idautoPersonClaimFlag,
    photoURL: usr.idautoPersonPhotoURL,
    active: usr.active,
    isLocked: usr.idautoLocked,
    createTimestamp: usr.createTimestamp ? formatLDAPDate(usr.createTimestamp) : '',
    displayName: usr.displayName,
    employeeType: usr.employeeType,
    givenName: usr.givenName,
    hr: usr.hr,
    idautoDisabled: usr.idautoDisabled === 'TRUE',
    idautoPasswordLastModified: usr.idautoPasswordLastModified ? formatLDAPDate(usr.idautoPasswordLastModified) : '',
    idautoPersonActivationDate: usr.idautoPersonActivationDate,
    idautoPersonBirthdate: usr.idautoPersonBirthdate,
    idautoPersonClaimCode: usr.idautoPersonClaimCode,
    idautoPersonClaimFlag: usr.idautoPersonClaimFlag === 'TRUE',
    idautoPersonCourseCodes: usr.idautoPersonCourseCodes,
    idautoPersonDeptDescr: usr.idautoPersonDeptDescr,
    idautoPersonExt4: usr.idautoPersonExt4,
    idautoPersonExt5: usr.idautoPersonExt5,
    idautoPersonExt6: usr.idautoPersonExt6,
    idautoPersonExt7: usr.idautoPersonExt7,
    idautoPersonExt8: usr.idautoPersonExt8,
    idautoPersonExt9: usr.idautoPersonExt9,
    idautoPersonExt12: usr.idautoPersonExt12,
    idautoPersonExtBool2: usr.idautoPersonExtBool2,
    idautoPersonGender: usr.idautoPersonGender,
    idautoPersonHomeEmail: usr.idautoPersonHomeEmail,
    idautoPersonHRID: usr.idautoPersonHRID,
    idautoPersonJobCode: usr.idautoPersonJobCode,
    idautoPersonJobTitle: usr.idautoPersonJobTitle,
    idautoPersonLocCodes: usr.idautoPersonLocCodes,
    idautoPersonLocNames: usr.idautoPersonLocNames,
    idautoPersonRenameFlagDate: usr.idautoPersonRenameFlagDate,
    idautoPersonRenameUsername: usr.idautoPersonRenameUsername,
    idautoPersonSAMAccountName: usr.idautoPersonSAMAccountName,
    idautoPersonSchoolCodes: usr.idautoPersonSchoolCodes,
    idautoPersonSchoolNames: usr.idautoPersonSchoolNames,
    idautoPersonSourceStatus: usr.idautoPersonSourceStatus,
    idautoPersonStuID: usr.idautoPersonStuID,
    idautoPersonSystem3ID: usr.idautoPersonSystem3ID,
    idautoPersonSystem4ID: usr.idautoPersonSystem4ID,
    idautoPersonUserNameMV: usr.idautoPersonUserNameMV,
    isActive: usr.idautoDisabled !== 'TRUE',
    mail: usr.mail,
    mobile: usr.mobile,
    modifiersName: usr.modifiersName,
    modifyTimestamp: usr.modifyTimestamp ? formatLDAPDate(usr.modifyTimestamp): '',
    pwdChangedTime: usr.pwdChangedTime ? formatLDAPDate(usr.pwdChangedTime) : '',
    pwdReset: usr.pwdReset === 'TRUE',
    sis: usr.sis,
    sn: usr.sn,
  },
});

export const userDataProvider = {
  getList: async (
    resource: string,
    params: {
      filter:string,
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
    const ldapFilter = prepLdapFilter(params.filter);

    const { page, perPage } = params.pagination;
    const { field, order } = params.sort;
    const query = {
      sort: JSON.stringify(field),
      order: JSON.stringify(order),
      startIndex: JSON.stringify((page - 1) * perPage),
      endIndex: JSON.stringify(page * perPage - 1),
      filter: JSON.stringify(ldapFilter),
    };
    const url = `${apiUrl}/${resource}?${stringify(query)}`;
    
    const { json } = await fetchData(url);
    const { data, range } = json;
    const mappedUsers = data.map((usr: RIUser) => mapBasicUserAttributes(usr));

    return {
      data: mappedUsers,
      total: range,
    };
  },
  getOne: async (resource: string, params: { id:string }) => {
    const url = `${apiUrl}/${resource}/${params.id}`;

    const { json } = await fetchData(url);

    return mapUserAttributes(json.data);
  },

  getMany: async (resource: string, params: { ids:string[] }) => {
    const ldapFilter = prepLdapFilter({ ids: params.ids });
    const query = {
      filter: JSON.stringify(ldapFilter),
    };
    const url = `${apiUrl}/${resource}?${stringify(query)}`;
    const { json } = await fetchData(url);
    const { data } = json;
    return { data };
  },

  getManyReference: async (resource: string, params: string) => {
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

  update: async (resource: string, params: string) => {
    console.log(`${resource}, ${params}`);
    return { data: [] };
  },

  updateMany: async (resource: string, params: string) => {
    console.log(`${resource}, ${params}`);
    return { data: [] };
  },

  delete: async (resource: string, params: string) => {
    console.log(`${resource}, ${params}`);
    return { data: [] };
  },

  deleteMany: async (resource: string, params: string) => {
    console.log(`${resource}, ${params}`);
    return { data: [] };
  },
};
