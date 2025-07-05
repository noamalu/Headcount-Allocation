import React, { useEffect, useState } from 'react';
import EmployeeDetailsModal from './EmployeeDetailsModal';
import EmployeeCard from './EmployeeCard';
import { Employee } from '../../../Types/EmployeeType';
import '../../../Styles/Employees.css';
import '../../../Styles/Shared.css';
import {getEmployees} from '../../../Services/EmployeesService';
import { useDataContext } from '../../../Context/DataContext';


const EmployeesSpan: React.FC = () => {
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);
  const { employees, setEmployees, addEmployee, updateEmployee, deleteEmployee } = useDataContext();

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



const handleEmployeeDeleted = (employeeId: number) => {
  deleteEmployee(employeeId);
};

const handleEmployeeUpdated = (updatedEmployee: Employee) => {
  updateEmployee(updatedEmployee);
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
          employeeId={selectedEmployee.employeeId} 
          onClose={handleCloseModal}
        />
      )}
    </div>
  );
};

export default EmployeesSpan;