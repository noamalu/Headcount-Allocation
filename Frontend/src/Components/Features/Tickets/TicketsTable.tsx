import React, { useEffect, useState } from 'react';
// import ProjectDetailsModal from './ProjectDetailsModal';
import { Ticket, formatDate } from '../../../Types/TicketType';
import '../../../Styles/Projects.css';
import '../../../Styles/Shared.css';
import { getTicketsByLoggedUser } from '../../../Services/TicketsService';
// @ts-ignore
// import { Tooltip } from 'react-tooltip';

const TicketsTable: React.FC<{ onTicketCreated: (callback: (ticket: Ticket) => void) => void }> = ({ onTicketCreated }) => {
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [selectedTicket, setSelectedTicket] = useState<Ticket | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchTickets = async () => {
    setIsLoading(true); 
    setError(null); 

    try {
      const data = await getTicketsByLoggedUser();
      setTickets(data);
    } catch (err) {
      setError('Failed to fetch Tickets. Please try again later.');
    } finally {
      setIsLoading(false); 
    }
  };

  useEffect(() => {
    fetchTickets();
}, []);


useEffect(() => {
    const handleTicketCreated = (newTicket: Ticket) => {
        setTickets((prevTickets) => [...prevTickets, newTicket]);
    };
    onTicketCreated(handleTicketCreated); 
}, [onTicketCreated]);

const handleOpenModal = (ticket: Ticket) => {
  setSelectedTicket(ticket);
};

const handleCloseModal = () => {
  setSelectedTicket(null);
};

if (isLoading) {
    return <p>Loading tickets...</p>;
}

if (error) {
    return <p className="error">{error}</p>;
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
            <th>Description</th>
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
                <div className={`status-icon ${ticket.isOpen ? 'status-open' : 'status-closed'}`}>
                    {ticket.isOpen ? '✔' : ''}
                </div>
              </td>
              <td>
                {/* <span 
                  id={`tooltip-${ticket.ticketId}`} 
                  className="tooltip-trigger" 
                  data-tooltip-content={ticket.description}>
                  ...
                </span>
                <Tooltip anchorSelect={`#tooltip-${ticket.ticketId}`} place="top" /> */}
              </td>
              <td>
                <button className="action-button" onClick={() => handleOpenModal(ticket)}>🔗</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {/* {selectedTicket && (
        <TicketDetailsModal ticket={selectedTicket} onClose={handleCloseModal} />
      )} */}
    </div>
  );
};

export default TicketsTable;