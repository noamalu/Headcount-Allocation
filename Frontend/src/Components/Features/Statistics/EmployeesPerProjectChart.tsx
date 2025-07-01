// src/Components/Statistics/EmployeesPerProjectChart.tsx

import React, { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid, Legend } from 'recharts';
import StatisticsService from '../../../Services/StatisticsService';
import { Project } from '../../../Types/ProjectType';

interface ProjectEmployeeCount {
  projectName: string;
  count: number;
}

const EmployeesPerProjectChart: React.FC = () => {
  const [data, setData] = useState<ProjectEmployeeCount[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const raw = await StatisticsService.getNumEmployeesInProject();
        const formatted: ProjectEmployeeCount[] = raw.map((item: { project: Project; count: number }) => ({
          projectName: item.project.projectName,
          count: item.count,
        }));
        setData(formatted);
      } catch (error) {
        console.error('Error fetching project employee counts:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="chart-container">
      <h2>Employees per Project</h2>
      <ResponsiveContainer width="100%" height={300}>
        <BarChart
          data={data}
          layout="vertical"
          margin={{ top: 20, right: 30, left: 100, bottom: 5 }}
        >
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis type="number" />
          <YAxis type="category" dataKey="projectName" />
          <Tooltip />
          <Legend />
          <Bar dataKey="count" fill="#36a2eb" name="Employees" />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
};

export default EmployeesPerProjectChart;
