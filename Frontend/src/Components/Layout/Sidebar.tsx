import React from 'react';
import '../../Styles/Layout.css'
import { NavLink } from 'react-router-dom';

const Sidebar = () => {
  return (
    <div className="sidebar">
      <ul>
        <li className="active">Dashboard</li>
        <li>  
          <NavLink to="/projects" className={getActiveClass}>Projects</NavLink>
        </li>
        <li>  
          <NavLink to="/employees" className={getActiveClass}>Employees</NavLink>
        </li>
        <li>Calendar</li>
        <li>Tickets</li>
        <li>Notifications</li>
      </ul>
    </div>
  );
};

const getActiveClass = ({ isActive }: { isActive: boolean }) => 
  isActive ? 'active' : '';

export default Sidebar;

