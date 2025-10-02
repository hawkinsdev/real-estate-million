import { ArrowDownward, ArrowUpward, AttachMoney, Home, Person } from '@mui/icons-material';
import {
  Alert,
  Box,
  Card,
  CardContent,
  CircularProgress,
  FormControl,
  Grid,
  IconButton,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  TextField,
  Typography
} from '@mui/material';
import React, { useState } from 'react';
import { OwnerWithProperties } from '../../../common/types/property';
import { OwnerCard } from './OwnerCard';

interface OwnerListProps {
  owners: OwnerWithProperties[];
  loading?: boolean;
  error?: string | null;
  onViewDetails?: (ownerId: string) => void;
}

export const OwnerList: React.FC<OwnerListProps> = ({
  owners,
  loading = false,
  error = null,
  onViewDetails
}) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState<'name' | 'properties' | 'value'>('name');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('asc');

  // Filtrar owners por término de búsqueda
  const filteredOwners = owners.filter(owner =>
    owner.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    owner.address.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Ordenar owners
  const sortedOwners = [...filteredOwners].sort((a, b) => {
    let comparison = 0;
    
    switch (sortBy) {
      case 'name':
        comparison = a.name.localeCompare(b.name);
        break;
      case 'properties':
        comparison = a.properties.length - b.properties.length;
        break;
      case 'value':
        const aValue = a.properties.reduce((sum, p) => sum + p.price, 0);
        const bValue = b.properties.reduce((sum, p) => sum + p.price, 0);
        comparison = aValue - bValue;
        break;
    }
    
    return sortOrder === 'asc' ? comparison : -comparison;
  });

  const getTotalStats = () => {
    const totalProperties = owners.reduce((sum, owner) => sum + owner.properties.length, 0);
    const totalValue = owners.reduce((sum, owner) => 
      sum + owner.properties.reduce((propSum, prop) => propSum + prop.price, 0), 0
    );
    return { totalProperties, totalValue };
  };

  const { totalProperties, totalValue } = getTotalStats();

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount);
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 6 }}>
        <CircularProgress />
        <Typography sx={{ ml: 2 }}>Cargando propietarios...</Typography>
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ mt: 2 }}>
        <Typography variant="h6">Error al cargar propietarios</Typography>
        <Typography variant="body2">{error}</Typography>
      </Alert>
    );
  }

  return (
    <Box sx={{ mt: 2 }}>
      {/* Header con estadísticas */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Typography variant="h4" gutterBottom>
          Propietarios ({owners.length})
        </Typography>
        
        <Grid container spacing={3} sx={{ mb: 3 }}>
          <Grid item xs={12} md={4}>
            <Card sx={{ textAlign: 'center', bgcolor: 'primary.light', color: 'primary.contrastText' }}>
              <CardContent>
                <Person sx={{ fontSize: 40, mb: 1 }} />
                <Typography variant="h4">{owners.length}</Typography>
                <Typography variant="body2">Propietarios</Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} md={4}>
            <Card sx={{ textAlign: 'center', bgcolor: 'success.light', color: 'success.contrastText' }}>
              <CardContent>
                <Home sx={{ fontSize: 40, mb: 1 }} />
                <Typography variant="h4">{totalProperties}</Typography>
                <Typography variant="body2">Propiedades Totales</Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} md={4}>
            <Card sx={{ textAlign: 'center', bgcolor: 'warning.light', color: 'warning.contrastText' }}>
              <CardContent>
                <AttachMoney sx={{ fontSize: 40, mb: 1 }} />
                <Typography variant="h6" sx={{ fontSize: '1rem' }}>
                  {formatCurrency(totalValue)}
                </Typography>
                <Typography variant="body2">Valor Total</Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>

        {/* Controles de búsqueda y ordenamiento */}
        <Box sx={{ display: 'flex', gap: 2, flexDirection: { xs: 'column', md: 'row' } }}>
          <TextField
            fullWidth
            placeholder="Buscar por nombre o dirección..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            variant="outlined"
            size="small"
          />
          
          <Box sx={{ display: 'flex', gap: 1, minWidth: { md: 'auto' } }}>
            <FormControl size="small" sx={{ minWidth: 200 }}>
              <InputLabel>Ordenar por</InputLabel>
              <Select
                value={sortBy}
                label="Ordenar por"
                onChange={(e) => setSortBy(e.target.value as typeof sortBy)}
              >
                <MenuItem value="name">Nombre</MenuItem>
                <MenuItem value="properties">Propiedades</MenuItem>
                <MenuItem value="value">Valor</MenuItem>
              </Select>
            </FormControl>
            
            <IconButton
              onClick={() => setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc')}
              color="primary"
            >
              {sortOrder === 'asc' ? <ArrowUpward /> : <ArrowDownward />}
            </IconButton>
          </Box>
        </Box>
      </Paper>

      {/* Lista de propietarios */}
      {sortedOwners.length === 0 ? (
        <Paper sx={{ p: 6, textAlign: 'center' }}>
          <Person sx={{ fontSize: 60, color: 'text.secondary', mb: 2 }} />
          <Typography variant="h6" color="text.secondary">
            {searchTerm ? 'No se encontraron propietarios que coincidan con la búsqueda' : 'No hay propietarios registrados'}
          </Typography>
        </Paper>
      ) : (
        <Grid container spacing={3}>
          {sortedOwners.map((owner) => (
            <Grid item xs={12} sm={6} lg={4} key={owner.idOwner}>
              <OwnerCard
                owner={owner}
                onViewDetails={onViewDetails}
              />
            </Grid>
          ))}
        </Grid>
      )}
    </Box>
  );
};
