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
          const formatted: ProjectEmployeeCount[] = raw.map(
            (item: { project: Project; count: number }) => ({
              projectName: item.project.projectName,
              count: item.count,
            })
          );
          setData(formatted);
        } catch (error) {
          console.error('Error fetching project employee counts:', error);
        }
      };
  
      fetchData();
    }, []);
  
    return (
      <div className="chart-container">
        <div className="card-header">
          <h2><i className="fas fa-users"></i> Employees per Project</h2>
          <span className="badge">Last updated: today</span>
        </div>
  
        {data.length === 0 ? (
          <p>No data available.</p>
        ) : (
          <ResponsiveContainer width="100%" height={300}>
            <BarChart
                data={data}
                margin={{ top: 20, right: 30, left: 20, bottom: 40 }}
                >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="projectName" />
                <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="count" fill="#36a2eb" name="Employees" />
            </BarChart>
          </ResponsiveContainer>
        )}
      </div>
    );
  };
  
  export default EmployeesPerProjectChart;