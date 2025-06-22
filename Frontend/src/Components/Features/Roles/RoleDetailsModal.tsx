import React, { useState, useEffect } from 'react';
import AssignEmployeeModal from './AssignEmployeeModal';
import EditRoleModal from './EditRoleModal';
import { formatDate, Role } from '../../../Types/RoleType';
import { Language, formateLanguage } from '../../../Types/LanguageType';
import '../../../Styles/Modal.css';
import '../../../Styles/DetailsModal.css';
import '../../../Styles/Shared.css';
import { formateSkillToString } from '../../../Types/SkillType';
import EmployeesService from '../../../Services/EmployeesService';
import ProjectsService from '../../../Services/ProjectsService';
import { Employee } from '../../../Types/EmployeeType';
import ManualAssignEmployeeModal from './ManualAssignEmployeeModal';
import { useDataContext } from '../../../Context/DataContext';
import { getTimeZoneStringByIndex } from '../../../Types/EnumType';


interface RoleDetailsModalProps {
  projectId: number;
  roleId: number;
  onClose: () => void;
  onSave?: (newRole: Role) => void; // Add this line
  // onAssignEmployeeToRole?: (roleId: number, employeeId: number) => void
}

const RoleDetailsModal: React.FC<RoleDetailsModalProps> = ({ projectId, roleId, onClose, onSave }) => {
  const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);
  const [isManualAssignModalOpen, setIsManualAssignModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  // const [assignedEmployee, setassignedEmployee] = useState<Employee | null>(null);
  const [showConfirmDelete, setShowConfirmDelete] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);
  const {roles, updateRole, deleteRole, employees } = useDataContext();
  const role = roles.find(r => r.roleId === roleId);
  const assignedEmployee = employees.find(e => e.employeeId === role?.employeeId);


