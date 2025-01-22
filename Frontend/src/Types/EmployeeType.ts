import { EmployeeSkill } from './EmployeeSkillType'
import { Role } from './RoleType'

export interface Employee {
    employeeId: number; 
    userName: string;
    phoneNumber: string;
    email: string;
    projectId: number;
    timeZone: number[];
    foreignLanguages: string[];
    jobPercentage: number;
    skills: EmployeeSkill[];
    yearsExperience: number;
    roles: Role[];
}