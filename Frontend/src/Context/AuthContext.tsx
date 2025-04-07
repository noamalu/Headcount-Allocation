import React, { createContext, useContext, useState } from 'react';
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
      // console.log(`Fake login as ${username}`);
      // setIsLoggedIn(true);
      // setIsAdmin(false);
      // setCurrentUser(username);
      // setCurrentId(1);
      const userId = await SessionService.login(username, password);
      if (userId) {
        setIsLoggedIn(true);
        setIsAdmin(false); // To update in future
        setCurrentUser(username);
        console.log(`User ${username} logged in successfully with ID: ${userId}`);
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