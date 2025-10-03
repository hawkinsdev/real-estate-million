import {
  Box,
  Button,
  Card,
  CardActions,
  CardContent,
  CardMedia,
  Chip,
  Typography,
} from '@mui/material';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Property } from '../../../common/types/property';

interface PropertyCardProps {
  property: Property;
}

const PropertyCard: React.FC<PropertyCardProps> = ({ property }) => {
  const navigate = useNavigate();

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(price);
  };

  const handleViewDetails = () => {
    navigate(`/property/${property.idProperty}`);
  };

  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 4,
        },
      }}
    >
      <CardMedia
        component="img"
        height="200"
        image={property.images[0].file || '/placeholder-property.jpg'}
        alt={property.name}
        sx={{
          objectFit: 'cover',
          backgroundColor: '#f5f5f5',
        }}
        onError={(e) => {
          const target = e.target as HTMLImageElement;
          target.src = 'https://via.placeholder.com/400x200/f5f5f5/999999?text=Sin+Imagen';
        }}
      />
      
      <CardContent sx={{ flexGrow: 1, pb: 1 }}>
        <Typography
          variant="h6"
          component="h2"
          gutterBottom
          sx={{
            fontSize: '1.1rem',
            fontWeight: 600,
            lineHeight: 1.3,
            display: '-webkit-box',
            WebkitLineClamp: 2,
            WebkitBoxOrient: 'vertical',
            overflow: 'hidden',
          }}
        >
          {property.name}
        </Typography>
        
        <Typography
          variant="body2"
          color="text.secondary"
          sx={{
            mb: 2,
            display: '-webkit-box',
            WebkitLineClamp: 2,
            WebkitBoxOrient: 'vertical',
            overflow: 'hidden',
          }}
        >
          📍 {property.address}
        </Typography>
        
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
          <Typography
            variant="h6"
            color="primary"
            sx={{
              fontWeight: 700,
              fontSize: '1.2rem',
            }}
          >
            {formatPrice(property.price)}
          </Typography>
          
          <Chip
            label="Disponible"
            color="success"
            size="small"
            variant="outlined"
          />
        </Box>
      </CardContent>
      
      <CardActions sx={{ p: 2, pt: 0 }}>
        <Button
          size="small"
          variant="contained"
          fullWidth
          onClick={handleViewDetails}
          sx={{
            textTransform: 'none',
            fontWeight: 600,
          }}
        >
          Ver Detalles
        </Button>
      </CardActions>
    </Card>
  );
};

export default PropertyCard;
