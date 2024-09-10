import React from 'react';
import { Link } from 'react-router-dom';
import './Header.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';

function Header({ cartCount }) {
  return (
    <header className="header">
      <div className="logo">
        <Link to="/">Simple Shop</Link>
      </div>
      <div className="cart">
        <Link to="/basket">
          <FontAwesomeIcon icon={faShoppingCart} />
          {cartCount > 0 && (
            <span className="cart-count">{cartCount}</span>
          )}
        </Link>
      </div>
    </header>
  );
}

export default Header;