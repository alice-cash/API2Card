using System;
using System.Collections.Generic;
using System.Text;

namespace API2Card.JSON
{
    public class CardJSON
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

    }
}
