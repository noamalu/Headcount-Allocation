import React, { useState, useEffect } from 'react';
import { Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import ProjectsService from '../../../Services/ProjectsService';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import { useDataContext } from '../../../Context/DataContext';


interface EditProjectModalProps {
  project: Project;
  onClose: () => void;
  // onSave: (updatedProject: Project) => void;
}

// const EditProjectModal: React.FC<EditProjectModalProps> = ({ project, onClose, onSave }) => {
  const EditProjectModal: React.FC<EditProjectModalProps> = ({ project, onClose }) => {
    const [editedProject, setEditedProject] = useState<Project>({ ...project });
    const [selectedRole, setSelectedRole] = useState<Role | null>(null);
    const [uiError, setUiError] = useState<string | null>(null);
    const [apiError, setApiError] = useState<string | null>(null);
  
    const { updateProject , roles } = useDataContext();
    const projectRoles = roles.filter(r => r.projectId === project.projectId);


    useEffect(() => {
      if (apiError) {
        alert(apiError);
      }
    }, [apiError]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setEditedProject({ ...editedProject, [name]: value });
  };

  useEffect(() => {
    console.log('SelectedRole changed:', selectedRole);
  }, [selectedRole]);

  const handleSave = async () => {
    if (!editedProject.projectName || !editedProject.description || !editedProject.deadline || editedProject.requiredHours <= 0) {
      setUiError("All fields are required, and required hours must be greater than 0.");
      return;
    }
    setUiError(null);
    try {
      console.log("Sending edit project for: ", editedProject.projectName," with details:", editedProject.description, editedProject.requiredHours, editedProject.deadline);
      await ProjectsService.editProject(editedProject);
      setApiError(null);
      // onSave(editedProject); 
      updateProject(editedProject);
      onClose(); 
    } catch (error) {
      console.error('Error updating project:', error);
      setApiError('An error occurred while updating the project');
    }
  };

  const handleOpenModal = (role: Role) => {
      console.log("Opening role modal for:", role.roleName, "Role data:", role);
      setSelectedRole(role);
    };

    const handleCloseModal = () => {
        setSelectedRole(null);
    };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <input
                type="text"
                id="projectName"
                name="projectName"
                value={editedProject.projectName}
                onChange={handleInputChange}
                className="input-as-h2-field"
        />

        {uiError && (
          <div className="ui-error">
            {uiError}
          </div>
        )}

        <div className="modal-info">
          <div className="modal-info-row">
            <div className="edit">
              <input
                type="date"
                id="deadline"
                name="deadline"
                value={editedProject.deadline.toString().split('T')[0]}
                onChange={handleInputChange}
                className="input-field"
              />
              </div>
              <div className="field-with-icon">
              <i className="fas fa-clock"></i>
                <input
                  type="number"
                  id="requiredHours"
                  name="requiredHours"
                  value={editedProject.requiredHours}
                  onChange={handleInputChange}
                  className="input-field"
                />
              <span>hours</span>
            </div>
          </div>
          <div className="edit">
          <textarea
              id="description"
              name="description"
              value={editedProject.description}
              onChange={handleInputChange}
              className="textarea-field"
            />
            </div>
        </div>

        <table className="roles-table">
          <thead>
            <tr>
              <th>Role</th>
              <th>Employee ID</th>
              <th>Action</th>
            </tr>
          </thead>
          {/* <tbody>
            {Object.keys(editedProject.roles).length > 0 ? (
              Object.entries(editedProject.roles).map(([roleId, role]) => {
                console.log("Rendering role:", role); 
                return (
                  <tr key={roleId}>
                    <td>{role.roleName}</td>
                    <td>{role.employeeId && role.employeeId !== -1 ? role.employeeId : "-"}</td>
                    <td>
                      <button className="action-button" onClick={() => handleOpenModal(role)} disabled>
                        🔗
                      </button>
                    </td>
                  </tr>
                );
              }) */}
              <tbody>
                {projectRoles.length > 0 ? (
                   projectRoles.map((role) => (
                    <tr key={role.roleId}>
                      <td>{role.roleName}</td>
                      <td>{role.employeeId && role.employeeId !== -1 ? role.employeeId : "-"}</td>
                      <td>
                        <button className="action-button" onClick={() => handleOpenModal(role)} disabled>
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
          {/* <button className="save-button" onClick={() => {
             console.log('Saving edited Project');
             handleSave();
             }}> */}
          <button className="save-button" onClick={handleSave}>
            <i className="fa-solid fa-square-check"></i> save
          </button> 
        </div>
        {selectedRole && (
          <RoleDetailsModal projectId = {project.projectId} roleId={selectedRole.roleId} onClose={handleCloseModal} />
        )}
      </div>
    </div>
  );
};

export default EditProjectModal;
