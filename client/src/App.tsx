import { AppBar, Box, Button, Container, Toolbar, Typography } from '@mui/material';
import { Route, Routes, useLocation, useNavigate } from 'react-router-dom';
import { OwnerDetailPage } from './modules/owner/pages/OwnerDetailPage';
import { OwnerListPage } from './modules/owner/pages/OwnerListPage';
import PropertyDetailPage from './modules/property/pages/PropertyDetailPage';
import PropertyListPage from './modules/property/pages/PropertyListPage';

function App() {
  const navigate = useNavigate();
  const location = useLocation();

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Real Estate - Sistema de Gestión
          </Typography>
          <Button 
            color="inherit" 
            onClick={() => navigate('/')}
            variant={location.pathname === '/' ? 'outlined' : 'text'}
            sx={{ mr: 2 }}
          >
            Propiedades
          </Button>
          <Button 
            color="inherit" 
            onClick={() => navigate('/owners')}
            variant={location.pathname.startsWith('/owners') ? 'outlined' : 'text'}
          >
            Propietarios
          </Button>
        </Toolbar>
      </AppBar>
      
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Routes>
          <Route path="/" element={<PropertyListPage />} />
          <Route path="/property/:id" element={<PropertyDetailPage />} />
          <Route path="/owners" element={<OwnerListPage />} />
          <Route path="/owners/:id" element={<OwnerDetailPage />} />
        </Routes>
      </Container>
    </Box>
  );
}

export default App;
