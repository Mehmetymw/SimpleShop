import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import CatalogPage from './pages/CatalogPage';
import BasketPage from './pages/BasketPage';
import Header from './components/Header';
import { CartProvider, useCart } from './context/CartContext';
import { SnackbarProvider } from 'notistack';

function App() {
  return (
    <CartProvider>
      <SnackbarProvider maxSnack={3}>
        <Router>
          <HeaderWithCartCount />
          <Routes>
            <Route path="/" element={<CatalogPage />} />
            <Route path="/basket" element={<BasketPage />} />
          </Routes>
        </Router>
      </SnackbarProvider>
    </CartProvider>
  );
}

function HeaderWithCartCount() {
  const { cartCount } = useCart();
  return <Header cartCount={cartCount} />;
}

export default App;
