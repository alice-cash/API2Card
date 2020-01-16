using System;
using System.Collections.Generic;
using System.Text;

namespace API2Card
{
    public enum CardSize
    {
        S35x50,
        S75x50,
        S25x35,
        S225x35
    }

    /// <summary>
    /// Returns characer limit data for card sizes. This was based on trial and error and 98% of cards display very well with these limits.
    /// </summary>
    public class CardSizeData
    {
        public CardSize Size;

        public int MaxLines
        {
            get
            {
                switch (Size)
                {
                    case CardSize.S225x35: return 20;
                    case CardSize.S25x35: return 20;
                    case CardSize.S35x50: return 26;
                    case CardSize.S75x50: return 34;
                    default: return 1;
                }
            }
        }
        public int MaxCharactersPerLine
        {
            get
            {
                switch (Size)
                {
                    case CardSize.S225x35: return 32;
                    case CardSize.S25x35: return 37;
                    case CardSize.S35x50: return 55;
                    case CardSize.S75x50: return 135;
                    default: return 1;
                }
            }
        }
    }

}
