import { getSkillStringByIndex } from "./EnumType";

export interface Skill {
    skillId: number;
    skillTypeId: number;
    level: number;
    priority: number;
}

export const formateSkillToString = (type: number): string => {
    console.log("formateSkillToString input: " + type + " ");
    return getSkillStringByIndex(type);
};




