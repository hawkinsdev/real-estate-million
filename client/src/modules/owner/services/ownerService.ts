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
  async getAllOwners(): Promise<Owner[]> {
    const response = await api.get(API_ENDPOINTS.OWNERS.ALL);
    return response.data;
  },

  async getAllOwnersWithProperties(): Promise<OwnerWithProperties[]> {
    const response = await api.get(API_ENDPOINTS.OWNERS.WITH_PROPERTIES);
    return response.data;
  },

  async getOwnerById(id: string): Promise<Owner> {
    const response = await api.get(API_ENDPOINTS.OWNERS.DETAILS(id));
    return response.data;
  },

  async getOwnerWithPropertiesById(id: string): Promise<OwnerWithProperties> {
    const response = await api.get(API_ENDPOINTS.OWNERS.WITH_PROPERTIES_BY_ID(id));
    return response.data;
  }
};
