import React, { useState, useEffect } from 'react';
import AssignEmployeeModal from './AssignEmployeeModal';
import EditRoleModal from './EditRoleModal';
import { Role } from '../../../Types/RoleType';
import { Language, formateLanguage } from '../../../Types/LanguageType';
import '../../../Styles/Modal.css';
import '../../../Styles/RoleModal.css';
import '../../../Styles/Shared.css';
import { formateSkillToString } from '../../../Types/SkillType';


interface RoleDetailsModalProps {
  projectId: number;
  role: Role;
  onClose: () => void;
  onSave?: (newRole: Role) => void; // Add this line
}

const RoleDetailsModal: React.FC<RoleDetailsModalProps> = ({projectId,  role, onClose }) => {
  const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);

  useEffect(() => {

    console.log("RoleDetailsModal mounted or updated for role:", role);
    console.log("Type of foreignLanguages:", typeof role.foreignLanguages);
    console.log("Is foreignLanguages an array?:", Array.isArray(role.foreignLanguages));
    console.log("Content of foreignLanguages:", role.foreignLanguages);
  }, [role]);

  const handleAssign = (employeeId: number) => {
    console.log(`Assigned ${employeeId} to role ${role.roleName}`);
    // כאן תוכל להוסיף לוגיקה של עדכון Backend או Frontend
  };

  const handleCloseModal = () => {
  };

    const handleEditSave = (updatedRole: Role) => {
      console.log('Role updated:', updatedRole);
      // Update the role details here (e.g., send to API or update state)
    };

  

  return (
    <div className="modal-overlay role-modal">
      <div className="modal-content role-modal">
        {/* כפתור סגירה */}
        <button className="close-button" onClick={onClose}>✖</button>
        
        {/* כותרת עם שם התפקיד */}
        <h2 className="role-name">{role.roleName}</h2>
        
        {/* שם העובד המשויך */}
        <div className="employee-info">
          <span className="employee-avatar">👤</span>
          <p className="employee-name">{role.employeeId || "No employee assigned"}</p>
        </div>

        {/* פרטי התפקיד */}
        <div className="role-details">
          <div className="detail-banner">
            <i className="fas fa-globe" ></i>
            <span><strong>Time Zone:</strong> {role.timeZone}</span>
          </div>
          <div className="detail-banner">
            <i className="fas fa-briefcase"></i>
            <span><strong>Years of Experience:</strong> {role.yearsExperience}</span>
          </div>
          <div className="detail-banner">
            <i className="fas fa-percentage"></i>
            <span><strong>Job Percentage:</strong> {role.jobPercentage * 100}%</span>
          </div>
        </div>
        <div className="role-details">
          <div className="detail-banner">
            <i className="fas fa-align-left"></i>
            <span><strong>Description:</strong> {role.description}</span>
          </div>

          {/* Languages Section */}
          <div className="detail-banner">
            <i className="fas fa-language"></i>
            <strong>Foreign Languages:</strong>
            <div className="languages-list">
              {role.foreignLanguages && Object.keys(role.foreignLanguages).length > 0 ? (
                Object.entries(role.foreignLanguages).map(([key, lang]) => (
                  <div key={key} className="language-item">
                    <strong>{formateLanguage(lang.languageTypeId)}</strong>: Level {lang.level}
                  </div>
                ))
              ) : (
                <span className="no-data">No foreign languages</span>
              )}
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
                    <th>Required Ranking</th>
                    <th>Priority</th>
                    <th>Employee’s Ranking</th>
                  </tr>
                </thead>
                <tbody>
                  {role.skills && Object.keys(role.skills).length > 0 ? (
                    Object.entries(role.skills).map(([key, skill]) => (
                      <tr key={key}>
                        <td>{formateSkillToString(skill.SkillTypeId)}</td>
                        <td>{skill.level}</td>
                        <td>{skill.priority}</td>
                        <td>0</td> {/* Employee ranking - ערך לדוגמה */}
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan={4}>No skills available</td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
          </div>

        {/* כפתורי פעולה */}
        <div className="modal-actions">
          <button className="delete-button">🗑 Delete</button>
          <button onClick={() => setIsAssignModalOpen(true)} className="assign-button">👤 Assign Employee</button>
          <button className="edit-button" onClick={() => { console.log('Opening edit modal:', !isEditModalOpen); setIsEditModalOpen(true); }}>
            <i className="fas fa-pen"></i> Edit
          </button>        </div>
      </div>

      {/* חלון שיוך עובד */}
      {isAssignModalOpen && (
        <AssignEmployeeModal
          projectId={projectId}
          roleId={role.roleId}
          onClose={() => setIsAssignModalOpen(false)}
          onAssign={handleAssign}
        />
      )}
      {isEditModalOpen && ( 
          <EditRoleModal
            role={role}
            onClose={() => setIsEditModalOpen(false)}
            onSave={handleEditSave} />
          )}
    </div>
  );
};

export default RoleDetailsModal;