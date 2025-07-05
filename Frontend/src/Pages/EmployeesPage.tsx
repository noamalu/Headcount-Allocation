import React, { useState, useRef } from 'react';
import CreateEmployeeModal from '../Components/Features/Employees/CreateEmployeeModal';
import '../Styles/Projects.css';
import { Employee } from '../Types/EmployeeType';
import EmployeesSpan from '../Components/Features/Employees/EmployeesSpan';
 

const EmployeesPage: React.FC = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);

    const handleOpenModal = () => {
        setIsModalOpen(true); 
    };

    const handleCloseModal = () => {
        setIsModalOpen(false); 
    };


    return (
        <div className="employees-page">
            <div className="employees-header">
                <h1 className="page-title">Employees</h1> 
                <button className="add-employee-button" onClick={handleOpenModal}>+ New Employee</button>
            </div>
            <EmployeesSpan />
            {isModalOpen && (
                <CreateEmployeeModal onClose={() => setIsModalOpen(false)} />
            )}
        </div>
    );
};

export default EmployeesPage;