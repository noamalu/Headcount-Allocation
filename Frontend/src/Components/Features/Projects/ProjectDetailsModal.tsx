import React, { useState, useEffect } from 'react';
import { formatDate, Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditProjectModal from './EditProjectModal';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import CreateRoleModal from '../Roles/CreateRoleModal';
import ProjectsService, { getProjectRoles } from '../../../Services/ProjectsService';
import { useAuth } from '../../../Context/AuthContext'
import { useDataContext } from '../../../Context/DataContext';


interface ProjectDetailsModalProps {
  project: Project; 
  onClose: () => void; 
}

const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose }) => {  
  const {isAdmin} = useAuth();
  const { roles, addRole, addRolesIfNotExist, addOrUpdateRoles, updateRole } = useDataContext();
  const { projects, updateProject, deleteProject } = useDataContext();
  const projectRoles = roles.filter((r) => r.projectId === project.projectId);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isCreateRoleModalOpen, setIsCreateRoleModalOpen] = useState(false);
  const [showConfirmDelete, setShowConfirmDelete] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);
  const currentProject = projects.find(p => p.projectId === project.projectId) || project;
 

  useEffect(() => {
    const fetchRoles = async () => {
      try {
        const fetchedRoles = await getProjectRoles(project.projectId);
        addOrUpdateRoles(fetchedRoles);
      } catch (error) {
        console.error('Failed to fetch roles for project:', error);
        setApiError('Failed to load roles');
      }
    };
    fetchRoles();
  }, [project.projectId]);

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);

  const handleEditSave = (updatedProject: Project) => {
    updateProject(updatedProject);
    console.log('Project updated:', updatedProject);
  };

  const handleDelete = async () => {
    try {
      await ProjectsService.deleteProject(project.projectId);
      deleteProject(project.projectId);
      onClose(); 
    } catch (error) {
      alert("Failed to delete the project.");
      console.error(error);
    }
  };


  const handleRoleCreated = (newRole: Role) => {
    console.log('handleRoleCreated new role:', newRole.roleName);
    addRole(newRole);
  };

  const handleRoleEdited = (updatedRole: Role) => {
    updateRole(updatedRole);
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
    const roleToUpdate = roles.find((r) => r.roleId === roleId);
    if (roleToUpdate) {
      updateRole({ ...roleToUpdate, employeeId });
    }
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
            <div className="detail-small">
              <i className="fas fa-calendar-alt"></i> {formatDate(currentProject.deadline)}
            </div>
            <div className="detail-small">
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
            {projectRoles.length > 0 ? (
              projectRoles.map((role) => (
                <tr key={role.roleId}>
                  <td>{role.roleName}</td>
                  <td>{role.employeeId >= 0 ? role.employeeId : "-"}</td>
                  <td>
                    <button className="action-button" onClick={() => handleOpenModal(role)}>
                      🔗
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={3}>No roles available</td>
              </tr>
            )}
          </tbody>
        </table>

        <div className="modal-actions">
        {isAdmin && (
          <button className="edit-button" onClick={() => { console.log('Opening edit modal:', !isEditModalOpen); setIsEditModalOpen(true); }}>
            <i className="fas fa-pen"></i> Edit
          </button> 
        )}
        {isAdmin && (
          <button className="addRole-button"onClick={() => { console.log('Opening add role:', !isCreateRoleModalOpen); setIsCreateRoleModalOpen(true); }}>
            <i className="fas fa-plus"></i> Add Role
          </button>
        )}
         {isAdmin && (
          <button className="delete-button" onClick={() => setShowConfirmDelete(true)}>
            <i className="fas fa-trash"></i> Delete
          </button>
        )}
        </div>

        {selectedRole && (
          <RoleDetailsModal 
          projectId={project.projectId} 
          roleId={selectedRole.roleId} 
          onClose={handleCloseModal} />
        )}
        {isCreateRoleModalOpen && (
          <CreateRoleModal 
          projectId={project.projectId}
          onClose={() => setIsCreateRoleModalOpen(false)}
          />
        )}
         {isEditModalOpen && (
          <EditProjectModal
          project={currentProject}
          onClose={() => setIsEditModalOpen(false)} />
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