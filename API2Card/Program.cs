using API2Card.JSON;
using API2Card.JSON.Magic_Item;
using API2Card.JSON.Monster;
using API2Card.JSON.Spells;
using API2Card.JSON.Weapon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace API2Card
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { "S35x50", "Magic_Item" };
            Search searchParamaters = Search.ParseArgs(args);
            if(searchParamaters.Sucess == false)
            {
                PrintHelp();
                return;
            }

            //File.WriteAllText("./a.json", RunQuery(searchParamaters));
            Console.WriteLine(RunQuery(searchParamaters));
        }

        

        private static void PrintHelp()
        {
            Console.WriteLine("Requires the following arguments, the card size, search type, and any search filters.");
            Console.WriteLine("Current card sizes supported: ");
            foreach (CardSize suit in (CardSize[])Enum.GetValues(typeof(CardSize)))
            {
                Console.WriteLine(suit.ToString());
            }

            Console.WriteLine("Current search types supported: ");
            foreach (Search.SearchKeys keys in (Search.SearchKeys[])Enum.GetValues(typeof(Search.SearchKeys)))
            {
                Console.WriteLine(keys.ToString());
            }

            Console.WriteLine("Current Spell args supported: ");
            foreach (Search.SearchKeysSpell keys in (Search.SearchKeysSpell[])Enum.GetValues(typeof(Search.SearchKeysSpell)))
            {
                Console.WriteLine(keys.ToString());
            }
        }

        public static string RunQuery(Search searchOptions)
        {

            bool Next = true;
            List<CardJSON> CardList = new List<CardJSON>();
            CardSizeData csd = new CardSizeData
            {
                Size = searchOptions.CardSize
            };
            string uri = searchOptions.GenerateSearchQuery();
            using (WebClient wc = new WebClient())
            {
                while (Next)
                {
                    string json = wc.DownloadString(uri);
                    if (searchOptions.Type == Search.SearchKeys.Spell)
                    {
                        Result<Spell> SearchResults = JsonConvert.DeserializeObject<Result<Spell>>(json);

                        foreach (Spell s in SearchResults.results)
                        {
                            CardJSON[] Cards = SpellCard.GenerateCard(s, CardType.Spell, csd);
                            CardList.AddRange(Cards);
                        }

                        if (SearchResults.next != null)
                            uri = SearchResults.next;
                        else
                            Next = false;

                    }

                    if (searchOptions.Type == Search.SearchKeys.Monster)
                    {
                        Result<Monster> SearchResults = JsonConvert.DeserializeObject<Result<Monster>>(json);

                        foreach (Monster s in SearchResults.results)
                        {
                            CardJSON[] Cards = MonsterCard.GenerateCard(s, CardType.Monster, csd);
                            CardList.AddRange(Cards);
                        }

                        if (SearchResults.next != null)
                            uri = SearchResults.next;
                        else
                            Next = false;
                    }

                    if (searchOptions.Type == Search.SearchKeys.Magic_Item)
                    {
                        Result<Magic_Item> SearchResults = JsonConvert.DeserializeObject<Result<Magic_Item>>(json);

                        foreach (Magic_Item s in SearchResults.results)
                        {
                            CardJSON[] Cards = Magic_ItemCard.GenerateCard(s, CardType.Magic_Item, csd);
                            CardList.AddRange(Cards);
                        }

                        if (SearchResults.next != null)
                            uri = SearchResults.next;
                        else
                            Next = false;
                    }


                    if (searchOptions.Type == Search.SearchKeys.Weapon)
                    {
                        Result<Weapon> SearchResults = JsonConvert.DeserializeObject<Result<Weapon>>(json);

                        foreach (Weapon s in SearchResults.results)
                        {
                            CardJSON[] Cards = WeaponCard.GenerateCard(s, CardType.Weapon, csd);
                            CardList.AddRange(Cards);
                        }

                        if (SearchResults.next != null)
                            uri = SearchResults.next;
                        else
                            Next = false;
                    }

                }
            }
            return JsonConvert.SerializeObject(CardList.ToArray());
        }
    }
}
