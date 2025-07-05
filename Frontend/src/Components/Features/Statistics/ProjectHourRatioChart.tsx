
import React, { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid, Legend, Cell } from 'recharts';
import StatisticsService from '../../../Services/StatisticsService';
import { Project } from '../../../Types/ProjectType';

interface RatioData {
    projectName: string;
    ratioPercent: number;
    fillColor: string;
  }
  
  const ProjectHourRatioChart: React.FC = () => {
    const [data, setData] = useState<RatioData[]>([]);
  
    useEffect(() => {
      const fetchData = async () => {
        try {
          const raw = await StatisticsService.getProjectHourRatios();
  
          const formatted: RatioData[] = raw.map((item: { project: Project; hours: number }) => {
            const { requiredHours, projectName } = item.project;
            const ratio = requiredHours > 0 ? (item.hours / requiredHours) * 100 : 0;
            const percent = Math.round(ratio);
          
            let fillColor = '#FF6B6B'; 
            if (percent >= 100) fillColor = '#4CAF50'; 
            else if (percent >= 70) fillColor = '#FFD93D'; 
          
            return {
              projectName,
              ratioPercent: percent,
              fillColor,
            };
          });
  
          setData(formatted);
        } catch (error) {
          console.error('Error fetching project hour ratios:', error);
        }
      };
  
      fetchData();
    }, []);
  
    return (
      <div className="chart-container">
        <div className="card-header">
          <h2><i className="fas fa-clock"></i> Project Hour Fulfillment</h2>
          <span className="badge">Last updated: today</span>
        </div>
  
        <ResponsiveContainer width="100%" height={300}>
          <BarChart
            data={data}
            layout="vertical"
            margin={{ top: 20, right: 30, left: 100, bottom: 5 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis type="number" unit="%" />
            <YAxis type="category" dataKey="projectName" />
            <Tooltip />
            <Legend />
            <Bar dataKey="ratioPercent" name="Hour Ratio">
                {data.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.fillColor} />
                ))}
            </Bar>
          </BarChart>
        </ResponsiveContainer>
      </div>
    );
  };
  
  export default ProjectHourRatioChart;