using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace API2Card.JSON.Monster
{
    public class MonsterCard : Card<Monster>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="card_icon"></param>
        /// <returns></returns>
        public static CardJSON[] GenerateCard(Monster spell, CardType card_icon, CardSizeData card_data)
        {
            MonsterCard factory = new MonsterCard()
            {
                data = spell,
                target_class = card_icon,
                card_data = card_data,
            };

            factory.card_header = factory._makeCardHeader();
            factory.card_footer = factory._makeCardFooter();
            factory.additionalLines = _getLines(factory.card_footer, card_data.MaxCharactersPerLine);
            factory.card_text = factory._makeCardText();

            return factory._makeCard(1);
        }

        public override List<String> _makeCardText()
        {
            List<string> card_text = new List<string>();


            if (data.special_abilities != null)
            {
                card_text.Add(_section("Abilities"));
                foreach (var ability in data.special_abilities)
                    card_text.AddRange(_gen_desc(ability.name, ability.desc));
            }

            if (data.actions != null)
            {
                card_text.Add(_section("Actions"));
                foreach (var actions in data.actions)
                    card_text.AddRange(_gen_desc(actions.name, actions.desc));
            }
            if (data.reactions != null)
            {
                card_text.Add(_section("Reactions"));
                foreach (var reaction in data.reactions)
                    card_text.AddRange(_gen_desc(reaction.name, reaction.desc));
            }


            if (data.legendary_desc != null && data.legendary_desc != "")
            {
                card_text.Add(_section("Legendary Description"));
                card_text.Add(_text(data.legendary_desc));
            }
            if (data.legendary_actions != null)
            {
                card_text.Add(_section("Legendary Actions"));
                foreach (var reaction in data.legendary_actions)
                    card_text.Add(_description(reaction.name, reaction.desc));
            }
            /*   if (data.higher_level.Trim().Length > 0)
               {
                   card_text.Add(_section("Higher Levels"));
                   foreach (string s in _gen_desc("", data.higher_level))
                       card_text.Add(s);
               }*/
            return card_text;
        }

        public override List<String> _makeCardHeader()
        {
            List<string> card_text = new List<string>();

            card_text.Add(_subtitle(string.Format("{0} {1} {2}", data.size, data.type, data.subtype)));
            card_text.Add(_hr());
            card_text.Add(_property("Armor Class", data.armor_class));
            card_text.Add(_property("Hit Points", string.Format("{0} {1}",data.hit_points, data.hit_dice)));

            string skills = BuildSkills();
            if (skills != "")
                card_text.Add(_property("Skills", skills));
            
            card_text.Add(_property("Challenge", data.challenge_rating));

            string savingThrows = BuildSaves();
            if (savingThrows != "")
                card_text.Add(_property("Saving Throws:", savingThrows));

            card_text.Add(_dndstats(data.strength, data.dexterity, data.constitution, data.intelligence, data.wisdom, data.charisma));

            card_text.Add(_hr());
            if ((data.senses is string && data.senses != "") ||
                (data.languages is string && data.languages != "") ||
                (data.condition_immunities is string && data.condition_immunities != ""))
            {
                if (data.senses is string && data.senses != "")
                    card_text.Add(_property("Senses", data.senses));
                if (data.languages is string && data.languages != "")
                    card_text.Add(_property("Languages", data.languages));
                if (data.condition_immunities is string && data.condition_immunities != "")
                    card_text.Add(_property("Condition Immunities", data.condition_immunities));
                card_text.Add(_hr());
            }

            return card_text;
        }

        private string BuildSkills()
        {
            StringBuilder SB = new StringBuilder();
            if (data.Skills.acrobatics is int)
                SB.AppendFormat("Acrobatics +{0} ", data.Skills.acrobatics);
            if (data.Skills.arcana is int)
                SB.AppendFormat("Arcana +{0} ", data.Skills.arcana);
            if (data.Skills.athletics is int)
                SB.AppendFormat("Athletics +{0} ", data.Skills.athletics);
            if (data.Skills.deception is int)
                SB.AppendFormat("Deception +{0} ", data.Skills.deception);
            if (data.Skills.insight is int)
                SB.AppendFormat("Insight +{0} ", data.Skills.insight);
            if (data.Skills.intimidation is int)
                SB.AppendFormat("Intimidation +{0} ", data.Skills.intimidation);
            if (data.Skills.investigation is int)
                SB.AppendFormat("Investigation +{0} ", data.Skills.investigation);
            if (data.Skills.medicine is int)
                SB.AppendFormat("Medicine +{0} ", data.Skills.medicine);
            if (data.Skills.nature is int)
                SB.AppendFormat("Nature +{0} ", data.Skills.nature);
            if (data.Skills.perception is int)
                SB.AppendFormat("Perception +{0} ", data.Skills.perception);
            if (data.Skills.religion is int)
                SB.AppendFormat("Religion +{0} ", data.Skills.religion);
            if (data.Skills.stealth is int)
                SB.AppendFormat("Stealth +{0} ", data.Skills.stealth);
            if (data.Skills.survival is int)
                SB.AppendFormat("Survival +{0} ", data.Skills.survival);
            return SB.ToString();
        }

        private string BuildSaves()
        {
            StringBuilder SB = new StringBuilder();
            if (data.strength_save is int)
                SB.AppendFormat("Str +{0} ", data.strength_save);
            if (data.dexterity_save is int)
                SB.AppendFormat("Dex +{0} ", data.dexterity_save);
            if (data.constitution_save is int)
                SB.AppendFormat("Cons +{0} ", data.constitution_save);
            if (data.intelligence_save is int)
                SB.AppendFormat("Intel +{0} ", data.intelligence_save);
            if (data.charisma_save is int)
                SB.AppendFormat("Wis +{0} ", data.charisma_save);
            if (data.wisdom_save is int)
                SB.AppendFormat("Wis +{0} ", data.wisdom_save);
            return SB.ToString();
        }

        public override List<String> _makeCardFooter()
        {
            List<string> card_text = new List<string>();
            return card_text;
        }

    }
}
