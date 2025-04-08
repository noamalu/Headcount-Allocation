import React from 'react';
import '../../../Styles/Employees.css'
import {Employee} from '../../../Types/EmployeeType'


interface EmployeeCardProps {
  employee: Employee; // Specify that the prop is of type Project
  onClick: () => void; // Callback for closing the modal
}

const EmployeeCard: React.FC<EmployeeCardProps> = ({ employee, onClick }) => {
    return (
        <div className="employee-card" onClick={onClick}>
            <div className="avatar-container">
                <i className="fas fa-user-circle avatar-icon" />
                {/* <img src={avatarUrl} alt={name} className="avatar" /> */}
            </div>
            <h3 className="employee-name">{employee.employeeName}</h3>
            <p className="employee-experience">Experience: {employee.yearsExperience} years</p>
            {/* <p className="employee-department">{employee.department}</p> */}
        </div>
    );
};

export default EmployeeCard;