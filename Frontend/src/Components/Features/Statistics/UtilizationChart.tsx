// src/Components/Statistics/UtilizationChart.tsx

import React, { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid, Legend } from 'recharts';
import StatisticsService from '../../../Services/StatisticsService';
import { Employee } from '../../../Types/EmployeeType';

interface UtilizationData {
  timeZone: string;
  averagePercentage: number;
}

const UtilizationChart: React.FC = () => {
  const [data, setData] = useState<UtilizationData[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const raw: Record<string, Employee[]> = await StatisticsService.getEmployeesUtilization();

        const result: UtilizationData[] = Object.entries(raw).map(([timeZone, employees]) => {
          const total = employees.reduce((sum, e) => sum + (e.jobPercentage * 100), 0);
          const avg = employees.length > 0 ? total / employees.length : 0;

          return {
            timeZone,
            averagePercentage: Math.round(avg),
          };
        });

        setData(result);
      } catch (error) {
        console.error('Error fetching utilization data:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="chart-container">
      <h2>Average Utilization by Time Zone</h2>
      <ResponsiveContainer width="100%" height={300}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="timeZone" />
          <YAxis unit="%" />
          <Tooltip />
          <Legend />
          <Bar dataKey="averagePercentage" fill="#8884d8" name="Utilization" />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
};

export default UtilizationChart;
