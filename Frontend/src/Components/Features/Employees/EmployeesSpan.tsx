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
  const [apiError, setApiError] = useState<string | null>(null);

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);


  const fetchEmployees = async () => {
    setIsLoading(true); 
    setApiError(null); 
    try {
      const data = await getEmployees();
      setEmployees(data);
    } catch (err) {
      setApiError('Failed to fetch employees. Please try again later.');
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
    onEmployeeCreated(handleEmployeeCreated); 
}, [onEmployeeCreated]);

const handleEmployeeDeleted = (employeeId: number) => {
  setEmployees((prev) => prev.filter(e => e.employeeId !== employeeId));
};

const handleEmployeeUpdated = (updatedEmployee: Employee) => {
  setEmployees((prev) =>
    prev.map(e =>
      e.employeeId === updatedEmployee.employeeId ? updatedEmployee : e
    )
  );
};

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
        <EmployeeDetailsModal
          employee={selectedEmployee} 
          onClose={handleCloseModal}
          onEmployeeDeleted={handleEmployeeDeleted}
          onEmployeeUpdated={handleEmployeeUpdated}
        />
      )}
    </div>
  );
};

export default EmployeesSpan;