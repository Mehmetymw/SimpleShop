import React, { createContext, useState, useContext } from 'react';
import axios from 'axios';
import Config from '../Config'; 

const CartContext = createContext();

export function useCart() {
  return useContext(CartContext);
}

export function CartProvider({ children }) {
  const [cart, setCart] = useState([]);

  const addToCart = async (item) => {
    try {
      const data = {
        userId: Config.DEFAULT_USER, 
        catalogId: item.id,
        quantity: 1
      };

      await axios.post(Config.BASKET_ENDPOINT, data);

      setCart(prevCart => [...prevCart, item]);
    } catch (error) {
      console.error('Error adding item to cart', error);
    }
  };

  return (
    <CartContext.Provider value={{ cart, addToCart }}>
      {children}
    </CartContext.Provider>
  );
}
