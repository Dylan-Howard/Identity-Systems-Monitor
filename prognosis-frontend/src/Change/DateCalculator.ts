
const today = new Date();

export const calcMinMaxDateRanges = (today: Date) => ({
  minDate: `${today.getFullYear()-1}-${today.getMonth() + 1}-${today.getDate()}`,
  maxDate: `${today.getFullYear()}-${today.getMonth() + 1}-${today.getDate()}`,
});

export const getMinDate = () => ({
  minDate: `${today.getFullYear()-1}-${today.getMonth() + 1}-${today.getDate()}`,
});

/**
 * Calculate the difference in milliseconds between the two dates, then convert the difference
 * in milliseconds to days.
 * @param dateString - Represents date as a string
 * @returns Days between today and the given string
 */
export const getDateDifference = (dateString: string) => Math.floor(
  (new Date().getTime() - new Date(dateString).getTime()) / (1000 * 60 * 60 * 24)
);

export const getDateFromLDAPString = (dateString: string) => {
  return new Date(
    dateString.substring(0,4) + '-'
    + dateString.substring(4,6) + '-'
    + dateString.substring(6,8) + 'T'
    + dateString.substring(8,10) + ':'
    + dateString.substring(10,12) + ':'
    + dateString.substring(12,14) + '.000Z'
  );
}