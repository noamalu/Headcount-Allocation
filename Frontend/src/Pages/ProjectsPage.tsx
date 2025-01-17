import React, { useState } from 'react';
import ProjectsTable from '../Components/Features/Projects/ProjectsTable'; // ייבוא הטבלה
import '../Styles/Projects.css';
import NewProjectModal from '../Components/Features/Projects/NewProjectModal';

const ProjectsPage: React.FC = () => {
    const [isModalOpen, setIsModalOpen] = useState(false); // מצב לשליטה במודאל

    const handleOpenModal = () => {
        setIsModalOpen(true); // פתיחת המודאל
    };

    const handleCloseModal = () => {
        setIsModalOpen(false); // סגירת המודאל
    };

    return (
        <div className="projects-page">
            <div className="projects-header">
                <h1 className="page-title">My Projects</h1> {/* כותרת */}
                <button className="add-project-button" onClick={handleOpenModal}>+ New Project</button>
            </div>
            <ProjectsTable /> {/* טבלה */}
            {/* הצגת המודאל לפי המצב */}
            {isModalOpen && <NewProjectModal onClose={handleCloseModal} />}
        </div>
    );
};

export default ProjectsPage;