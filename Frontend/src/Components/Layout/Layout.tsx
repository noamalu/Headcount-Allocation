// src/Components/Layout/Layout.tsx
import React from 'react';
import { useLocation } from 'react-router-dom';
import Sidebar from './Sidebar';
import Header from './Header';
import { useAuth } from '../../Context/AuthContext';
import '../../Styles/Layout.css';

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { isLoggedIn } = useAuth();
  const location = useLocation();
  const isLoginPage = location.pathname === '/login';

  return (
    <div className={`app-container ${isLoginPage ? 'login-layout' : ''}`}>
      {isLoggedIn && !isLoginPage && <Sidebar />}
      {isLoggedIn && !isLoginPage && <Header />}
      {isLoginPage ? children : <main>{children}</main>}
    </div>
  );
};

export default Layout;