import React, { useEffect, useState } from 'react';
import ProjectDetailsModal from './ProjectDetailsModal';
import { Project, formatDate } from '../../../Types/ProjectType';
import '../../../Styles/Projects.css';
import '../../../Styles/Shared.css';
import { getProjects } from '../../../Services/ProjectsService';
import { useDataContext } from '../../../Context/DataContext';


const ProjectsTable: React.FC = () => {
  const { projects, setProjects, addProject, updateProject, deleteProject } = useDataContext();
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);

  const fetchProjects = async () => {
    setIsLoading(true); 
     setApiError(null); 
    try {
      const data = await getProjects();
      setProjects(data);
    } catch (err) {
      setApiError('Failed to fetch projects. Please try again later.');
    } finally {
      setIsLoading(false); 
    }
  };
  useEffect(() => {
    fetchProjects();
}, []);


const handleProjectDeleted = (projectId: number) => {
  deleteProject(projectId);
};


useEffect(() => {
  if (apiError) {
    alert(apiError);
  }
}, [apiError]);

const handleOpenModal = (project: Project) => {
  setSelectedProject(project);
};

const handleCloseModal = () => {
  setSelectedProject(null);
};

if (isLoading) {
    return <p>Loading projects...</p>;
}


  return (
    <div>
      <table className="projects-table">
        <thead>
          <tr>
            <th>Project</th>
            <th>Deadline</th>
            <th>Required Hours</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {projects.map((project) => (
            <tr key={project.projectId}>
              <td>{project.projectName}</td>
              <td>{formatDate(project.deadline)}</td>
              <td>{project.requiredHours}</td>
              <td>
                <button className="action-button" onClick={() => handleOpenModal(project)}>🔗</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {selectedProject && (
        <ProjectDetailsModal
          project={selectedProject} 
          onClose={handleCloseModal}
        />
      )}
    </div>
  );
};

export default ProjectsTable;




