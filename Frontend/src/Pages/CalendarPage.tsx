import React from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import '../Styles/Calendar.css'

const CalendarPage: React.FC = () => {
  return (
    <div className="calendar-page">
        <div className="calendar-header">
            <h1 className="page-title">Calendar</h1> 
        </div>
        <FullCalendar
            plugins={[dayGridPlugin]}
            initialView="dayGridMonth"
            height="auto"
            events={[]} 
        />
    </div>
  );
};

export default CalendarPage;
