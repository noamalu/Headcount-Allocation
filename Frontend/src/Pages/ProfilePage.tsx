// src/Pages/ProfilePage.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import { Employee } from '../Types/EmployeeType'; 
import '../Styles/Profile.css';
import '../Styles/Shared.css';

import EmployeesService, { getEmployeeRolesById } from '../Services/EmployeesService';
import { Role } from '../Types/RoleType';
import RoleDetailsModal from '../Components/Features/Roles/RoleDetailsModal';
import { useDataContext } from '../Context/DataContext';
import { getTimeZoneStringByIndex } from '../Types/EnumType';

const ProfilePage: React.FC = () => {
  const { currentId, currentUser, isAdmin, logout } = useAuth();
  const navigate = useNavigate();
  const [user, setUser] = useState<Employee | null>(null);
  // const [roles, setRoles] = useState<Role[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [apiError, setApiError] = useState<string | null>(null);
  const { roles, } = useDataContext();
  const employeeRoles = roles.filter((r) => r.employeeId === currentId);


  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);

  useEffect(() => {
    console.log("ProfilePage loaded");
    console.log("Current ID from AuthContext:", currentId);
    const fetchUser = async () => {
      try {
        console.log("Attempting to fetch user data...");
        const employeeData = await EmployeesService.getEmployeeById(currentId);
        console.log("Employee data received:", employeeData);
        setUser(employeeData);  
      } catch (error) {
        console.error('Error fetching employee data:', error);
        setApiError('Failed to fetch user profile');
      }
    };
    if (currentId != null && currentId >= 0) {
      fetchUser()
    }
  }, [currentId]);

  // useEffect(() => {
  //   console.log("Start fetch user roles");
  //   const fetchEmployeeRoles = async () => {
  //     try {
  //       if (user != null) {
  //         const response = await getEmployeeRolesById(user.employeeId);
  //         user.roles = response;
  //         setRoles(response);
  //         setLoading(false);
  //       } 
  //     } catch (err: any) {
  //       console.error('Error fetching employee roles:', err);
  //       setApiError('Failed to fetch roles');
  //       setLoading(false);
  //     }
  //   };
  //   if (user) {
  //     fetchEmployeeRoles()
  //   }
  // }, [currentId]);

  if (!user) {
    return <div>Loading profile...</div>;
  }

   
  const handleOpenModal = (role: Role) => {
    console.log("Opening role modal for:", role.roleName, "Role data:", role);
    setSelectedRole(role);
  };
  
  const handleCloseModal = () => {
    setSelectedRole(null);
  }; 

  const handleLogout = () => {
    logout();
    navigate('/login');
  };


  if (selectedRole) {
    console.log("Selected role being passed to RoleDetailsModal:", selectedRole);
  }

  return (
    <div className="profile-page">
      <h1 className="page-title">👤 My Profile</h1>

      <div className="profile-card">
        <div className="profile-header">
          <i className="fas fa-user-circle avatar-icon" />
          <h2 className="username">{user.employeeName}</h2>
          {isAdmin && (
          <span className="role-badge">Administrator</span>)}
        </div>

        <div className="profile-details">
          <div className="detail-row"><span>Email:</span> {user.email}</div>
          <div className="detail-row"><span>Phone:</span> {user.phoneNumber}</div>
          <div className="detail-row"><span>Experience:</span> {user.yearsExperience} years</div>
          <div className="detail-row"><span>Job Percentage:</span> {(user.jobPercentage * 100).toFixed(0)}%</div>
          <div className="detail-row"><span>Time Zone:</span> {getTimeZoneStringByIndex(user.timeZone)}</div>
        </div>

        <div className="profile-details">
          <table className="roles-table ">
            <thead>
              <tr>
                  <th>Role Name</th>
                  <th>Project ID</th>
                  <th>Role</th>
              </tr>
            </thead>
            <tbody>
              {employeeRoles && employeeRoles.length > 0 ? (
                  employeeRoles.map((role, index) => (
                  <tr key={index}>
                      <td>{role.roleName}</td>
                      <td>{role.projectId}</td>
                      <td>
                        <button className="action-button" onClick={() => handleOpenModal(role)}>
                          🔗
                        </button>
                      </td>
                  </tr>
                  ))
              ) : (
                  <tr>
                    <td colSpan={3} className="no-roles">No roles available for this user.</td>
                  </tr>
              )}
            </tbody>
          </table>
        </div>
        <button className="logout-button" onClick={handleLogout}>Logout</button>
        {selectedRole && (
          <RoleDetailsModal 
          projectId={selectedRole.projectId} 
          roleId={selectedRole.roleId} 
          onClose={handleCloseModal}/>
        )}

      </div>
    </div>
  );
};

export default ProfilePage;