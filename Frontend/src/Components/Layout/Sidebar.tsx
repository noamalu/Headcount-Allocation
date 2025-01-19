import React from 'react';
import '../../Styles/Layout.css'

const Sidebar = () => {
  return (
    <div className="sidebar">
      <ul>
        <li className="active">Dashboard</li>
        <li>Projects</li>
        <li>Employees</li>
        <li>Notifications</li>
      </ul>
    </div>
  );
};

export default Sidebar;

