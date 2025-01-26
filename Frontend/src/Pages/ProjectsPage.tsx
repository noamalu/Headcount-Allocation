import React, { useState, useRef } from 'react';
import ProjectsTable from '../Components/Features/Projects/ProjectsTable'; // ייבוא הטבלה
import '../Styles/Projects.css';
import CreateProjectModal from '../Components/Features/Projects/CreateProjectModal';
import { Project } from '../Types/ProjectType';
 

const ProjectsPage: React.FC = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const tableRef = useRef<(project: Project) => void>();

    const handleProjectCreated = (project: Project) => {
        if (tableRef.current) {
            tableRef.current(project); // שדר לטבלה
        }
    };

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
            <ProjectsTable onProjectCreated={(callback) => (tableRef.current = callback)} />            {/* הצגת המודאל לפי המצב */}
            {isModalOpen && (
                <CreateProjectModal
                    onClose={() => setIsModalOpen(false)} // סגירת המודל
                    onProjectCreated={handleProjectCreated} // עדכון הרשימה
                />
            )}
        </div>
    );
};

export default ProjectsPage;