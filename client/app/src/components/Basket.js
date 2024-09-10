import React from 'react';
import '../Basket.css';
import placeholderImage from "../assets/150x150.png";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons';

function Basket({ items, catalogs, onIncrease, onDecrease, onRemove }) {
  return (
    <div className="basket">
      {items.length === 0 ? (
        <div>Your basket is empty</div>
      ) : (
        <ul className="basket-list">
          {items.map((item) => {
            const catalog = catalogs[item.catalogId];
            return (
              catalog && (
                <li key={item.id} className="basket-item">
                  <div className="basket-item-content">
                    <img src={placeholderImage} alt={catalog.name} className="basket-item-image" />
                    <div className="basket-item-details">
                      <div className="basket-item-title">{catalog.name}</div>
                      <div className="basket-item-description">{catalog.description}</div>
                      <div className="basket-item-price">Price: ${catalog.price}</div>
                    </div>
                  </div>
                  <div className="basket-item-controls">
                    <div className="quantity-controls">
                      <button onClick={() => onDecrease(item.id)}>-</button>
                      <span>{item.quantity}</span>
                      <button onClick={() => onIncrease(item.id)}>+</button>
                    </div>
                    <button className="remove-button" onClick={() => onRemove(item.id)}>
                      <FontAwesomeIcon icon={faTrashAlt} />
                    </button>
                  </div>
                </li>
              )
            );
          })}
        </ul>
      )}
    </div>
  );
}

export default Basket;
