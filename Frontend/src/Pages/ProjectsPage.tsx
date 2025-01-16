import React from 'react';
import ProjectsTable from '../Components/Features/Projects/ProjectsTable'; // ייבוא הטבלה
import '../Styles/Projects.css';
import AddButton from '../Components/Shared/AddButton'

const ProjectsPage: React.FC = () => {
    return (
        <div className="projects-page">
            <h1 className="page-title">My Projects</h1> {/* כותרת */}
            <ProjectsTable /> {/* טבלה */}
            <AddButton />
        </div>
    );
};

export default ProjectsPage;