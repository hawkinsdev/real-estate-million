import { Home, Person } from '@mui/icons-material';
import {
  Avatar,
  Box,
  Button,
  Card,
  CardActions,
  CardContent,
  Chip,
  Divider,
  Grid,
  List,
  ListItem,
  ListItemText,
  Typography
} from '@mui/material';
import React from 'react';
import { OwnerWithProperties } from '../../../common/types/property';

interface OwnerCardProps {
  owner: OwnerWithProperties;
  onViewDetails?: (ownerId: string) => void;
}

export const OwnerCard: React.FC<OwnerCardProps> = ({ owner, onViewDetails }) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount);
  };

  const getTotalPropertiesValue = () => {
    return owner.properties.reduce((total, property) => total + property.price, 0);
  };

  return (
    <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <CardContent sx={{ flexGrow: 1 }}>
        {/* Header con foto y información básica */}
        <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
          <Avatar
            src={owner.photo}
            sx={{ width: 64, height: 64, mr: 2, bgcolor: 'primary.main' }}
          >
            {!owner.photo && <Person />}
          </Avatar>
          <Box sx={{ flexGrow: 1, minWidth: 0 }}>
            <Typography variant="h6" component="h3" noWrap>
              {owner.name}
            </Typography>
            <Typography variant="body2" color="text.secondary" noWrap>
              {owner.address}
            </Typography>
            <Box sx={{ display: 'flex', gap: 1, mt: 0.5 }}>
              <Chip
                label={`${owner.age} años`}
                size="small"
                variant="outlined"
              />
            </Box>
          </Box>
        </Box>

        <Divider sx={{ mb: 2 }} />

        {/* Estadísticas de propiedades */}
        <Grid container spacing={2} sx={{ mb: 2 }}>
          <Grid item xs={6}>
            <Box
              sx={{
                textAlign: 'center',
                p: 2,
                bgcolor: 'primary.light',
                borderRadius: 1,
                color: 'primary.contrastText'
              }}
            >
              <Typography variant="h5" component="div">
                {owner.properties.length}
              </Typography>
              <Typography variant="caption">
                {owner.properties.length === 1 ? 'Propiedad' : 'Propiedades'}
              </Typography>
            </Box>
          </Grid>
          <Grid item xs={6}>
            <Box
              sx={{
                textAlign: 'center',
                p: 2,
                bgcolor: 'success.light',
                borderRadius: 1,
                color: 'success.contrastText'
              }}
            >
              <Typography variant="h6" component="div" sx={{ fontSize: '0.9rem' }}>
                {formatCurrency(getTotalPropertiesValue())}
              </Typography>
              <Typography variant="caption">
                Valor Total
              </Typography>
            </Box>
          </Grid>
        </Grid>

        {/* Lista de propiedades */}
        {owner.properties.length > 0 ? (
          <Box>
            <Typography variant="subtitle2" gutterBottom>
              Propiedades:
            </Typography>
            <Box sx={{ maxHeight: 120, overflow: 'auto' }}>
              <List dense>
                {owner.properties.slice(0, 3).map((property) => (
                  <ListItem key={property.idProperty} divider>
                    <ListItemText
                      primary={property.name}
                      secondary={`${property.address} - ${formatCurrency(property.price)}`}
                      primaryTypographyProps={{ noWrap: true, fontSize: '0.875rem' }}
                      secondaryTypographyProps={{ noWrap: true, fontSize: '0.75rem' }}
                    />
                  </ListItem>
                ))}
                {owner.properties.length > 3 && (
                  <ListItem>
                    <ListItemText
                      primary={`+${owner.properties.length - 3} propiedades más`}
                      primaryTypographyProps={{ 
                        style: { fontStyle: 'italic', color: 'text.secondary' }
                      }}
                    />
                  </ListItem>
                )}
              </List>
            </Box>
          </Box>
        ) : (
          <Box sx={{ textAlign: 'center', py: 2, color: 'text.secondary' }}>
            <Home sx={{ fontSize: 40, mb: 1, opacity: 0.5 }} />
            <Typography variant="body2">
              No tiene propiedades registradas
            </Typography>
          </Box>
        )}
      </CardContent>

      {/* Botón de ver detalles */}
      {onViewDetails && (
        <CardActions>
          <Button
            fullWidth
            variant="contained"
            onClick={() => onViewDetails(owner.idOwner)}
            startIcon={<Person />}
          >
            Ver Detalles
          </Button>
        </CardActions>
      )}
    </Card>
  );
};
