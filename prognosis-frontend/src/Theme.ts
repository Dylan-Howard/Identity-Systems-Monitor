
// import { createTheme } from '@mui/material/styles';
import { defaultTheme } from 'react-admin';

export const iamTheme = {
  ...defaultTheme,
  palette: {
    ...defaultTheme.palette,
    primary: {
      light: '#9DA6FB',
      main: '#6E7BFA',
      dark: '#4E5FF9',
      contrastText: '#F8F8FC',
      white: '#ffffffff',
      ghostWhite: '#F8F8FC',
      orangeCrayola: '#ff8340ff',
      gunmetal: '#123241ff',
      violet: '#ef9bf9ff',
      skyBlue: '#68dcf7ff',
      babyPowder: '#F2F3EDff',
      paynesGray: '#626F7Eff',
      night: '',
      background: '#F8F8FC',
    },
    secondary: {
      light: '#B9BFBD',
      main: '#0E2941',
      dark: '#EDBE6E',
      contrastText: '#F8F8FC',
    },
    error: {
      light: '#F3DFB7',
      main: '#E07A5F',
      dark: '#C5A982',
      contrastText: '#000',
    }
    // background: '#F4F1DE',
  },
  components: {
    ...defaultTheme.components,
    // AppBar:{
    //   styleOverrides: {
    //     root: {
    //         backgroundColor: "#F8F8FC",
    //         },
    //     }
    // },
    // sidebar: {

    // },
    // RaDatagrid: {
    //     styleOverrides: {
    //       root: {
    //           // backgroundColor: "#FAF9F0",
    //           // "& .RaDatagrid-headerCell": {
    //           //     backgroundColor: "MistyRose",
    //           // },
    //       }
    //    }
    // },
    MuiTypography: {
      defaultProps: {
        variantMapping: {
          h1: 'h2',
          h2: 'h2',
          h3: 'h2',
          h4: 'h2',
          h5: 'h2',
          h6: 'h2',
          subtitle1: 'h2',
          subtitle2: 'h2',
          body1: 'span',
          body2: 'span',
        },
      },
    },
}

};