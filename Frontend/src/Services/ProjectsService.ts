import { APIClient } from './APIClient';
import { fetchResponse } from './GeneralService';
import ClientResponse from './Response';
import { Project } from '../Types/ProjectType';
import { Role } from '../Types/RoleType';

class ProjectsService {


 static async sendCreateProject(project: Omit<Project, "projectId" | "roles">): Promise<Project> {
        try {
            const response = await APIClient('/api/Project/Create', {
                method: 'POST',
                body: JSON.stringify(project),
                headers: { 'Content-Type': 'application/json' },
            });
            if (response.status === 200) {
                return response.data; 
            } else {
                throw new Error("Failed to create project: " + response.statusText);
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







// export const getProjects = async (): Promise<Project[]> => {
//     return fetchResponse(
//       APIClient('/projects', { method: 'GET' }).then((res) => res.data as ClientResponse<Project[]>)
//     );
//   };

export const getProjects = async (): Promise<Project[]> => {
    const response = await APIClient('/api/Project/All', { method: 'GET' });
    console.log('getProjects Response:', response); // לוג לבדיקה
    return fetchResponse(response); // עדיין משתמשים ב-fetchResponse כדי לשמר את הזרימה
  };

// export const createProject = async (projectData: {
//   name: string;
//   deadline: string;
//   description: string;
// }) => {
//   return fetchResponse(
//     APIClient('/projects', {
//       method: 'POST',
//       body: JSON.stringify(projectData),
//     })
//   );
// };

export const createProject = async (projectData: {
    name: string;
    deadline: string;
    description: string;
  }): Promise<Project> => {
    const response = await APIClient('/projects', {
      method: 'POST',
      body: JSON.stringify(projectData),
    });
    console.log('createProject Response:', response); // לוג לבדיקה
    return fetchResponse(response); // שמירה על הקריאה ל-fetchResponse
  };

  export default ProjectsService;