// src/Components/Statistics/ProjectHourRatioChart.tsx

import React, { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid, Legend } from 'recharts';
import StatisticsService from '../../../Services/StatisticsService';
import { Project } from '../../../Types/ProjectType';

interface RatioData {
  projectName: string;
  hourRatio: number;
}

const ProjectHourRatioChart: React.FC = () => {
  const [data, setData] = useState<RatioData[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const raw = await StatisticsService.getProjectHourRatios(); // ← הקריאה מהשירות שלך
        const result: RatioData[] = raw.map((item: { project: Project; hours: number }) => ({
          projectName: item.project.projectName,
          hourRatio: Math.round(item.hours * 100), // אחוז
        }));
        setData(result);
      } catch (error) {
        console.error('Error fetching hour ratio data:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="chart-container">
      <h2>Project Hour Fulfillment</h2>
      <ResponsiveContainer width="100%" height={300}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="projectName" />
          <YAxis unit="%" />
          <Tooltip />
          <Legend />
          <Bar dataKey="hourRatio" fill="#ffc658" name="Hour Ratio" />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
};

export default ProjectHourRatioChart;
