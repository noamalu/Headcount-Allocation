import { useState } from "react";
import { Ticket } from "../../../Types/TicketType";
import { AbsenceReasonEnum } from "../../../Types/EnumType";
import { useDataContext } from "../../../Context/DataContext";
import TicketsService from "../../../Services/TicketsService";
// import '../../../Styles/Modal.css';
// import '../../../Styles/Shared.css'; 


const EditTicketModal: React.FC<{
    ticket: Ticket;
    onClose: () => void;
    // onSave: (updatedTicket: Ticket) => void;
  }> = ({ ticket, onClose }) => {
    const [editedTicket, setEditedTicket] = useState<Ticket>({ ...ticket });
    const [uiError, setUiError] = useState<string | null>(null);
    const [apiError, setApiError] = useState<string | null>(null);
    const { updateTicket } = useDataContext();
    const absenceReasons = Object.values(AbsenceReasonEnum);
    
  
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
      const { name, value } = e.target;
      setEditedTicket((prev) => ({ ...prev, [name]: value }));
    };
  
    const handleSave = async () => {
      if (!editedTicket.absenceReason.trim() || !editedTicket.description.trim()) {
        setUiError("All fields must be filled.");
        return;
      }
      setUiError(null);
      try {
        console.log("Sending edit ticket for: ", editedTicket.ticketId," with details:", editedTicket.absenceReason, editedTicket.description);
        await TicketsService.editTicket(editedTicket.employeeId, ticket);
        setApiError(null);
        // onSave(editedProject); 
        updateTicket(editedTicket);
        onClose(); 
      } catch (error) {
        console.error('Error updating ticket:', error);
        setApiError('An error occurred while updating the ticket');
      }
    };
  
    return (
      <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Edit Ticket</h2>

          {uiError && <div className="ui-error">{uiError}</div>}

          <div className="modal-info">
            <div className="detail-small">
              <i className="fa-solid fa-user" ></i>
              {ticket.employeeName}
            </div>
  
          <div className="field-with-icon flexible">
            <i className="fas fa-calendar-week"></i>
              Start Date:
              <input
                type="date"
                name="startDate"
                value={editedTicket.startDate.toString().split('T')[0]}
                onChange={handleInputChange}
                className="input-date"
              />
              End Date:
              <input
                type="date"
                name="endDate"
                value={editedTicket.endDate.toString().split('T')[0]}
                onChange={handleInputChange}
                className="input-date"
              />
          </div>
  
          <div className="field-with-icon flexible">
            <span>
              <i className="fas fa-circle-question"></i>
              Absence Reason:
            </span>
            <select
              name="absenceReason"
              value={editedTicket.absenceReason}
              onChange={handleInputChange}
              className="dropdown">
              <option value="" disabled>Select Reason</option>
                {absenceReasons.map((reason) => (
                    <option key={reason} value={reason}>{reason}</option>
                ))}
            </select>
          </div>
  
          <div className="field-with-icon wide">
            <i className="fas fa-align-left"></i>
            Description:
                  <textarea
                      id="description"
                      name="description"
                      value={editedTicket.description}
                      onChange={handleInputChange}
                      className="textarea-field"
                    />
                </div>
          </div>
  
          <div className="modal-actions">
            <button className="save-button" onClick={handleSave}>
              <i className="fas fa-save"></i> Save
            </button>
          </div>
        </div>
      </div>
    );
  };

  
export default EditTicketModal;