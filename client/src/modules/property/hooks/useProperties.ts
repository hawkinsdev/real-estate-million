import { Property, PropertyFilter, PropertySimple } from '@/common/types/property';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import { propertyService } from '../services/propertyService';

// Hook para obtener todas las propiedades en formato simple
export const usePropertiesSimple = () => {
  return useQuery<PropertySimple[], Error>(
    'properties-simple',
    () => propertyService.getAllPropertiesSimple(),
    {
      staleTime: 5 * 60 * 1000, // 5 minutos
    }
  );
};

// Hook para obtener todas las propiedades completas
export const useProperties = () => {
  return useQuery<Property[], Error>(
    'properties',
    () => propertyService.getAllProperties(),
    {
      staleTime: 5 * 60 * 1000, // 5 minutos
    }
  );
};

// Hook para obtener una propiedad por ID
export const useProperty = (id: string) => {
  return useQuery<Property, Error>(
    ['property', id],
    () => propertyService.getPropertyById(id),
    {
      enabled: !!id,
      staleTime: 5 * 60 * 1000, // 5 minutos
    }
  );
};

// Hook para buscar propiedades con filtros (formato simple)
export const useSearchPropertiesSimple = (filters: PropertyFilter) => {
  return useQuery<PropertySimple[], Error>(
    ['properties-search-simple', filters],
    () => propertyService.searchPropertiesSimple(filters),
    {
      enabled: Object.values(filters).some(value => value !== undefined && value !== ''),
      staleTime: 2 * 60 * 1000, // 2 minutos
    }
  );
};

// Hook para buscar propiedades con filtros (formato completo)
export const useSearchProperties = (filters: PropertyFilter) => {
  return useQuery<Property[], Error>(
    ['properties-search', filters],
    () => propertyService.searchProperties(filters),
    {
      enabled: Object.values(filters).some(value => value !== undefined && value !== ''),
      staleTime: 2 * 60 * 1000, // 2 minutos
    }
  );
};

// Hook para crear una propiedad
export const useCreateProperty = () => {
  const queryClient = useQueryClient();
  
  return useMutation(
    (property: Omit<Property, 'idProperty' | 'createdAt' | 'updatedAt'>) =>
      propertyService.createProperty(property),
    {
      onSuccess: () => {
        queryClient.invalidateQueries('properties');
        queryClient.invalidateQueries('properties-simple');
      },
    }
  );
};

// Hook para actualizar una propiedad
export const useUpdateProperty = () => {
  const queryClient = useQueryClient();
  
  return useMutation(
    ({ id, property }: { id: string; property: Partial<Property> }) =>
      propertyService.updateProperty(id, property),
    {
      onSuccess: (_, { id }) => {
        queryClient.invalidateQueries('properties');
        queryClient.invalidateQueries('properties-simple');
        queryClient.invalidateQueries(['property', id]);
      },
    }
  );
};

// Hook para eliminar una propiedad
export const useDeleteProperty = () => {
  const queryClient = useQueryClient();
  
  return useMutation(
    (id: string) => propertyService.deleteProperty(id),
    {
      onSuccess: () => {
        queryClient.invalidateQueries('properties');
        queryClient.invalidateQueries('properties-simple');
      },
    }
  );
};
