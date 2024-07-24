import Card from '@mui/material/Card';
import Grid from '@mui/material/Grid';
import Skeleton from '@mui/material/Skeleton';

export const ProfileSkeleton = () => (
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