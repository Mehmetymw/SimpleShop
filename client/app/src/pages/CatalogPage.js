import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CatalogList from '../components/CatalogList';
import Config from '../Config';
import '../CatalogPage.css';
import "../App.css";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

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
    const userId = Config.DEFAULT_USER; 
    
    try {
      console.log('Attempting to add to cart...');
      await axios.post(Config.CATALOG_ENDPOINT, { catalogId, userId });
      setCartCount(prevCount => prevCount + 1);
      toast('Item added to cart successfully!');
    } catch (error) {
      console.error('Error adding item to cart', error);
      toast('Failed to add item to cart.');
    }
  };

  return (
    <div className="catalog-container page-container">
      <h1 className="catalog-header">Catalog</h1>
      <ToastContainer />
      <CatalogList catalogs={catalogs} onAddToCart={handleAddToCart} />
    </div>
  );
}

export default CatalogPage;
