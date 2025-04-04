import React, { useState } from 'react';
import { Employee } from '../../../Types/EmployeeType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';

interface EmployeeDetailsModalProps {
    employee: Employee;
    onClose: () => void;
  }
  
  const EmployeeDetailsModal: React.FC<EmployeeDetailsModalProps> = ({ employee, onClose }) => {
    const [isEditMode, setIsEditMode] = useState(false);
  
    return (
      <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>{employee.employeeName}</h2>
          <div className="modal-info">
            <p><strong>Phone:</strong> {employee.phoneNumber}</p>
            <p><strong>Email:</strong> {employee.email || '-'}</p>
            <p><strong>Years of Experience:</strong> {employee.yearsExperience} years</p>
            <p><strong>Job Percentage:</strong> {employee.jobPercentage}%</p>
            <p><strong>Time Zone:</strong> {employee.timeZone}</p>
          </div>
  
          <h3>Skills</h3>
          <ul>
            {employee.skills.map((skill, index) => (
              <li key={index}>{skill.skillTypeId} - Level: {skill.level} (Priority: {skill.priority})</li>
            ))}
          </ul>
  
          <h3>Languages</h3>
          <ul>
            {employee.foreignLanguages.map((lang, index) => (
              <li key={index}>Language ID: {lang.languageId} - Level: {lang.level}</li>
            ))}
          </ul>
  
          <h3>Roles</h3>
          <ul>
            {employee.roles.map((role, index) => (
              <li key={index}>{role.roleName} - Project ID: {role.projectId}</li>
            ))}
          </ul>
  
          <div className="modal-actions">
            <button className="edit-button" onClick={() => setIsEditMode(true)}>Edit</button>
            <button className="delete-button">Delete</button>
          </div>
        </div>
      </div>
    );
  };
  
  export default EmployeeDetailsModal;
  