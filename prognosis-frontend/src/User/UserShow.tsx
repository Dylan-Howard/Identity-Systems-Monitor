import {
  ShowBase,
  useRecordContext,
  TextField,
  useShowContext,
  SimpleShowLayout,
  Labeled,
  BooleanField,
  DateField,
  FunctionField,
} from 'react-admin';

import './UserShow.css';

import {
  Grid,
  Card,
  Skeleton,
  Stack,
  Chip,
  Button,
  Typography,
  Breadcrumbs,
  Link,
} from "@mui/material";
import WarningIcon from '@mui/icons-material/Warning';
import { UserShowActions } from './UserShowActions';

const UserTitle = () => {
  const record = useRecordContext();
  // the record can be empty while loading
  return <span>{record ? record.mail : ''}</span>;
};

const UserPhoto = () => {
  const record = useRecordContext();
  if (!record) {
    return null;
  }
  const initials = record.givenName[0] + record.sn[0];
  return record.photoURL
    ? (
      <div className="UserProfileFrame">
        <img
          src={record.photoURL}
          alt ={`${record.displayName}'s profile photo`}
          className="UserProfilePhoto"
        />
      </div>
    )
    : (
      <div className="UserProfileFrame">
        <span className="UserProfileLabel">{initials}</span>
      </div>
    )
}

const MultiValuedField = ({ source }: { source: string }) => {
  const record = useRecordContext();
  if (!record) {
    return null;
  }

  return Array.isArray(record[source])
    ? (
      <ul>
        {
          record[source].map((itm: string) => <li key={itm}>{itm}</li>)
        }
      </ul>
    )
    : record[source]
}

const UserSISCardContent = () => {
  const record = useRecordContext();
  if (!record || !record.sis) {
    return 'No data here';
  }

  const isValid = {
    username: record.mail !== record.sis.username,
    email: record.mail !== record.sis.email,
    saml: !record.sis.isSAMLAccount,
    status: record.sis.disable,
  };

  return (
    <>
      <Grid container spacing={2}>
        <Grid item sm={12} md={6}>
          <Labeled label="Person GUID" fullWidth>
            <TextField source="idautoPersonSystem4ID" emptyText="not defined" />
          </Labeled>
          <Stack direction="row" spacing={2}>
            <Labeled label="Username">
              <TextField source="sis.username" emptyText="not defined" />
            </Labeled>
            {
              isValid.username ? <Chip color="error" size="small" label="Warning" /> : ''
            }
          </Stack>
          <Stack direction="row" spacing={2}>
            <Labeled label="Email">
              <TextField source="sis.email" emptyText="not defined" />
            </Labeled>
            {
              isValid.email ? <Chip color="error" size="small" label="Warning" /> : ''
            }
          </Stack>
        </Grid>
        <Grid item sm={12} md={6}>
          <Stack direction="row" spacing={2}>
            <Labeled label="Account Disabled">
              <BooleanField source="sis.disable" emptyText="not defined" />
            </Labeled>
            {
              isValid.status ? <Chip color="error" size="small" label="Warning" /> : ''
            }
          </Stack>
          <Stack direction="row" spacing={2}>
            <Labeled label="SSO Enabled">
              <BooleanField source="sis.isSAMLAccount" emptyText="not defined" />
            </Labeled>
            {
              isValid.saml ? <Chip color="error" size="small" label="Warning" /> : ''
            }
          </Stack>
          <Labeled label="Modified Timestamp">
            <DateField source="sis.dateLastModified" emptyText="not defined" />
          </Labeled>
        </Grid>
      </Grid>
    </>
  )
}

