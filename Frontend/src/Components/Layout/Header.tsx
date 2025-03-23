import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../Context/AuthContext';
import '../../Styles/Layout.css';

const Header: React.FC = () => {
  const { currentUser, isLoggedIn, logout } = useAuth();
  const [showDropdown, setShowDropdown] = useState(false);
  const navigate = useNavigate();

  const handleUserClick = () => {
    if (!isLoggedIn) return; // אם לא מחובר – לא עושים כלום
    setShowDropdown((prev) => !prev);
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const handleProfile = () => {
    navigate('/profile');
    setShowDropdown(false);
  };

  return (
    <div className="header">
      <nav>
        <div className="breadcrumb">Projects/</div>
        <div className="icon-container">
          <div className="user-icon-wrapper">
            <i className="fas fa-user" onClick={handleUserClick}></i>

            {isLoggedIn && currentUser && (
              <span className="username-label">{currentUser}</span>
            )}

            {isLoggedIn && showDropdown && (
              <div className="user-dropdown">
                <p>Hello, {currentUser}</p>
                <button onClick={handleProfile}>My Profile</button>
                <button onClick={handleLogout}>Logout</button>
              </div>
            )}
          </div>

          <i className="fas fa-cog"></i>
          <i className="fas fa-bell"></i>
        </div>
      </nav>
    </div>
  );
};

export default Header;

// const Header: React.FC = () => {
//     return (
//         <div className="header">
//             <nav>
//                 <div className="breadcrumb">Projects/</div> {/* נתיב ניווט */}
//                 <div className="icon-container">
//                     <i className="fas fa-user"></i>
//                     <i className="fas fa-cog"></i>
//                     <i className="fas fa-bell"></i>
//                 </div>
//             </nav>
//         </div>
//     );
// };

// export default Header;