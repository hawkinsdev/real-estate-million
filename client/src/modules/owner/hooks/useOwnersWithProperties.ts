import { useEffect, useState } from 'react';
import { OwnerWithProperties } from '../../../common/types/property';
import { ownerService } from '../services/ownerService';

export const useOwnersWithProperties = () => {
  const [ownersWithProperties, setOwnersWithProperties] = useState<OwnerWithProperties[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchOwnersWithProperties = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await ownerService.getAllOwnersWithProperties();
      setOwnersWithProperties(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error al cargar propietarios con propiedades');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchOwnersWithProperties();
  }, []);

  return {
    ownersWithProperties,
    loading,
    error,
    refetch: fetchOwnersWithProperties
  };
};
