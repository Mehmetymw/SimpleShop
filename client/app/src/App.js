import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import CatalogPage from './pages/CatalogPage';
import BasketPage from './pages/BasketPage';
import Header from './components/Header';
import { CartProvider, useCart } from './context/CartContext';

function App() {
  return (
    <CartProvider>
      <Router>
        <HeaderWithCartCount />
        <Routes>
          <Route path="/" element={<CatalogPage />} />
          <Route path="/basket" element={<BasketPage />} />
        </Routes>
      </Router>
    </CartProvider>
  );
}

function HeaderWithCartCount() {
  const { cartCount } = useCart();
  return <Header cartCount={cartCount} />;
}

export default App;