useEffect(() => {
  if (apiError) {
    alert(apiError);
  }
}, [apiError]);
  

  // useEffect(() => {
  //   if (role.employeeId != -1 && role.employeeId != null && assignedEmployee == null) {
  //     console.log("Selected employee id:", assignedEmployee);
  //     console.log("Role employee id:", role.employeeId);
  //     handleEmployeeDetails(role.employeeId);
  //   }
  // }, [role]);

  // useEffect(() => {
  //   if (assignedEmployee) {
  //     console.log("Selected employee updated:", assignedEmployee);
  //   }
  // }, [assignedEmployee]);

  // const handleAssign = async (employee: Employee) => {
  //   console.log(`Assigned ${employee.employeeName} to role ${role.roleName}`);
  //   try {
  //     role.employeeId = employee.employeeId;
  //     console.log("assignedEmployeeName before change: " + assignedEmployee?.employeeName + ", will be changed to: " + employee.employeeName);
  //     setassignedEmployee({ ...employee });
  //     console.log("assignedEmployeeName after change: " + assignedEmployee?.employeeName);
  //     const res = await EmployeesService.assignEmployeeToRole(employee.employeeId, role);
  //     onAssignEmployeeToRole?.(role.roleId, employee.employeeId);
  //     console.log('employee assigned successfully:', employee.employeeId);
  //   } catch (error) {
  //     console.error('Error assigning employee:', error);
  //     setApiError('An error occurred while assigning the employee');
  //   }
  // };

  const handleAssign = async (employee: Employee) => {
    if (role) {
      try {
        const updatedRole = {...role, employeeId: employee.employeeId };
        await EmployeesService.assignEmployeeToRole(employee.employeeId, updatedRole);
        updateRole(updatedRole);
      } catch (error) {
        console.error('Error assigning employee:', error);
        setApiError('An error occurred while assigning the employee');
      }
    }   
  };

  // const handleEmployeeDetails = async (employeeId: number) => {
  //   console.log(`handleEmployeeDetails: ${employeeId}`);
  //   try {
  //     const employee =  await EmployeesService.getEmployeeById(employeeId);
  //     console.log("assignedEmployeeName before change: " + assignedEmployee);
  //     setassignedEmployee({ ...employee });
  //   } catch (error) {
  //     console.error('Error fetching employee details:', error);
  //     setApiError('An error occurred while fetching employee details');
  //   }
  // };

  const handleCloseModal = () => {
  };

    const handleEditSave = (updatedRole: Role) => {
      console.log('Role updated:', updatedRole);
      // Update the role details here (e.g., send to API or update state)
    };

  const handleDelete = async () => {
    try {
      await ProjectsService.deleteRole(roleId, projectId);
      deleteRole(roleId);
      onClose(); 
    } catch (error) {
      alert("Failed to delete the role.");
      console.error(error);
    }
  };
  
  return ( role &&
    <div className="modal-overlay details-modal">
      <div className="modal-content details-modal">
        <button className="close-button" onClick={onClose}>✖</button>
      
        <h2 className="role-name">{role?.roleName}</h2>
        
        <div className="employee-info">
          <span className="employee-avatar">👤</span>
          <p className="employee-name">
            {assignedEmployee ? assignedEmployee.employeeName : role?.employeeId !== -1 ? role?.employeeId : "No employee assigned"}
          </p>
        </div>

        
        <div className="details-section">
          <div className="detail-banner">
            <i className="fas fa-calendar-alt" ></i>
            <span><strong>Start Date:</strong> {formatDate(role?.startDate)}</span>
          </div>
          <div className="detail-banner">
            <i className="fas fa-globe" ></i>
            <span><strong>Time Zone:</strong> {getTimeZoneStringByIndex(role?.timeZone)}</span>
          </div>
          <div className="detail-banner">
            <i className="fas fa-briefcase"></i>
            <span><strong>Years of Experience:</strong> {role?.yearsExperience}</span>
          </div>
          <div className="detail-banner">
            <i className="fas fa-percentage"></i>
            <span><strong>Job Percentage:</strong> {(role? role.jobPercentage * 100 : 0).toFixed(0)}%</span>
          </div>
        </div>
        <div className="details-section">
          <div className="detail-banner">
            <i className="fas fa-align-left"></i>
            <span><strong>Description:</strong> {role?.description}</span>
          </div>

          {/* Languages Section */}
          <div className="detail-banner">
            <i className="fas fa-language"></i>
            <strong>Foreign Languages:</strong>
            <div className="languages-list">
              {role && role.foreignLanguages && Object.keys(role?.foreignLanguages).length > 0 ? (
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
                  {role && role.skills && Object.keys(role.skills).length > 0 ? (
                    role.skills.map((skill, index) => (
                      <tr key={index}>
                        <td>{formateSkillToString(skill.skillTypeId)}</td>  
                        <td>{skill.level}</td>
                        <td>{skill.priority}</td>
                        <td>
                          {assignedEmployee?
                             assignedEmployee.skills.find((empSkill) => empSkill.skillTypeId === skill.skillTypeId)
                              ? assignedEmployee.skills.find((empSkill) => empSkill.skillTypeId === skill.skillTypeId)!.level
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

        
        <div className="modal-actions">
          <button className="delete-button" onClick={() => setShowConfirmDelete(true)}>
            <i className="fas fa-trash"></i> Delete
          </button>
          <button onClick={() => setIsAssignModalOpen(true)} className="assign-button">👤 Assign Employee</button>
          <button className="edit-button" onClick={() => { console.log('Opening edit modal:', !isEditModalOpen); setIsEditModalOpen(true); }}>
            <i className="fas fa-pen"></i> Edit
          </button>
        </div>
      </div>


      {role && isAssignModalOpen && (
        <AssignEmployeeModal
          projectId={projectId}
          roleId={role.roleId}
          onClose={() => setIsAssignModalOpen(false)}
          // onAssign={handleAssign}
          openManualAssignModal={() => setIsManualAssignModalOpen(true)}
        />
      )}

      {role && isManualAssignModalOpen && (
        <ManualAssignEmployeeModal
          projectId={projectId}
          roleId={role.roleId}
          onClose={() => setIsManualAssignModalOpen(false)}
          // onAssign={handleAssign}
        />
      )}
      {role && isEditModalOpen && ( 
          <EditRoleModal
            projectId={projectId}
            role={role}
            employeeName={assignedEmployee != null ? assignedEmployee.employeeName : ""}
            onClose={() => setIsEditModalOpen(false)} />
            // onSave={handleEditSave} />
          )}
          {showConfirmDelete && (
          <div className="confirm-overlay">
            <div className="confirm-dialog">
              <p>Are you sure you want to delete this role?</p>
              <div className="confirm-buttons">
                <button className="confirm-button" onClick={handleDelete}>
                  <i className="fas fa-trash"></i> Delete
                </button>
                <button className="cancel-button" onClick={() => setShowConfirmDelete(false)}>
                <i className="fas fa-close"></i> Cancel
                </button>
              </div>
            </div>
          </div>
        )}
    </div>
  );
};

export default RoleDetailsModal;