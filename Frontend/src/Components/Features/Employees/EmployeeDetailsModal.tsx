import React, { useState } from 'react';
import { Employee } from '../../../Types/EmployeeType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { formateLanguage } from '../../../Types/LanguageType';
import { formateSkillToString } from '../../../Types/SkillType';

interface EmployeeDetailsModalProps {
    employee: Employee;
    onClose: () => void;
  }
  
  const EmployeeDetailsModal: React.FC<EmployeeDetailsModalProps> = ({ employee, onClose }) => {
    const [isEditMode, setIsEditMode] = useState(false);
  
    return (
      <div className="modal-overlay details-modal">
        <div className="modal-content details-modal">
          <button className="close-button" onClick={onClose}>✖</button>
          <span className="employee-avatar">👤</span>
          <h2 className="employee-name">{employee.employeeName}</h2>
          <div className="details-section">
            <div className="detail-banner">
                <i className="fas fa-phone"></i>
                <span><strong>Phone:</strong> {employee.phoneNumber}</span>
            </div>
            <div className="detail-banner">
                <i className="fa-solid fa-envelope"></i>
                <span><strong>Email:</strong> {employee.email || '-'}</span>
            </div>
            <div className="detail-banner">
                <i className="fas fa-briefcase"></i>
                <span><strong>Years of Experience:</strong> {employee.yearsExperience}</span>
            </div>
            <div className="detail-banner">
                <i className="fas fa-percentage"></i>
                <span><strong>Job Percentage:</strong> {employee.jobPercentage * 100}%</span>
            </div>
            <div className="detail-banner">
                <i className="fas fa-globe" ></i>
                <span><strong>Time Zone:</strong> {employee.timeZone}</span>
            </div>
          </div>
  
        
          {/* Skills Section */}
          <div className="detail-banner">
            <div className="skills-section">
                <i className="fas fa-tools"></i>
                <strong> Skills:</strong>
                <table className="skills-table">
                    <thead>
                        <tr>
                            <th>Skill</th>
                            <th>Ranking</th>
                        </tr>
                    </thead>
                    <tbody>
                        {employee.skills.length > 0 ? (
                            employee.skills.map((skill, index) => (
                            <tr key={index}>
                                <td>{formateSkillToString(skill.skillTypeId)}</td>
                                <td>{skill.level}</td>
                            </tr>
                            ))
                        ) : (
                            <tr>
                            <td colSpan={3}>No skills available</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
          </div>

          {/* Languages Section */}
            <div className="detail-banner">
                <i className="fas fa-language"></i>
                <strong>Foreign Languages:</strong>
                <div className="languages-list">
                {employee.foreignLanguages && Object.keys(employee.foreignLanguages).length > 0 ? (
                    Object.entries(employee.foreignLanguages).map(([key, lang]) => (
                    <div key={key} className="language-item">
                        <strong>{formateLanguage(lang.languageTypeId)}</strong>: Level {lang.level}
                    </div>
                    ))
                ) : (
                    <span className="no-data">No foreign languages</span>
                )}
                </div>
            </div>
  
          {/* <h3>Roles</h3>
          <ul>
            {employee.roles.map((role, index) => (
              <li key={index}>{role.roleName} (Project ID: {role.projectId})</li>
            ))}
          </ul>
   */}
          <div className="modal-actions">
            <button className="edit-button" onClick={() => setIsEditMode(true)}>Edit</button>
            <button className="delete-button">Delete</button>
          </div>
        </div>
      </div>
    );
  };
  
  export default EmployeeDetailsModal;
  