import { Home, Person, Refresh } from '@mui/icons-material';
import { Box, Breadcrumbs, Button, Link, Typography } from '@mui/material';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { OwnerList } from '../components/OwnerList';
import { useOwnersWithProperties } from '../hooks/useOwners';

export const OwnerListPage: React.FC = () => {
  const navigate = useNavigate();
  const { ownersWithProperties, loading, error, refetch } = useOwnersWithProperties();

  const handleViewDetails = (ownerId: string) => {
    navigate(`/owners/${ownerId}`);
  };

  const handleRefresh = () => {
    refetch();
  };

  return (
    <Box>
      {/* Header */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 3 }}>
        <Box>
          <Typography variant="h3" component="h1" gutterBottom>
            Gestión de Propietarios
          </Typography>
          <Typography variant="body1" color="text.secondary">
            Administra y visualiza todos los propietarios y sus propiedades
          </Typography>
        </Box>
        
        <Box sx={{ display: 'flex', gap: 2 }}>
          <Button
            onClick={handleRefresh}
            disabled={loading}
            variant="outlined"
            startIcon={<Refresh />}
          >
            Actualizar
          </Button>
          
        </Box>
      </Box>

      {/* Breadcrumb */}
      <Breadcrumbs sx={{ mb: 3 }}>
        <Link
          component="button"
          variant="body2"
          onClick={() => navigate('/')}
          sx={{ display: 'flex', alignItems: 'center', textDecoration: 'none' }}
        >
          <Home sx={{ mr: 0.5, fontSize: 16 }} />
          Inicio
        </Link>
        <Typography variant="body2" color="text.primary" sx={{ display: 'flex', alignItems: 'center' }}>
          <Person sx={{ mr: 0.5, fontSize: 16 }} />
          Propietarios
        </Typography>
      </Breadcrumbs>

      {/* Lista de propietarios */}
      <OwnerList
        owners={ownersWithProperties}
        loading={loading}
        error={error}
        onViewDetails={handleViewDetails}
      />
    </Box>
  );
};
