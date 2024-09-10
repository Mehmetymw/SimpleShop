import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Basket from '../components/Basket';
import Config from '../Config';
import "../App.css";

function BasketPage() {
  const [basketItems, setBasketItems] = useState([]);
  const [catalogs, setCatalogs] = useState({});

  useEffect(() => {
    async function fetchBasket() {
      try {
        const response = await axios.get(`${Config.BASKET_ENDPOINT}/`+Config.DEFAULT_USER);
        setBasketItems(response.data);
        await fetchCatalogs(response.data);
      } catch (error) {
        console.error('Error fetching basket', error);
      }
    }

    async function fetchCatalogs(basketItems) {
      try {
        const catalogIds = [...new Set(basketItems.map(item => item.catalogId))];
        const response = await axios.get(`${Config.CATALOG_ENDPOINT}?ids=${catalogIds.join(',')}`);
        const catalogsData = response.data.reduce((acc, catalog) => {
          acc[catalog.id] = catalog;
          return acc;
        }, {});
        setCatalogs(catalogsData);
      } catch (error) {
        console.error('Error fetching catalogs', error);
      }
    }

    fetchBasket();
  }, []);

  const updateQuantity = async (itemId, quantity) => {
    try {
      const item = basketItems.find(item => item.id === itemId);
      await axios.put(`${Config.BASKET_ENDPOINT}`, {
        id: item.id,
        userId: item.userId,
        catalogId: item.catalogId,
        quantity: quantity
      });
      setBasketItems(basketItems.map(item => item.id === itemId ? { ...item, quantity } : item));
    } catch (error) {
      console.error('Error updating quantity', error);
    }
  };

  const handleIncrease = (itemId) => {
    const item = basketItems.find(item => item.id === itemId);
    updateQuantity(itemId, item.quantity + 1);
  };

  const handleDecrease = (itemId) => {
    const item = basketItems.find(item => item.id === itemId);
    if (item.quantity > 1) {
      updateQuantity(itemId, item.quantity - 1);
    }
  };


  const handleRemove = async (itemId) => {
    try {
      await axios.delete(`${Config.BASKET_ENDPOINT}/${Config.DEFAULT_USER}/${itemId}`);
      setBasketItems(basketItems.filter(item => item.id !== itemId));
    } catch (error) {
      console.error('Error removing item from basket', error);
    }
  };

  return (
    <div className="page-container">
      <h1>Basket</h1>
      <Basket
        items={basketItems}
        catalogs={catalogs}
        onIncrease={handleIncrease}
        onDecrease={handleDecrease}
        onRemove={handleRemove}
      />
    </div>
  );
}

export default BasketPage;
