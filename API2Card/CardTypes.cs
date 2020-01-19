using System;
using System.Collections.Generic;
using System.Text;

namespace API2Card
{
    public enum CardType
    {
        Spell,
        Monster,
        Weapon,
        Magic_Item
    }

    public static class CardType_Helper
    {
        public static string GetIconForType(CardType TargetType)
        {
            switch (TargetType)
            {
                case CardType.Spell: return "crystal-wand";
            }
            return "perspective-dice-six-faces-random";
        }

    }
}
