import React, { useState, useEffect } from 'react';
import AssignEmployeeModal from './AssignEmployeeModal';
import EditRoleModal from './EditRoleModal';
import { Role } from '../../../Types/RoleType';
import { Language, formateLanguage } from '../../../Types/LanguageType';
import '../../../Styles/Modal.css';
import '../../../Styles/DetailsModal.css';
import '../../../Styles/Shared.css';
import { formateSkillToString } from '../../../Types/SkillType';
import EmployeesService from '../../../Services/EmployeesService';
import { Employee } from '../../../Types/EmployeeType';


interface RoleDetailsModalProps {
  projectId: number;
  role: Role;
  onClose: () => void;
  onSave?: (newRole: Role) => void; // Add this line
  onAssignEmployeeToRole: (roleId: number, employeeId: number) => void}

const RoleDetailsModal: React.FC<RoleDetailsModalProps> = ({projectId,  role, onClose, onSave, onAssignEmployeeToRole}) => {
  const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const [error, setError] = useState<string>(""); 
  

  useEffect(() => {
    if (role.employeeId != -1 && selectedEmployee == null) {
      handleEmployeeDetails(role.employeeId);
    }
  }, [role]);

  useEffect(() => {
    if (selectedEmployee) {
      console.log("Selected employee updated:", selectedEmployee);
    }
  }, [selectedEmployee]);

  const handleAssign = async (employee: Employee) => {
    console.log(`Assigned ${employee.employeeName} to role ${role.roleName}`);
    try {
      role.employeeId = employee.employeeId;
      console.log("selectedEmployeeName before change: " + selectedEmployee?.employeeName + ", will be changed to: " + employee.employeeName);
      setSelectedEmployee({ ...employee });
      console.log("selectedEmployeeName after change: " + selectedEmployee?.employeeName);
      const res = await EmployeesService.assignEmployeeToRole(employee.employeeId, role);
      onAssignEmployeeToRole(role.roleId, employee.employeeId);
      console.log('employee assigned successfully:', employee.employeeId);
    } catch (error) {
        console.error('Error assigning employee:', error);
      setError('An error occurred while assigning the employee');
    }
  };

  const handleEmployeeDetails = async (employeeId: number) => {
    console.log(`handleEmployeeDetails: ${employeeId}`);
    try {
      const employee =  await EmployeesService.getEmployeeById(employeeId);
      console.log("selectedEmployeeName before change: " + selectedEmployee);
      console.log("selectedEmployeeName should be: " + employee);
      setSelectedEmployee({ ...employee });
      console.log("selectedEmployeeName should be after: " + employee);
      console.log("selectedEmployeeName after change: " + selectedEmployee);
    } catch (error) {
        console.error('Error assigning employee:', error);
      setError('An error occurred while assigning the employee');
    }
  };

  const handleCloseModal = () => {
  };

    const handleEditSave = (updatedRole: Role) => {
      console.log('Role updated:', updatedRole);
      // Update the role details here (e.g., send to API or update state)
    };

  

  return (
    <div className="modal-overlay details-modal">
      <div className="modal-content details-modal">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2 className="role-name">{role.roleName}</h2>      
        <div className="employee-info">
          <span className="employee-avatar">👤</span>
          <p className="employee-name">
            {selectedEmployee != null ? selectedEmployee.employeeName : role.employeeId != -1 ? role.employeeId : "No employee assigned"}
          </p>
        </div>
        <div className="details-section">
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
        <div className="details-section">
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
                    role.skills.map((skill, index) => (
                      <tr key={index}>
                        <td>{formateSkillToString(skill.skillTypeId)}</td>  
                        <td>{skill.level}</td>
                        <td>{skill.priority}</td>
                        <td>
                          {selectedEmployee?
                             selectedEmployee.skills.find((empSkill) => empSkill.skillTypeId === skill.skillTypeId)
                              ? selectedEmployee.skills.find((empSkill) => empSkill.skillTypeId === skill.skillTypeId)!.level
                              : "-" 
                            : "-"} 
                        </td>
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
          </button>        
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
      {isEditModalOpen && ( 
          <EditRoleModal
            projectId={projectId}
            role={role}
            onClose={() => setIsEditModalOpen(false)}
            onSave={handleEditSave} />
          )}
    </div>
  );
};

export default RoleDetailsModal;