import React, { useState } from 'react';
import { Project } from '../../../Types/ProjectType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';

interface EditProjectModalProps {
  project: Project;
  onClose: () => void;
  onSave: (updatedProject: Project) => void;
}

const EditProjectModal: React.FC<EditProjectModalProps> = ({ project, onClose, onSave }) => {
  const [editedProject, setEditedProject] = useState<Project>({ ...project });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setEditedProject({ ...editedProject, [name]: value });
  };

  const handleSave = () => {
    onSave(editedProject);
    onClose();
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>{editedProject.projectName}</h2>
        <div className="modal-info">
          <div className="modal-info-row">
            <div className="field-group">
              <label htmlFor="deadline">Deadline:</label>
              <input
                type="date"
                id="deadline"
                name="date"
                value={editedProject.date}
                onChange={handleInputChange}
              />
            </div>
            <div className="field-group">
              <label htmlFor="requiredHours">Required Hours:</label>
              <input
                type="number"
                id="requiredHours"
                name="requiredHours"
                value={editedProject.requiredHours}
                onChange={handleInputChange}
              />
            </div>
          </div>
          <div className="field-group">
            <label htmlFor="description">Description:</label>
            <textarea
              id="description"
              name="description"
              value={editedProject.description}
              onChange={handleInputChange}
            />
          </div>
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
              <tr key={role.roleId}>
                <td>{role.roleName}</td>
                <td>{role.employeeId}</td>
                <td>
                  <button className="action-button">
                    <i className="fas fa-pen"></i> Edit Role
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="modal-actions">
          <button className="addRole-button">
            <i className="fas fa-plus"></i> Add Role
          </button>
          <button className="delete-button">
            <i className="fas fa-trash"></i> Delete Project
          </button>
          <button className="cancel-button" onClick={onClose}>Cancel</button>
          <button className="save-button" onClick={handleSave}>Save</button>
        </div>
      </div>
    </div>
  );
};

export default EditProjectModal;
