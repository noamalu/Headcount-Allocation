import React, { useState, useEffect } from 'react';
import AssignEmployeeModal from './AssignEmployeeModal';
import { Role } from '../../../Types/RoleType';
import {Skill} from '../../../Types/SkillType';
import '../../../Styles/Modal.css';
import '../../../Styles/DetailsModal.css';
import '../../../Styles/Shared.css';
import { Employee } from '../../../Types/EmployeeType';


interface EditRoleModalProps {
  projectId: number;
  role: Role;
  onClose: () => void;
  onSave: (newRole: Role) => void; 
}

const EditRoleModal: React.FC<EditRoleModalProps> = ({projectId, role, onClose, onSave }) => {
  const [editedRole, setEditedRole] = useState<Role>({ ...role });
  const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      const { name, value } = e.target;
      setEditedRole({ ...editedRole, [name]: value });
    };
  
  useEffect(() => {
    console.log("RoleDetailsModal mounted or updated for role:", role);
  }, [role]);

  const handleSave = () => {
    onSave(editedRole);
    onClose();
  };

  const handleAssign = (employee: Employee) => {
    console.log(`Assigned ${employee.employeeId} to role ${role.roleName}`);
    // כאן תוכל להוסיף לוגיקה של עדכון Backend או Frontend
  };


  return (
    <div className="modal-overlay role-modal">
      <div className="modal-content role-modal">
        <button className="close-button" onClick={onClose}>✖</button>
        <input
            type="text"
            id="roleName"
            name="roleName"
            value={editedRole.roleName}
            onChange={handleInputChange}
            className="input-as-h2-field"
          />

        <div className="employee-info">
          <span className="employee-avatar">👤</span>
          <p className="employee-name">{role.employeeId || "No employee assigned"}</p>
        </div>

        {/* פרטי התפקיד */}
        <div className="role-details">
          <div className="detail-banner-edit">
            <i className="fas fa-globe" ></i>
            <span>
              <strong>Time Zone:</strong> 
              <input
                type="number"
                id="timeZone"
                name="timeZone"
                value={editedRole.timeZone}
                onChange={handleInputChange}
                className="input-field"
              />
            </span>
          </div>
          <div className="detail-banner-edit">
            <i className="fas fa-briefcase"></i>
            <span>
              <strong>Years of Experience:</strong> 
              <input
                type="number"
                id="yearsOfExperience"
                name="yearsOfExperience"
                value={editedRole.yearsExperience}
                onChange={handleInputChange}
                className="input-field"
              />
            </span>
          </div>
          <div className="detail-banner-edit">
            <i className="fas fa-percentage"></i>
            <span>
              <strong>Job Percentage:</strong>
              <input
                type="percent"
                id="jobPercentage"
                name="jobPercentage"
                value={editedRole.jobPercentage}
                onChange={handleInputChange}
                className="input-field"
              />
               </span>
          </div>
          <div className="detail-banner-edit">
            <i className="fas fa-language"></i>
            <span><strong>Foreign Languages:</strong> {role.foreignLanguages.length > 0 ? role.foreignLanguages.join(', ') : "None"}</span>
          </div>
        </div>
          <div className="detail-banner-edit">
            <i className="fas fa-align-left"></i>
              <span>
              <strong>Description:</strong> 
              <textarea
              id="description"
              name="description"
              value={editedRole.description}
              onChange={handleInputChange}
              className="textarea-field"
              />
              </span>
          </div>
        {/* טבלת מאפיינים */}
        <div className="skills-section">
          <table className="skills-table">
            <thead>
              <tr>
                <th>Skill</th>
                <th>Required ranking</th>
                <th>Priority</th>
                <th>Employee’s ranking</th>
              </tr>
            </thead>
            <tbody>
            {role.skills.map((skill) => (
                <tr key={skill.skillId}>
                  <td>{skill.skillTypeId}</td>
                  <td>{skill.level}</td>
                  <td>{skill.priority}</td>
                  <td> {0}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* כפתורי פעולה */}
        <div className="modal-actions">
          <button className="delete-button">🗑 Delete</button>
          <button onClick={() => setIsAssignModalOpen(true)} className="assign-button">👤 Assign Employee</button>
          <button className="edit-button">✏ Edit</button>
        </div>
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
    </div>
  );
};

export default EditRoleModal;