import React, { useState, useEffect } from 'react';
import { formatDate, Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditProjectModal from './EditProjectModal';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import CreateRoleModal from '../Roles/CreateRoleModal';
import { getProjectRoles } from '../../../Services/ProjectsService';



interface ProjectDetailsModalProps {
  project: Project; // Specify that the prop is of type Project
  onClose: () => void; // Callback for closing the modal
}

const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose }) => {
  const [roles, setRoles] = useState<Role[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isCreateRoleModalOpen, setIsCreateRoleModalOpen] = useState(false);
  
  useEffect(() => {
    const fetchRoles = async () => {
      try {
        console.log('Fetching roles for project:', project.projectId);
        const roles = await getProjectRoles(project.projectId); // קריאה לפונקציה שמחזירה את התפקידים
        setRoles(roles || {}); // עדכון ה-state עם התפקידים שחזרו
      } catch (error) {
        console.error('Error fetching project roles:', error);
      }
    };
  
    if (project) {
      fetchRoles(); // קריאה לפונקציה בכניסה לפרויקט
    }
  }, [project]);


  const handleRoleCreated = (newRole: Role) => {
    console.log('handleRoleCreated new role:', newRole.roleName);
    setRoles((prevRoles) => {
      const updatedRoles = {
        ...prevRoles,
        [newRole.roleId]: newRole,
      };
      console.log("Updated roles:", updatedRoles);
      return updatedRoles;
    });
  };

  const handleRoleEdited = (updatedRole: Role) => {
    setRoles((prevRoles) => {
      const updatedRoles = {
        ...prevRoles,
        [updatedRole.roleId]: { 
          ...prevRoles[updatedRole.roleId], 
          ...updatedRole, 
        },
      };
      return updatedRoles;
    });
  };

//   useEffect(() => {
//   const handleRoleCreated = (newRole: Role) => {
//           setRoles((prevRoles) => [...prevRoles, newRole]);
//       };
//       onRoleCreated(handleRoleCreated); // רישום callback לקבלת פרויקט חדש
// }, [onRoleCreated]);

// console.log('SelectedRole changed:', selectedRole);
//   }, [selectedRole]);
      
  const handleOpenModal = (role: Role) => {
    console.log("Opening role modal for:", role.roleName, "Role data:", role); // בדוק את הערך
    setSelectedRole(role);
  };

  const handleCloseModal = () => {
    setSelectedRole(null);
  };

  const handleEditSave = (updatedProject: Project) => {
    console.log('Project updated:', updatedProject);
    // Update the project details here (e.g., send to API or update state)
  };

  const handleAssignEmployeeToRole = (roleId: number, employeeId: number) => {
    console.log("Assigning employee", employeeId, "to role", roleId);
    setRoles((prevRoles) => {
      const updatedRoles = { ...prevRoles }; // יצירת עותק של roles
      if (updatedRoles[roleId]) {
        updatedRoles[roleId].employeeId = employeeId; // עדכון ה-employeeId עבור התפקיד המתאים
      }
      return updatedRoles; // החזרת המצב המעודכן
    });
    console.log("Updated roles:", roles); // וידוא התוצאה
  };

  if (selectedRole) {
    console.log("Selected role being passed to RoleDetailsModal:", selectedRole);
  }

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>{project.projectName}</h2>
        <div className="modal-info">
          <div className="modal-info-row">
            <div className="deadline">
              <i className="fas fa-calendar-alt"></i> {formatDate(project.deadline)}
            </div>
            <div className="required-hours">
              <i className="fas fa-clock"></i> {project.requiredHours} hours
            </div>
          </div>
          <div className="description">{project.description}</div>
        </div>
        <table className="roles-table">
          <thead>
            <tr>
              <th>Role</th>
              <th>Employee ID</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {Object.keys(roles).length > 0 ? (
              Object.entries(roles).map(([roleId, role]) => {
                console.log("Rendering role:", role); // בדקי מה מוצג בטבלה
                return (
                  <tr key={roleId}>
                    <td>{role.roleName}</td>
                    <td>{role.employeeId && role.employeeId !== -1 ? role.employeeId : "-"}</td>
                    <td>
                      <button className="action-button" onClick={() => handleOpenModal(role)}>
                        🔗
                      </button>
                    </td>
                  </tr>
                );
              })
            ) : (
              <tr>
                <td colSpan={3}>No roles available</td>
              </tr>
            )}
          </tbody>
        </table>
        <div className="modal-actions">
        <button className="edit-button" onClick={() => { console.log('Opening edit modal:', !isEditModalOpen); setIsEditModalOpen(true); }}>
            <i className="fas fa-pen"></i> Edit
          </button>
          <button className="addRole-button"onClick={() => { console.log('Opening add role:', !isCreateRoleModalOpen); setIsCreateRoleModalOpen(true); }}>
            <i className="fas fa-plus"></i> Add Role
          </button>
          <button className="delete-button">
            <i className="fas fa-trash"></i> Delete
          </button>
        </div>
        {selectedRole && (
          <RoleDetailsModal 
          projectId={project.projectId} 
          role={selectedRole} 
          onClose={handleCloseModal}
          onAssignEmployeeToRole={handleAssignEmployeeToRole} />
        )}
        {isCreateRoleModalOpen && (
          <CreateRoleModal 
          projectId={project.projectId}
          onClose={() => setIsCreateRoleModalOpen(false)}
          onRoleCreated={handleRoleCreated} />
        )}
         {isEditModalOpen && (
          <EditProjectModal
            project={project}
            onClose={() => setIsEditModalOpen(false)}
            onSave={handleEditSave} />
          )}
      </div>
    </div>
  );
};



export default ProjectDetailsModal;