import { Property } from '@/common/types/property';
import { useQuery } from 'react-query';
import { propertyService } from '../services/propertyService';

export const useProperty = (id: string) => {
  return useQuery<Property, Error>(
    ['property', id],
    () => propertyService.getPropertyById(id),
    {
      enabled: !!id,
      staleTime: 5 * 60 * 1000,
    }
  );
};
