using System.Collections.Generic;

namespace API2Card.JSON.Monster
{
    public class Monster : Result
    {
        public string slug;
       // public string name;
        public string size;
        public string type;
        public string subtype;
        public string group;
        public string alignment;
        public int armor_class;
        public string armor_desc;
        public int hit_points;
        public string hit_dice;
        public Move_Speed speed;
        public class Move_Speed
        {
            public int? burrow;
            public int? climb;
            public int? fly;
            public bool? hover;
            public int? lightwalking;
            public string notes;
            public int? swim;
            public int? walk;
        }
        public int strength;
        public int dexterity;
        public int constitution;
        public int intelligence;
        public int wisdom;
        public int charisma;
        public int? strength_save;
        public int? dexterity_save;
        public int? constitution_save;
        public int? intelligence_save;
        public int? wisdom_save;
        public int? charisma_save;
        public int? perception;
        public Skill_Groups Skills;
        public class Skill_Groups
        {
            public int? acrobatics;
            public int? arcana;
            public int? athletics;
            public int? attack_bonus;
            public int? deception;
            public int? desc;
            public int? history;
            public int? insight;
            public int? intimidation;
            public int? investigation;
            public int? medicine;
            public int? name;
            public int? nature;
            public int? perception;
            public int? performance;
            public int? persuasion;
            public int? religion;
            public int? stealth;
            public int? survival;
        }
        public string damage_vulnerabilities;
        public string damage_resistances;
        public string damage_immunities;
        public string condition_immunities;
        public string senses;
        public string languages;
        public string challenge_rating;
        public Actions[] actions;
        public class Actions
        {
            public string attack_bonus;
            public string damage_dice;
            public string desc;
            public string name;
            public string damage_bonus;
        }


        public NameDescPair[] reactions;
        public string legendary_desc;
        public NameDescPair[] legendary_actions;

        public class NameDescPair
        {
            public string desc;
            public string name;
        }
        public NameDescPair[] special_abilities;


        public string[] spell_list;
        public string img_main;
        public string document__slug;
        public string document__title;
        public string document__license_url;
    }
}


