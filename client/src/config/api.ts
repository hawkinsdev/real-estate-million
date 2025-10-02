export const API_CONFIG = {
  BASE_URL: process.env.REACT_APP_API_URL || 'http://localhost:5000/api',
  TIMEOUT: 10000, // 10 segundos
};

export const API_ENDPOINTS = {
  PROPERTIES: {
    SIMPLE: '/Property/simple',
    SEARCH_SIMPLE: '/Property/search/simple',
    DETAILS: (id: string) => `/Property/${id}`,
    SEARCH: '/Property/search',
    CREATE: '/Property',
    UPDATE: (id: string) => `/Property/${id}`,
    DELETE: (id: string) => `/Property/${id}`,
  },
  OWNERS: {
    ALL: '/owner',
    DETAILS: (id: string) => `/owner/${id}`,
  },
};
