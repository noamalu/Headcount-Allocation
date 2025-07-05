import React, { createContext, useContext, useEffect, useState } from 'react';
import SessionService from '../Services/SessionService';

interface AuthContextType {
  isLoggedIn: boolean;
  isAdmin: boolean;
  currentUser: string | null;
  currentId: number;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);
  const [currentUser, setCurrentUser] = useState<string | null>(null);
  const [currentId, setCurrentId] = useState<number>(-1);
  

  const login = async (username: string, password: string) => {
    try {
      const userId = await SessionService.login(username, password);
      if (userId != null && userId > -1) {
        setIsLoggedIn(true);
        setCurrentUser(username);
        setCurrentId(userId);
        console.log(`User ${currentUser} logged in successfully with ID: ${currentId}`);
        const checkAdmin = await SessionService.isAdmin(userId);

        if (checkAdmin != null) {
          setIsAdmin(checkAdmin);
          console.log(`User ${currentUser} checked if admin successfully with ID: ${currentId} - ${checkAdmin}`);
        } else {
          throw new Error('Invalid isAdminCheck');
        }
      } else {
        throw new Error('Invalid login credentials');
      }
      
    } catch (error: any) {
      console.error('Login failed:', error.message);
      throw error;
    }
  };

  const logout = () => {
    setIsLoggedIn(false);
    setIsAdmin(false);
    setCurrentUser(null);
    setCurrentId(-1);
    console.log('User logged out');
  };

  useEffect(() => {
    console.log(`🔄 State updated: isLoggedIn=${isLoggedIn}, currentUser=${currentUser}, currentId=${currentId}, isAdmin=${isAdmin}`);
  }, [isLoggedIn, currentUser, currentId, isAdmin]);

  return (
    <AuthContext.Provider value={{ isLoggedIn, isAdmin, currentUser, currentId, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};