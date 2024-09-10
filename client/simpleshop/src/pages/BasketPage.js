import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Basket from '../components/Basket';
import Config from '../Config'

function BasketPage() {
  const [basketItems, setBasketItems] = useState([]);

  useEffect(() => {
    async function fetchBasket() {
      try {
        const response = await axios.get(Config.BASKET_ENDPOINT);
        setBasketItems(response.data);
      } catch (error) {
        console.error('Error fetching basket', error);
      }
    }

    fetchBasket();
  }, []);

  return (
    <div>
      <h1>Basket</h1>
      <Basket items={basketItems} />
    </div>
  );
}

export default BasketPage;
