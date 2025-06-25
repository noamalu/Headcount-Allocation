import { Skill } from './SkillType'
import { Language } from './LanguageType'

import { Role } from './RoleType'

export interface Employee {
    employeeName: string;
    employeeId: number; 
    phoneNumber: string;
    email?: string;
    timeZone: number;
    foreignLanguages: Language[];
    skills: Skill[];
    yearsExperience: number;
    jobPercentage: number;
    roles: Role[];
    password?: string; 
}