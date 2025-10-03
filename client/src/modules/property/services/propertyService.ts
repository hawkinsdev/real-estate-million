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
  async getAllPropertiesSimple(): Promise<PropertySimple[]> {
    const response = await api.get(API_ENDPOINTS.PROPERTIES.ALL);
    return response.data;
  },

  async getAllProperties(): Promise<Property[]> {
    const response = await api.get(API_ENDPOINTS.PROPERTIES.ALL);
    return response.data;
  },

  async getPropertyById(id: string): Promise<Property> {
    const response = await api.get(API_ENDPOINTS.PROPERTIES.DETAILS(id));
    return response.data;
  },

  async searchPropertiesSimple(filters: PropertyFilter): Promise<PropertySimple[]> {
    const params = new URLSearchParams();
    
    if (filters.name) params.append('name', filters.name);
    if (filters.address) params.append('address', filters.address);
    if (filters.minPrice !== undefined) params.append('minPrice', filters.minPrice.toString());
    if (filters.maxPrice !== undefined) params.append('maxPrice', filters.maxPrice.toString());

    const response = await api.get(`${API_ENDPOINTS.PROPERTIES.SEARCH_SIMPLE}?${params.toString()}`);
    return response.data;
  },

  async searchProperties(filters: PropertyFilter): Promise<Property[]> {
    const params = new URLSearchParams();
    
    if (filters.name) params.append('name', filters.name);
    if (filters.address) params.append('address', filters.address);
    if (filters.minPrice !== undefined) params.append('minPrice', filters.minPrice.toString());
    if (filters.maxPrice !== undefined) params.append('maxPrice', filters.maxPrice.toString());

    const response = await api.get(`${API_ENDPOINTS.PROPERTIES.SEARCH}?${params.toString()}`);
    return response.data;
  },

  async createProperty(property: Omit<Property, 'idProperty' | 'createdAt' | 'updatedAt'>): Promise<Property> {
    const response = await api.post(API_ENDPOINTS.PROPERTIES.CREATE, property);
    return response.data;
  },

  async updateProperty(id: string, property: Partial<Property>): Promise<void> {
    await api.put(API_ENDPOINTS.PROPERTIES.UPDATE(id), property);
  },

  async deleteProperty(id: string): Promise<void> {
    await api.delete(`/property/${id}`);
  }
};
