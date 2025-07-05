import { AbsenceReasonEnum } from "./EnumType";

export interface Ticket {
    ticketId: number; 
    employeeId: number;
    employeeName: string;
    startDate: string;
    endDate: string;
    absenceReason: AbsenceReasonEnum;
    description: string;
    open: boolean;
  }

  export const formatDate = (isoDate: string): string => {
    const date = new Date(isoDate);
    return date.toLocaleDateString("en-GB"); 
};