import React, { useState, useRef } from 'react';
import ProjectsTable from '../Components/Features/Projects/ProjectsTable'; 
import '../Styles/Projects.css';
import CreateProjectModal from '../Components/Features/Projects/CreateProjectModal';
import { Project } from '../Types/ProjectType';
import { useAuth } from '../Context/AuthContext'



const ProjectsPage: React.FC = () => {
    const {isAdmin} = useAuth();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const tableRef = useRef<(project: Project) => void>();
    const updateRef = useRef<(project: Project) => void>();

    const handleProjectCreated = (project: Project) => {
        if (tableRef.current) {
            tableRef.current(project); // Send to table
        }
    };

    const handleProjectUpdated = (project: Project) => {
        if (updateRef.current) {
          updateRef.current(project);
        }
    };

    const handleOpenModal = () => {
        setIsModalOpen(true); 
    };

    const handleCloseModal = () => {
        setIsModalOpen(false); 
    };


    return (
        <div className="projects-page">
            <div className="projects-header">
                <h1 className="page-title">My Projects</h1> 
                {isAdmin &&
                 <button className="add-project-button" onClick={handleOpenModal}>+ New Project</button>}
            </div>
            <ProjectsTable
                onProjectCreated={(callback) => (tableRef.current = callback)}
                onProjectUpdated={(callback) => (updateRef.current = callback)}
            />     
            {isModalOpen && (
                <CreateProjectModal
                    onClose={() => setIsModalOpen(false)} 
                    onProjectCreated={handleProjectCreated} // Update list
                />
            )}
        </div>
    );
};

export default ProjectsPage;