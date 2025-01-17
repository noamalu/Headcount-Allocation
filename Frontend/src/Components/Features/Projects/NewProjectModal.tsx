import React, { useState, useEffect } from 'react';
import { Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';


const NewProjectModal: React.FC<{ onClose: () => void }> = ({ onClose }) => {
    const [projectName, setProjectName] = useState('');
    const [deadline, setDeadline] = useState('');
    const [description, setDescription] = useState('');
  
    const handleSubmit = () => {
      const newProject = {
        name: projectName,
        deadline,
        description,
      };
      console.log('Creating Project:', newProject);
      onClose();
    };
  
    return (
      <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Create New Project</h2>
          <div className="modal-info">
            <div>
              <label>Project Name: </label>
              <input
                type="text"
                value={projectName}
                onChange={(e) => setProjectName(e.target.value)}
                placeholder="Enter project name"
                className="input-field"
              />
            </div>
            <div>
              <label>Deadline: </label>
              <input
                type="date"
                value={deadline}
                onChange={(e) => setDeadline(e.target.value)}
                className="input-field"
              />
            </div>
          </div>
          <div className="modal-info">
            <label>Description:</label>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Enter project description"
              className="textarea-field"
            ></textarea>
          </div>
          <table className="roles-input-table">
            <thead>
              <tr>
                <th>Role</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {/* טבלה ריקה לעת עתה */}
            </tbody>
          </table>
          <div className="modal-actions">
            <button
              className="addRole-button"
              onClick={() => {
                // כרגע אין פעולה בכפתור
                console.log('Add Role button clicked');
              }}
            >
              + Add Role
            </button>
            <button className="edit-button" onClick={handleSubmit}>
              Save Project
            </button>
          </div>
        </div>
      </div>
    );
  };
  
  export default NewProjectModal;
  