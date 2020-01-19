using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace API2Card.JSON
{
    public abstract class Card<T> where T : Result
    {
        public T data;
        public CardType target_class;
        public CardSizeData card_data;

        public List<string> card_text;
        public List<string> card_header;
        public List<string> card_footer;
        public int additionalLines = 0;

   

        public abstract List<String> _makeCardText();

        public abstract List<String> _makeCardHeader();

        public abstract List<String> _makeCardFooter();


        public CardJSON[] _makeCard(int CardNumber)
        {


            List<CardJSON> resultArray = new List<CardJSON>();
            List<string> builtCardText = new List<string>();

            bool IsFirstCard = CardNumber == 1;

            // Determine how much the fluff around the text requires
            int cardHeaderLines = _getLines(card_header, card_data.MaxCharactersPerLine);
            int cardFooterLines = _getLines(card_footer, card_data.MaxCharactersPerLine);
            if (IsFirstCard)
                additionalLines = cardFooterLines;


            CardJSON card = new CardJSON
            {
                color = "black",
                title = CardNumber == 1 ? data.name : string.Format("{0} (Part {1})", data.name, CardNumber),
                icon = CardType_Helper.GetIconForType(target_class)
            };

            //Add the card now to ensure it appears before any "part X" cards
            resultArray.Add(card);

            //For only the first card we add header lines
            if (CardNumber == 1)
                builtCardText.AddRange(card_header);

            builtCardText.AddRange(card_text);


            if (_getLines(builtCardText, card_data.MaxCharactersPerLine) + additionalLines > card_data.MaxLines)
            {
                List<string> card_overtext = new List<string>();
                while (_getLines(builtCardText, card_data.MaxCharactersPerLine) + additionalLines > card_data.MaxLines && builtCardText.Count > 1)
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
                if (IsFirstCard)
                    additionalLines = 2;

                CardNumber++;
                card_text = card_overtext;
                CardJSON[] rv2 = _makeCard(CardNumber);

                builtCardText.Add(_fill());
                builtCardText.Add(_text(string.Format("Continues on {0}", rv2[0].title)));

                resultArray.AddRange(rv2);
            }

            //If we can or must add footer text.
            if (IsFirstCard || _getLines(builtCardText, card_data.MaxCharactersPerLine) + cardFooterLines <= card_data.MaxLines)
            {
                builtCardText.AddRange(card_footer);
            }
            card.contents = builtCardText.ToArray();

            return resultArray.ToArray();

        }


        public static bool _checkIfAbandon(string text)
        {
            return text.StartsWith("section") || text.StartsWith("subtitle");
        }

        public static int _getLines(List<string> text, int char_per_line)
        {
            double size = 0, multiplyer = 1;
            foreach (string line in text)
            {
                if (line.StartsWith("section") || line.StartsWith("subtitle"))
                    multiplyer = 1.1f;
                if (line.StartsWith("property") || line.StartsWith("bullet"))
                    multiplyer = 1.2f;
                if (line.StartsWith("dndstats"))
                    multiplyer = 2.5f;
                var subline = line.Contains("|") ? line.Substring(line.IndexOf('|')) : line;
                size += Math.Ceiling(subline.Length / (double)char_per_line) * multiplyer;
            }
            return (int)Math.Ceiling(size);
        }

        public static string _subtitle(string text)
        {
            return string.Format("subtitle | {0}", text.Trim());
        }

        public static string _fill(int c = 1)
        {
            return string.Format("fill | {0}", c);
        }

        public static string _section(string text)
        {
            return string.Format("section | {0}", text);
        }

        public static string _property(string title, string text)
        {
            return string.Format("property | {0} | {1}", title.Trim(), text.Trim());
        }

        public static string _property(string title, int text)
        {
            return string.Format("property | {0} | {1}", title.Trim(), text);
        }

        public static string _dndstats(int str, int dex, int con, int intel, int wis, int cha)
        {
            return string.Format("dndstats | {0} | {1} | {2} | {3} | {4} | {5}", str, dex, con, intel, wis, cha);
        }

        public static string _hr()
        {
            return "rule";
        }

        public static string _description(string title, string text)
        {
            return string.Format("description | {0} | {1}", title.Trim(), text.Trim());
        }

        public static string _bullet(string text)
        {
            return string.Format("bullet | {0}", text.Trim());
        }

        public static string _text(string text)
        {
            return string.Format("text | {0}", text.Trim());
        }

        public string[] _gen_desc(string title, string text)
        {

            if (text.Contains("|"))
            {
                text = text.Replace("|", "&#x7C;");
            }

            if (title != "" && text.IndexOf("\n") == -1 && text.IndexOf("**") == -1 && text.Length < card_data.MaxLines * card_data.MaxCharactersPerLine)
                return new string[] { _description(title, text) };

            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            List<string> rv = new List<string>();
            int i_real = 0;
            for (int i = 0; i < lines.Length; i++, i_real++)
            {
                string firstLine = "";
                while (lines[i].Length > (card_data.MaxLines - additionalLines) * card_data.MaxCharactersPerLine)
                {

                    int si = lines[i].LastIndexOf(".", (card_data.MaxLines - additionalLines) * card_data.MaxCharactersPerLine);
                    if (si == -1)
                        si = lines[i].LastIndexOf(" ", (card_data.MaxLines - additionalLines) * card_data.MaxCharactersPerLine);  //Really long run-on sentance;
                    if (si == -1)
                        si = (card_data.MaxLines - 1) * card_data.MaxCharactersPerLine;  //Really long thing with no spaces
                    rv.Add(_parseline(title, lines[i].Substring(0, si + 1), i_real, firstLine));
                    if (firstLine == "")
                        firstLine = lines[i].Substring(0, si + 1);
                    lines[i] = lines[i].Substring(si + 1);
                    i_real++;
                }
                rv.Add(_parseline(title, lines[i].Trim(), i_real, firstLine));
            }
            return rv.ToArray();

        }

        public static string _parseline(string title, string text, int line, string firstLine = "")
        {
            string testAgainst = text.Trim();
            if (firstLine != "")
                testAgainst = firstLine.Trim();

            if (title != "" && line == 0 && !testAgainst.StartsWith("**"))
                return _description(title, text);
            if (testAgainst.StartsWith('-'))
                return _bullet(text.TrimStart(new char[] { '-', ' ' }));
            if (testAgainst.StartsWith("###"))
            {
                return _section(text.TrimStart(new char[] { ' ', '#' }));
            }
            if (testAgainst.StartsWith("**"))
            {
                string pattern = @"^\*\*(.*)\*\*(.*)$";
                Regex rx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(testAgainst);
                if (matches.Count != 1)
                    throw new Exception(string.Format("'{0}' does not match pattern '/{1}/'", text, pattern));
                if (firstLine == "")
                {
                    title = matches[0].Groups[1].Value;
                    text = matches[0].Groups[2].Value;
                }
                else
                {
                    title = string.Format("{0}(cont.)", matches[0].Groups[1].Value);
                }

                return _property(title, text);
            }
            return _text(text);
        }

    }
}
