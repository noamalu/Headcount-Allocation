import React from 'react';

const Header = () => {
  return (
    <div className="header">
      <h1>
        <span role="img" aria-label="home">🏠</span> / Projects
      </h1>
      <div className="header-icons">
        <span role="img" aria-label="user">👤</span>
        <span role="img" aria-label="settings">⚙️</span>
        <span role="img" aria-label="notifications">🔔</span>
      </div>
    </div>
  );
};

export default Header;