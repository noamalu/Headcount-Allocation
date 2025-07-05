import React from 'react';
import '../../Styles/Layout.css'
import { NavLink } from 'react-router-dom';
import { useAuth } from '../../Context/AuthContext';

const Sidebar = () => {
  const { isAdmin } = useAuth();
  return (
    <div className="sidebar">
      <ul>
        <li>
          <NavLink to="/statistics" className={getActiveClass}>Dashboard</NavLink>
        </li>
        <li>  
          <NavLink to="/projects" className={getActiveClass}>Projects</NavLink>
        </li>
        {isAdmin && (
          <li>
            <NavLink to="/employees" className={getActiveClass}>Employees</NavLink>
          </li>
        )}
        <li>
          <NavLink to="/calendar" className={getActiveClass}>Calendar</NavLink>
        </li>
        <li>
          <NavLink to="/tickets" className={getActiveClass}>Tickets</NavLink>
        </li>
        
      </ul>
    </div>
  );
};

const getActiveClass = ({ isActive }: { isActive: boolean }) => 
  isActive ? 'active' : '';

export default Sidebar;