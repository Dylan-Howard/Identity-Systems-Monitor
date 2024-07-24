
export type Service = {
  serviceId: string;
  name: string;
};

/*
\( NEWID\(\), @GoogleGUID, ([0-9]+), '([0-9]+-[0-9]+-[0-9]+ 02:00:00)'\)
{ id: "00000-00000-00000-00000-00000", serviceId: "00000-00000-00000-00000-00001", count: $1, timestamp: $2, }
*/