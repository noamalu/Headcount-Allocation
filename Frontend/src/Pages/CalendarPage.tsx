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
            events={[
              {
                title: "📌 Project deadline: Alpha",
                date: "2025-05-05",
                backgroundColor: "#E8CEBF",
                borderColor: "#E8CEBF",
                textColor: "#4F4846",
              },
              {
                title: "👤 Assignment: Dana → Role X",
                date: "2025-05-07",
                backgroundColor: "#A9C9A4",
                borderColor: "#A9C9A4",
                textColor: "#4F4846",
              },
              {
                title: "⚠️ Alert: Overload",
                date: "2025-05-13",
                backgroundColor: "#D96666",
                borderColor: "#D96666",
                textColor: "white",
              },
              {
                title: "📌 Project deadline: Beta",
                date: "2025-05-22",
                backgroundColor: "#E8CEBF",
                borderColor: "#E8CEBF",
                textColor: "#4F4846",
              },
              {
                title: "👤 Assignment: Noa → Role Z",
                date: "2025-05-28",
                backgroundColor: "#A9C9A4",
                borderColor: "#A9C9A4",
                textColor: "#4F4846",
              },
            ]}
        />
    </div>
  );
};

export default CalendarPage;
