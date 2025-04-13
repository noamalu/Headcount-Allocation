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


useEffect(() => {
    const handleProjectCreated = (newProject: Project) => {
        setProjects((prevProjects) => [...prevProjects, newProject]);
    };
    onProjectCreated(handleProjectCreated); 
}, [onProjectCreated]);

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
        <ProjectDetailsModal project={selectedProject} onClose={handleCloseModal} />
      )}
    </div>
  );
};

export default ProjectsTable;




// import React, { useEffect, useState } from 'react';
// import { Project, formatDate } from '../../../Types/ProjectType';
// import { getProjects } from '../../../Services/ProjectsService';
// import ProjectDetailsModal from './ProjectDetailsModal';
// import '../../../Styles/ProjectsTable.css';

// interface ProjectsTableProps {
//   onProjectCreated: (callback: (project: Project) => void) => void;
// }

// const ProjectsTable: React.FC<ProjectsTableProps> = ({ onProjectCreated }) => {
//   const [projects, setProjects] = useState<Project[]>([]);
//   const [selectedProject, setSelectedProject] = useState<Project | null>(null);
//   const [isLoading, setIsLoading] = useState(false);
//   const [apiError, setApiError] = useState<string | null>(null);

//   const fetchProjects = async () => {
//     setIsLoading(true);
//     setApiError(null);
//     try {
//       const data = await getProjects();
//       setProjects(data);
//     } catch (err) {
//       setApiError('Failed to fetch projects. Please try again later.');
//     } finally {
//       setIsLoading(false);
//     }
//   };

//   useEffect(() => {
//     fetchProjects();
//   }, []);

//   useEffect(() => {
//     const handleProjectCreated = (newProject: Project) => {
//       setProjects((prevProjects) => [...prevProjects, newProject]);
//     };
//     onProjectCreated(handleProjectCreated);
//   }, [onProjectCreated]);

//   if (isLoading) {
//     return <div className="loader">Loading...</div>;
//   }

//   if (apiError) {
//     return <div className="error-alert">{apiError}</div>;
//   }

//   return (
//     <div className="projects-table-wrapper">
//       <div className="projects-table-container">
//         <table className="projects-table">
//           <thead>
//             <tr>
//               <th>Project</th>
//               <th>Deadline</th>
//               <th>Required Hours</th>
//               <th>Status</th>
//               <th>Action</th>
//             </tr>
//           </thead>
//           <tbody>
//             {projects.map((project, index) => (
//               <tr key={project.projectId} className={index % 2 === 0 ? 'row-light' : 'row-dark'}>
//                 <td>{project.projectName}</td>
//                 <td>{formatDate(project.deadline)}</td>
//                 <td>{project.requiredHours}</td>
//                 <td>
//                   <span className={`status-tag ${project.requiredHours === 0 ? 'status-complete' : 'status-required'}`}>
//                     {project.requiredHours === 0 ? 'Staffing Complete' : 'Staffing Required'}
//                   </span>
//                 </td>
//                 <td>
//                   <button className="view-button" onClick={() => setSelectedProject(project)}>
//                     View
//                   </button>
//                 </td>
//               </tr>
//             ))}
//           </tbody>
//         </table>
//       </div>

//       {selectedProject && (
//         <div className="modal-backdrop">
//           <div className="modal">
//             <button className="modal-close" onClick={() => setSelectedProject(null)}>×</button>
//             <ProjectDetailsModal project={selectedProject} onClose={() => setSelectedProject(null)} />
//           </div>
//         </div>
//       )}
//     </div>
//   );
// };

// export default ProjectsTable;
