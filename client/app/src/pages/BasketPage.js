import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Basket from '../components/Basket';
import Config from '../Config';
import "../App.css";

function BasketPage() {
  const [basketItems, setBasketItems] = useState([]);
  const [catalogs, setCatalogs] = useState({});
  const [totalQuantity, setTotalQuantity] = useState(0);
  const [totalPrice, setTotalPrice] = useState(0);

  useEffect(() => {
    async function fetchBasket() {
      try {
        const response = await axios.get(`${Config.BASKET_ENDPOINT}/${Config.DEFAULT_USER}`);
        const items = response.data;
        setBasketItems(items);
        await fetchCatalogs(items);
        calculateTotalQuantity(items);
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

  function calculateTotalQuantity(items) {
    const total = items.reduce((acc, item) => acc + item.quantity, 0);
    setTotalQuantity(total);
  }
  const updateQuantity = async (itemId, quantity) => {
    if (quantity == 0) {
      return await handleRemove(itemId)
    }
    try {
      const item = basketItems.find(item => item.id === itemId);
      await axios.put(`${Config.BASKET_ENDPOINT}`, {
        id: item.id,
        userId: item.userId,
        catalogId: item.catalogId,
        quantity: quantity
      });
      const updatedItems = basketItems.map(item => item.id === itemId ? { ...item, quantity } : item);
      setBasketItems(updatedItems);
      calculateTotalQuantity(updatedItems);
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
      const updatedItems = basketItems.filter(item => item.id !== itemId);
      setBasketItems(updatedItems);
      calculateTotalQuantity(updatedItems);
    } catch (error) {
      console.error('Error removing item', error);
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
      {
        totalQuantity > 0 &&
        <div className="total-container">
          <h3>Total</h3>

          <div>
            <div>Total Price: {totalPrice}</div>
            <div>Total Quantity: {totalQuantity}</div>
            <button className='pay-button'>Pay</button>
          </div>

        </div>
      }
    </div>
  );
}

export default BasketPage;
