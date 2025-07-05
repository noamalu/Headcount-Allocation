import React from 'react';
import UtilizationChart from '../Components/Features/Statistics/UtilizationChart';
import ProjectHourRatioChart from '../Components/Features/Statistics/ProjectHourRatioChart';
import VacationReasonsChart from '../Components/Features/Statistics/AbsenceReasonsChart';
import ProjectsProgressChart from '../Components/Features/Statistics/ProjectsProgressChart';
import EmployeesPerProjectChart from '../Components/Features/Statistics/EmployeesPerProjectChart';
import '../Styles/Statistics.css';

const StatisticsPage: React.FC = () => {
  return (
    <div className="statistics-page">
      <h1 className="page-title">Dashboard</h1>
      <div className="stats-grid">
        <ProjectsProgressChart />
        <VacationReasonsChart />
        <UtilizationChart />
        <ProjectHourRatioChart />
        <EmployeesPerProjectChart />
      </div>
    </div>
  );
};

export default StatisticsPage;
