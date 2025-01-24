import React, { useState, useEffect } from 'react';
import { Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditProjectModal from './EditProjectModal';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import CreateRoleModal from '../Roles/CreateRoleModal';


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
    console.log('SelectedRole changed:', selectedRole);
  }, [selectedRole]);

  const handleRoleCreated = (newRole: Role) => {
          setRoles((prevRoles) => [...prevRoles, newRole]);
      };
      
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

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>{project.projectName}</h2>
        <div className="modal-info">
          <div className="modal-info-row">
            <div className="deadline">
              <i className="fas fa-calendar-alt"></i> {project.deadline}
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
              <th>Employee</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {/* {project.roles.map((role) => (
              <tr key={role.roleId}>
                <td>{role.roleName}</td>
                <td>{role.employeeId}</td>
                <td>
                  <button className="action-button" onClick={() => handleOpenModal(role)}>
                    🔗
                  </button>
                </td>
              </tr>
            ))} */}
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
          <RoleDetailsModal role={selectedRole} onClose={handleCloseModal} />
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