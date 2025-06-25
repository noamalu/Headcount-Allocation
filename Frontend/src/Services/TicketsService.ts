import { Ticket } from "../Types/TicketType";
import { AbsenceReasonEnum } from "../Types/EnumType";
import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';

class TicketsService {

static async sendCreateTicket(ticket: Omit<Ticket, "ticketId">): Promise<number> {
    var employeeId = ticket.employeeId;
    console.log("attempt to create ticket for " + ticket.employeeName + " with reason: " + ticket.absenceReason);
          try {
            const response = await APIClient(`/api/Employee/${employeeId}/Ticket`, {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
              },
              body: JSON.stringify(ticket), 
            });
              if (!response.errorOccured) {
                console.log("Ticket created successfully (TicketsService)");
                return response.value; 
                
              } else {
                  throw new Error("Failed to create ticket: " + JSON.stringify(response, null, 2));
              }
          } catch (error) {
              console.error("Error in sendCreateTicket:", error);
              throw error; 
          }
      }

      static async deleteTicket(employeeId: number, ticketId: number): Promise<void> {
        console.log("attempt to delete ticket " + ticketId + " from employee " + employeeId);
        return;
        try {
            const response = await APIClient(`/api/Employee/${employeeId}/Ticket/${ticketId}`, {
              method: 'DELETE',
            });
    
            if (response.errorOccured) {
              throw new Error("Failed to delete ticket: " + JSON.stringify(response, null, 2));
            } else {
              return; 
            }
            
        } catch (error) {
            console.error("Error in deleteTicket:", error);
            throw error;
        }
      }

      static async editTicket(employeeId: number, ticket: Ticket): Promise<void> {
          console.log("attempt to edit ticket " + ticket.absenceReason + " for employee " + employeeId);
          try {
              const response = await APIClient(`/api/Employee/${employeeId}/Ticket/`, {
                method: 'PUT',
                body: JSON.stringify(ticket),
                headers: {
                  'Content-Type': 'application/json',
                },
              });
      
              if (response.errorOccured) {
                throw new Error("Failed to edit Ticket: " + JSON.stringify(response, null, 2));
              } else {
                return; 
              }
      
          } catch (error) {
              console.error("Error in editTicket:", error);
              throw error;
          }
        }

}

export const getAllTickets = async (): Promise<Ticket[]> => {
    try {
        const response = await APIClient('/api/Manager/Tickets', { method: 'GET' });
        console.log('getTickets Response:', response); 
        if (!response.errorOccured) {
            return fetchResponse(response); 
        }
        else {
            throw new Error("Failed to getAllTickets: " + JSON.stringify(response, null, 2));
        }
    } catch (error) {
        console.error(`Error fetching Tickets for admin:`, error);
        throw error;
    }
  };

  export const getTicketsByEmployeeId = async (employeeId: number): Promise<Ticket[]> => {
    try {
        const response = await APIClient(`/api/Employee/${employeeId}/Ticket`, { method: 'GET' });
        console.log('getTicketsByEmployeeId Response:', response); 
        if (!response.errorOccured) {
            return fetchResponse(response); 
        }
        else {
            throw new Error("Failed to getTicketsByEmployeeId: " + JSON.stringify(response, null, 2));
        }
    } catch (error) {
        console.error(`Error fetching Tickets for admin:`, error);
        throw error;
    }
  };




  

export default TicketsService;


export const getTicketsByLoggedUser = async (): Promise<Ticket[]> => {

    const exampleTickets: Ticket[] = [
        {
            ticketId: 11,
            employeeId: 101,
            employeeName: "Stas",
            startDate: "2025-04-01",
            endDate: "2025-04-30",
            absenceReason: AbsenceReasonEnum.MaterPaterLeave,
            description: "Paternity leave after a son's birth",
            isOpen: true
        },
        {
            ticketId: 12,
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