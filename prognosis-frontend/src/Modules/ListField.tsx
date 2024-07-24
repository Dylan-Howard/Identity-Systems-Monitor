import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Typography from '@mui/material/Typography';

export const ListField = ({ value }: { value: string | string[] }) => (
  !Array.isArray(value)
    ? <Typography>{value}</Typography>
    : <List>
      {
        value.map((itm: string) => (
          <ListItem key={`listItem-${itm}`}>
            <Typography>{itm}</Typography>
          </ListItem>
        ))
      }
    </List>
);