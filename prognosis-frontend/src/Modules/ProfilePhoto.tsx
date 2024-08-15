import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

export const ProfilePhoto = (
  { url, firstName, lastName }: { url: string, firstName: string, lastName: string }
) => {
  const initials = firstName[0] + lastName[0];
  return (
    <Stack
      justifyContent="center"
      alignItems="center"
      sx={{
        width: '90%',
        borderRadius: '50%',
        backgroundColor: '#6E7BFA33',
        overflow: 'hidden',
        aspectRatio: 1,
        maxWidth: '300px',
      }}
    >
      {
        !url
        ? <Typography fontSize={64} sx={{ color: '#6E7BFA' }}>{initials}</Typography>
        : (
          <Box
            component="img"
            sx={{ width: '100%', objectFit: 'contain' }}
            alt={`${firstName} ${lastName}'s profile photo`}
            src={url}
          />
        )
      }
    </Stack>
  )
}