const ShowUserProfile = () => {
  const { isLoading } = useShowContext();
  const record = useRecordContext();
  if (isLoading) {
    return (
      <>
        <Skeleton animation="wave" width={400} height={100} />
        <Card className="UserShowCard">
          <Grid container spacing={2}>
            <Grid item sm={12} md={6}>
              <Skeleton animation="wave" width={400} height={350} />
            </Grid>
            <Grid item sm={12} md={6}>
              <Skeleton animation="wave" height={48} />
              <Skeleton animation="wave" height={48} />
              <Skeleton animation="wave" height={48} />
              <Skeleton animation="wave" height={48} />
            </Grid>
          </Grid>
        </Card>
      </>
    );
  }

  const isError = {
    claimFlag: !record.idautoPersonClaimFlag,
    isActive: !record.isActive,
  };

  return (
    <SimpleShowLayout className="UserShow">
      <Breadcrumbs aria-label="breadcrumb">
        <Link underline="hover" color="inherit" href="/#/users">
          Users
        </Link>
        <Typography color="text.primary">{record.mail}</Typography>
      </Breadcrumbs>
      <h1 className="UserShowTypography"><UserTitle /></h1>
      <Card className="UserShowCard">
        <Stack direction="row-reverse" spacing={2}>
          <UserShowActions />
        </Stack>
        <Grid container spacing={4}>
          <Grid item sm={12} md={6}>
            <UserPhoto />
          </Grid>
          <Grid item sm={12} md={6}>
            <Labeled label="Identifier" fullWidth>
              <FunctionField
                source="idautoPersonHRID"
                render={(record: { idautoPersonHRID: string; idautoPersonStuID: string; }) => record.idautoPersonHRID || record.idautoPersonStuID }
              />
            </Labeled>
            <Labeled label="Display Name" fullWidth>
              <TextField source="displayName" emptyText="not defined" />
            </Labeled>
            <Stack direction="row" spacing={2}>
              <Labeled label="Active" fullWidth>
                <BooleanField source="isActive" emptyText="not defined" />
              </Labeled>
              {
                isError.isActive ? <Chip color="error" size="small" label="Warning" /> : ''
              }
            </Stack>
            <Labeled label="Claim Code" fullWidth>
              <TextField source="idautoPersonClaimCode" emptyText="not defined" />
            </Labeled>
            <Stack direction="row" spacing={2}>
              <Labeled label="Claim Flag">
                <BooleanField source="idautoPersonClaimFlag" emptyText="not defined" />
              </Labeled>
              {
                isError.claimFlag ? <Chip color="error" size="small" label="Warning" /> : ''
              }
            </Stack>
            <Labeled label="Primary Location" fullWidth>
              <FunctionField
                source="idautoPersonLocCodes"
                render={
                  (record: {
                    idautoPersonLocCodes: string | undefined,
                    idautoPersonSchoolCodes: string | undefined,
                    idautoPersonLocNames: string | undefined,
                    idautoPersonSchoolNames: string | undefined,
                  }) => (
                    `${record.idautoPersonLocCodes || record.idautoPersonSchoolCodes} - ${record.idautoPersonLocNames || record.idautoPersonSchoolNames}`
                  )
                }
              />
            </Labeled>
            <Labeled label="Pending Rename" fullWidth>
              <TextField source="idautoPersonRenameUsername"/ >
            </Labeled>
            <Labeled label="Rename Flagged Date" fullWidth>
              <DateField source="idautoPersonRenameFlagDate"/ >
            </Labeled>
            <Labeled label="Password Last Set" fullWidth>
              <DateField source="pwdChangedTime" emptyText="not defined" />
            </Labeled>
            <Labeled label="Must Change Password" fullWidth>
              <BooleanField source="pwdReset" emptyText="not defined" />  
            </Labeled>
          </Grid>
        </Grid>
      </Card>
      <Card className="UserShowCard">
        <h2 className="UserShowTypography">System</h2>
        <Grid container spacing={2}>
          <Grid item sm={12} md={6}>
            <Labeled label="IdAuto Usernames" fullWidth>
              <MultiValuedField source="idautoPersonUserNameMV" />
            </Labeled>
            <Labeled label="SAM Account Name" fullWidth>
              <TextField source="idautoPersonSAMAccountName" emptyText="not defined" />
            </Labeled>
            <Labeled label="Immutable ID" fullWidth>
              <TextField source="idautoPersonSystem3ID" emptyText="not defined" />
            </Labeled>
            <Labeled label="Infinite Campus Person GUID" fullWidth>
              <TextField source="idautoPersonSystem4ID" emptyText="not defined" />
            </Labeled>
          </Grid>
          <Grid item sm={12} md={6}>
            <Labeled label="Created" fullWidth>
              <DateField source="createTimestamp" emptyText="not defined" />
            </Labeled>
            <Labeled label="Activation Date" fullWidth>
              <DateField source="idautoPersonActivationDate" emptyText="not defined" />
            </Labeled>
            <Labeled label="Modified Timestamp" fullWidth>
              <DateField source="modifyTimestamp" emptyText="not defined" />
            </Labeled>
            <Labeled label="Modified By" fullWidth>
              <TextField source="modifiersName" emptyText="not defined" />
            </Labeled>
          </Grid>
        </Grid>
        <Stack direction="row" spacing={2}>
          {/* <Typography variant="body1">
            <WarningIcon /> <strong>Alerts</strong> for user
          </Typography> */}
          <Button
            color="primary"
            variant="text"
            startIcon={<WarningIcon />}
          >
            View alerts
          </Button>
        </Stack>
      </Card>
      <Card className="UserShowCard">
        <h2 className="UserShowTypography">Infinite Campus</h2>
        <UserSISCardContent />
      </Card>
      <Card className="UserShowCard">
        <h2 className="UserShowTypography">Demographics</h2>
        <Grid container spacing={2}>
          <Grid item sm={12} md={6}>
          <Labeled label="First Name" fullWidth>
            <TextField source="givenName" emptyText="not defined" />
          </Labeled>
          <Labeled label="Last Name" fullWidth>
            <TextField source="sn" emptyText="not defined" />  
          </Labeled>
          <Labeled label="Gender" fullWidth>
            <TextField source="idautoPersonGender" emptyText="not defined" />
          </Labeled>
          <Labeled label="Address" fullWidth>
            <TextField source="hr.address" emptyText="not defined" />
          </Labeled>
          </Grid>
          <Grid item sm={12} md={6}>
            <Labeled label="Birth Date" fullWidth>
              <DateField source="idautoPersonBirthdate" emptyText="not defined" />
            </Labeled>
            <Labeled label="Mobile Number" fullWidth>
              <TextField source="mobile" emptyText="not defined" />
            </Labeled>
            <Labeled label="Home Email" fullWidth>
              <TextField source="idautoPersonHomeEmail" emptyText="not defined" />
            </Labeled>
          </Grid>
        </Grid>
      </Card>
      <Card className="UserShowCard">
        <h2 className="UserShowTypography">Employment</h2>
        <Grid container spacing={2}>
          <Grid item sm={12} md={6}>
            <Labeled label="Job Code" fullWidth>
              <TextField source="idautoPersonJobCode" emptyText="not defined" />
            </Labeled>
            <Labeled label="Job Title" fullWidth>
              <TextField source="idautoPersonJobTitle" emptyText="not defined" />
            </Labeled>
            <Labeled label="Location Code" fullWidth>
              <TextField source="idautoPersonLocCodes" emptyText="not defined" />
            </Labeled>
            <Labeled label="Location" fullWidth>
              <TextField source="idautoPersonLocNames" emptyText="not defined" />
            </Labeled>
          </Grid>
          <Grid item sm={12} md={6}>
            <Labeled label="Certificate Number" fullWidth>
              <TextField source="idautoPersonExt5" emptyText="not defined" />
            </Labeled>
            <Labeled label="Certificate Type" fullWidth>
              <TextField source="idautoPersonExt6" emptyText="not defined" />
            </Labeled>
            <Labeled label="Certificate Date" fullWidth>
              <DateField source="idautoPersonExt7" emptyText="not defined" />
            </Labeled>
            <Labeled label="Certificate Expiration" fullWidth>
              <DateField source="idautoPersonExt8" emptyText="not defined" />
            </Labeled>
            <Labeled label="Certificate Area" fullWidth>
              <TextField source="idautoPersonExt9" emptyText="not defined" />
            </Labeled>
          </Grid>
        </Grid>
      </Card>
      <Card className="UserShowCard">
        <Stack direction="row" spacing={2} justifyContent="space-between">
          <h2 className="UserShowTypography">Education</h2>
          <Button
            color="primary"
            variant="text"
          >
            View Enrollments
          </Button>
        </Stack>
        <Grid container spacing={2}>
          <Grid item sm={12} md={6}>
            <Labeled label="Course Codes" fullWidth>
              <MultiValuedField source="idautoPersonCourseCodes" />
            </Labeled>
          </Grid>
          <Grid item sm={12} md={6}>
            <Labeled label="School Name" fullWidth>
              <MultiValuedField source="idautoPersonSchoolNames" />
            </Labeled>
            <Labeled label="School Code" fullWidth>
              <MultiValuedField source="idautoPersonSchoolCodes" />
            </Labeled>
          </Grid>
        </Grid>      
      </Card>
    </SimpleShowLayout>
  )
}

export const UserShow = () => (
  <ShowBase>
    <ShowUserProfile />
  </ShowBase>
);