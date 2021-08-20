using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace API2Card.JSON.Magic_Item
{
    public class Magic_ItemCard :  Card<Magic_Item>
    {

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="card_icon"></param>
        /// <returns></returns>
        public static CardJSON[] GenerateCard(Magic_Item spell, CardType card_icon, CardSizeData card_data)
        {
            Magic_ItemCard factory = new Magic_ItemCard()
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

           card_text.AddRange(_gen_desc("Item: ", data.desc));

            return card_text;
        }

        public override List<String> _makeCardHeader()
        {
            List<string> card_text = new List<string>();

            card_text.Add(_subtitle(string.Format("{0} {1}", data.rarity, data.type)));
            card_text.Add(_hr());
            if(data.requires_attunement != null && data.requires_attunement != "") card_text.Add(_property("Requires Attunement ", ""));
            card_text.Add(_hr());

            return card_text;      
        }

        public override List<String> _makeCardFooter()
        {
            List<string> card_text = new List<string>();

            return card_text;
        }

    }
}
