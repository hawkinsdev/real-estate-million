export const API_CONFIG = {
  BASE_URL: process.env.REACT_APP_API_URL || 'http://localhost:5000/api',
  TIMEOUT: 10000, // 10 segundos
};

export const API_ENDPOINTS = {
  PROPERTIES: {
    ALL: '/Property',
    SIMPLE: '/Property/simple',
    SEARCH_SIMPLE: '/Property/search/simple',
    DETAILS: (id: string) => `/Property/${id}`,
    SEARCH: '/Property/search',
    CREATE: '/Property',
    UPDATE: (id: string) => `/Property/${id}`,
    DELETE: (id: string) => `/Property/${id}`,
  },
  OWNERS: {
    ALL: '/Owners',
    WITH_PROPERTIES: '/Owners/with-properties',
    DETAILS: (id: string) => `/Owners/${id}`,
    WITH_PROPERTIES_BY_ID: (id: string) => `/Owners/${id}/with-properties`,
    CREATE: '/Owners',
    UPDATE: (id: string) => `/Owners/${id}`,
    DELETE: (id: string) => `/Owners/${id}`,
    EXISTS: (id: string) => `/Owners/${id}/exists`,
  },
};
