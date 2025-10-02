import axios from 'axios';
import { Owner, OwnerWithProperties } from '../../../common/types/property';
import { API_CONFIG, API_ENDPOINTS } from '../../../config/api';

const api = axios.create({
  baseURL: API_CONFIG.BASE_URL,
  timeout: API_CONFIG.TIMEOUT,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const ownerService = {
  // Obtener todos los owners
  async getAllOwners(): Promise<Owner[]> {
    const response = await api.get(API_ENDPOINTS.OWNERS.ALL);
    return response.data;
  },

  // Obtener todos los owners con sus propiedades
  async getAllOwnersWithProperties(): Promise<OwnerWithProperties[]> {
    const response = await api.get(API_ENDPOINTS.OWNERS.WITH_PROPERTIES);
    return response.data;
  },

  // Obtener un owner por ID
  async getOwnerById(id: string): Promise<Owner> {
    const response = await api.get(API_ENDPOINTS.OWNERS.DETAILS(id));
    return response.data;
  },

  // Obtener un owner con sus propiedades por ID
  async getOwnerWithPropertiesById(id: string): Promise<OwnerWithProperties> {
    const response = await api.get(API_ENDPOINTS.OWNERS.WITH_PROPERTIES_BY_ID(id));
    return response.data;
  },

  // Crear un nuevo owner
  async createOwner(owner: Omit<Owner, 'idOwner' | 'createdAt' | 'updatedAt'>): Promise<Owner> {
    const response = await api.post(API_ENDPOINTS.OWNERS.CREATE, owner);
    return response.data;
  },

  // Actualizar un owner
  async updateOwner(id: string, owner: Partial<Omit<Owner, 'idOwner' | 'createdAt' | 'updatedAt'>>): Promise<void> {
    await api.put(API_ENDPOINTS.OWNERS.UPDATE(id), owner);
  },

  // Eliminar un owner
  async deleteOwner(id: string): Promise<void> {
    await api.delete(API_ENDPOINTS.OWNERS.DELETE(id));
  },

  // Verificar si un owner existe
  async ownerExists(id: string): Promise<boolean> {
    try {
      await api.head(API_ENDPOINTS.OWNERS.EXISTS(id));
      return true;
    } catch {
      return false;
    }
  }
};
