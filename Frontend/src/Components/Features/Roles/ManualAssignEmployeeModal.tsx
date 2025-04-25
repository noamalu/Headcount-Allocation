import React, { useEffect, useState } from 'react';
import '../../../Styles/Modal.css';
import '../../../Styles/AssignModal.css';
import { Employee } from '../../../Types/EmployeeType'
import { getTimeZoneStringByIndex, getLanguageStringByIndex, getSkillStringByIndex } from '../../../Types/EnumType';
import { getAssignOptionsToRole } from '../../../Services/ProjectsService';
import { getEmployees } from '../../../Services/EmployeesService';


const ManualAssignEmployeeModal = ({
  projectId,
  roleId,
  onClose,
  onAssign,
}: {
  projectId: number;
  roleId: number;
  onClose: () => void;
  onAssign: (employee: Employee) => void;
}) => {
  const [employees, setEmployees] = useState<Employee[]>([]); // הגדרת טיפוס
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const [expandedEmployeeId, setExpandedEmployeeId] = useState<number | null>(null);

  useEffect(() => {
    const fetchEmployees = async () => {
      try {
        const data: Employee[] = await getEmployees();
        setEmployees(data);
        if (data.length > 0) {
          setSelectedEmployee(data[0]);
        }
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
    console.log("handleManualAssign in ManualAssignEmployeeModal");
    if (selectedEmployee) {
      onAssign(selectedEmployee);
      onClose();
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>Assign Employee Manually</h2>
        <p>Select the Employee to assign.</p>

        <table className="employees-table">
          <thead>
            <tr>
              <th></th>
              <th>Name</th>
              <th>Experience</th>
              <th>Job Percentage</th>
              <th>Phone Number</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
  {employees.map((employee) => (
    <React.Fragment key={employee.employeeId}>
      <tr>
        <td>
          <input
            type="radio"
            name="selectedEmployee"
            className="custom-radio"
            checked={selectedEmployee === employee}
            onChange={() => setSelectedEmployee(employee)} // עדכון הבחירה
          />
        </td>
        <td>{employee.employeeName}</td>
        <td>{employee.yearsExperience} years</td>
        <td>{employee.jobPercentage}%</td>
        <td>{employee.phoneNumber}</td>
        <td>
          <button className="show-details-button" onClick={() => handleExpand(employee.employeeId)}>
            {expandedEmployeeId === employee.employeeId ? 'Hide Details' : 'Show Details'}
          </button>
        </td>
      </tr>
      {expandedEmployeeId === employee.employeeId && (
        <tr className="employee-details">
          <td colSpan={6}>
            {/* Time Zone */}
              <strong>Time Zone:</strong> 
              <ul className="details-list">
                {getTimeZoneStringByIndex(employee.timeZone)}
              </ul>        
              
            {/* Skills */}
            <strong>Skills:</strong>
            {employee.skills.length > 0 ? (
              <ul className="details-list">
                {employee.skills.map((skill, index) => (
                  <li key={index}>
                    {getSkillStringByIndex(skill.skillTypeId)} (Level: {skill.level})
                  </li>
                ))}
              </ul>
            ) : (
              <span>No skills available</span>
            )}

            {/* Languages */}
            <strong>Languages:</strong>
            {employee.foreignLanguages.length > 0 ? (
              <ul className="details-list">
                {employee.foreignLanguages.map((language, index) => (
                  <li key={index}>
                    {getLanguageStringByIndex(language.languageTypeId)} (Level: {language.level})
                  </li>
                ))}
              </ul>
            ) : (
              <span>No languages available</span>
            )}
            
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
            disabled={!selectedEmployee}>
            Assign
          </button>
        </div>
      </div>
    </div>
  );
};

export default ManualAssignEmployeeModal;
