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
import ManualAssignEmployeeModal from './ManualAssignEmployeeModal';
import { useDataContext } from '../../../Context/DataContext';


interface RoleDetailsModalProps {
  projectId: number;
  role: Role;
  onClose: () => void;
  onSave?: (newRole: Role) => void; // Add this line
  // onAssignEmployeeToRole?: (roleId: number, employeeId: number) => void
}

const RoleDetailsModal: React.FC<RoleDetailsModalProps> = ({ projectId, role, onClose, onSave }) => {
  const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);
  const [isManualAssignModalOpen, setIsManualAssignModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  // const [assignedEmployee, setassignedEmployee] = useState<Employee | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const { updateRole, employees } = useDataContext();
  const assignedEmployee = employees.find(e => e.employeeId === role.employeeId);


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
    try {
      const updatedRole = { ...role, employeeId: employee.employeeId };
      await EmployeesService.assignEmployeeToRole(employee.employeeId, updatedRole);
      updateRole(updatedRole); // גלובלי
    } catch (error) {
      console.error('Error assigning employee:', error);
      setApiError('An error occurred while assigning the employee');
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

  

  return (
    <div className="modal-overlay details-modal">
      <div className="modal-content details-modal">
        <button className="close-button" onClick={onClose}>✖</button>
      
        <h2 className="role-name">{role.roleName}</h2>
        
        <div className="employee-info">
          <span className="employee-avatar">👤</span>
          <p className="employee-name">
            {assignedEmployee ? assignedEmployee.employeeName : role.employeeId !== -1 ? role.employeeId : "No employee assigned"}
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
            <span><strong>Job Percentage:</strong> {(role.jobPercentage * 100).toFixed(0)}%</span>
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
          <button className="delete-button">🗑 Delete</button>
          <button onClick={() => setIsAssignModalOpen(true)} className="assign-button">👤 Assign Employee</button>
          <button className="edit-button" onClick={() => { console.log('Opening edit modal:', !isEditModalOpen); setIsEditModalOpen(true); }}>
            <i className="fas fa-pen"></i> Edit
          </button>
        </div>
      </div>


      {isAssignModalOpen && (
        <AssignEmployeeModal
          projectId={projectId}
          roleId={role.roleId}
          onClose={() => setIsAssignModalOpen(false)}
          onAssign={handleAssign}
          openManualAssignModal={() => setIsManualAssignModalOpen(true)}
        />
      )}

      {isManualAssignModalOpen && (
        <ManualAssignEmployeeModal
          projectId={projectId}
          roleId={role.roleId}
          onClose={() => setIsManualAssignModalOpen(false)}
          onAssign={handleAssign}
        />
      )}
      {isEditModalOpen && ( 
          <EditRoleModal
            projectId={projectId}
            role={role}
            employeeName={assignedEmployee != null ? assignedEmployee.employeeName : ""}
            onClose={() => setIsEditModalOpen(false)}
            onSave={handleEditSave} />
          )}
    </div>
  );
};

export default RoleDetailsModal;