import React, { useState, useEffect } from 'react';
import { formatDate, Ticket } from '../../../Types/TicketType';
import { Role } from '../../../Types/RoleType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { getEmployeeRolesById } from '../../../Services/EmployeesService';
import RoleDetailsModal from '../Roles/RoleDetailsModal';
import EditTicketModal from './EditTicketModal';

interface TicketDetailsModalProps {
  ticket: Ticket;
  onClose: () => void;
}

const TicketDetailsModal: React.FC<TicketDetailsModalProps> = ({ ticket, onClose }) => {
  const [roles, setRoles] = useState<Role[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);

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
          <h2>Ticket Details</h2>

          <div className="modal-info">
            <div className="field-display">
              <label>Employee Name:</label>
              <span>{ticket.employeeName}</span>
            </div>

            <div className="modal-info-row">
              <div className="field-display">
                <label>Start Date:</label>
                <span>{formatDate(ticket.startDate)}</span>
              </div>
              <div className="field-display">
                <label>End Date:</label>
                <span>{formatDate(ticket.endDate)}</span>
              </div>
            </div>

            <div className="field-display">
              <label>Absence Reason:</label>
              <span>{ticket.absenceReason}</span>
            </div>

            <div className="field-display">
              <label>Description:</label>
              <div className="textarea-style">{ticket.description || "No description provided."}</div>
            </div>

            <div className="field-display">
              <label>Employee Roles:</label>
              <table className="roles-table">
                <thead>
                  <tr>
                    <th>Role Name</th>
                    <th>Project ID</th>
                    <th>Action</th>
                  </tr>
                </thead>
                <tbody>
                  {roles.length > 0 ? (
                    roles.map((role, index) => (
                      <tr key={index}>
                        <td>{role.roleName}</td>
                        <td>{role.projectId}</td>
                        <td>
                          <button onClick={() => handleOpenModal(role)} className="action-button">🔗</button>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan={3} className="no-roles">No roles available</td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>

          <div className="modal-actions">
            <button className="delete-button">🗑 Delete</button>
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
              onClose={() => setIsEditModalOpen(false)}
              onSave={() => {}} />
          )}

          {selectedRole && (
            <RoleDetailsModal 
              projectId={selectedRole.projectId} 
              roleId={selectedRole.roleId} 
              onClose={handleCloseModal} 
            />
          )}
        </div>
      </div>
    );
  };

export default TicketDetailsModal;
