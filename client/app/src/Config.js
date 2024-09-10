const API_BASE_URL = 'http://localhost';

const config = {
  API_BASE_URL,
  CATALOG_ENDPOINT: `${API_BASE_URL}:5001/api/catalog`,
  BASKET_ENDPOINT: `${API_BASE_URL}:5002/api/basket`,
  DEFAULT_USER : "test"
};

export default config;
