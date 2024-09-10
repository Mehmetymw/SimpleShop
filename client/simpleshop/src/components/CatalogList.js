import React from 'react';

function CatalogList({ catalogs }) {
  return (
    <ul>
      {catalogs.map(catalog => (
        <li key={catalog.id}>
          <h2>{catalog.name}</h2>
          <p>{catalog.description}</p>
          <p>Price: ${catalog.price}</p>
        </li>
      ))}
    </ul>
  );
}

export default CatalogList;
