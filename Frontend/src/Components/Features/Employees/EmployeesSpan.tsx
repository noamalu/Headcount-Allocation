import React, { useEffect, useState } from 'react';
import EmployeeDetailsModal from './EmployeeDetailsModal';
import EmployeeCard from './EmployeeCard';
import { Employee } from '../../../Types/EmployeeType';
import '../../../Styles/Employees.css';
import '../../../Styles/Shared.css';
import {getEmployees} from '../../../Services/EmployeesService';

const EmployeesSpan: React.FC<{ onEmployeeCreated: (callback: (employee: Employee) => void) => void }> = ({ onEmployeeCreated }) => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchEmployees = async () => {
    setIsLoading(true); 
    setError(null); 
    try {
      const data = await getEmployees();
      setEmployees(data);
    } catch (err) {
      setError('Failed to fetch employees. Please try again later.');
    } finally {
      setIsLoading(false); 
    }
  };

  useEffect(() => {
    fetchEmployees();
}, []);


useEffect(() => {
    const handleEmployeeCreated = (newEmployee: Employee) => {
        setEmployees((prevEmployees) => [...prevEmployees, newEmployee]);
    };
    onEmployeeCreated(handleEmployeeCreated); // רישום callback לקבלת פרויקט חדש
}, [onEmployeeCreated]);

const handleOpenModal = (employee: Employee) => {
  console.log(`Edit ${employee.employeeName}`);
  setSelectedEmployee(employee);
};

const handleCloseModal = () => {
  setSelectedEmployee(null);
};

if (isLoading) {
    return <p>Loading employees...</p>;
}

if (error) {
    return <p className="error">{error}</p>;
}

  return (
    <div>
      <div className="employees-span">
        {employees.map((employee) => (
          <EmployeeCard
            key={employee.employeeId}
            employee={employee}
            onClick={() => handleOpenModal(employee)}
          />
        ))}
      </div>
      {selectedEmployee && (
        <EmployeeDetailsModal employee={selectedEmployee} onClose={handleCloseModal} />
      )}
    </div>
  );
};

export default EmployeesSpan;




