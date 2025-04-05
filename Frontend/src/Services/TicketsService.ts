import { Ticket } from "../Types/TicketType";
import { AbsenceReasonEnum } from "../Types/EnumType";


export const getTicketsByLoggedUser = async (): Promise<Ticket[]> => {

    const exampleTickets: Ticket[] = [
        {
            ticketId: 1,
            employeeId: 101,
            employeeName: "Stas",
            startDate: "2025-04-01",
            endDate: "2025-04-30",
            absenceReason: AbsenceReasonEnum.MaterPaterLeave,
            description: "Paternity leave after a son's birth",
            isOpen: true
        },
        {
            ticketId: 2,
            employeeId: 102,
            employeeName: "Michal",
            startDate: "2025-05-01",
            endDate: "2025-05-15",
            absenceReason: AbsenceReasonEnum.ReserveDuty,
            description: "Annaul reserve duty period, might get 2 weeks longer",
            isOpen: false
        }
    ];

    return exampleTickets; // מחזיר את הטיקטים לדוגמה
};
