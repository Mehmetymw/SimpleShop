import React from 'react';
import { useCart } from '../context/CartContext.js';
import '../CatalogList.css';
import placeholderImage from "../assets/150x150.png";

function CatalogList({ catalogs }) {
  const { addToCart } = useCart();

  const handleAddToCart = (catalog) => {
    console.log('Adding to cart:', catalog); 
    addToCart(catalog);
  };

  return (
    <ul className="catalog-list">
      {catalogs.map(catalog => (
        <li key={catalog.id} className="catalog-item">
          <img src={placeholderImage} alt={catalog.name} />
          <h2>{catalog.name}</h2>
          <p>{catalog.description}</p>
          <p>Price: ${catalog.price}</p>
          <button className="add-to-cart" onClick={() => handleAddToCart(catalog)}>
            Add to Cart
          </button>
        </li>
      ))}
    </ul>
  );
}

export default CatalogList;
