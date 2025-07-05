import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import '../Styles/Login.css';
import '../Styles/Shared.css';
import { initWebSocket } from '../Services/NotificationsService';


const LoginPage: React.FC = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [uiError, setUiError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const navigate = useNavigate();
  const { login } = useAuth();

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);


  const handleLogin = async () => {
    if (!username || !password) {
      setUiError('Please enter both username and password.');
      return;
    }
    setUiError(null);
    try {
      await login(username, password);
      const address = `ws://127.0.0.1:4562/${username}-alerts`;
      initWebSocket(address);
      setApiError(null);
      navigate('/profile');
    } catch (error) {
      console.error('Login Page - Login failed');
      setApiError('Login failed. Please check your credentials and try again.');
    }
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h2>Welcome to Headcount Allocation</h2>

        {uiError && <div className="ui-error">{uiError}</div>}

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
        <button className="login-button" onClick={handleLogin}>Login</button>
      </div>
    </div>
  );
};

export default LoginPage;