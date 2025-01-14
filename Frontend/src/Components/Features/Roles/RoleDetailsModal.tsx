import React, { useState, useEffect } from 'react';
import AssignEmployeeModal from './AssignEmployeeModal';
import { Role } from '../../../Types/RoleType';
import '../../../Styles/Modal.css';


interface RoleDetailsModalProps {
  role: Role;
  onClose: () => void;
}

const RoleDetailsModal: React.FC<RoleDetailsModalProps> = ({ role, onClose }) => {
  const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);

  useEffect(() => {
    console.log("RoleDetailsModal mounted or updated for role:", role);
  }, [role]);

  const handleAssign = (newEmployee: string) => {
    console.log(`Assigned ${newEmployee} to role ${role.name}`);
    // כאן תוכל להוסיף לוגיקה של עדכון Backend או Frontend
  };

  return (
    <div className="modal-overlay role-modal">
      <div className="modal-content role-modal">
        {/* כפתור סגירה */}
        <button className="close-button" onClick={onClose}>✖</button>
        
        {/* כותרת עם שם התפקיד */}
        <h2 className="role-name">{role.name}</h2>
        
        {/* שם העובד המשויך */}
        <div className="employee-info">
          <span className="employee-avatar">👤</span>
          <p className="employee-name">{role.employee || "No employee assigned"}</p>
        </div>

        {/* תיאור התפקיד */}
        <div className="role-description">
          <p>{role.description}</p>
        </div>

        {/* טבלת מאפיינים */}
        <div className="attributes-section">
          <table className="attributes-table">
            <thead>
              <tr>
                <th>Attributes</th>
                <th>Required ranking</th>
                <th>Priority</th>
                <th>Employee’s ranking</th>
              </tr>
            </thead>
            <tbody>
              {role.attributes.map((attr, index) => (
                <tr key={index}>
                  <td>{attr.attribute}</td>
                  <td>{attr.requiredRank}</td>
                  <td>{attr.priority}</td>
                  <td>{attr.requiredRank}</td> {/* נניח שהמידע של הדירוג עובד בא מהדרישה */}
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* כפתורי פעולה */}
        <div className="modal-actions">
          <button className="delete-button">🗑 Delete</button>
          <button onClick={() => setIsAssignModalOpen(true)} className="assign-button">Assign Employee</button>
          <button className="edit-button">✏ Edit</button>
        </div>
      </div>

      {/* חלון שיוך עובד */}
      {isAssignModalOpen && (
        <AssignEmployeeModal
          role={role.name}
          onClose={() => setIsAssignModalOpen(false)}
          onAssign={handleAssign}
        />
      )}
    </div>
  );
};

export default RoleDetailsModal;