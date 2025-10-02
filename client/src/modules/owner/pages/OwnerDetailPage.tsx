import { ArrowBack, AttachMoney, Cake, CalendarToday, Home, LocationOn, Person } from '@mui/icons-material';
import {
  Alert,
  Avatar,
  Box,
  Breadcrumbs,
  Button,
  Card,
  CardContent,
  Chip,
  CircularProgress,
  Divider,
  Grid,
  Link,
  List,
  ListItem,
  ListItemText,
  Typography
} from '@mui/material';
import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useOwnerWithProperties } from '../hooks/useOwners';

export const OwnerDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { ownerWithProperties, loading, error } = useOwnerWithProperties(id || null);

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

  const getTotalValue = () => {
    if (!ownerWithProperties) return 0;
    return ownerWithProperties.properties.reduce((total, property) => total + property.price, 0);
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '60vh' }}>
        <CircularProgress />
        <Typography sx={{ ml: 2 }}>Cargando propietario...</Typography>
      </Box>
    );
  }

  if (error || !ownerWithProperties) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '60vh' }}>
        <Alert severity="error" sx={{ maxWidth: 400 }}>
          <Typography variant="h6">Error al cargar propietario</Typography>
          <Typography variant="body2">{error || 'Propietario no encontrado'}</Typography>
          <Button
            onClick={() => navigate('/owners')}
            sx={{ mt: 2 }}
            variant="contained"
          >
            Volver a la lista
          </Button>
        </Alert>
      </Box>
    );
  }

  return (
    <Box>
      {/* Header */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 3 }}>
        <Box>
          <Typography variant="h3" component="h1" gutterBottom>
            Detalles del Propietario
          </Typography>
          <Typography variant="body1" color="text.secondary">
            Información completa y propiedades de {ownerWithProperties.name}
          </Typography>
        </Box>
        
        <Button
          onClick={() => navigate('/owners')}
          variant="outlined"
          startIcon={<ArrowBack />}
        >
          Volver
        </Button>
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
        <Link
          component="button"
          variant="body2"
          onClick={() => navigate('/owners')}
          sx={{ display: 'flex', alignItems: 'center', textDecoration: 'none' }}
        >
          <Person sx={{ mr: 0.5, fontSize: 16 }} />
          Propietarios
        </Link>
        <Typography variant="body2" color="text.primary">
          {ownerWithProperties.name}
        </Typography>
      </Breadcrumbs>

      <Grid container spacing={3}>
        {/* Información del propietario */}
        <Grid item xs={12} lg={4}>
          <Card>
            <CardContent>
              <Box sx={{ textAlign: 'center', mb: 3 }}>
                <Avatar
                  src={ownerWithProperties.photo}
                  sx={{ width: 120, height: 120, mx: 'auto', mb: 2 }}
                >
                  <Person sx={{ fontSize: 60 }} />
                </Avatar>
                <Typography variant="h4" gutterBottom>
                  {ownerWithProperties.name}
                </Typography>
              </Box>

              <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center' }}>
                  <LocationOn sx={{ mr: 1 }} />
                  Dirección
                </Typography>
                <Typography variant="body1" sx={{ ml: 4 }}>
                  {ownerWithProperties.address}
                </Typography>
              </Box>
              
              <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center' }}>
                  <Person sx={{ mr: 1 }} />
                  Edad
                </Typography>
                <Typography variant="body1" sx={{ ml: 4 }}>
                  {ownerWithProperties.age} años
                </Typography>
              </Box>
              
              <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center' }}>
                  <Cake sx={{ mr: 1 }} />
                  Fecha de Nacimiento
                </Typography>
                <Typography variant="body1" sx={{ ml: 4 }}>
                  {formatDate(ownerWithProperties.birthday)}
                </Typography>
              </Box>
              
              <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center' }}>
                  <CalendarToday sx={{ mr: 1 }} />
                  Registrado
                </Typography>
                <Typography variant="body1" sx={{ ml: 4 }}>
                  {formatDate(ownerWithProperties.createdAt)}
                </Typography>
              </Box>
              
              <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center' }}>
                  <CalendarToday sx={{ mr: 1 }} />
                  Última Actualización
                </Typography>
                <Typography variant="body1" sx={{ ml: 4 }}>
                  {formatDate(ownerWithProperties.updatedAt)}
                </Typography>
              </Box>

              <Divider sx={{ my: 3 }} />

              {/* Estadísticas */}
              <Grid container spacing={2}>
                <Grid item xs={6}>
                  <Card sx={{ textAlign: 'center', bgcolor: 'primary.light', color: 'primary.contrastText' }}>
                    <CardContent>
                      <Typography variant="h4">
                        {ownerWithProperties.properties.length}
                      </Typography>
                      <Typography variant="body2">
                        {ownerWithProperties.properties.length === 1 ? 'Propiedad' : 'Propiedades'}
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
                <Grid item xs={6}>
                  <Card sx={{ textAlign: 'center', bgcolor: 'success.light', color: 'success.contrastText' }}>
                    <CardContent>
                      <Typography variant="h6" sx={{ fontSize: '1rem' }}>
                        {formatCurrency(getTotalValue())}
                      </Typography>
                      <Typography variant="body2">
                        Valor Total
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </Grid>

        {/* Lista de propiedades */}
        <Grid item xs={12} lg={8}>
          <Card>
            <CardContent>
              <Typography variant="h5" gutterBottom>
                Propiedades ({ownerWithProperties.properties.length})
              </Typography>

              {ownerWithProperties.properties.length === 0 ? (
                <Box sx={{ textAlign: 'center', py: 6, color: 'text.secondary' }}>
                  <AttachMoney sx={{ fontSize: 80, mb: 2, opacity: 0.3 }} />
                  <Typography variant="h6">
                    Este propietario no tiene propiedades registradas
                  </Typography>
                </Box>
              ) : (
                <List>
                  {ownerWithProperties.properties.map((property, index) => (
                    <React.Fragment key={property.idProperty}>
                      <ListItem
                        sx={{
                          border: 1,
                          borderColor: 'divider',
                          borderRadius: 1,
                          mb: 2,
                          '&:hover': {
                            bgcolor: 'action.hover'
                          }
                        }}
                      >
                        <ListItemText
                          primary={
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                              <Box sx={{ flexGrow: 1 }}>
                                <Typography variant="h6" gutterBottom>
                                  {property.name}
                                </Typography>
                                <Typography variant="body2" color="text.secondary" gutterBottom>
                                  {property.address}
                                </Typography>
                                <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
                                  <Chip
                                    label={`Código: ${property.codeInternal}`}
                                    size="small"
                                    variant="outlined"
                                  />
                                  <Chip
                                    label={`Año: ${property.year}`}
                                    size="small"
                                    variant="outlined"
                                  />
                                </Box>
                              </Box>
                              <Box sx={{ textAlign: 'right', ml: 2 }}>
                                <Typography variant="h5" color="success.main" gutterBottom>
                                  {formatCurrency(property.price)}
                                </Typography>
                                <Button
                                  onClick={() => navigate(`/property/${property.idProperty}`)}
                                  variant="contained"
                                  size="small"
                                >
                                  Ver Detalles
                                </Button>
                              </Box>
                            </Box>
                          }
                        />
                      </ListItem>
                      {index < ownerWithProperties.properties.length - 1 && <Divider />}
                    </React.Fragment>
                  ))}
                </List>
              )}
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>
  );
};