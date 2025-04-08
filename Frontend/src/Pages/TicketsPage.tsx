import React, { useState, useRef } from 'react';
import TicketsTable from '../Components/Features/Tickets/TicketsTable'; 
import '../Styles/Projects.css';
import CreateTicketModal from '../Components/Features/Tickets/CreateTicketModal';
import { Ticket } from '../Types/TicketType';
 

const TicketsPage: React.FC = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const tableRef = useRef<(ticket: Ticket) => void>();

    const handleTicketCreated = (ticket: Ticket) => {
        if (tableRef.current) {
            tableRef.current(ticket); 
        }
    };

    const handleOpenModal = () => {
        setIsModalOpen(true); 
    };

    const handleCloseModal = () => {
        setIsModalOpen(false); 
    };


    return (
        <div className="projects-page">
            <div className="projects-header">
                <h1 className="page-title">My Tickets</h1> 
                <button className="add-project-button" onClick={handleOpenModal}>+ New Ticket</button>
            </div>
            <TicketsTable onTicketCreated={(callback) => (tableRef.current = callback)} />            
            {isModalOpen && (
                <CreateTicketModal
                    onClose={() => setIsModalOpen(false)} 
                    onTicketCreated={handleTicketCreated} 
                />
            )}
        </div>
    );
};

export default TicketsPage;