import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';
import ClientResponse from './Response';
import { Employee } from '../Types/EmployeeType';
import { Project } from '../Types/ProjectType';

class StatisticsService {

  static async getEmployeesUtilization(): Promise<Record<string, Employee[]>> {
    try {
      const response = await APIClient('/api/Manager/Employees/Utilization', { method: 'GET' });
      console.log('getEmployeesUtilization Response:', response);
      if (!response.errorOccured) {
        return fetchResponse(response);
      } else {
        throw new Error('Failed to getEmployeesUtilization: ' + JSON.stringify(response, null, 2));
      }
    } catch (error) {
      console.error('Error in getEmployeesUtilization:', error);
      throw error;
    }
  }

  static async getProjectHourRatios(): Promise<{ project: Project; hours: number }[]> {
    try {
      const response = await APIClient('/api/Manager/Projects/Hours', { method: 'GET' });
      console.log('getProjectHourRatios Response:', response);
      if (!response.errorOccured) {
        return fetchResponse(response);
      } else {
        throw new Error('Failed to getProjectHourRatios: ' + JSON.stringify(response, null, 2));
      }
    } catch (error) {
      console.error('Error in getProjectHourRatios:', error);
      throw error;
    }
  }

  static async getProjectsThatEndThisMonth(): Promise<Project[]> {
    try {
      const response = await APIClient('/api/Manager/Projects/Deadlines', { method: 'GET' });
      console.log('getProjectsThatEndThisMonth Response:', response);
      if (!response.errorOccured) {
        return fetchResponse(response);
      } else {
        throw new Error('Failed to getProjectsThatEndThisMonth: ' + JSON.stringify(response, null, 2));
      }
    } catch (error) {
      console.error('Error in getProjectsThatEndThisMonth:', error);
      throw error;
    }
  }

  static async getNumEmployeesInProject(): Promise<{ project: Project; count: number }[]> {
    try {
      const response = await APIClient('/api/Manager/Projects/Employees/Count', { method: 'GET' });
      console.log('getNumEmployeesInProject Response:', response);
      if (!response.errorOccured) {
        return fetchResponse(response);
      } else {
        throw new Error('Failed to getNumEmployeesInProject: ' + JSON.stringify(response, null, 2));
      }
    } catch (error) {
      console.error('Error in getNumEmployeesInProject:', error);
      throw error;
    }
  }

  static async getEmployeesOnVacation(): Promise<Employee[]> {
    try {
      const response = await APIClient('/api/Manager/Employees/Vacation', { method: 'GET' });
      console.log('getEmployeesOnVacation Response:', response);
      if (!response.errorOccured) {
        return fetchResponse(response);
      } else {
        throw new Error('Failed to getEmployeesOnVacation: ' + JSON.stringify(response, null, 2));
      }
    } catch (error) {
      console.error('Error in getEmployeesOnVacation:', error);
      throw error;
    }
  }

  static async getVacationReasons(): Promise<Record<string, Employee[]>> {
    try {
      const response = await APIClient('/api/Manager/Employees/Vacation/Reasons', { method: 'GET' });
      console.log('getVacationReasons Response:', response);
      if (!response.errorOccured) {
        return fetchResponse(response);
      } else {
        throw new Error('Failed to getVacationReasons: ' + JSON.stringify(response, null, 2));
      }
    } catch (error) {
      console.error('Error in getVacationReasons:', error);
      throw error;
    }
  }
}

export default StatisticsService;
