using API2Card.JSON.Spells;
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

            //LoadJson("S25x35","https://api.open5e.com/spells/?level_int__iexact=", "./test.json");
            if (args.Length != 3) {
                PrintHelp();
                return;
            }
            LoadJson(args[0], args[1], args[2]);
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Requires 3 arguments, the card size, the API url to parse, and an output file name. Wrap these in double quotes to prevent commabd line issues.");
            Console.WriteLine("Current card sizes supported: ");
            foreach (CardSize suit in (CardSize[])Enum.GetValues(typeof(CardSize)))
            {
                Console.WriteLine(suit.ToString());
            }
        }

        public static void LoadJson(string cardSizeArg, string uri, string file)
        {
            CardSize cardSize;
            if(!Enum.TryParse<CardSize>(cardSizeArg, out cardSize))
            {
                Console.WriteLine("Unknown card size {0}", cardSizeArg);
                PrintHelp();
                return;
            }

            bool Next = true;
            List<SpellCard> CardList = new List<SpellCard>();
            using (WebClient wc = new WebClient())
            {
                while (Next)
                {
                    string json = wc.DownloadString(uri);
                    Result SearchResults = JsonConvert.DeserializeObject<Result>(json);

                    foreach (Spell s in SearchResults.results)
                    {
                        CardSizeData csd = new CardSizeData();
                        csd.Size = cardSize;
                        SpellCard[] Cards = SpellCard.GenerateSpellCard(s, CardType.Spell, csd);
                        CardList.AddRange(Cards);
                    }

                    if (SearchResults.next != null)
                        uri = SearchResults.next;
                    else
                        Next = false;
                }
            }
            File.WriteAllText(file, JsonConvert.SerializeObject(CardList.ToArray()));
        }
    }
}
