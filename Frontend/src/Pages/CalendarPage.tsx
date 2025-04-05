import React, { useState } from 'react';
import { Calendar, momentLocalizer, Event } from 'react-big-calendar';
import moment from 'moment';
import 'react-big-calendar/lib/css/react-big-calendar.css';

const localizer = momentLocalizer(moment);

const CalendarPage: React.FC = () => {
  const [events, setEvents] = useState<Event[]>([
    {
      title: 'Project Deadline',
      start: new Date(2025, 3, 15, 10, 0), // 15 אפריל 2025, 10:00
      end: new Date(2025, 3, 15, 11, 0),
      allDay: false,
    },
    {
      title: 'Employee Vacation',
      start: new Date(2025, 3, 20),
      end: new Date(2025, 3, 25),
      allDay: true,
    },
  ]);

  return (
    <div style={{ height: '80vh', margin: '20px' }}>
      <h2>Project Calendar</h2>
      <Calendar
        localizer={localizer}
        events={events}
        startAccessor="start"
        endAccessor="end"
        style={{ height: 500, margin: '20px' }}
        defaultView="month"
        views={['month', 'week', 'day']}
        popup
      />
    </div>
  );
};

export default CalendarPage;