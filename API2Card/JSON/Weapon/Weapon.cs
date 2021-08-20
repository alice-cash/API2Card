namespace API2Card.JSON.Weapon
{
    public class Weapon : Result
    {
        public string slug;
        public string category;
        public string document__slug;
        public string document__title;
        public string document__license_url;
        public string cost;
        public string damage_dice;
        public string damage_type;
        public string weight;
        public string[] properties;
    }
}