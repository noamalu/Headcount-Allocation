import { Skill } from './SkillType'
import { Language } from './LanguageType'

import { Role } from './RoleType'

export interface Employee {
    employeeId: number; 
    employeeName: string;
    phoneNumber: string;
    email?: string;
    timeZone: number;
    foreignLanguages: Language[];
    skills: Skill[];
    yearsExperience: number;
    jobPercentage: number;
    roles: Role[];
}