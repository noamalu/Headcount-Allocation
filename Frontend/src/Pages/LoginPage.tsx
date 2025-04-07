// src/Pages/Login.tsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import '../Styles/Login.css';
import '../Styles/Shared.css';


const LoginPage: React.FC = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleLogin = async () => {
    if (!username || !password) {
      setError('Please enter both username and password.');
      return;
    }
    const isAdmin = username.toLowerCase() === 'admin'; // TO CHANGE
    try {
      await login(username, password);
      navigate('/profile');
    } catch (error) {
      console.error("Login Page - Login failed");
    }
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h2>Welcome to Headcount Allocation</h2>
        <input
          type="text"
          placeholder="Username"
          className="login-input"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
        <input
          type={showPassword ? 'text' : 'password'}
          placeholder="Password"
          className="login-input"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <div className="login-options">
          <label className="checkbox-label">
            <input
              type="checkbox"
              checked={showPassword}
              onChange={(e) => setShowPassword(e.target.checked)}
            />
            Show Password
          </label>
          <button className="forgot-password" onClick={() => alert('Redirect to forgot password')}>
            Forgot password?
          </button>
        </div>
        {error && <div className="login-error">{error}</div>}
        <button className="login-button" onClick={handleLogin}>Login</button>
      </div>
    </div>
  );
};

export default LoginPage;