import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';
import { Role } from '../Types/RoleType';
import ClientResponse from './Response';
import { Employee } from '../Types/EmployeeType';
import { Ticket } from '../Types/TicketType';

class EmployeesService {

   static async sendCreateEmployee(employee: Omit<Employee, "employeeId" | "roles">): Promise<number> {
    console.log("attempt to create employee" + employee.employeeName);
          try {
              const response = await APIClient('/api/Manager/Employees', {
                  method: 'POST',
                  body: JSON.stringify(employee),
                  headers: { 'Content-Type': 'application/json' },
              });
              if (!response.errorOccured) {
                return response.value; 
              } else {
                  throw new Error("Failed to create employee: " + JSON.stringify(response, null, 2));
              }
          } catch (error) {
              console.error("Error in sendCreateEmployee:", error);
              throw error; 
          }
      }



  

  static async assignEmployeeToRole(employeeId: number, role: Role): Promise<Response> {
    console.log(`Attempting to assign employee ${employeeId} to role: ${role.roleName}`);
    try {
      const response = await APIClient(`/api/Employee/${employeeId}/Assign`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(role), 
      });

      console.log('assignEmployeeToRole Response:', response); 

      if (!response.errorOccured) {
        return fetchResponse(response);
      } else {
        throw new Error(
          `Failed to assign employee to role: ${JSON.stringify(response, null, 2)}`
        );
      }
    } catch (error) {
      console.error(`Error assigning employee ID ${employeeId} to role:`, error);
      throw error; // משליכים את השגיאה לטיפול חיצוני
    }
  }


static async getEmployeeById(employeeId : number): Promise<Employee> {
    try {
      const response = await APIClient(`/api/Employee/${employeeId}`, { method: 'GET' });
      console.log('GetEmployeeById Response:', response); 
      if (!response.errorOccured) {
        return  fetchResponse(response); 
      } else {
          throw new Error("Failed to GetEmployeeById: " + JSON.stringify(response, null, 2));
      }
    } catch (error) {
      console.error(`Error GetEmployeeById ${employeeId} `, error);
      throw error; 
    }
  }

}

export const getEmployees = async (): Promise<Employee[]> => {
    const response = await APIClient('/api/Employee/All', { method: 'GET' });
    console.log('getEmployees Response:', response); 
    return fetchResponse(response); 
  };

  


export default EmployeesService;