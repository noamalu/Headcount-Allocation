import React, { useState } from 'react';
import '../../../Styles/Modal.css';

interface AssignEmployeeModalProps {
  role: string; // שם התפקיד שאליו רוצים לשייך עובד
  onClose: () => void; // פונקציה לסגירת החלון
  onAssign: (employee: string) => void; // פונקציה שמבצעת את השיוך בפועל
}

const AssignEmployeeModal: React.FC<AssignEmployeeModalProps> = ({ role, onClose, onAssign }) => {
  const [selectedEmployee, setSelectedEmployee] = useState('');

  const handleAssign = () => {
    if (selectedEmployee) {
      onAssign(selectedEmployee);
      onClose(); // סוגר את החלון לאחר השיוך
    }
  };

  return (
    <div className="modal">
      <div className="modal-header">
        <h2>Assign Employee to {role}</h2>
        <button onClick={onClose}>X</button>
      </div>
      <div className="modal-body">
        <label htmlFor="employee">Select Employee:</label>
        <select
          id="employee"
          value={selectedEmployee}
          onChange={(e) => setSelectedEmployee(e.target.value)}
        >
          <option value="">--Select Employee--</option>
          {/* לדוגמה: */}
          <option value="Employee A">Employee A</option>
          <option value="Employee B">Employee B</option>
          <option value="Employee C">Employee C</option>
        </select>
        <button onClick={handleAssign}>Assign</button>
      </div>
    </div>
  );
};

export default AssignEmployeeModal;