import React, { useEffect, useState } from 'react';
import ProjectDetailsModal from './ProjectDetailsModal';
import { Project, formatDate } from '../../../Types/ProjectType';
import '../../../Styles/Projects.css';
import '../../../Styles/Shared.css';
import { getProjects } from '../../../Services/ProjectsService';

const ProjectsTable: React.FC<{ 
      onProjectCreated: (callback: (project: Project) => void) => void 
      onProjectUpdated: (callback: (project: Project) => void) => void;
    }> = ({ onProjectCreated, onProjectUpdated }) => {
  const [projects, setProjects] = useState<Project[]>([]);
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

const handleProjectUpdated = (updatedProject: Project) => {
  setProjects((prevProjects) =>
    prevProjects.map((p) =>
      p.projectId === updatedProject.projectId ? updatedProject : p
    )
  );
};

useEffect(() => {
  onProjectUpdated(handleProjectUpdated);
}, [onProjectUpdated]);


useEffect(() => {
    const handleProjectCreated = (newProject: Project) => {
        setProjects((prevProjects) => [...prevProjects, newProject]);
    };
    onProjectCreated(handleProjectCreated); 
}, [onProjectCreated]);

const handleProjectDeleted = (projectId: number) => {
  setProjects((prev) => prev.filter((p) => p.projectId !== projectId));
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
          onProjectUpdated={handleProjectUpdated}
          onProjectDeleted={handleProjectDeleted}
        />
      )}
    </div>
  );
};

export default ProjectsTable;




