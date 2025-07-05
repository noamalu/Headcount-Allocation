import React, { useState } from 'react';
import ProjectsTable from '../Components/Features/Projects/ProjectsTable'; 
import '../Styles/Projects.css';
import CreateProjectModal from '../Components/Features/Projects/CreateProjectModal';
import { Project } from '../Types/ProjectType';
import { useAuth } from '../Context/AuthContext'



const ProjectsPage: React.FC = () => {
    const {isAdmin} = useAuth();
    const [isModalOpen, setIsModalOpen] = useState(false);

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
            <ProjectsTable />
           
            {isModalOpen && (
                <CreateProjectModal
                onClose={handleCloseModal}
                />
            )}
        </div>
    );
};

export default ProjectsPage;