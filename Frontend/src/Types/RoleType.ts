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
    skills: Skill[];
    yearsExperience: number;
    jobPercentage: number;
    startDate: string;
  }

  export const formatDate = (isoDate: string): string => {
    const date = new Date(isoDate);
    return date.toLocaleDateString("en-GB"); 
};