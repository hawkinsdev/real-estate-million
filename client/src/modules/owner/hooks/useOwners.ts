import { useEffect, useState } from 'react';
import { Owner, OwnerWithProperties } from '../../../common/types/property';
import { ownerService } from '../services/ownerService';

export const useOwners = () => {
  const [owners, setOwners] = useState<Owner[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchOwners = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await ownerService.getAllOwners();
      setOwners(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error al cargar propietarios');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchOwners();
  }, []);

  return {
    owners,
    loading,
    error,
    refetch: fetchOwners
  };
};

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

export const useOwner = (id: string | null) => {
  const [owner, setOwner] = useState<Owner | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchOwner = async (ownerId: string) => {
    setLoading(true);
    setError(null);
    try {
      const data = await ownerService.getOwnerById(ownerId);
      setOwner(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error al cargar propietario');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (id) {
      fetchOwner(id);
    }
  }, [id]);

  return {
    owner,
    loading,
    error,
    refetch: () => id && fetchOwner(id)
  };
};

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
