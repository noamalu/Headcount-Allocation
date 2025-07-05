// src/Components/Statistics/VacationReasonsChart.tsx

import React, { useEffect, useState } from 'react';
import { PieChart, Pie, Tooltip, Cell, Legend, ResponsiveContainer } from 'recharts';
import StatisticsService from '../../../Services/StatisticsService';
import { Employee } from '../../../Types/EmployeeType';

interface ReasonData {
  reason: string;
  count: number;
}

const COLORS = [
  '#0088FE', 
  '#00C49F', 
  '#FFBB28', 
  '#FF8042',
  '#AA66CC', 
  '#FF4444', 
  '#4ECDC4', 
  '#36A2EB', 
  '#9966FF', 
  '#FF6384', 
  '#C9CBCF',
];
const VacationReasonsChart: React.FC = () => {
  const [data, setData] = useState<ReasonData[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const raw: Record<string, Employee[]> = await StatisticsService.getVacationReasons();
        const formatted: ReasonData[] = Object.entries(raw).map(([reason, employees]) => ({
          reason,
          count: employees.length,
        }));
        setData(formatted);
      } catch (error) {
        console.error('Error fetching vacation reasons:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="chart-container">
      <div className="card-header">
        <h2><i className="fas fa-plane-departure"></i> Absence Reasons</h2>
        <span className="badge">Last updated: today</span>
      </div>
      {data.length === 0 ? (
        <p>No data available.</p>
      ) : (
        <ResponsiveContainer width="100%" height={300}>
          <PieChart>
            <Pie
              data={data}
              dataKey="count"
              nameKey="reason"
              cx="50%"
              cy="50%"
              outerRadius={100}
              label
            >
              {data.map((_, index) => (
                <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
              ))}
            </Pie>
            <Tooltip />
            <Legend />
          </PieChart>
        </ResponsiveContainer>
      )}
    </div>
  );
};

export default VacationReasonsChart;