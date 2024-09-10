import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import CatalogPage from './pages/CatalogPage';
import BasketPage from './pages/BasketPage';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<CatalogPage />} />
        <Route path="/basket" element={<BasketPage />} />
      </Routes>
    </Router>
  );
}

export default App;
