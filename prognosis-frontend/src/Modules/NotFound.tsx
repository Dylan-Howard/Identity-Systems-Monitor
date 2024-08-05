import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import { Title } from 'react-admin';

const notFoundSrc = './public/pictures/not-found.svg';

const NotFound = () => (
  <Card>
    <Title title="Not Found" />
    <CardContent>
      <Typography>404: Page not found</Typography>
      <Box sx={{ height: 200, width: 200 }}>
        <img
          src={notFoundSrc}
          alt="Data not found at this link."
        />
      </Box>
    </CardContent>
  </Card>
);

export default NotFound;
