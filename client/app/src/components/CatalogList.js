import React, { useState } from 'react';
import { useCart } from '../context/CartContext';
import '../CatalogList.css';
import placeholderImage from "../assets/150x150.png";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheckCircle } from '@fortawesome/free-solid-svg-icons'; 

function CatalogList({ catalogs }) {
  const { addToCart } = useCart();
  const [addedItem, setAddedItem] = useState(null);

  const handleAddToCart = async (catalog) => {
    console.log('Adding to cart:', catalog); 
    await addToCart(catalog);
    setAddedItem(catalog.id);
    setTimeout(() => setAddedItem(null), 500); 
  };

  return (
    <ul className="catalog-list">
      {catalogs.map(catalog => (
        <li key={catalog.id} className="catalog-item">
          <img src={placeholderImage} alt={catalog.name} />
          <h2>{catalog.name}</h2>
          <p>{catalog.description}</p>
          <p>Price: ${catalog.price}</p>
          <button
            className={`add-to-cart ${addedItem === catalog.id ? 'added' : ''}`}
            onClick={() => handleAddToCart(catalog)}
          >
            {addedItem === catalog.id ? (
              <FontAwesomeIcon icon={faCheckCircle} style={{ color: 'white' }} />
            ) : (
              'Add to Cart'
            )}
          </button>
        </li>
      ))}
    </ul>
  );
}

export default CatalogList;
