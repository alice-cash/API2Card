using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace API2Card.JSON.Spells
{
    public class SpellCard
    {
        /// <summary>
        ///  The number of times this card is repeated. Useful for consumable items that you hand out multiple times.
        /// </summary>
        public int count;
        /// <summary>
        /// Name of the card color. You can use all CSS color names. http://www.w3schools.com/cssref/css_colornames.asp
        /// </summary>
        public string color;
        /// <summary>
        /// Name of the card icon. You can use most icons from game-icons.net. For example, the file name of this dagger
        /// is "plain-dagger.png", so you would use "plain-dagger" as the icon name. Additional custom icon names
        /// are defined in css/custom-icons.css.
        /// </summary>
        public string icon;
        /// <summary>
        /// Optional. Name of the big icon on the card back. If not specified, the icon from the "icon" property is used.
        /// </summary>
        public string icon_back;
        /// <summary>
        ///  Optional. URL of a backgound image for the card back. If specified, replaces the back icon.
        /// </summary>
        public string background_image;
        /// <summary>
        /// The title of the card.
        /// </summary>
        public string title;
        /// <summary>
        /// An array of strings, specifying all card elements, in top to bottom order (see below).
        /// </summary>
        public string[] contents;
        /// <summary>
        /// An array of strings, providing tags for the card.
        /// </summary>
        public string[] tags;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="card_icon"></param>
        /// <returns></returns>
        public static SpellCard[] GenerateSpellCard(Spell spell, CardType card_icon, CardSizeData card_data)
        {
            CardBuilderState state = new CardBuilderState()
            {
                spell = spell,
                target_class = card_icon,
                card_data = card_data,
                part = 1
            };
            state.card_header = _makeCardHeader(spell, card_icon, card_data);
            state.card_footer = _makeCardFooter(spell, card_icon, card_data);
            state.additionalLines = _getLines(state.card_footer, card_data.MaxCharactersPerLine);
            state.card_text = _makeCardText(spell, card_icon, card_data, state);

            return _makeCard(state);
        }

        private static List<String> _makeCardText(Spell spell, CardType target_class, CardSizeData card_data, CardBuilderState state)
        {
            List<string> card_text = new List<string>();

            foreach (string s in _gen_desc("Spell", spell.desc, card_data, state))
                card_text.Add(s);

            if (spell.higher_level.Trim().Length > 0)
            {
                card_text.Add(_section("Higher Levels"));
                foreach (string s in _gen_desc("", spell.higher_level, card_data, state))
                    card_text.Add(s);
            }
            return card_text;
        }

        private static List<String> _makeCardHeader(Spell spell, CardType target_class, CardSizeData card_data)
        {
            List<string> card_text = new List<string>();

            card_text.Add(_subtitle(string.Format("{0} {1}", spell.level, spell.school)));
            card_text.Add(_hr());
            card_text.Add(_property("Range", spell.range));
            card_text.Add(_property("Casting Time", spell.casting_time));
            card_text.Add(_property("Duration", spell.duration));
            card_text.Add(_property("Components", spell.components));
            card_text.Add(_hr());

            return card_text;      
        }

        private static List<String> _makeCardFooter(Spell spell, CardType target_class, CardSizeData card_data)
        {
            List<string> card_text = new List<string>();

            card_text.Add(_fill());
            card_text.Add(_section(spell.page));
            card_text.Add(_text(spell.dnd_class));

            return card_text;
        }

        private class CardBuilderState
        {
            public Spell spell;
            public CardType target_class;
            public CardSizeData card_data;
            public int part;
            public List<string> card_text;
            public List<string> card_header;
            public List<string> card_footer;
            public int additionalLines = 0;
        }

        private static SpellCard[] _makeCard(CardBuilderState state)
        {

            List<SpellCard> resultArray = new List<SpellCard>();
            List<string> builtCardText = new List<string>();

            bool IsFirstCard = state.part == 1;

            // Determine how much the fluff around the text requires
            int cardHeaderLines = _getLines(state.card_header, state.card_data.MaxCharactersPerLine);
            int cardFooterLines = _getLines(state.card_footer, state.card_data.MaxCharactersPerLine);
            if(IsFirstCard) state.additionalLines = cardFooterLines;


            SpellCard cardData = new SpellCard
            {
                color = "black",
                title = state.part == 1 ? state.spell.name : string.Format("{0} (Part {1})", state.spell.name, state.part),
                icon = CardType_Helper.GetIconForType(state.target_class)
            };

            //Add the card now to ensure it appears before any "part X" cards
            resultArray.Add(cardData);

            //For only the first card we add header lines
            if (state.part == 1) builtCardText.AddRange(state.card_header);

            builtCardText.AddRange(state.card_text);

            
            if (_getLines(builtCardText, state.card_data.MaxCharactersPerLine) + state.additionalLines > state.card_data.MaxLines)
            {
                List<string> card_overtext = new List<string>();
                while (_getLines(builtCardText, state.card_data.MaxCharactersPerLine) + state.additionalLines > state.card_data.MaxLines && builtCardText.Count > 1)
                {
                    card_overtext.Insert(0, builtCardText.Last());
                    builtCardText.RemoveAt(builtCardText.Count - 1);
                }
                if (_checkIfAbandon(builtCardText.Last()) && builtCardText.Count > 1)
                {
                    card_overtext.Insert(0, builtCardText.Last());
                    builtCardText.RemoveAt(builtCardText.Count - 1);
                }

                //After the first card has been calculated, removed the header/footer calculations and leave it for the "Continues on" line
                if (IsFirstCard) state.additionalLines = 2;

                state.part++;
                state.card_text = card_overtext;
                SpellCard[] rv2 = _makeCard(state);

                builtCardText.Add(_fill());
                builtCardText.Add(_text(string.Format("Continues on {0}", rv2[0].title)));

                resultArray.AddRange(rv2);
            }

            //If we can or must add footer text.
            if (IsFirstCard || _getLines(builtCardText, state.card_data.MaxCharactersPerLine) + cardFooterLines <= state.card_data.MaxLines)
            {
                builtCardText.AddRange(state.card_footer);
            }
            cardData.contents = builtCardText.ToArray();

            return resultArray.ToArray();
        }

        private static bool _checkIfAbandon(string text)
        {
            return text.StartsWith("section") || text.StartsWith("subtitle");
        }

        private static int _getLines(List<string> text, int char_per_line)
        {
            double size = 0, multiplyer = 1;
            foreach (string line in text)
            {
                if (line.StartsWith("section") || line.StartsWith("subtitle")) multiplyer = 1.1f;
                if (line.StartsWith("property") || line.StartsWith("bullet")) multiplyer = 1.2f;
                var subline = line.Contains("|") ? line.Substring(line.IndexOf('|')) : line;
                size += Math.Ceiling(subline.Length / (double)char_per_line) * multiplyer;
            }
            return (int)Math.Ceiling(size);
        }

        private static string _subtitle(string text)
        {
            return string.Format("subtitle | {0}", text.Trim());
        }

        private static string _fill(int c = 1)
        {
            return string.Format("fill | {0}", c);
        }

        private static string _section(string text)
        {
            return string.Format("section | {0}", text);
        }

        private static string _property(string title, string text)
        {
            return string.Format("property | {0} | {1}", title.Trim(), text.Trim());
        }

        private static string _hr()
        {
            return "rule";
        }

        private static string _description(string title, string text)
        {
            return string.Format("description | {0} | {1}", title.Trim(), text.Trim());
        }

        private static string _bullet(string text)
        {
            return string.Format("bullet | {0}", text.Trim());
        }

        private static string _text(string text)
        {
            return string.Format("text | {0}", text.Trim());
        }

        private static string[] _gen_desc(string title, string text, CardSizeData card_data, CardBuilderState state)
        {
            
            if (text.Contains("|")) {
                text = text.Replace("|", "&#x7C;"); }

            if (title != "" && text.IndexOf("\n") == -1 && text.IndexOf("**") == -1 && text.Length < card_data.MaxLines * card_data.MaxCharactersPerLine) return new string[]{ _description(title, text)};

            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            List<string> rv = new List<string>();
            int i_real = 0;
            for (int i = 0; i < lines.Length; i++, i_real++)
            {
                string firstLine = "";
                while (lines[i].Length > (card_data.MaxLines - state.additionalLines ) * card_data.MaxCharactersPerLine)
                {
                    
                    int si = lines[i].LastIndexOf(".", (card_data.MaxLines - state.additionalLines) * card_data.MaxCharactersPerLine);
                    if (si == -1) si = lines[i].LastIndexOf(" ", (card_data.MaxLines - state.additionalLines) * card_data.MaxCharactersPerLine);  //Really long run-on sentance;
                    if (si == -1) si = (card_data.MaxLines - 1) * card_data.MaxCharactersPerLine;  //Really long thing with no spaces
                    rv.Add(_parseline(title, lines[i].Substring(0, si + 1),i_real, firstLine));
                    if (firstLine == "")
                        firstLine = lines[i].Substring(0, si + 1);
                    lines[i] = lines[i].Substring(si + 1);
                    i_real++;
                }
                rv.Add(_parseline(title, lines[i].Trim(), i_real, firstLine));
            }
            return rv.ToArray();

        }
        private static string _parseline(string title, string text, int line, string firstLine = "")
        {
            string testAgainst = text.Trim();
            if (firstLine != "") testAgainst = firstLine.Trim();

            if (title != "" && line == 0 && !testAgainst.StartsWith("**")) return _description(title, text);
            if (testAgainst.StartsWith('-')) return _bullet(text.TrimStart(new char[]{ '-',' '}));
            if (testAgainst.StartsWith("###"))
            {
                return _section(text.TrimStart(new char[] { ' ', '#' }));
            }
            if (testAgainst.StartsWith("**")) {
                string pattern = @"^\*\*(.*)\*\*(.*)$";
                Regex rx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(testAgainst);
                if (matches.Count != 1) throw new Exception(string.Format("'{0}' does not match pattern '/{1}/'", text, pattern));
                if (firstLine == "")
                {
                    title = matches[0].Groups[1].Value;
                    text = matches[0].Groups[2].Value;
                } else
                {
                    title = string.Format("{0}(cont.)", matches[0].Groups[1].Value);
                }

                return _property(title, text);
            }
            return _text(text);
        }

    }
}
