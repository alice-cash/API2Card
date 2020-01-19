using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace API2Card.JSON.Spells
{
    public class SpellCard:  Card<Spell>
    {

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="card_icon"></param>
        /// <returns></returns>
        public static CardJSON[] GenerateCard(Spell spell, CardType card_icon, CardSizeData card_data)
        {
            SpellCard factory = new SpellCard()
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

            foreach (string s in _gen_desc("Spell", data.desc))
                card_text.Add(s);

            if (data.higher_level.Trim().Length > 0)
            {
                card_text.Add(_section("Higher Levels"));
                foreach (string s in _gen_desc("", data.higher_level))
                    card_text.Add(s);
            }
            return card_text;
        }

        public override List<String> _makeCardHeader()
        {
            List<string> card_text = new List<string>();

            card_text.Add(_subtitle(string.Format("{0} {1}", data.level, data.school)));
            card_text.Add(_hr());
            card_text.Add(_property("Range", data.range));
            card_text.Add(_property("Casting Time", data.casting_time));
            card_text.Add(_property("Duration", data.duration));
            card_text.Add(_property("Components", data.components));
            card_text.Add(_hr());

            return card_text;      
        }

        public override List<String> _makeCardFooter()
        {
            List<string> card_text = new List<string>();

            card_text.Add(_fill());
            card_text.Add(_section(data.page));
            card_text.Add(_text(data.dnd_class));

            return card_text;
        }

    }
}
