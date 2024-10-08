import { useEffect, useState } from 'react';
import { fetchUtils } from 'react-admin';

import CssBaseline from '@mui/material/CssBaseline';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';

import { ScoreCard } from './Modules/ScoreCard';
import { Chart } from './Modules/Chart';
import { Copyright } from './Modules/Copyright';

const apiUrl = import.meta.env.VITE_JSON_SERVER_URL;

export const Dashboard = () => {
  const [metrics, setMetrics] = useState({
    id: 'placeholder',
    totals: [
      { id: '', title: 'Total', total: 0 },
    ],
    trends: [
      {
        id: '',
        title: 'Trend',
        data: [ { name: 'Point', count: 0 } ],
      }
    ],
    changes: [
      { user: '', currentId: '', formerId: '', timestamp: '' },
    ]
  });
  const [status, setStatus] = useState('loading');

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetchUtils.fetchJson(`${apiUrl}/dashboard`);
        const { json } = response;
        setMetrics(json);
        setStatus('ready');
      } catch (error) {
        setStatus('error');
      }
    }

    fetchData();
  }, [])

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <Box
        component="main"
        sx={{
          backgroundColor: (theme) =>
            theme.palette.mode === 'light'
              ? theme.palette.grey[100]
              : theme.palette.grey[900],
          flexGrow: 1,
          height: '100vh',
          overflow: 'auto',
        }}
      >
        <Toolbar />
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
          {/* Totals */}
          <Typography variant="h2" sx={{ fontSize: 32, fontWeight: 600, mb: 2 }}>Totals</Typography>
          <Grid container spacing={3} sx={{ mb: 4 }}>
            {
              metrics.totals.map((tot) => (
                <Grid item sm={4} md={3} key={`${tot.title}-total`}>
                  <ScoreCard title={tot.title} score={tot.total} state={tot.id ? 'ready' : 'loading'} />
                </Grid>
              ))
            }
          </Grid>
          {/* Trends */}
          <Typography variant="h2" sx={{ fontSize: 32, fontWeight: 600, mb: 2 }}>Trends</Typography>
          <Grid container spacing={3} sx={{ mb: 4 }}>
            {
              metrics.trends.map((trn) => (
                <Grid item xs={12} md={6} key={`${trn.title}-trend`}>
                  <Chart title={trn.title} data={trn.data} />
                </Grid>
              ))
            }
          </Grid>
          <Copyright sx={{ pt: 4 }} />
        </Container>
      </Box>
    </Box>
  );
}