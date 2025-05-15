import { useState } from "react";
import { Ticket } from "../../../Types/TicketType";
import { AbsenceReasonEnum } from "../../../Types/EnumType";

const EditTicketModal: React.FC<{
    ticket: Ticket;
    onClose: () => void;
    onSave: (updatedTicket: Ticket) => void;
  }> = ({ ticket, onClose, onSave }) => {
    const [editedTicket, setEditedTicket] = useState<Ticket>({ ...ticket });
    const [uiError, setUiError] = useState<string | null>(null);

    const absenceReasons = Object.values(AbsenceReasonEnum);
    
  
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
      const { name, value } = e.target;
      setEditedTicket((prev) => ({ ...prev, [name]: value }));
    };
  
    const handleSave = () => {
      if (!editedTicket.absenceReason.trim() || !editedTicket.description.trim()) {
        setUiError("All fields must be filled.");
        return;
      }
  
      setUiError(null);
      onSave(editedTicket);
      onClose();
    };
  
    return (
      <div className="modal-overlay details-modal">
        <div className="modal-content details-modal">
          <button className="close-button" onClick={onClose}>✖</button>
          <h2>Edit Ticket</h2>
  
          {uiError && <div className="ui-error">{uiError}</div>}

          <div className="details-section">

          <div className="detail-banner row">
            <i className="fa-solid fa-user" ></i>
            <span><strong>Employee Name:</strong> {ticket.employeeName}</span>
          </div>
  
          <div className="edit-banner row">
            <i className="fas fa-calendar-week"></i>
            <span>
              <strong>Start Date:</strong>
              <input
                type="date"
                name="startDate"
                value={editedTicket.startDate.toString().split('T')[0]}
                onChange={handleInputChange}
              />
            </span>
            <span>
              <strong>End Date:</strong>
              <input
                type="date"
                name="endDate"
                value={editedTicket.endDate.toString().split('T')[0]}
                onChange={handleInputChange}
              />
            </span>
          </div>
  
          <div className="edit-banner">
            <i className="fas fa-circle-question"></i>
            <span>
              <strong>Absence Reason:</strong>
              <select
                value={editedTicket.absenceReason}
                onChange={handleInputChange}
                className="dropdown"
                >
                <option value="" disabled>Select Reason</option>
                {absenceReasons.map((reason) => (
                    <option key={reason} value={reason}>{reason}</option>
                ))}
              </select>
            </span>
          </div>
  
          <div className="edit-banner wide">
            <i className="fas fa-align-left"></i>
            <div className="field-container">
                <label><strong>Description:</strong></label>
                  <textarea
                      id="description"
                      name="description"
                      value={editedTicket.description}
                      onChange={handleInputChange}
                      className="textarea-field"
                    />
                </div>
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