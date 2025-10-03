import { Property } from '@/common/types/property';
import { useQuery } from 'react-query';
import { propertyService } from '../services/propertyService';

export const useProperties = () => {
  return useQuery<Property[], Error>(
    'properties',
    () => propertyService.getAllProperties(),
    {
      staleTime: 5 * 60 * 1000,
    }
  );
};
