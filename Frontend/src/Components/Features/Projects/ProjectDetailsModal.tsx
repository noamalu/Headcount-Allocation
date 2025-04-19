import React, { useState, useEffect } from 'react';
import { formatDate, Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditProjectModal from './EditProjectModal';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import CreateRoleModal from '../Roles/CreateRoleModal';
import ProjectsService, { getProjectRoles } from '../../../Services/ProjectsService';



interface ProjectDetailsModalProps {
  project: Project; // Specify that the prop is of type Project
  onClose: () => void; // Callback for closing the modal
  onProjectUpdated: (project: Project) => void;
  onProjectDeleted: (projectId: number) => void;
}

const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose, onProjectUpdated, onProjectDeleted }) => {
  const [currentProject, setCurrentProject] = useState<Project>(project);
  const [roles, setRoles] = useState<Role[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isCreateRoleModalOpen, setIsCreateRoleModalOpen] = useState(false);
  const [showConfirmDelete, setShowConfirmDelete] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);

  const handleEditSave = (updatedProject: Project) => {
    setCurrentProject(updatedProject);
    onProjectUpdated(updatedProject);
    console.log('Project updated:', updatedProject);
  };

  const handleDelete = async () => {
    try {
      await ProjectsService.deleteProject(currentProject.projectId);
      onProjectDeleted(currentProject.projectId); 
      onClose(); 
    } catch (error) {
      alert("Failed to delete the project.");
      console.error(error);
    }
  };
  
  useEffect(() => {
    const fetchRoles = async () => {
      try {
        console.log('Fetching roles for project:', project.projectId);
        const roles = await getProjectRoles(project.projectId); 
        setRoles(roles || {}); 
        setCurrentProject((prev) => ({
          ...prev,
          roles: roles || []
        }));
      } catch (error) {
        console.error('Error fetching project roles:', error);
        setApiError('Failed to fetch project roles');
      }
    };
  
    if (project) {
      fetchRoles(); 
    }
  }, [project]);

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);


  const handleRoleCreated = (newRole: Role) => {
    console.log('handleRoleCreated new role:', newRole.roleName);
    setRoles((prevRoles) => {
      const updatedRoles = {
        ...prevRoles,
        [newRole.roleId]: newRole,
      };
      console.log("Updated roles:", updatedRoles);
      setCurrentProject(prev => ({ ...prev, roles: updatedRoles }));
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
      setCurrentProject(prev => ({ ...prev, roles: updatedRoles }));
      return updatedRoles;
    });
  };
      
  const handleOpenModal = (role: Role) => {
    console.log("Opening role modal for:", role.roleName, "Role data:", role); 
    setSelectedRole(role);
  };

  const handleCloseModal = () => {
    setSelectedRole(null);
  };


  const handleAssignEmployeeToRole = (roleId: number, employeeId: number) => {
    console.log("Assigning employee", employeeId, "to role", roleId);
    setRoles((prevRoles) => {
      const updatedRoles = { ...prevRoles }; 
      if (updatedRoles[roleId]) {
        updatedRoles[roleId].employeeId = employeeId; 
      }
      setCurrentProject(prev => ({ ...prev, roles: updatedRoles }));
      return updatedRoles; 
    });
    console.log("Updated roles:", roles); 
  };


  if (selectedRole) {
    console.log("Selected role being passed to RoleDetailsModal:", selectedRole);
  }

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>{currentProject.projectName}</h2>
        <div className="modal-info">
          <div className="modal-info-row">
            <div className="deadline">
              <i className="fas fa-calendar-alt"></i> {formatDate(currentProject.deadline)}
            </div>
            <div className="required-hours">
              <i className="fas fa-clock"></i> {currentProject.requiredHours} hours
            </div>
          </div>
          <div className="description">{currentProject.description}</div>
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
          <button className="delete-button" onClick={() => setShowConfirmDelete(true)}>
            <i className="fas fa-trash"></i> Delete
          </button>
        </div>
        {selectedRole && (
          <RoleDetailsModal 
          projectId={currentProject.projectId} 
          role={selectedRole} 
          onClose={handleCloseModal}
          onAssignEmployeeToRole={handleAssignEmployeeToRole} />
        )}
        {isCreateRoleModalOpen && (
          <CreateRoleModal 
          projectId={currentProject.projectId}
          onClose={() => setIsCreateRoleModalOpen(false)}
          onRoleCreated={handleRoleCreated} />
        )}
         {isEditModalOpen && (
          <EditProjectModal
          project={currentProject}
          onClose={() => setIsEditModalOpen(false)}
          onSave={handleEditSave} />
        )}
        {showConfirmDelete && (
          <div className="confirm-overlay">
            <div className="confirm-dialog">
              <p>Are you sure you want to delete this project?</p>
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



export default ProjectDetailsModal;