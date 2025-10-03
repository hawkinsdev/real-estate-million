import { Property, PropertyFilter } from '@/common/types/property';
import {
  Alert,
  Box,
  CircularProgress,
  Container,
  Grid,
  Pagination,
  Paper,
  Typography,
} from '@mui/material';
import React, { useState } from 'react';
import PropertyCard from '../components/PropertyCard';
import PropertyFilters from '../components/PropertyFilters';
import { useProperties } from '../hooks/useProperties';
import { useSearchProperties } from '../hooks/useSearchProperties';

const PropertyListPage: React.FC = () => {
  const [filters, setFilters] = useState<PropertyFilter>({});
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 6;

  const hasActiveFilters = Object.values(filters).some(value => 
    value !== undefined && value !== ''
  );
  const {
    data: filteredProperties,
    isLoading: isSearchLoading,
    error: searchError,
  } = useSearchProperties(filters);

  const {
    data: allProperties,
    isLoading: isAllLoading,
    error: allError,
  } = useProperties();

  const properties = hasActiveFilters ? filteredProperties : allProperties;
  const isLoading = hasActiveFilters ? isSearchLoading : isAllLoading;
  const error = hasActiveFilters ? searchError : allError;

  const handleFilter = (newFilters: PropertyFilter) => {
    setFilters(newFilters);
    setCurrentPage(1);
  };

  const totalPages = properties ? Math.ceil(properties.length / itemsPerPage) : 0;
  const startIndex = (currentPage - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const paginatedProperties = properties?.slice(startIndex, endIndex) || [];

  const handlePageChange = (_: React.ChangeEvent<unknown>, page: number) => {
    setCurrentPage(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Alert severity="error" sx={{ mb: 3 }}>
          Error al cargar las propiedades: {error.message}
        </Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ mb: 4, textAlign: 'center' }}>
        <Typography variant="h3" component="h1" gutterBottom sx={{ fontWeight: 700 }}>
          🏠 Propiedades Disponibles
        </Typography>
        <Typography variant="h6" color="text.secondary">
          Encuentra la propiedad perfecta para ti
        </Typography>
      </Box>

      <PropertyFilters onFilter={handleFilter} loading={isLoading} />

      <Box sx={{ mb: 3 }}>
        <Paper sx={{ p: 2, backgroundColor: '#f8f9fa' }}>
          <Typography variant="h6" gutterBottom>
            {isLoading ? (
              'Cargando propiedades...'
            ) : (
              <>
                {properties?.length || 0} propiedades encontradas
                {hasActiveFilters && (
                  <Typography component="span" variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                    (con filtros aplicados)
                  </Typography>
                )}
              </>
            )}
          </Typography>
        </Paper>
      </Box>

      {isLoading && (
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
          <CircularProgress size={60} />
        </Box>
      )}

      {!isLoading && paginatedProperties.length > 0 && (
        <>
          <Grid container spacing={3} sx={{ mb: 4 }}>
            {paginatedProperties.map((property: Property, index: number) => (
              <Grid item xs={12} sm={6} md={4} key={`${property.idOwner}-${index}`}>
                <PropertyCard property={property} />
              </Grid>
            ))}
          </Grid>

          {totalPages > 1 && (
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
              <Pagination
                count={totalPages}
                page={currentPage}
                onChange={handlePageChange}
                color="primary"
                size="large"
                showFirstButton
                showLastButton
              />
            </Box>
          )}
        </>
      )}

      {!isLoading && (!properties || properties.length === 0) && (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h5" gutterBottom>
            😔 No se encontraron propiedades
          </Typography>
          <Typography variant="body1" color="text.secondary">
            {hasActiveFilters
              ? 'Intenta ajustar los filtros de búsqueda para encontrar más propiedades.'
              : 'No hay propiedades disponibles en este momento.'}
          </Typography>
        </Box>
      )}
    </Container>
  );
};

export default PropertyListPage;
