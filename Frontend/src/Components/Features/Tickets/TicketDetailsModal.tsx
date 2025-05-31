import React, { useState, useEffect } from 'react';
import { formatDate, Ticket } from '../../../Types/TicketType';
import { Role } from '../../../Types/RoleType';
// import '../../../Styles/Modal.css';
// import '../../../Styles/Shared.css';
// import '../../../Styles/DetailsModal.css';
import { getEmployeeRolesById } from '../../../Services/EmployeesService';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditTicketModal from './EditTicketModal';
import { useDataContext } from '../../../Context/DataContext';
import TicketsService from '../../../Services/TicketsService';

interface TicketDetailsModalProps {
  ticketId: number;
  onClose: () => void;
}

const TicketDetailsModal: React.FC<TicketDetailsModalProps> = ({ ticketId, onClose }) => {
  const [roles, setRoles] = useState<Role[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);
  const { tickets, deleteTicket } = useDataContext();
  const [showConfirmDelete, setShowConfirmDelete] = useState(false);
  const ticket = tickets.find((t) => t.ticketId === ticketId);

  if (!ticket) {
    return <div>Ticket not found</div>;
  }

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);

  useEffect(() => {
    const fetchEmployeeRoles = async () => {
      try {
        const response = await getEmployeeRolesById(ticket.employeeId);
        setRoles(response);
        setLoading(false);
      } catch (err: any) {
        console.error('Error fetching employee roles:', err);
        setApiError('Failed to fetch roles');
        setLoading(false);
      }
    };
    fetchEmployeeRoles();
  }, [ticket.employeeId]);

  if (loading) return <div>Loading ticket details...</div>;

  const handleOpenModal = (role: Role) => {
    setSelectedRole(role);
  };

  const handleCloseModal = () => {
    setSelectedRole(null);
  };

  const handleDelete = async () => {
    try {
      await TicketsService.deleteTicket(ticket.employeeId, ticketId);
      deleteTicket(ticketId);
      onClose(); 
    } catch (error) {
      alert("Failed to delete the ticket.");
      console.error(error);
    }
  };
  

//   return (
//     <div className="modal-overlay details-modal">
//       <div className="modal-content details-modal">
//         <button className="close-button" onClick={onClose}>✖</button>
//         <h2>Ticket Details</h2>

//             <div className="detail-banner row">
//                 <i className="fa-solid fa-user" ></i>
//                 <span><strong>Employee Name:</strong> {ticket.employeeName}</span>
//             </div>
           
//             <div className="detail-banner row">
//                 <i className="fa-solid fa-calendar-week"></i> 
//                 <span><strong>Dates:</strong> {formatDate(ticket.startDate)} ➜ {formatDate(ticket.endDate)}</span>
//             </div>

//             <div className="detail-banner row">
//                 <i className="fa-solid fa-circle-question"></i>
//                 <span><strong>Absence Reason:</strong> {ticket.absenceReason}</span>
//             </div>

//             <div className="detail-banner row">
//                 <i className="fas fa-align-left"></i>
//                 <span><strong>Description:</strong> {ticket.description}</span>
//             </div>

//             {/* <div className="details-section"> */}
//                 <div className="detail-banner">
//                     <i className="fa-solid fa-chalkboard-user"></i>
//                     <strong> Employee Roles:</strong>
//                     <table className="roles-table">
//                         <thead>
//                         <tr>
//                             <th>Role Name</th>
//                             <th>Project ID</th>
//                             <th>Role</th>
//                         </tr>
//                         </thead>
//                         <tbody>
//                         {roles.length > 0 ? (
//                             roles.map((role, index) => (
//                             <tr key={index}>
//                                 <td>{role.roleName}</td>
//                                 <td>{role.projectId}</td>
//                                 <td>
//                                 <button onClick={() => handleOpenModal(role)} className="action-button">🔗</button>
//                                 </td>
//                             </tr>
//                             ))
//                         ) : (
//                             <tr>
//                             <td colSpan={3} className="no-roles">No roles available</td>
//                             </tr>
//                         )}
//                         </tbody>
//                     </table>
//                 </div>
//             {/* </div> */}

