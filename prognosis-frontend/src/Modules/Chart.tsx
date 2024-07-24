import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import {
  LineChart,
  XAxis,
  YAxis,
  CartesianGrid,
  Line,
  Legend,
  Tooltip,
  ResponsiveContainer
} from 'recharts';

type DataPoint = {
  name: string,
  count: number,
};

export const Chart = ({ title, data } : { title: string, data: DataPoint[] }) => {
  return (
    <Paper sx={{
      p: 2,
    }}>
      <Typography variant="body1" fontSize={18} fontWeight={600} sx={{ mb: 2 }}>{`${title}`}</Typography>
      <ResponsiveContainer width="100%" height={300}>
        <LineChart width={500} height={300} data={data}>
          <XAxis dataKey="name"/>
          <YAxis/>
          <Tooltip />
          <Legend />
          <CartesianGrid strokeDasharray="5 5"/>
          <Line type="monotone" dataKey="count" stroke="#8884d8" />
        </LineChart>
      </ResponsiveContainer>
      
    </Paper>
  );
}
