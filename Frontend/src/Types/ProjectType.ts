import { Role } from './RoleType'

export interface Project {
    projectId: number; 
    projectName: string;
    description: string;
    deadline: string; 
    requiredHours: number;
    roles: Role[];
  }

  export const formatDate = (isoDate: string): string => {
    const date = new Date(isoDate);
    return date.toLocaleDateString("en-GB");
};