import { PropertyFilter } from '@/common/types/property';
import {
    Box,
    Button,
    Card,
    CardContent,
    Grid,
    Slider,
    TextField,
    Typography
} from '@mui/material';
import React, { useState } from 'react';

interface PropertyFiltersProps {
  onFilter: (filters: PropertyFilter) => void;
  loading?: boolean;
}

const PropertyFilters: React.FC<PropertyFiltersProps> = ({ onFilter, loading = false }) => {
  const [filters, setFilters] = useState<PropertyFilter>({
    name: '',
    address: '',
    minPrice: undefined,
    maxPrice: undefined,
  });

  const [priceRange, setPriceRange] = useState<[number, number]>([0, 2000000000]);

  const handleInputChange = (field: keyof PropertyFilter) => (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const value = event.target.value;
    setFilters(prev => ({
      ...prev,
      [field]: value || undefined,
    }));
  };

  const handlePriceRangeChange = (_: Event, newValue: number | number[]) => {
    const range = newValue as [number, number];
    setPriceRange(range);
    setFilters(prev => ({
      ...prev,
      minPrice: range[0] > 0 ? range[0] : undefined,
      maxPrice: range[1] < 2000000000 ? range[1] : undefined,
    }));
  };

  const handleSearch = () => {
    onFilter(filters);
  };

  const handleClear = () => {
    const clearedFilters: PropertyFilter = {
      name: '',
      address: '',
      minPrice: undefined,
      maxPrice: undefined,
    };
    setFilters(clearedFilters);
    setPriceRange([0, 2000000000]);
    onFilter(clearedFilters);
  };

  const formatPrice = (value: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(value);
  };

  return (
    <Card sx={{ mb: 3 }}>
      <CardContent>
        <Typography variant="h6" gutterBottom>
          🔍 Filtros de Búsqueda
        </Typography>
        
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <TextField
              fullWidth
              label="Nombre de la propiedad"
              value={filters.name || ''}
              onChange={handleInputChange('name')}
              placeholder="Ej: Casa moderna, Apartamento..."
              disabled={loading}
            />
          </Grid>
          
          <Grid item xs={12} md={6}>
            <TextField
              fullWidth
              label="Dirección"
              value={filters.address || ''}
              onChange={handleInputChange('address')}
              placeholder="Ej: Bogotá, Medellín..."
              disabled={loading}
            />
          </Grid>
          
          <Grid item xs={12}>
            <Typography gutterBottom>
              Rango de Precio: {formatPrice(priceRange[0])} - {formatPrice(priceRange[1])}
            </Typography>
            <Slider
              value={priceRange}
              onChange={handlePriceRangeChange}
              valueLabelDisplay="auto"
              valueLabelFormat={formatPrice}
              min={0}
              max={2000000000}
              step={10000000}
              marks={[
                { value: 0, label: '$0' },
                { value: 500000000, label: '$500M' },
                { value: 1000000000, label: '$1B' },
                { value: 1500000000, label: '$1.5B' },
                { value: 2000000000, label: '$2B+' },
              ]}
              disabled={loading}
            />
          </Grid>
          
          <Grid item xs={12}>
            <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
              <Button
                variant="outlined"
                onClick={handleClear}
                disabled={loading}
              >
                Limpiar Filtros
              </Button>
              <Button
                variant="contained"
                onClick={handleSearch}
                disabled={loading}
                sx={{ minWidth: 120 }}
              >
                {loading ? 'Buscando...' : 'Buscar'}
              </Button>
            </Box>
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  );
};

export default PropertyFilters;
