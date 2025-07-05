import React, { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid, Legend } from 'recharts';
import StatisticsService from '../../../Services/StatisticsService';
import { Employee } from '../../../Types/EmployeeType';

interface CategoryCount {
  category: string;
  count: number;
}

const UtilizationChart: React.FC = () => {
  const [data, setData] = useState<CategoryCount[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const raw: Record<string, Employee[]> = await StatisticsService.getEmployeesUtilization();
        console.log('RAW:', raw);
        const formatted: CategoryCount[] = Object.entries(raw).map(([category, employees]) => ({
          category,
          count: employees.length,
        }));
        console.log("Formatted categories:", formatted);

        setData(formatted);
      } catch (error) {
        console.error('Error fetching utilization data:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="chart-container">
      <div className="card-header">
        <h2><i className="fas fa-chart-bar"></i> Utilization Categories</h2>
        <span className="badge">Last updated: today</span>
      </div>
      {data.length === 0 ? (
        <p>No utilization data available.</p>
      ) : (
      <ResponsiveContainer width="100%" height={300}>
        <BarChart
          data={data}
          margin={{ top: 20, right: 30, left: 100, bottom: 5 }}
        >
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="category" />
          <YAxis allowDecimals={false} />
          <Tooltip />
          <Legend />
          <Bar dataKey="count" fill="#8884d8" name="Employees" />
        </BarChart>
      </ResponsiveContainer>
      )}
    </div>

  );
};

export default UtilizationChart;
