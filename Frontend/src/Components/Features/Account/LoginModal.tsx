import React, { useState } from 'react';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';

interface LoginModalProps {
  onClose: () => void;
  onLogin: (username: string) => void;
}

const LoginModal: React.FC<LoginModalProps> = ({ onClose, onLogin }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [showPassword, setShowPassword] = useState(false);

  const handleLogin = () => {
    if (!username || !password) {
      setError('Please enter both username and password.');
      return;
    }

    // Simulate successful login
    onLogin(username);
    onClose();
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content login-modal">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>Login</h2>

        <div className="modal-info">
          <label>Username:</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="input-field"
          />
        </div>

        <div className="modal-info">
          <label>Password:</label>
          <input
            type={showPassword ? 'text' : 'password'}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="input-field"
          />
          <div className="login-options">
            <label className="show-password">
              <input
                type="checkbox"
                checked={showPassword}
                onChange={(e) => setShowPassword(e.target.checked)}
              />
              Show Password
            </label>
            <button
              className="forgot-password"
              onClick={() => alert('Redirect to forgot password flow')}>
              Forgot password?
            </button>
          </div>
        </div>

        {error && <p className="error-text">{error}</p>}

        <div className="modal-actions">
          <button className="edit-button" onClick={handleLogin}>Login</button>
        </div>
      </div>
    </div>
  );
};

export default LoginModal;
