import React, { useEffect, useState } from 'react';
import '../../../Styles/Modal.css';
import { Employee } from '../../../Types/EmployeeType'
import { getAssignOptionsToRole } from '../../../Services/ProjectsService';

const AssignEmployeeModal = ({
  projectId,
  roleId,
  onClose,
  onAssign,
}: {
  projectId: number;
  roleId: number;
  onClose: () => void;
  onAssign: (employeeId: number) => void;
}) => {
  const [employees, setEmployees] = useState<Employee[]>([]); // הגדרת טיפוס
  const [selectedEmployee, setSelectedEmployee] = useState<number | null>(null);
  const [expandedEmployeeId, setExpandedEmployeeId] = useState<number | null>(null);

  useEffect(() => {
    // קריאה ל-API להחזרת העובדים המובילים לתפקיד
    const fetchEmployees = async () => {
      try {
        const data: Employee[] = await getAssignOptionsToRole(projectId, roleId);
        setEmployees(data);
      } catch (error) {
        console.error('Error fetching employees:', error);
      }
    };

    fetchEmployees();
  }, [roleId]);

  const handleExpand = (employeeId: number) => {
    setExpandedEmployeeId(expandedEmployeeId === employeeId ? null : employeeId);
  };

  const handleAssign = () => {
    if (selectedEmployee) {
      onAssign(selectedEmployee);
      onClose();
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>Assign Employee to Role</h2>
        <p>Select the best candidate for the role based on their qualifications.</p>

        <table className="employees-table">
          <thead>
            <tr>
              <th>Name</th>
              <th>Experience</th>
              <th>Job Percentage</th>
              <th>Rating</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {employees.map((employee) => (
              <React.Fragment key={employee.employeeId}>
                <tr>
                  <td>{employee.employeeName}</td>
                  <td>{employee.yearsExperience} years</td>
                  <td>{employee.jobPercentage}%</td>
                  <td>{employee.phoneNumber}</td>
                  <td>
                    <button onClick={() => handleExpand(employee.employeeId)}>
                      {expandedEmployeeId === employee.employeeId ? 'Hide Details' : 'Show Details'}
                    </button>
                    <input
                      type="radio"
                      name="selectedEmployee"
                      checked={selectedEmployee === employee.employeeId}
                      onChange={() => setSelectedEmployee(employee.employeeId)}
                    />
                  </td>
                </tr>
                {expandedEmployeeId === employee.employeeId && (
                  <tr className="employee-details">
                    <td colSpan={5}>
                      <strong>Skills:</strong> {employee.skills.join(', ')}<br />
                      <strong>Languages:</strong> {employee.foreignLanguages.join(', ')}<br />
                      <strong>Time Zone:</strong> {employee.timeZone}
                    </td>
                  </tr>
                )}
              </React.Fragment>
            ))}
          </tbody>
        </table>

        <div className="modal-actions">
          <button
            className="assign-button"
            onClick={handleAssign}
            disabled={!selectedEmployee}
          >
            Assign
          </button>
        </div>
      </div>
    </div>
  );
};

export default AssignEmployeeModal;
