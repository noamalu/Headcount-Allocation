import { Role } from './RoleType'

export interface Project {
    projectId: number; 
    projectName: string;
    description: string;
    deadline: string; // To change to date when connecting API
    requiredHours: number;
    // roles: Role[];
  }