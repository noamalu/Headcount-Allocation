import { EmployeeSkill } from './EmployeeSkillType'
import { Role } from './RoleType'

export interface Employee {
    employeeId: number; 
    employeeName: string;
    phoneNumber: string;
    email: string;
    timeZone: number[];
    foreignLanguages: string[];
    jobPercentage: number;
    skills: EmployeeSkill[];
    yearsExperience: number;
    roles: Role[];
}