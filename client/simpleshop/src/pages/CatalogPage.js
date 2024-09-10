import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CatalogList from '../components/CatalogList';
import Config from '../Config'

function CatalogPage() {
  const [catalogs, setCatalogs] = useState([]);

  useEffect(() => {
    async function fetchCatalogs() {
      try {
        const response = await axios.get(Config.CATALOG_ENDPOINT);
        setCatalogs(response.data);
      } catch (error) {
        console.error('Error fetching catalogs', error);
      }
    }

    fetchCatalogs();
  }, []);

  return (
    <div>
      <h1>Catalogs</h1>
      <CatalogList catalogs={catalogs} />
    </div>
  );
}

export default CatalogPage;
