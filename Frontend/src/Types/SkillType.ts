
import { SkillEnum } from "./EnumType";

export interface Skill {
    skillId: number;
    skillTypeId: SkillEnum;
    level: number;
    priority: number;
}

export const formateSkillToString = (type: number): string => {
    console.log("formateSkillToString input: " + type + " ");
    return SkillEnum[type];
};




