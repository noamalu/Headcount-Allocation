import React, { useState, useEffect } from 'react';
import { Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';



interface ProjectDetailsModalProps {
  project: Project; // Specify that the prop is of type Project
  onClose: () => void; // Callback for closing the modal
}

const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose }) => {
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);

  useEffect(() => {
    console.log("SelectedRole changed:", selectedRole);
  }, [selectedRole]);

  const handleOpenModal = (role: Role) => {
    console.log("Opening role modal for:", role.name);
    setSelectedRole(role);
  };

  const handleCloseModal = () => {
    setSelectedRole(null);
  };

// const ProjectDetailsModal: React.FC<ProjectDetailsModalProps> = ({ project, onClose }) => {
  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>{project.name}</h2>
        <p><strong>Deadline:</strong> {project.deadline}</p>
        <p><strong>Description:</strong> {project.description}</p>
        <h3>Roles</h3>
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
                  <button className="action-button" onClick={() => handleOpenModal(role)}>🔗</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {selectedRole && (() => {
        console.log("Rendering RoleDetailsModal for:", selectedRole);
        return (
          <RoleDetailsModal role={selectedRole} onClose={handleCloseModal} />
        );
      })()}
        <div className="modal-actions">
          <button className="delete-button">🗑 Delete</button>
          <button className="addRole-button">+ Add Role</button>
          <button className="edit-button">✏ Edit</button>
        </div>
      </div>
    </div>
  );
};

export default ProjectDetailsModal;