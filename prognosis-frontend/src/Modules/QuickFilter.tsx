import Chip from '@mui/material/Chip';

const QuickFilter = ({ source, label, defaultValue }: { source: string, label: string, defaultValue: any }) => {
  return <Chip sx={{ marginBottom: 1 }} label={label} />;
};

export default QuickFilter;