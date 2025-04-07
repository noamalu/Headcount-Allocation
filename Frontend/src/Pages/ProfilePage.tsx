// src/Pages/ProfilePage.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import { Employee } from '../Types/EmployeeType'; 
import '../Styles/Profile.css';
import EmployeesService from '../Services/EmployeesService';

const ProfilePage: React.FC = () => {
  const { currentId, currentUser, isAdmin, logout } = useAuth();
  const navigate = useNavigate();
  const [user, setUser] = useState<Employee | null>(null);

  useEffect(() => {
    console.log("ProfilePage loaded");
    console.log("Current ID from AuthContext:", currentId);
    const fetchUser = async () => {
      try {
        console.log("Attempting to fetch user data...");
        const employeeData = await EmployeesService.getEmployeeById(currentId);
        console.log("Employee data received:", employeeData);
        setUser(employeeData);  // שמירה ישירה של הנתונים
      } catch (error) {
        console.error('Error fetching employee data:', error);
      }
    };

    if (currentId) {
      fetchUser();
    }
  }, [currentId]);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (!user) {
    return <div>Loading profile...</div>;
  }

  return (
    <div className="profile-page">
      <h1 className="page-title">👤 My Profile</h1>

      <div className="profile-card">
        <div className="profile-header">
          <i className="fas fa-user-circle avatar-icon" />
          <h2 className="username">{user.employeeName}</h2>
          <span className="role-badge">{isAdmin ? 'Administrator' : 'Regular User'}</span>
        </div>

        <div className="profile-details">
          <div className="detail-row"><span>Email:</span> {user.email}</div>
          <div className="detail-row"><span>Phone:</span> {user.phoneNumber}</div>
          <div className="detail-row"><span>Experience:</span> {user.yearsExperience} years</div>
          <div className="detail-row"><span>Job Percentage:</span> {user.jobPercentage * 100}%</div>
          <div className="detail-row"><span>Time Zone:</span> {user.timeZone}</div>
        </div>

        <button className="logout-button" onClick={handleLogout}>Logout</button>
      </div>
    </div>
  );
};

export default ProfilePage;