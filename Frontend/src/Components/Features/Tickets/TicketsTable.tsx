import React, { useEffect, useState } from 'react';
import { Ticket, formatDate } from '../../../Types/TicketType';
import '../../../Styles/Projects.css';
import '../../../Styles/Shared.css';
import { getAllTickets, getTicketsByEmployeeId } from '../../../Services/TicketsService';
import { useAuth } from '../../../Context/AuthContext';
import TicketDetailsModal from './TicketDetailsModal';
import { useDataContext } from '../../../Context/DataContext';
import { getAbsenceReasonStringByEnumString } from '../../../Types/EnumType';

const TicketsTable: React.FC = () => {
  const { tickets, setTickets } = useDataContext();
  const [selectedTicket, setSelectedTicket] = useState<Ticket | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);
  const { currentUser, currentId, isAdmin } = useAuth();

  useEffect(() => {
    if (apiError) {
      alert(apiError);
    }
  }, [apiError]);


  const fetchTickets = async () => {
    setIsLoading(true); 
    setApiError(null); 

    try {
      if (isAdmin) {
        const data = await getAllTickets();
        console.log("getAllTickets ", data);
        setTickets(data);
      } else {
        const data = await getTicketsByEmployeeId(currentId);
        console.log("getTicketsByEmployeeId ", data);
        setTickets(data);
      }
    } catch (err) {
      setApiError('Failed to fetch Tickets. Please try again later.');
    } finally {
      setIsLoading(false); 
    }
  };

  useEffect(() => {
    fetchTickets();
}, []);


const handleOpenModal = (ticket: Ticket) => {
  setSelectedTicket(ticket);
};

const handleCloseModal = () => {
  setSelectedTicket(null);
};

if (isLoading) {
    return <p>Loading tickets...</p>;
}


  return (
    <div>
      <table className="projects-table">
        <thead>
          <tr>
            <th>Employee</th>
            <th>Absence Reason</th>
            <th>Start Date</th>
            <th>End Date</th>
            <th>Status</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {tickets.map((ticket) => (
            <tr key={ticket.ticketId}>
              <td>{ticket.employeeName}</td>
              <td>{ticket.absenceReason}</td>
              <td>{formatDate(ticket.startDate)}</td>
              <td>{formatDate(ticket.endDate)}</td>
              <td>
                <div className={`status-icon ${ticket.open ? 'status-open' : 'status-closed'}`}>
                    {ticket.open ? '' : '✔'}
                </div>
              </td>
              <td>
                <button className="action-button" onClick={() => handleOpenModal(ticket)}>🔗</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {selectedTicket && (
        <TicketDetailsModal ticketId={selectedTicket.ticketId} onClose={handleCloseModal} />
      )}
    </div>
  );
};

export default TicketsTable;