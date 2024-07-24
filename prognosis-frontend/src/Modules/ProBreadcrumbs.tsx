import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';

function mapParts(parts: string[] ) {
  const links = [];
  
  let path = '/#';
  for (let i = 0; i < parts.length; i += 1) {
    path += '/' + parts[i];
    links.push({ name: parts[i], path });
  }

  return links;
}

export function ProBreadcrumbs({ parts } : { parts: string[] }) {

  const current = parts[parts.length - 1];
  const links = mapParts(parts.slice(0, parts.length - 1));

  return (
    <Breadcrumbs aria-label="breadcrumb">
      {
        links.map(({ name, path }) => (
          <Link key={`key-${path}`} underline="hover" color="inherit" href={path}>{name}</Link>
        ))
      }
      <Typography color="text.primary">{current}</Typography>
    </Breadcrumbs>
  )
}