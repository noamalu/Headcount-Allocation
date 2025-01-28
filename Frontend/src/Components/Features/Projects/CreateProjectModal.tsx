import React, { useState, useEffect } from 'react';
import { Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import ProjectsTable from './ProjectsTable';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import ProjectsService from '../../../Services/ProjectsService';
// import AddRoleModal from '../Roles/NewRoleModal';


const CreateProjectModal: React.FC<{ 
  onClose: () => void;
   onProjectCreated: (project: Project) => void }>
    = ({
      onClose,
      onProjectCreated,
    }) => {
    const [projectName, setProjectName] = useState('');
    const [deadline, setDeadline] = useState('');
    const [requiredHours, setRequiredHoursline] = useState<number>(0);
    const [description, setDescription] = useState('');
    const [error, setError] = useState<string>(""); 
    const [isCreateRoleModalOpen, setIsCreateRoleModalOpen] = useState(false);
    // const [isAddRoleModalOpen, setIsAddRoleModalOpen] = useState(false);
    // const [roles, setRoles] = useState<Role[]>([]);    
  
    const handleSubmit = async () => {
      if (!projectName || !description || !deadline || requiredHours <= 0) {
        setError("All fields are required, and required hours must be greater than 0.");
        return;
      }
      const newProject: Project = {
        projectId: -1, 
        projectName,
        description,
        deadline,
        requiredHours,
        roles: []
      };
      try {
        const newProjectId = await ProjectsService.sendCreateProject(newProject);
        newProject.projectId = newProjectId;
        console.log('Project created successfully:', newProject);
        onProjectCreated(newProject);
        onClose(); // סגירת המודל
    } catch (error) {
        console.error('Error creating project:', error);
        setError('An error occurred while creating the project.');
    }
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
            <div>
              <label>Required Hours: </label>
              <input
                type="number"
                value={requiredHours}
                onChange={(e) => setRequiredHoursline(Number(e.target.value))}
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
          <div className="modal-actions">
            <button className="save-button" onClick={handleSubmit}>
              <i className="fas fa-save"></i> Save Project
            </button>
            {/* <button className="addRole-button"onClick={() => { console.log('Opening add role:', !isCreateRoleModalOpen); setIsCreateRoleModalOpen(true); }}>
            <i className="fas fa-plus"></i> Add Role
          </button> */}
            {/* {isAddRoleModalOpen && (
            <AddRoleModal
              onSave={(newRole) => {
                setRoles([...roles, newRole]);
                setIsAddRoleModalOpen(false);
              }}
              onClose={() => setIsAddRoleModalOpen(false)}
            />
          )} */}
          </div>
        </div>
      </div>
    );
  };
  
  export default CreateProjectModal;
  