import React from 'react';
import '../../../Styles/Employees.css'
import {Employee} from '../../../Types/EmployeeType'


interface EmployeeCardProps {
  employee: Employee; 
  onClick: () => void; 
}

const EmployeeCard: React.FC<EmployeeCardProps> = ({ employee, onClick }) => {
    return (
        <div className="employee-card" onClick={onClick}>
            <div className="avatar-container">
                <i className="fas fa-user-circle avatar-icon" />
            </div>
            <h3 className="employee-name">{employee.employeeName}</h3>
            <p className="employee-experience">Experience: {employee.yearsExperience} years</p>
        </div>
    );
};

export default EmployeeCard;