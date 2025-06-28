import React, { useState } from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import { useDataContext } from '../Context/DataContext';
import ProjectDetailsModal from '../Components/Features/Projects/ProjectDetailsModal';
import { Project } from '../Types/ProjectType';
import '../Styles/Calendar.css';
import { Role } from '../Types/RoleType';
import RoleDetailsModal from '../Components/Features/Roles/RoleDetailsModal';
import { Ticket } from '../Types/TicketType';
import TicketDetailsModal from '../Components/Features/Tickets/TicketDetailsModal';

const CalendarPage: React.FC = () => {
  const { projects, roles, tickets} = useDataContext();
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  const [selectedRole, setSelectedRole] = useState<Role| null>(null);
  const [selectedTicket, setSelectedTicket] = useState<Ticket| null>(null);



  const events = [
  ...formatProjectsToEvents(projects),
  ...formatRolesToEvents(roles),
  ...formatTicketsToEvents(tickets)
  ];

  const handleEventClick = (info: any) => {
    const { type, projectId, roleId, ticketId} = info.event.extendedProps;
    if (type === 'project') {
      const project = projects.find((p) => p.projectId === projectId);
      if (project) {
        setSelectedProject(project);
      }
    }
    if (type === 'role') {
      const role = roles.find((r) => r.roleId === roleId);
      if (role) {
        setSelectedRole(role);
      }
    }
    if (type === 'ticket') {
      const ticket = tickets.find((t) => t.ticketId === ticketId);
      if (ticket) {
        setSelectedTicket(ticket);
      }
    }
  };

  return (
    <div className="calendar-page">
      <div className="calendar-header">
        <h1 className="page-title">Calendar</h1>
      </div>

      <FullCalendar
        plugins={[dayGridPlugin]}
        initialView="dayGridMonth"
        height="auto"
        events={events}
        eventClick={handleEventClick}
      />

      {selectedProject && (
        <ProjectDetailsModal
          project={selectedProject}
          onClose={() => setSelectedProject(null)}
        />
      )}

      {selectedRole && (
        <RoleDetailsModal
          roleId={selectedRole.roleId}
          projectId={selectedRole.projectId}
          onClose={() => setSelectedRole(null)}
        />
      )}

      {selectedTicket && (
        <TicketDetailsModal
          ticketId={selectedTicket.ticketId}
          onClose={() => setSelectedTicket(null)}
        />
      )}
      
    </div>
  );
};

export default CalendarPage;

const formatProjectsToEvents = (projects: Project[]) => {
  return projects.map((project) => ({
    title: `📌 ${project.projectName} Deadline`,
    date: project.deadline?.split('T')[0], 
    backgroundColor: '#E8CEBF',
    borderColor: '#E8CEBF',
    textColor: '#4F4846',
    extendedProps: {
      type: 'project',
      projectId: project.projectId,
    },
  }));
};

const formatRolesToEvents = (roles: Role[]) => {
  return roles
    .filter((role) => role.startDate)
    .map((role) => ({
      title: `👤 Role start: ${role.roleName}`,
      date: role.startDate.split('T')[0],
      backgroundColor: '#A9C9A4',
      borderColor: '#A9C9A4',
      textColor: '#4F4846',
      extendedProps: {
        type: 'role',
        roleId: role.roleId,
        projectId: role.projectId, 
      },
    }));
};

const formatTicketsToEvents = (tickets: Ticket[]) => {
  return tickets.map((ticket) => ({
    title: `🎫 ${ticket.employeeName} : ${ticket.absenceReason}`,
    start: ticket.startDate?.split('T')[0],
    end: ticket.endDate?.split('T')[0],
    backgroundColor: '#FFD580',
    borderColor: '#FFD580',
    textColor: '#4F4846',
    extendedProps: {
      type: 'ticket',
      ticketId: ticket.ticketId,
    },
  }));
};
