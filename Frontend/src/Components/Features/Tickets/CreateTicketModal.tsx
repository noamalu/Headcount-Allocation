import React, { useState, useEffect } from 'react';
import { Ticket } from '../../../Types/TicketType';
import TicketsTable from './TicketsTable';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import { AbsenceReasonEnum } from '../../../Types/EnumType';
// import {sendCreateTicket} from '../../../Services/TicketsService';


const CreateTicketModal: React.FC<{ 
  onClose: () => void;
   onTicketCreated: (ticket: Ticket) => void }>
       = ({
         onClose,
         onTicketCreated,
       }) => {
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [absenceReason, setAbsenceReason] = useState<AbsenceReasonEnum | ''>('');
    const [description, setDescription] = useState('');
    const [error, setError] = useState<string>(""); 

    const absenceReasons = Object.values(AbsenceReasonEnum);
  
    const handleSubmit = async () => {
      if (!startDate || !endDate || !absenceReason || startDate > endDate) {
        setError("Start date, End date, and Absence reason are required.");
        return;
      }
      if (startDate > endDate) {
        setError("Start date can't be greater than End date.");
        return;
      }
      const newTicket: Ticket = {
        ticketId: -1, 
        employeeId: -1,
        employeeName: "",
        startDate,
        endDate,
        absenceReason: absenceReason,
        description,
        isOpen: true,
      };
      try {
        const newTicketId = 1;
        // const newTicketId = await sendCreateTicket(newTicket);
        newTicket.ticketId = newTicketId;
        console.log('Ticket created successfully:', newTicket);
        onTicketCreated(newTicket);
        onClose(); 
    } catch (error) {
        console.error('Error creating Ticket:', error);
        setError('An error occurred while creating the Ticket.');
    }
  };
  
    return (
      <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Create New Ticket</h2>
          <div className="modal-info">
            <div>
                <label>Employee Name: </label>
                <input
                type="text"
                value="temp"
                readOnly
                className="input-field readonly-field"
                />
            </div>
            <div className="modal-info-row">
                <div>
                <label>Start Date: </label>
                <input
                    type="date"
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    className="input-field"
                />
                </div>
                <div>
                <label>End Date: </label>
                <input
                    type="date"
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    className="input-field"
                />
                </div>
            </div>
            <div>
                <label>Absence Reason: </label>
                <select
                    value={absenceReason}
                    onChange={(e) => setAbsenceReason(e.target.value as AbsenceReasonEnum)}
                    className="dropdown"
                    >
                    <option value="" disabled>Select Reason</option>
                    {absenceReasons.map((reason) => (
                        <option key={reason} value={reason}>{reason}</option>
                    ))}
                </select>
          </div>
        </div>
        <div className="modal-info">
            <label>Description (Optional):</label>
            <textarea
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Enter more informaion.."
                className="textarea-field"
            ></textarea>
        </div>
        <div className="modal-actions">
            <button className="save-button" onClick={handleSubmit}>
                <i className="fas fa-save"></i> Save Ticket
            </button>
        </div>
      </div>
    </div>
  );
};

export default CreateTicketModal;
  