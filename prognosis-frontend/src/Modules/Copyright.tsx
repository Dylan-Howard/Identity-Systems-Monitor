import Box from '@mui/material/Box';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';

export const Copyright = (props: any) => (
  <Box sx={{ mt: 2, mb: 2 }} {...props}>
    <Typography variant="body2" color="text.secondary" align="center">
      {'Made with care by '}
      <Link color="inherit" href="https://github.com/Dylan-Howard/">
        Dylan Howard
      </Link>{' '}
      {new Date().getFullYear()}
      {'.'}
    </Typography>
  </Box>
);