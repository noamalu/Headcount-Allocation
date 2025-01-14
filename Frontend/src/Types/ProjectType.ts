import { Role } from './RoleType'

export interface Project {
    id: number; 
    name: string;
    deadline: string;
    progress: number;
    description: string;
    roles: Role[];

  }