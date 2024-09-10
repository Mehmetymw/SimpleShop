import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CatalogList from '../components/CatalogList';
import Config from '../Config';
import '../CatalogPage.css';
import "../App.css"

function CatalogPage() {
  const [catalogs, setCatalogs] = useState([]);
  const [cartCount, setCartCount] = useState(0);

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

  const handleAddToCart = async (catalogId) => {
    //Başka bir kullanıcı yok
    const userId = "test"; 
    
    try {
      await axios.post(Config.CART_ENDPOINT, {
        catalogId,
        userId
      });
      setCartCount(prevCount => prevCount + 1);
    } catch (error) {
      console.error('Error adding item to cart', error);
    }
  };

  return (
    <div className="catalog-container page-container">
      <h1 className="catalog-header">Catalog</h1>
      <CatalogList catalogs={catalogs} onAddToCart={handleAddToCart} />
    </div>
  );
}

export default CatalogPage;
