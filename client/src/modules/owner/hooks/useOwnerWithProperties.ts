import { useEffect, useState } from 'react';
import { OwnerWithProperties } from '../../../common/types/property';
import { ownerService } from '../services/ownerService';

export const useOwnerWithProperties = (id: string | null) => {
  const [ownerWithProperties, setOwnerWithProperties] = useState<OwnerWithProperties | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchOwnerWithProperties = async (ownerId: string) => {
    setLoading(true);
    setError(null);
    try {
      const data = await ownerService.getOwnerWithPropertiesById(ownerId);
      setOwnerWithProperties(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error al cargar propietario con propiedades');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (id) {
      fetchOwnerWithProperties(id);
    }
  }, [id]);

  return {
    ownerWithProperties,
    loading,
    error,
    refetch: () => id && fetchOwnerWithProperties(id)
  };
};
