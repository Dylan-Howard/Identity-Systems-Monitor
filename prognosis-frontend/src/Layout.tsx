// import { Layout } from 'react-admin';
// import MyMenu from './MyMenu';
// import { react } from 'react';
import { Box } from '@mui/material';
import { AppBar, Menu, Sidebar } from 'react-admin';

export const IAMLayout = ({ children, dashboard } : { children: any, dashboard?: any }) => (
  // <Layout
  //   {...props}
  //   sx={{ displayPrint: 'block', maxWidth: 800 }}
  // />
  <Box 
    display="flex"
    flexDirection="column"
    zIndex={1}
    minHeight="100vh"
    // backgroundColor="theme.palette.background.default"
    position="relative"
    sx={{ bgcolor: 'primary.background' }}
  >
    <Box
        display="flex"
        flexDirection="column"
        overflow="auto"
    >
        <AppBar />
        <Box display="flex" flexGrow={1}>
            <Sidebar>
                <Menu hasDashboard={!!dashboard} />
            </Sidebar>
            <Box
                display="flex"
                flexDirection="column"
                flexGrow={2}
                p={3}
                marginTop="4em"
                paddingLeft={5}
            >
                {children}
            </Box>
        </Box>
    </Box>
  </Box>
);