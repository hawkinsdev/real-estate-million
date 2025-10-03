import { Property, PropertyFilter, PropertySimple } from '@/common/types/property';
import axios from 'axios';
import { API_CONFIG, API_ENDPOINTS } from '../../../config/api';

const api = axios.create({
  baseURL: API_CONFIG.BASE_URL,
  timeout: API_CONFIG.TIMEOUT,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const propertyService = {
  // Obtener todas las propiedades en formato simple
  async getAllPropertiesSimple(): Promise<PropertySimple[]> {
    const response = await api.get(API_ENDPOINTS.PROPERTIES.ALL);
    return response.data;
  },

  // Obtener todas las propiedades completas
  async getAllProperties(): Promise<Property[]> {
    const response = await api.get(API_ENDPOINTS.PROPERTIES.ALL);
    return response.data;
  },

  // Obtener una propiedad por ID
  async getPropertyById(id: string): Promise<Property> {
    const response = await api.get(API_ENDPOINTS.PROPERTIES.DETAILS(id));
    return response.data;
  },

  // Buscar propiedades con filtros (formato simple)
  async searchPropertiesSimple(filters: PropertyFilter): Promise<PropertySimple[]> {
    const params = new URLSearchParams();
    
    if (filters.name) params.append('name', filters.name);
    if (filters.address) params.append('address', filters.address);
    if (filters.minPrice !== undefined) params.append('minPrice', filters.minPrice.toString());
    if (filters.maxPrice !== undefined) params.append('maxPrice', filters.maxPrice.toString());

    const response = await api.get(`${API_ENDPOINTS.PROPERTIES.SEARCH_SIMPLE}?${params.toString()}`);
    return response.data;
  },

  // Buscar propiedades con filtros (formato completo)
  async searchProperties(filters: PropertyFilter): Promise<Property[]> {
    const params = new URLSearchParams();
    
    if (filters.name) params.append('name', filters.name);
    if (filters.address) params.append('address', filters.address);
    if (filters.minPrice !== undefined) params.append('minPrice', filters.minPrice.toString());
    if (filters.maxPrice !== undefined) params.append('maxPrice', filters.maxPrice.toString());

    const response = await api.get(`${API_ENDPOINTS.PROPERTIES.SEARCH}?${params.toString()}`);
    return response.data;
  },

  // Crear una nueva propiedad
  async createProperty(property: Omit<Property, 'idProperty' | 'createdAt' | 'updatedAt'>): Promise<Property> {
    const response = await api.post(API_ENDPOINTS.PROPERTIES.CREATE, property);
    return response.data;
  },

  // Actualizar una propiedad
  async updateProperty(id: string, property: Partial<Property>): Promise<void> {
    await api.put(API_ENDPOINTS.PROPERTIES.UPDATE(id), property);
  },

  // Eliminar una propiedad
  async deleteProperty(id: string): Promise<void> {
    await api.delete(`/property/${id}`);
  },

  // Verificar si una propiedad existe
  async propertyExists(id: string): Promise<boolean> {
    try {
      await api.head(`/property/${id}`);
      return true;
    } catch {
      return false;
    }
  }
};
