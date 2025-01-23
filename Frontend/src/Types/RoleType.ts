import { RoleSkill } from './RoleSkillType'

export interface Role {
    // roleId: number; 
    roleName: string;
    projectId: number;
    employeeId?: number;
    description: string;
    timeZone: number;
    // foreignLanguages: [];
    // skills: [];
    yearsExperience: number;
    jobPercentage: number;
    // attributes: { attribute: string; requiredRank: number | string; priority: number }[];
    foreignLanguages: [];
    skills: [];
  }