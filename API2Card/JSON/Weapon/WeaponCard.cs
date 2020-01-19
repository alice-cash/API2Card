using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace API2Card.JSON.Weapon
{
    public class WeaponCard:  Card<Weapon>
    {

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="card_icon"></param>
        /// <returns></returns>
        public static CardJSON[] GenerateCard(Weapon spell, CardType card_icon, CardSizeData card_data)
        {
            WeaponCard factory = new WeaponCard()
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

            if (data.properties != null && data.properties.Length > 0)
                foreach(string property in data.properties)
                    card_text.AddRange(_gen_desc(property,PropertyDesc(property)));

            return card_text;
        }

        public override List<String> _makeCardHeader()
        {
            List<string> card_text = new List<string>();

            card_text.Add(_subtitle(string.Format("{0} ({1})", data.category, data.cost)));
            card_text.Add(_hr());
            card_text.Add(_property("Damage", string.Format("{0} {1}", data.damage_dice, data.damage_type)));
            if(data.properties != null && data.properties.Length > 0) card_text.Add(_property("Properties ", string.Join(", ", data.properties)));
           // card_text.Add(_hr());

            return card_text;      
        }

        public override List<String> _makeCardFooter()
        {
            List<string> card_text = new List<string>();

            return card_text;
        }

        // Reference https://open5e.com/sections/weapons
        private string PropertyDesc(string property)
        {
            switch (property.Split(" ")[0].ToLowerInvariant())
            {
                case "finesse": return "When making an attack with a finesse weapon, you use your choice of your Strength or Dexterity modifier for the attack and damage rolls.You must use the same modifier for both rolls.";
                case "heavy": return "Small creatures have disadvantage on attack rolls with heavy weapons.A heavy weapon's size and bulk make it too large for a Small creature to use effectively.";
                case "light": return "A light weapon is small and easy to handle, making it ideal for use when fighting with two weapons.";
                case "loading": return "Because of the time required to load this weapon, you can fire only one piece of ammunition from it when you use an action, bonus action, or reaction to fire it, regardless of the number of attacks you can normally make.";
                case "range": return "A weapon that can be used to make a ranged attack has a range in parentheses after the ammunition or thrown property.The range lists two numbers.The first is the weapon's normal range in feet, and the second indicates the weapon's long range.When attacking a target beyond normal range, you have disadvantage on the attack roll.You can't attack a target beyond the weapon's long range.";
                case "reach": return "This weapon adds 5 feet to your reach when you attack with it, as well as when determining your reach for opportunity attacks with it.";
                case "thrown": return "If a weapon has the thrown property, you can throw the weapon to make a ranged attack.If the weapon is a melee weapon, you use the same ability modifier for that attack roll and damage roll that you would use for a melee attack with the weapon.";
                case "two-handed": return "This weapon requires two hands when you attack with it.";
                case "versatile": return "This weapon can be used with one or two hands. A damage value in parentheses appears with the property - the damage when the weapon is used with two hands to make a melee attack.";
                case "ammunition": return "This weapon requires the appropriate ammunition to attack. Each time you attack with the weapon, you expend one piece of ammunition. Drawing the ammunition from a quiver, case, or other container is part of the attack (you need a free hand to load a one-handed weapon). At the end of the battle, you can recover half your expended ammunition by taking a minute to search the battlefield.";

            }

            if(property == "special")
            {
                if (data.slug == "net")
                    return "A Large or smaller creature hit by a net is restrained until it is freed. A net has no effect on creatures that are formless, or creatures that are Huge or larger. A creature can use its action to make a DC 10 Strength check, freeing itself or another creature within its reach on a success. Dealing 5 slashing damage to the net (AC 10) also frees the creature without harming it, ending the effect and destroying the net. When you use an action, bonus action, or reaction to attack with a net, you can make only one attack regardless of the number of attacks you can normally make.";
                if (data.slug == "lance")
                    return "You have disadvantage when you use a lance to attack a target within 5 feet of you. Also, a lance requires two hands to wield when you aren't mounted.";
            }
            return "A weapon with the special property has unusual rules governing its use (Insert details here.)";
        }

    }
}
