import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import Skeleton from '@mui/material/Skeleton';

export const ScoreCard = (
  { title, score, state } : { title: string, score: number, state: string }
) => {
  if (state === 'error') {
    return (
      <span>Error loading data...</span>
    );
  }

  return (
    state === 'loading'
      ? <Skeleton animation="wave" height={100} />
      : (
        <Paper sx={{ display: 'flex', flexDirection: 'column', p: 2 }}>
          <Typography fontSize={14}>
            {`${title}`}
          </Typography>
          <Typography fontSize={48} fontWeight={600}>
            {`${score.toLocaleString()}`}
          </Typography>
        </Paper>
      )
  );
}
