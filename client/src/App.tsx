import { AppBar, Box, Container, Toolbar, Typography } from '@mui/material';
import { Route, Routes } from 'react-router-dom';
import PropertyDetailPage from './modules/property/pages/PropertyDetailPage';
import PropertyListPage from './modules/property/pages/PropertyListPage';

function App() {
  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Real Estate - Propiedades
          </Typography>
        </Toolbar>
      </AppBar>
      
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Routes>
          <Route path="/" element={<PropertyListPage />} />
          <Route path="/property/:id" element={<PropertyDetailPage />} />
        </Routes>
      </Container>
    </Box>
  );
}

export default App;
