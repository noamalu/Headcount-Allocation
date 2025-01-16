import React, { useState, useEffect } from 'react';
import { Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import '../../../Styles/Modal.css';


interface ProjectDetailsModalProps {
  project: Project; // Specify that the prop is of type Project
  onClose: () => void; // Callback for closing the modal
}

const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose }) => {
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);

  useEffect(() => {
    console.log('SelectedRole changed:', selectedRole);
  }, [selectedRole]);

  const handleOpenModal = (role: Role) => {
    console.log('Opening role modal for:', role.name);
    setSelectedRole(role);
  };

  const handleCloseModal = () => {
    setSelectedRole(null);
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>{project.name}</h2>
        <div className="modal-info">
          <div className="deadline">
            <i className="fas fa-calendar-alt"></i> {project.deadline}
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
            {project.roles.map((role) => (
              <tr key={role.id}>
                <td>{role.name}</td>
                <td>{role.employee}</td>
                <td>
                  <button className="action-button" onClick={() => handleOpenModal(role)}>
                    🔗
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="modal-actions">
          <button className="delete-button">
            <i className="fas fa-trash"></i> Delete
          </button>
          <button className="addRole-button">
            <i className="fas fa-plus"></i> Add Role
          </button>
          <button className="edit-button">
            <i className="fas fa-pen"></i> Edit
          </button>
        </div>
        {selectedRole && (
          <RoleDetailsModal role={selectedRole} onClose={handleCloseModal} />
        )}
      </div>
    </div>
  );
};



export default ProjectDetailsModal;