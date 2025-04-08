import React, { useState, useEffect } from 'react';
import { formatDate, Ticket } from '../../../Types/TicketType';
import { Role } from '../../../Types/RoleType';
import '../../../Styles/Modal.css';
import '../../../Styles/Shared.css';
import '../../../Styles/DetailsModal.css';
import { getEmployeeRolesById } from '../../../Services/EmployeesService';
import RoleDetailsModal from '../Roles/RoleDetailsModal';

interface TicketDetailsModalProps {
  ticket: Ticket;
  onClose: () => void;
}

const TicketDetailsModal: React.FC<TicketDetailsModalProps> = ({ ticket, onClose }) => {
  const [roles, setRoles] = useState<Role[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchEmployeeRoles = async () => {
      try {
        const response = await getEmployeeRolesById(ticket.employeeId);
        setRoles(response);
        setLoading(false);
      } catch (err: any) {
        console.error('Error fetching employee roles:', err);
        setError('Failed to fetch roles');
        setLoading(false);
      }
    };
    fetchEmployeeRoles();
  }, [ticket.employeeId]);

  if (loading) return <div>Loading ticket details...</div>;
  if (error) return <div>{error}</div>;

  const handleOpenModal = (role: Role) => {
    setSelectedRole(role);
  };

  const handleCloseModal = () => {
    setSelectedRole(null);
  };

  return (
    <div className="modal-overlay details-modal">
      <div className="modal-content details-modal">
        <button className="close-button" onClick={onClose}>✖</button>
        <h2>Ticket Details</h2>
        <div className="details-section">
            <div className="detail-banner">
                <i className="fa-solid fa-user" ></i>
                <span><strong>Employee Name:</strong> {ticket.employeeName}</span>
            </div>
            <div className="modal-info-row">
                <div className="detail-banner">
                    <i className="fa-solid fa-calendar"></i> 
                    <span><strong>Start Date:</strong> {formatDate(ticket.startDate)}</span>
                </div>
                <div className="detail-banner">
                    <i className="fa-solid fa-calendar-week"></i> 
                    <span><strong>End Date:</strong> {formatDate(ticket.endDate)}</span>
                </div>
            </div>
            <div className="modal-info-row">
                <div className="detail-banner">
                    <i className="fa-solid fa-circle-question"></i>
                    <span><strong>Absence Reason:</strong> {ticket.absenceReason}</span>
                </div>
            </div>
            </div>
            <div className="description">
                <div className="detail-banner">
                    <i className="fas fa-align-left"></i>
                    <span><strong>Description:</strong> {ticket.description}</span>
                </div>
            </div>
            <div className="details-section">
                <div className="detail-banner">
                    <i className="fa-solid fa-chalkboard-user"></i>
                    <strong> Employee Roles:</strong>
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
                <button className="save-button">✔ Close Ticket</button>
            </div>
            {selectedRole && (
            <RoleDetailsModal 
                projectId={selectedRole.projectId} 
                role={selectedRole} 
                onClose={handleCloseModal} 
            />
            )}
        </div>
    </div>
  );
};

export default TicketDetailsModal;
