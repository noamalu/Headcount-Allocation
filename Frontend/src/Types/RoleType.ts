import { Skill } from './SkillType'
import { Language } from './LanguageType';

export interface Role {
    roleId: number; 
    roleName: string;
    projectId: number;
    employeeId: number;
    description: string;
    timeZone: number;
    foreignLanguages: Language[];
    skills:Skill [];
    yearsExperience: number;
    jobPercentage: number;
    // attributes: { attribute: string; requiredRank: number | string; priority: number }[];
    // foreignLanguages: [];
    // skills: [];
  }