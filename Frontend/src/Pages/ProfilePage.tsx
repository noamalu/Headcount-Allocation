// src/Pages/ProfilePage.tsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import '../Styles/Profile.css';

const ProfilePage: React.FC = () => {
  const { currentUser, isAdmin, logout } = useAuth();
  const navigate = useNavigate();

  // סימולציה של פרטי משתמש (בעתיד – מ-API או הקשר מורחב)
  const user = {
    userName: currentUser,
    email: `${currentUser?.toLowerCase()}@example.com`,
    phoneNumber: '050-1234567',
    yearExp: 5,
    jobPercentage: 80,
    timeZone: 'UTC+2',
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="profile-page">
      <h1 className="page-title">👤 My Profile</h1>

      <div className="profile-card">
        <div className="profile-header">
          <i className="fas fa-user-circle avatar-icon" />
          <h2 className="username">{user.userName}</h2>
          <span className="role-badge">{isAdmin ? 'Administrator' : 'Regular User'}</span>
        </div>

        <div className="profile-details">
          <div className="detail-row"><span>Email:</span> {user.email}</div>
          <div className="detail-row"><span>Phone:</span> {user.phoneNumber}</div>
          <div className="detail-row"><span>Experience:</span> {user.yearExp} years</div>
          <div className="detail-row"><span>Job Percentage:</span> {user.jobPercentage}%</div>
          <div className="detail-row"><span>Time Zone:</span> {user.timeZone}</div>
        </div>

        <button className="logout-button" onClick={handleLogout}>Logout</button>
      </div>
    </div>
  );
};

export default ProfilePage;