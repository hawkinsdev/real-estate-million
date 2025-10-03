import { ArrowBack } from '@mui/icons-material';
import {
  Alert,
  Box,
  Button,
  Card,
  CardContent,
  CardMedia,
  Chip,
  CircularProgress,
  Container,
  Divider,
  Grid,
  Typography,
} from '@mui/material';
import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useProperty } from '../hooks/useProperty';

const PropertyDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: property, isLoading, error } = useProperty(id || '');

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(price);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('es-CO', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  if (isLoading) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
          <CircularProgress size={60} />
        </Box>
      </Container>
    );
  }

  if (error || !property) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Alert severity="error" sx={{ mb: 3 }}>
          Error al cargar la propiedad: {error?.message || 'Propiedad no encontrada'}
        </Alert>
        <Button
          variant="contained"
          startIcon={<ArrowBack />}
          onClick={() => navigate('/')}
        >
          Volver al Listado
        </Button>
      </Container>
    );
  }

  const mainImage = property.images?.find(img => img.enabled)?.file || 
    'https://via.placeholder.com/800x600/f5f5f5/999999?text=Sin+Imagen';

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Button
        variant="outlined"
        startIcon={<ArrowBack />}
        onClick={() => navigate('/')}
        sx={{ mb: 3 }}
      >
        Volver al Listado
      </Button>

      <Grid container spacing={4}>
        <Grid item xs={12} md={8}>
          <Card>
            <CardMedia
              component="img"
              height="400"
              image={mainImage}
              alt={property.name}
              sx={{ objectFit: 'cover' }}
            />
          </Card>
        </Grid>

        <Grid item xs={12} md={4}>
          <Card sx={{ height: 'fit-content' }}>
            <CardContent>
              <Typography variant="h4" component="h1" gutterBottom sx={{ fontWeight: 700 }}>
                {property.name}
              </Typography>
              
              <Typography variant="h5" color="primary" gutterBottom sx={{ fontWeight: 700 }}>
                {formatPrice(property.price)}
              </Typography>
              
              <Box sx={{ display: 'flex', gap: 1, mb: 2 }}>
                <Chip label="Disponible" color="success" variant="outlined" />
                <Chip label={`Año ${property.year}`} variant="outlined" />
              </Box>
              
              <Divider sx={{ my: 2 }} />
              
              <Typography variant="body1" color="text.secondary" gutterBottom>
                📍 {property.address}
              </Typography>
              
              <Typography variant="body2" color="text.secondary" gutterBottom>
                🏷️ Código: {property.codeInternal}
              </Typography>
              
              <Typography variant="body2" color="text.secondary" gutterBottom>
                📅 Creado: {formatDate(property.createdAt)}
              </Typography>
              
              {property.owner && (
                <Typography variant="body2" color="text.secondary">
                  👤 Propietario: {property.owner.name}
                </Typography>
              )}
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12}>
          <Card>
            <CardContent>
              <Typography variant="h5" gutterBottom sx={{ fontWeight: 600 }}>
                Información Detallada
              </Typography>
              
              <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>
                    Características de la Propiedad
                  </Typography>
                  <Box sx={{ pl: 2 }}>
                    <Typography variant="body1" gutterBottom>
                      <strong>Nombre:</strong> {property.name}
                    </Typography>
                    <Typography variant="body1" gutterBottom>
                      <strong>Dirección:</strong> {property.address}
                    </Typography>
                    <Typography variant="body1" gutterBottom>
                      <strong>Precio:</strong> {formatPrice(property.price)}
                    </Typography>
                    <Typography variant="body1" gutterBottom>
                      <strong>Año de construcción:</strong> {property.year}
                    </Typography>
                    <Typography variant="body1" gutterBottom>
                      <strong>Código interno:</strong> {property.codeInternal}
                    </Typography>
                  </Box>
                </Grid>
                
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>
                    Información del Propietario
                  </Typography>
                  {property.owner ? (
                    <Box sx={{ pl: 2 }}>
                      <Typography variant="body1" gutterBottom>
                        <strong>Nombre:</strong> {property.owner.name}
                      </Typography>
                      <Typography variant="body1" gutterBottom>
                        <strong>Dirección:</strong> {property.owner.address}
                      </Typography>
                      <Typography variant="body1" gutterBottom>
                        <strong>Fecha de nacimiento:</strong> {formatDate(property.owner.birthday)}
                      </Typography>
                    </Box>
                  ) : (
                    <Typography variant="body2" color="text.secondary">
                      Información del propietario no disponible
                    </Typography>
                  )}
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </Grid>

        {property.images && property.images.length > 1 && (
          <Grid item xs={12}>
            <Card>
              <CardContent>
                <Typography variant="h5" gutterBottom sx={{ fontWeight: 600 }}>
                  Galería de Imágenes
                </Typography>
                <Grid container spacing={2}>
                  {property.images
                    .filter(img => img.enabled)
                    .slice(1) // Excluir la imagen principal
                    .map((image, index) => (
                      <Grid item xs={12} sm={6} md={4} key={index}>
                        <CardMedia
                          component="img"
                          height="200"
                          image={image.file}
                          alt={`${property.name} - Imagen ${index + 2}`}
                          sx={{ objectFit: 'cover', borderRadius: 1 }}
                        />
                      </Grid>
                    ))}
                </Grid>
              </CardContent>
            </Card>
          </Grid>
        )}

        {property.propertyTraces && property.propertyTraces.length > 0 && (
          <Grid item xs={12}>
            <Card>
              <CardContent>
                <Typography variant="h5" gutterBottom sx={{ fontWeight: 600 }}>
                  Historial de Transacciones
                </Typography>
                <Grid container spacing={2}>
                  {property.propertyTraces.map((trace, index) => (
                    <Grid item xs={12} md={6} key={index}>
                      <Box sx={{ p: 2, border: 1, borderColor: 'divider', borderRadius: 1 }}>
                        <Typography variant="h6" gutterBottom>
                          {trace.name}
                        </Typography>
                        <Typography variant="body2" color="text.secondary" gutterBottom>
                          Fecha: {formatDate(trace.dateSale)}
                        </Typography>
                        <Typography variant="body1" gutterBottom>
                          Valor: {formatPrice(trace.value)}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          Impuestos: {formatPrice(trace.tax)}
                        </Typography>
                      </Box>
                    </Grid>
                  ))}
                </Grid>
              </CardContent>
            </Card>
          </Grid>
        )}
      </Grid>
    </Container>
  );
};

export default PropertyDetailPage;
