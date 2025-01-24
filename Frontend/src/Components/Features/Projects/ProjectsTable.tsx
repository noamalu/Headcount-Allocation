// import React, { useState } from 'react';
// import ProjectDetailsModal from './ProjectDetailsModal';
// import { Project } from '../../../Types/ProjectType';
// import '../../../Styles/ProjectsTable.css'
// import '../../../Styles/Shared.css'


//   const ProjectsTable: React.FC = () => {
//     const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  
//     const handleOpenModal = (project: Project) => {
//       setSelectedProject(project);
//     };


// const projects = [
//   { id: 1, name: 'Bla Bla 1', deadline: '01/01/2025', progress: 50, description: 'The bla bla project is bla bla...', roles: [{ id: 1, name:'API', employee: 'Nofar Cohen', description: "", attributes: []}, { id: 2, name:'Front', employee: 'Hadas Printz', description: "", attributes: []}, { id: 3, name:'Back', employee: 'Noa Malul', description: "", attributes: []}] },
//   { id: 2, name: 'Charta 2', deadline: '14/06/2026', progress: 30, description: 'Charta project details...', roles: [] },
//   { id: 3, name: 'Stupid 3', deadline: '26/11/2025', progress: 70, description: 'Details about Stupid 3...', roles: [] },
//   { id: 4, name: 'Random 4', deadline: '01/07/2026', progress: 40, description: 'Details about Random 4...', roles: [] },
//   { id: 5, name: 'Hamtzaa 5', deadline: '01/01/2027', progress: 60, description: 'Details about Hamtzaa 5...', roles: [] },
//   { id: 6, name: 'Ein Li Musag 6', deadline: '01/01/2025', progress: 20, description: 'Details about Musag 6...', roles: [] },
//   // Add other projects here
// ];



//   const handleCloseModal = () => {
//     setSelectedProject(null);
//   };

import React, { useEffect, useState } from 'react';
import ProjectDetailsModal from './ProjectDetailsModal';
import { Project } from '../../../Types/ProjectType';
import '../../../Styles/Projects.css';
import '../../../Styles/Shared.css';
import { getProjects } from '../../../Services/ProjectsService'; // פונקציה להבאת נתוני פרויקטים

const ProjectsTable: React.FC<{ onProjectCreated: (callback: (project: Project) => void) => void }> = ({ onProjectCreated }) => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // פונקציה לקריאה ל-API
  const fetchProjects = async () => {
    setIsLoading(true); // הצגת טעינה
    setError(null); // איפוס הודעות שגיאה

    try {
      const data = await getProjects(); // קריאה ל-API
      setProjects(data);
    } catch (err) {
      setError('Failed to fetch projects. Please try again later.');
    } finally {
      setIsLoading(false); // סיום טעינה
    }
  };

  useEffect(() => {
    fetchProjects();
}, []);

// עדכון הרשימה עם פרויקט חדש
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
              <td>{project.deadline}</td>
              <td>{project.roles.length}</td>
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




