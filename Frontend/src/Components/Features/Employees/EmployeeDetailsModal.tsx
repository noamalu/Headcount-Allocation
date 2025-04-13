import React, { useEffect, useState } from 'react';
import { Employee } from '../../../Types/EmployeeType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { formateLanguage } from '../../../Types/LanguageType';
import { formateSkillToString } from '../../../Types/SkillType';
import { Role } from '../../../Types/RoleType';
import { getEmployeeRolesById } from '../../../Services/EmployeesService';
import RoleDetailsModal from '../Roles/RoleDetailsModal';


interface EmployeeDetailsModalProps {
    employee: Employee;
    onClose: () => void;
  }
  
  const EmployeeDetailsModal: React.FC<EmployeeDetailsModalProps> = ({ employee, onClose }) => {
    const [isEditMode, setIsEditMode] = useState(false);
    const [roles, setRoles] = useState<Role[]>([]);
    const [selectedRole, setSelectedRole] = useState<Role | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [apiError, setApiError] = useState<string | null>(null);

    useEffect(() => {
      if (apiError) {
        alert(apiError);
      }
    }, [apiError]);
  
  
    useEffect(() => {
      const fetchEmployeeRoles = async () => {
        try {
          const response = await getEmployeeRolesById(employee.employeeId);
          employee.roles = response;
          setRoles(response);
          setLoading(false);
        } catch (err: any) {
          console.error('Error fetching employee roles:', err);
          setApiError('Failed to fetch roles');
          setLoading(false);
        }
      };
      fetchEmployeeRoles();
    }, [employee.employeeId]);
  
    if (loading) return <div>Loading employee roles...</div>;

    const handleOpenModal = (role: Role) => {
        console.log("Opening role modal for:", role.roleName, "Role data:", role);
        setSelectedRole(role);
    };
    
    const handleCloseModal = () => {
      setSelectedRole(null);
    };
    
      
    // const handleAssignEmployeeToRole = (roleId: number, employeeId: number) => {
    //   console.log("Assigning employee", employeeId, "to role", roleId);
    //   setRoles((prevRoles) => {
    //     const updatedRoles = { ...prevRoles }; // יצירת עותק של roles
    //     if (updatedRoles[roleId]) {
    //       updatedRoles[roleId].employeeId = employeeId; // עדכון ה-employeeId עבור התפקיד המתאים
    //     }
    //     return updatedRoles; // החזרת המצב המעודכן
    //   });
    //   console.log("Updated roles:", roles); // וידוא התוצאה
    // };
      
    if (selectedRole) {
      console.log("Selected role being passed to RoleDetailsModal:", selectedRole);
    }

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
                    {employee.roles.length > 0 ? (
                        employee.roles.map((role, index) => (
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
            <button className="edit-button" onClick={() => setIsEditMode(true)}>Edit</button>
            <button className="delete-button">Delete</button>
          </div>

          {selectedRole && (
          <RoleDetailsModal 
          projectId={selectedRole.projectId} 
          role={selectedRole} 
          onClose={handleCloseModal}/>
        )}
        </div>
      </div>
    );
  };
  
  export default EmployeeDetailsModal;

  // onAssignEmployeeToRole={handleAssignEmployeeToRole}