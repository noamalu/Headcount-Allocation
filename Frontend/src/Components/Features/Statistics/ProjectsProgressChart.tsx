// src/Components/Statistics/ProjectsProgressChart.tsx

import React, { useEffect, useState } from 'react';
import StatisticsService from '../../../Services/StatisticsService';
import { Project } from '../../../Types/ProjectType';
import '../../../Styles/Statistics.css';

const ProjectsProgressChart: React.FC = () => {
  const [projects, setProjects] = useState<Project[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await StatisticsService.getProjectsThatEndThisMonth();
        setProjects(data);
      } catch (error) {
        console.error('Error fetching ending projects:', error);
      }
    };

    fetchData();
  }, []);

  const getDaysLeft = (deadline: string): number => {
    const today = new Date();
    const end = new Date(deadline);
    const diff = end.getTime() - today.getTime();
    return Math.ceil(diff / (1000 * 60 * 60 * 24));
  };

  const getProgressColor = (daysLeft: number) => {
    if (daysLeft <= 3) return 'urgent';
    if (daysLeft <= 7) return 'soon';
    return 'normal';
  };

  return (
    <div className="chart-container">
      <div className="card-header">
        <h2><i className="fas fa-calendar-week"></i> Projects Ending Soon</h2>
        <span className="badge">Last updated: today</span>
      </div>

      <div className="projects-list">
        {projects.map((project) => {
          const daysLeft = getDaysLeft(project.deadline);
          const progress = Math.max(0, 100 - daysLeft * 10); // הערכה גסה
          const barClass = getProgressColor(daysLeft);

          return (
            <div key={project.projectId} className="project-card">
              <div className="project-header">
                <span className="project-name">{project.projectName}</span>
                <span className="deadline">{new Date(project.deadline).toLocaleDateString()}</span>
              </div>
              <p className="project-desc">{project.description}</p>
              <div className="progress-bar-wrapper">
                <div className={`progress-bar ${barClass}`} style={{ width: `${progress}%` }}></div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default ProjectsProgressChart;
