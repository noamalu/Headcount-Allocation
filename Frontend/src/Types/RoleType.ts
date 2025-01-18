import { RoleSkill } from './RoleSkillType'

export interface Role {
    roleId: number; 
    roleName: string;
    projectId: number;
    employeeId?: number;
    description: string;
    timeZoneId: number;
    foreignLanguages: string[];
    skills: RoleSkill[];
    yearsExperience: number;
    jobPercentage: number;
    // attributes: { attribute: string; requiredRank: number | string; priority: number }[];
    // foreignLanguages: RoleLanguage[];
    // skills: RoleSkill[];
  }