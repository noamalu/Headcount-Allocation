import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';
import { Role } from '../Types/RoleType';
import ClientResponse from './Response';
import { Employee } from '../Types/EmployeeType';

class EmployeesService {

  static async assignEmployeeToRole(employeeId: number, role: Role): Promise<Response> {
    console.log(`Attempting to assign employee ${employeeId} to role: ${role.roleName}`);
    try {
      const response = await APIClient(`/api/Employee/${employeeId}/Assign`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(role), // המרת האובייקט role ל-JSON
      });

      console.log('assignEmployeeToRole Response:', response); // לוג לבדיקה

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

export default EmployeesService;