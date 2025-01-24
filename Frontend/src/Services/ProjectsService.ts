import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';
import ClientResponse from './Response';
import { Project } from '../Types/ProjectType';
import { Role } from '../Types/RoleType';

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

  //   static async addRolesToProject(projectId: number, roles: Role[]): Promise<Role[]> {
  //     try {
  //         const response = await APIClient(`/api/Project/${projectId}/Roles`, {
  //             method: 'POST',
  //             body: JSON.stringify(roles),
  //             headers: { 'Content-Type': 'application/json' },
  //         });
  //         if (response.status === 200) {
  //             return response.data; 
  //         } else {
  //             throw new Error("Failed to add roles to project: " + response.statusText);
  //         }
  //     } catch (error) {
  //         console.error("Error in addRolesToProject:", error);
  //         throw error; 
  //     }
  // }
  

}

export const getProjects = async (): Promise<Project[]> => {
    const response = await APIClient('/api/Project/All', { method: 'GET' });
    console.log('getProjects Response:', response); // לוג לבדיקה
    return fetchResponse(response); // עדיין משתמשים ב-fetchResponse כדי לשמר את הזרימה
  };


  export default ProjectsService;