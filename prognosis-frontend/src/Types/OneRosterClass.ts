
export type ClassEnrollment = {
  userSourcedId: string,
  username: string,
  role: string,
  primary: boolean,
  beginDate: Date,
  endDate: Date,
};

type OneRosterClass = {
  id: string,
  identifier: string,
  status: boolean,
  dateLastModified: string,
  title: string,
  classType: string,
  classCode: string,
  location: string,
  school: string,
  organization?: string,
  enrollments: ClassEnrollment[],
};

export default OneRosterClass;
