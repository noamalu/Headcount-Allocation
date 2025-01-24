import React, { useEffect, useState } from 'react';
import ProjectDetailsModal from './ProjectDetailsModal';
import { Project, formatDate } from '../../../Types/ProjectType';
import '../../../Styles/Projects.css';
import '../../../Styles/Shared.css';
import { getProjects } from '../../../Services/ProjectsService';

const ProjectsTable: React.FC<{ onProjectCreated: (callback: (project: Project) => void) => void }> = ({ onProjectCreated }) => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // פונקציה לקריאה ל-API
  const fetchProjects = async () => {
    setIsLoading(true); 
    setError(null); 

    try {
      const data = await getProjects();
      setProjects(data);
    } catch (err) {
      setError('Failed to fetch projects. Please try again later.');
    } finally {
      setIsLoading(false); 
    }
  };

  useEffect(() => {
    fetchProjects();
}, []);


useEffect(() => {
    const handleProjectCreated = (newProject: Project) => {
        setProjects((prevProjects) => [...prevProjects, newProject]);
    };
    onProjectCreated(handleProjectCreated); // רישום callback לקבלת פרויקט חדש
}, [onProjectCreated]);

const handleOpenModal = (project: Project) => {
  setSelectedProject(project);
};

const handleCloseModal = () => {
  setSelectedProject(null);
};

if (isLoading) {
    return <p>Loading projects...</p>;
}

if (error) {
    return <p className="error">{error}</p>;
}


  const handleProjectCreated = (newProject: Project) => {
    setProjects((prevProjects) => [...prevProjects, newProject]); // מוסיף את הפרויקט לטבלה
  };

  return (
    <div>
      <table className="projects-table">
        <thead>
          <tr>
            <th>Project</th>
            <th>Deadline</th>
            <th>Positions</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {projects.map((project) => (
            <tr key={project.projectId}>
              <td>{project.projectName}</td>
              <td>{formatDate(project.deadline)}</td>
              <td>{project.roles.length || 0}</td>
              <td>
                <button className="action-button" onClick={() => handleOpenModal(project)}>🔗</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {selectedProject && (
        <ProjectDetailsModal project={selectedProject} onClose={handleCloseModal} />
      )}
    </div>
  );
};

export default ProjectsTable;




