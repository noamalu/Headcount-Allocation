import React, { useState, useEffect } from 'react';
import { Project } from '../../../Types/ProjectType';
import { Role } from '../../../Types/RoleType';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import ProjectsTable from './ProjectsTable';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import ProjectsService from '../../../Services/ProjectsService';
import { useDataContext } from '../../../Context/DataContext';


  const CreateProjectModal: React.FC<{ onClose: () => void }> = ({ onClose }) => {
    const [projectName, setProjectName] = useState('');
    const [deadline, setDeadline] = useState('');
    const [requiredHours, setRequiredHours] = useState<number>(0);
    const [description, setDescription] = useState('');
    const [uiError, setUiError] = useState<string | null>(null);
    const [apiError, setApiError] = useState<string | null>(null);
    const [isCreateRoleModalOpen, setIsCreateRoleModalOpen] = useState(false);
    const { addProject } = useDataContext();
    
    useEffect(() => {
      if (apiError) {
        alert(apiError);
      }
    }, [apiError]);
  
  
    const handleSubmit = async () => {
      if (!projectName || !description || !deadline || requiredHours <= 0) {
        setUiError("All fields are required, and required hours must be greater than 0.");
        return;
      }
      setUiError(null);

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
        // onProjectCreated(newProject);
        addProject(newProject);
        setApiError(null);
        onClose();
    } catch (error) {
        console.error('Error creating project:', error);
        setApiError('An error occurred while creating the project');
    }
  };
  
    return (
      <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Create New Project</h2>

          {uiError && (
          <div className="ui-error">
            {uiError}
          </div>
        )}

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
                onChange={(e) => setRequiredHours(Number(e.target.value))}
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
          </div>
        </div>
      </div>
    );
  };
  
  export default CreateProjectModal;
  