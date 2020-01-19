using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace API2Card
{
    public class Search
    {
        public enum SearchKeys
        {
            Spell,
            Monster,
            Weapon,
            Magic_Item
        }

        public enum SearchKeysSpell
        {
            Name,
            Level,
            Class
        }

        public enum SearchKeysMonster
        {
            Name,
            AC,
            CR,
            Type
        }

        public enum SearchKeysWeapon
        {
            Name
        }

        public enum SearchKeysMagic_Item
        {
            Name
        }

        public CardSize CardSize;
        public SearchKeys Type;
        public List<KeyValuePair<SearchKeysSpell, string>> SpellArgs;
        public List<KeyValuePair<SearchKeysMonster, string>> MonsterArgs;
        public List<KeyValuePair<SearchKeysWeapon, string>> WeaponArgs;
        public List<KeyValuePair<SearchKeysMagic_Item, string>> Magic_ItemArgs;
        public bool Sucess;
        public string Error;

        /*public Search(SearchKeys type, List<KeyValuePair<SearchKeysSpell,string>> args, bool sucess =true, string error = "")
        {
            Type = type;
            Args = args;
            Sucess = sucess;
            Error = error;
        }*/

        public string GenerateSearchQuery()
        {
            StringBuilder query = new StringBuilder();

            query.Append(ConvertSearchKeysToArg(Type));

            if(SpellArgs != null)
                foreach (KeyValuePair<SearchKeysSpell, string> entry in SpellArgs)
                    query.AppendFormat("{0}={1}&", ConvertSearchKeysSpellToArg(entry.Key), HttpUtility.UrlEncode(entry.Value));

            if (MonsterArgs != null)
                foreach (KeyValuePair<SearchKeysMonster, string> entry in MonsterArgs)
                    query.AppendFormat("{0}={1}&", ConvertSearchKeysMonsterToArg(entry.Key), HttpUtility.UrlEncode(entry.Value));

            if (WeaponArgs != null)
                foreach (KeyValuePair<SearchKeysWeapon, string> entry in WeaponArgs)
                    query.AppendFormat("{0}={1}&", ConvertSearchKeysWeaponToArg(entry.Key), HttpUtility.UrlEncode(entry.Value));

            if (Magic_ItemArgs != null)
                foreach (KeyValuePair<SearchKeysMagic_Item, string> entry in Magic_ItemArgs)
                    query.AppendFormat("{0}={1}&", ConvertSearchKeysMagic_ItemToArg(entry.Key), HttpUtility.UrlEncode(entry.Value));

            return query.ToString();
        }

        private string ConvertSearchKeysSpellToArg(SearchKeysSpell item)
        {
            switch (item)
            {
                case SearchKeysSpell.Name:
                    return "id_name";
                case SearchKeysSpell.Level:
                    return "id_level_int";
                case SearchKeysSpell.Class:
                    return "id_dnd_class";
            }
            throw new NotImplementedException();
        }


        private string ConvertSearchKeysMonsterToArg(SearchKeysMonster item)
        {
            switch (item)
            {
                case SearchKeysMonster.Name:
                    return "name";
                case SearchKeysMonster.AC:
                    return "armor_class";
                case SearchKeysMonster.CR:
                    return "challenge_rating";
                case SearchKeysMonster.Type:
                    return "type";
            }
            throw new NotImplementedException();
        }

        private string ConvertSearchKeysWeaponToArg(SearchKeysWeapon item)
        {
            switch (item)
            {
                case SearchKeysWeapon.Name:
                    return "name";
            }
            throw new NotImplementedException();
        }

        private string ConvertSearchKeysMagic_ItemToArg(SearchKeysMagic_Item item)
        {
            switch (item)
            {
                case SearchKeysMagic_Item.Name:
                    return "name";
            }
            throw new NotImplementedException();
        }

        private string ConvertSearchKeysToArg(SearchKeys item)
        {
            switch (item)
            {
                case SearchKeys.Spell:
                    return "https://api.open5e.com/spells/?";
                case SearchKeys.Magic_Item:
                    return "https://api.open5e.com/magicitems/?";
                case SearchKeys.Monster:
                    return "https://api.open5e.com/monsters/?";
                case SearchKeys.Weapon:
                    return "https://api.open5e.com/weapons/?";
            }
            throw new NotImplementedException();
        }

        public static Search ParseArgs(string[] args)
        {
            if (args.Length < 1)
                return new Search()
                {
                    Sucess = false,
                    Error = string.Format("No card type provided. One of the following is required: {0}.", string.Join(',',Enum.GetNames(typeof(CardSize))))
                };

            if (args.Length < 2)
                return new Search()
                {
                    Sucess = false,
                    Error = string.Format("No type provided. One of the following is required: {0}.", string.Join(',', Enum.GetNames(typeof(SearchKeys))))
                };

            if (!Enum.TryParse<CardSize>(args[0], true, out CardSize cardSize))
                return new Search()
                {
                    Sucess = false,
                    Error = string.Format("Invalid card size '{0}'. One of the following is required: {1}.", args[0], string.Join(',', Enum.GetNames(typeof(CardType))))
                };

            args = POP(args);

            if (!Enum.TryParse<SearchKeys>(args[0], true, out SearchKeys key))
                return new Search()
                {
                    Sucess = false,
                    Error = string.Format("Invalid type provided '{0}'. One of the following is required: {1}.", args[0], string.Join(',', Enum.GetNames(typeof(SearchKeys))))
                };

            args = POP(args);

            switch (key)
            {
                case SearchKeys.Spell:
                    return ParseArgsSpell(args, cardSize);
                case SearchKeys.Monster:
                    return ParseArgsMonster(args, cardSize);
                case SearchKeys.Weapon:
                    return ParseArgsWeapon(args, cardSize);
                case SearchKeys.Magic_Item:
                    return ParseArgsMagic_Item(args, cardSize);

            }

            throw new NotImplementedException();

        }

        private static Search ParseArgsSpell(string[] args, CardSize cardSize)
        {
            List<KeyValuePair<SearchKeysSpell, string>> spellSearch = new List<KeyValuePair<SearchKeysSpell, string>>();

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                if (!Enum.TryParse<SearchKeysSpell>(args[i], true, out SearchKeysSpell searchKey))
                {
                    return new Search()
                    {
                        Sucess = false,
                        Error = string.Format("Invalid type provided '{0}'. One of the following is required: {1}.", args[0], string.Join(',', Enum.GetNames(typeof(SearchKeysSpell))))
                    };
                }
                spellSearch.Add(new KeyValuePair<SearchKeysSpell, string>(searchKey, args[i + 1]));
            }

            return new Search()
            {
                CardSize = cardSize,
                Type = SearchKeys.Spell,
                SpellArgs = spellSearch,
                Sucess = true,
            };
        }

        private static Search ParseArgsWeapon(string[] args, CardSize cardSize)
        {
            List<KeyValuePair<SearchKeysWeapon, string>> spellSearch = new List<KeyValuePair<SearchKeysWeapon, string>>();

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                if (!Enum.TryParse<SearchKeysWeapon>(args[i], true, out SearchKeysWeapon searchKey))
                {
                    return new Search()
                    {
                        Sucess = false,
                        Error = string.Format("Invalid type provided '{0}'. One of the following is required: {1}.", args[0], string.Join(',', Enum.GetNames(typeof(SearchKeysWeapon))))
                    };
                }
                spellSearch.Add(new KeyValuePair<SearchKeysWeapon, string>(searchKey, args[i + 1]));
            }

            return new Search()
            {
                CardSize = cardSize,
                Type = SearchKeys.Weapon,
                WeaponArgs = spellSearch,
                Sucess = true,
            };
        }

        private static Search ParseArgsMonster(string[] args, CardSize cardSize)
        {
            SearchKeys key = SearchKeys.Monster;
            List<KeyValuePair<SearchKeysMonster, string>> monsterSearch = new List<KeyValuePair<SearchKeysMonster, string>>();

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                if (!Enum.TryParse<SearchKeysMonster>(args[i], true, out SearchKeysMonster searchKey))
                {
                    return new Search()
                    {
                        Sucess = false,
                        Error = string.Format("Invalid type provided '{0}'. One of the following is required: {1}.", args[0], string.Join(',', Enum.GetNames(typeof(SearchKeysMonster))))
                    };
                }
                monsterSearch.Add(new KeyValuePair<SearchKeysMonster, string>(searchKey, args[i + 1]));
            }

            return new Search()
            {
                CardSize = cardSize,
                Type = SearchKeys.Monster,
                MonsterArgs = monsterSearch,
                Sucess = true,
            };
        }


        private static Search ParseArgsMagic_Item(string[] args, CardSize cardSize)
        {

            List<KeyValuePair<SearchKeysMagic_Item, string>> monsterSearch = new List<KeyValuePair<SearchKeysMagic_Item, string>>();

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                if (!Enum.TryParse<SearchKeysMagic_Item>(args[i], true, out SearchKeysMagic_Item searchKey))
                {
                    return new Search()
                    {
                        Sucess = false,
                        Error = string.Format("Invalid type provided '{0}'. One of the following is required: {1}.", args[0], string.Join(',', Enum.GetNames(typeof(SearchKeysMagic_Item))))
                    };
                }
                monsterSearch.Add(new KeyValuePair<SearchKeysMagic_Item, string>(searchKey, args[i + 1]));
            }

            return new Search()
            {
                CardSize = cardSize,
                Type = SearchKeys.Magic_Item,
                Magic_ItemArgs = monsterSearch,
                Sucess = true,
            };
        }

        private static string[] POP(string[] args)
        {
            if (args.Length < 1) throw new ArgumentException("Argumnent list too small to pop");
            string[] newArgs = new string[args.Length-1];
            for (int i = 1; i < args.Length; i++)
                newArgs[i-1] = args[i];
            return newArgs;
        }
    }
}
