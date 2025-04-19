import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';
import ClientResponse from './Response';
import { Project } from '../Types/ProjectType';
import { Role } from '../Types/RoleType';
import { Employee } from '../Types/EmployeeType';

class ProjectsService {


    // returns the new project's ID
  static async sendCreateProject(project: Omit<Project, "projectId" | "roles">): Promise<number> {
    console.log("attempt to create project" + project.projectName);
          try {
              const response = await APIClient('/api/Project/Create', {
                  method: 'POST',
                  body: JSON.stringify(project),
                  headers: { 'Content-Type': 'application/json' },
              });
              if (!response.errorOccured) {
                return response.value; 
              } else {
                  throw new Error("Failed to create project: " + JSON.stringify(response, null, 2));
              }
          } catch (error) {
              console.error("Error in sendCreateProject:", error);
              throw error; 
          }
      }


    static async addRolesToProject(projectId: number, roles: Role[]): Promise<Role[]> {
      console.log("attempt to add role " + roles[0].roleName + " to project id: " + projectId);
      try {
        //   const response = await APIClient(`/api/Project/${projectId}/Roles`, {
            const response: Array<{ value: Role; errorMessage: string | null; errorOccured: boolean }> = await APIClient(`/api/Project/${projectId}/Roles`, {
              method: 'POST',
              body: JSON.stringify(roles),
              headers: { 'Content-Type': 'application/json' },
          });
          console.log("Response from APIClient:", response);
        if (!Array.isArray(response)) {
            throw new Error("Unexpected response format. Expected an array.");
        }
        response.forEach((item) => {
            if (item.errorOccured) {
                throw new Error(`Error for role: ${item.errorMessage || "Unknown error"}`);
            }
        });
        const newRoles = response.map((item) => item.value);
        console.log("Parsed roles:", newRoles);
        return newRoles;
    } catch (error) {
        console.error("Error in addRolesToProject:", error);
        throw error;
    }
  }


  static async editProject(project: Project): Promise<void> {
    console.log("attempt to edit project" + project.projectName);
    try {
        const response = await APIClient(`/api/Project/${project.projectId}/Edit`, {
          method: 'PUT',
          body: JSON.stringify(project),
          headers: {
            'Content-Type': 'application/json',
          },
        });

        if (response.errorOccured) {
          throw new Error("Failed to edit project: " + JSON.stringify(response, null, 2));
        } else {
          return; 
        }

    } catch (error) {
        console.error("Error in editProject:", error);
        throw error;
    }
  }

  static async deleteProject(projectId: number): Promise<void> {
    console.log("attempt to delete project" + projectId);
    try {
        const response = await APIClient(`/api/Project/Delete/${projectId}`, {
          method: 'DELETE',
        });

        if (response.errorOccured) {
          throw new Error("Failed to delete project: " + JSON.stringify(response, null, 2));
        } else {
          return; 
        }
        
    } catch (error) {
        console.error("Error in deleteProject:", error);
        throw error;
    }
  }
  

}

export const getProjects = async (): Promise<Project[]> => {
    const response = await APIClient('/api/Project/All', { method: 'GET' });
    console.log('getProjects Response:', response); // לוג לבדיקה
    return fetchResponse(response); // עדיין משתמשים ב-fetchResponse כדי לשמר את הזרימה
  };


  export const getProjectRoles = async (projectId: number): Promise<Role[]> => {
    try {
      const response = await APIClient(`/api/Project/${projectId}/Roles`, { method: 'GET' });
      console.log('getProjectRoles Response:', response); // לוג לבדיקה
      return fetchResponse(response); // שימוש בפונקציה fetchResponse לעיבוד התגובה
    } catch (error) {
      console.error(`Error fetching roles for project ID ${projectId}:`, error);
      throw error;
    }
  };

  export const getAssignOptionsToRole = async (projectId: number, roleId: number): Promise<Employee[]> => {
    try {
      const response = await APIClient(`/api/Project/${projectId}/Roles/${roleId}/Assign`, { method: 'GET' });
      console.log('getAssignOptionsToRole Response:', response); // לוג לבדיקה
      if (!response.errorOccured) {
        return  fetchResponse(response); 
      } else {
          throw new Error("Failed to getAssignOptionsToRole: " + JSON.stringify(response, null, 2));
      }
       // שימוש בפונקציה fetchResponse לעיבוד התגובה
    } catch (error) {
      console.error(`Error fetching assign options for project ID ${projectId} and role ID ${roleId}:`, error);
      throw error; // משליכים את השגיאה לטיפול חיצוני
    }
  };




  export default ProjectsService;