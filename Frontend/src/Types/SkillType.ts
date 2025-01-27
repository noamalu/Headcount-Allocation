import { getSkillStringByIndex } from "./EnumType";

export interface Skill {
    skillId: number;
    SkillTypeId: number;
    level: number;
    priority: number;
}

export const formateSkillToString = (type: number): string => {
    return getSkillStringByIndex(type); // פורמט יום/חודש/שנה
};




