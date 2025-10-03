import { Property, PropertyFilter } from '@/common/types/property';
import { useQuery } from 'react-query';
import { propertyService } from '../services/propertyService';

export const useSearchProperties = (filters: PropertyFilter) => {
  return useQuery<Property[], Error>(
    ['properties-search', filters],
    () => propertyService.searchProperties(filters),
    {
      enabled: Object.values(filters).some(value => value !== undefined && value !== ''),
      staleTime: 2 * 60 * 1000,
    }
  );
};