//             <div className="modal-actions">
//               <button className="delete-button">🗑 Delete</button>
//               {ticket.isOpen ? (
//                 <button className="save-button">✔ Close Ticket</button>
//               ) : (
//                 <button className="assign-button">↻ Re-open Ticket</button>
//               )}
//               <button className="edit-button" onClick={() => { console.log('Opening edit modal:', !isEditModalOpen); setIsEditModalOpen(true); }}>
//                 <i className="fas fa-pen"></i> Edit
//               </button>
//             </div>

//             {isEditModalOpen && (
//             <EditTicketModal
//               ticket={ticket}
//               onClose={() => setIsEditModalOpen(false)}
//               onSave={() => {}} />
//           )}

//             {selectedRole && (
//             <RoleDetailsModal 
//                 projectId={selectedRole.projectId} 
//                 role={selectedRole} 
//                 onClose={handleCloseModal} 
//             />
//             )}
//         </div>
//     </div>
//   );
// };

    return (
      <div className="modal-overlay">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>✖</button>
         
          <div className={`ticket-status-badge ${ticket.isOpen ? 'open' : 'closed'}`}>
            <i className={`fas ${ticket.isOpen ? 'fa-times-circle' : 'fa-check-circle'}`}></i>
            {ticket.isOpen ? ' Open Ticket' : ' Closed Ticket'}
          </div>

          <h2>Ticket Details</h2>

          <div className="modal-info">
            <div className="detail-small">
              <i className="fa-solid fa-user" ></i>
              {ticket.employeeName}
            </div>

            <div className="modal-info-row">
              <div className="detail-medium">
              <i className="fas fa-calendar-week"></i>                
                {formatDate(ticket.startDate)}
                <strong> - </strong>    
                {formatDate(ticket.endDate)}
              </div>

              <div className="detail-medium">
                <i className="fas fa-circle-question"></i>
                {ticket.absenceReason}
              </div>
            </div>


            <div className="description">
              <div className="textarea-style">{ticket.description || "No description provided."}</div>
            </div>

              {/* Roles Section */}
            <div className="detail-table">
            <div className="skills-section">
                <i className="fa-solid fa-chalkboard-user"></i>
                Employee roles:
                <table className="roles-table">
                <thead>
                    <tr>
                        <th>Role Name</th>
                        <th>Project ID</th>
                        <th>Role</th>
                    </tr>
                </thead>
                <tbody>
                    {roles.length > 0 ? (
                        roles.map((role, index) => (
                        <tr key={index}>
                            <td>{role.roleName}</td>
                            <td>{role.projectId}</td>
                            <td>
                              <button className="action-button" onClick={() => handleOpenModal(role)}>
                                🔗
                              </button>
                            </td>
                        </tr>
                        ))
                    ) : (
                    <tr>
                      <td colSpan={3} className="no-roles">No roles available for this user.</td>
                    </tr>
                    )}
                    </tbody>
                </table>
            </div>
          </div>
         </div>

          <div className="modal-actions">
            <button className="delete-button" onClick={() => setShowConfirmDelete(true)}>
              <i className="fas fa-trash"></i> Delete
            </button>
            {ticket.isOpen ? (
              <button className="save-button">✔ Close Ticket</button>
            ) : (
              <button className="assign-button">↻ Re-open Ticket</button>
            )}
            <button className="edit-button" onClick={() => setIsEditModalOpen(true)}>
              <i className="fas fa-pen"></i> Edit
            </button>
          </div>

          {isEditModalOpen && (
            <EditTicketModal
              ticket={ticket}
              onClose={() => setIsEditModalOpen(false)} />
              // onSave={() => {}} />
          )}

          {selectedRole && (
            <RoleDetailsModal 
              projectId={selectedRole.projectId} 
              roleId={selectedRole.roleId} 
              onClose={handleCloseModal} 
            />
          )}
          {showConfirmDelete && (
          <div className="confirm-overlay">
            <div className="confirm-dialog">
              <p>Are you sure you want to delete this ticket?</p>
              <div className="confirm-buttons">
                <button className="confirm-button" onClick={handleDelete}>
                  <i className="fas fa-trash"></i> Delete
                </button>
                <button className="cancel-button" onClick={() => setShowConfirmDelete(false)}>
                <i className="fas fa-close"></i> Cancel
                </button>
              </div>
            </div>
          </div>
        )}
        </div>
      </div>
    );
  };

export default TicketDetailsModal;
