import React, { useEffect, useState } from 'react';
import { Employee } from '../../../Types/EmployeeType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { formateLanguage } from '../../../Types/LanguageType';
import { formateSkillToString } from '../../../Types/SkillType';
import { Role } from '../../../Types/RoleType';
import EmployeesService, { getEmployeeRolesById } from '../../../Services/EmployeesService';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditEmployeeModal from './EditEmployeeModal';
import { useDataContext } from '../../../Context/DataContext';
import { getSkillLabel, getTimeZoneStringByIndex } from '../../../Types/EnumType';


interface EmployeeDetailsModalProps {
    employeeId: number;
    onClose: () => void;
  }
  
  
  const EmployeeDetailsModal: React.FC<EmployeeDetailsModalProps> = ({ employeeId, onClose }) => {
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [selectedRole, setSelectedRole] = useState<Role | null>(null);
    const [showConfirmDelete, setShowConfirmDelete] = useState(false);
    const [loading, setLoading] = useState<boolean>(true);
    const [apiError, setApiError] = useState<string | null>(null);
    const { employees, updateEmployee, deleteEmployee, roles } = useDataContext();
    const currentEmployee = employees.find(e => e.employeeId === employeeId);
    const employeeRoles = roles.filter((r) => r.employeeId === employeeId);


    useEffect(() => {
      if (apiError) {
        alert(apiError);
      }
    }, [apiError]);

   
    const handleDelete = async () => {
      try {
        await EmployeesService.deleteEmployee(employeeId);
        deleteEmployee(employeeId);
        onClose();
      } catch (error) {
        alert("Failed to delete employee.");
        console.error(error);
      }
    };
  
  
   
    const handleOpenModal = (role: Role) => {
        console.log("Opening role modal for:", role.roleName, "Role data:", role);
        setSelectedRole(role);
    };
    
    const handleCloseModal = () => {
      setSelectedRole(null);
    };
    
      
      
    if (selectedRole) {
      console.log("Selected role being passed to RoleDetailsModal:", selectedRole);
    }

    return (
      <div className="modal-overlay details-modal">
        <div className="modal-content details-modal">
          <button className="close-button" onClick={onClose}>✖</button>

          <span className="employee-avatar">👤</span>
          <h2 className="employee-name">{currentEmployee?.employeeName}</h2>
          
          <div className="details-section">
            <div className="detail-banner">
                <i className="fas fa-phone"></i>
                <span><strong>Phone:</strong> {currentEmployee?.phoneNumber}</span>
            </div>
            <div className="detail-banner">
                <i className="fa-solid fa-envelope"></i>
                <span><strong>Email:</strong> {currentEmployee?.email || '-'}</span>
            </div>
            <div className="detail-banner">
                <i className="fas fa-briefcase"></i>
                <span><strong>Years of Experience:</strong> {currentEmployee?.yearsExperience}</span>
            </div>
            <div className="detail-banner">
                <i className="fas fa-percentage"></i>
                <span><strong>Job Percentage:</strong> 
                    {currentEmployee ? `${(currentEmployee.jobPercentage * 100).toFixed(0)}%` : '-'}
                </span>
            </div>
            <div className="detail-banner">
                <i className="fas fa-globe" ></i>
                <span>
                  <strong>Time Zone:</strong> 
                  {(currentEmployee &&  currentEmployee?.timeZone > -1)
                    ? getTimeZoneStringByIndex(currentEmployee.timeZone)
                    : getTimeZoneStringByIndex(4)
                  }
                 </span>
            </div>
          </div>
  
          <div className="details-section">
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
                        {currentEmployee && currentEmployee.skills.length > 0 ? (
                            currentEmployee?.skills.map((skill, index) => (
                            <tr key={index}>
                                <td>{getSkillLabel(skill.skillTypeId)}</td>
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
          </div>

          {/* Languages Section */}
            <div className="detail-banner">
                <i className="fas fa-language"></i>
                <strong>Foreign Languages:</strong>
                <div className="languages-list">
                {currentEmployee?.foreignLanguages && Object.keys(currentEmployee?.foreignLanguages).length > 0 ? (
                    Object.entries(currentEmployee?.foreignLanguages).map(([key, lang]) => (
                    <div key={key} className="language-item">
                        <strong>{formateLanguage(lang.languageTypeId)}</strong>: Level {lang.level}
                    </div>
                    ))
                ) : (
                    <span className="no-data">No foreign languages</span>
                )}
                </div>
            </div>
  
            <div className="details-section">
              {/* Roles Section */}
            <div className="detail-banner">
            <div className="skills-section">
                <i className="fa-solid fa-chalkboard-user"></i>
                <strong> Roles:</strong>
                <table className="roles-table">
                <thead>
                    <tr>
                        <th>Role Name</th>
                        <th>Project ID</th>
                        <th>Role</th>
                    </tr>
                </thead>
                <tbody>
                    {employeeRoles.length > 0 ? (
                        employeeRoles.map((role, index) => (
                        <tr key={index}>
                            <td>{role.roleName}</td>
                            <td>{role.projectId}</td>
                            <td>
                              <button className="action-button" onClick={() => handleOpenModal(role)}>
                                🔗
                              </button>
                            </td>
                        </tr>
                        ))
                    ) : (
                    <tr>
                      <td colSpan={3} className="no-roles">No roles available for this user.</td>
                    </tr>
                    )}
                    </tbody>
                </table>
            </div>
          </div>
        </div>

          <div className="modal-actions">
          <button className="edit-button" onClick={() => setIsEditModalOpen(true)}>
            <i className="fas fa-pen"></i> Edit
          </button>
          <button className="delete-button" onClick={() => setShowConfirmDelete(true)}>
            <i className="fas fa-trash"></i> Delete
          </button>
          </div>

          {selectedRole && (
          <RoleDetailsModal 
          projectId={selectedRole.projectId} 
          roleId={selectedRole.roleId} 
          onClose={handleCloseModal}/>
        )}
        {isEditModalOpen && currentEmployee  && (
          <EditEmployeeModal
          employee={currentEmployee}
          onClose={() => setIsEditModalOpen(false)}
          // onSave={handleEditSave} 
          />
        )}
        {showConfirmDelete && (
          <div className="confirm-overlay">
            <div className="confirm-dialog">
              <p>Are you sure you want to delete this Employee?</p>
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
      </div>
    );
  };
  
  export default EmployeeDetailsModal;

