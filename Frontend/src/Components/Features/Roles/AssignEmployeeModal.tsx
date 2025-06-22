import React, { useEffect, useState } from 'react';
import '../../../Styles/Modal.css';
import '../../../Styles/AssignModal.css';
import { Employee } from '../../../Types/EmployeeType'
import { getTimeZoneStringByIndex, getLanguageStringByIndex, getSkillStringByIndex } from '../../../Types/EnumType';
import { getAssignOptionsToRole } from '../../../Services/ProjectsService';
import { useDataContext } from '../../../Context/DataContext';
import EmployeesService from '../../../Services/EmployeesService';


const AssignEmployeeModal = ({
  projectId,
  roleId,
  onClose,
  // onAssign,
  openManualAssignModal,
}: {
  projectId: number;
  roleId: number;
  onClose: () => void;
  // onAssign: (employee: Employee) => void;
  openManualAssignModal: () => void;
}) => {
  const [employees, setEmployees] = useState<Employee[]>([]); // הגדרת טיפוס
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const [expandedEmployeeId, setExpandedEmployeeId] = useState<number | null>(null);
  const [showManualButton, setShowManualButton] = useState(false);
  const { roles, updateRole } = useDataContext();
  const [uiError, setUiError] = useState<string | null>(null);
  const [apiError, setApiError] = useState<string | null>(null);
  const currentRole = roles.find((r) => r.roleId === roleId);

  useEffect(() => {
    const fetchEmployees = async () => {
      try {
        const data: Employee[] = await getAssignOptionsToRole(projectId, roleId);
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

  // const handleAssign = () => {
  //   console.log("handleAssign in AssignEmployeeModal");
  //   if (selectedEmployee) {
  //     onAssign(selectedEmployee);
  //     onClose();
  //   }
  // };

  const handleAssign = async () => {
    if (!selectedEmployee) {
      setUiError("You must select the employee you want to assign");
      return;
    }
    setUiError(null);
    try {
      if (selectedEmployee && currentRole) {
        const updatedRole = { ...currentRole, employeeId: selectedEmployee.employeeId };
        await EmployeesService.assignEmployeeToRole(selectedEmployee.employeeId, updatedRole);
        updateRole(updatedRole);
        onClose();
      }
    } catch (error) {
      console.error('Error assigning employee to role:', error);
      setApiError('An error occurred while assigning employee to role');
    }
  };

  useEffect(() => {
      if (apiError) {
        alert(apiError);
      }
    }, [apiError]);

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>Assign Employee to Role</h2>

        {uiError && (
          <div className="ui-error" style={{ whiteSpace: 'pre-line' }}>
            {uiError}
          </div>
        )}

        <p>Select the best candidate for the role based on their qualifications.</p>

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
          {employees
  .filter((employee) => employee.employeeId !== 0) 
  .map((employee) => (
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
        <td>{(employee.jobPercentage * 100).toFixed(0)}%</td>
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

        <div className="manual-assign-row">
          <span className="hint-question" onClick={() => setShowManualButton(true)}>
            🥲 Can't find a good match?
          </span>

          {showManualButton && (
            <button
            className="manual-icon-button"
            onClick={() => {
              onClose();
              openManualAssignModal();
            }}
          >
              <i className="fa-solid fa-repeat"></i>
              <span className="manual-tooltip">Manual Assignment</span>
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default AssignEmployeeModal;
