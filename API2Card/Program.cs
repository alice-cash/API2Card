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

#if DEBUG
            LoadJson("https://api.open5e.com/spells/?level_int__iexact=&limit=500", "./test.json");
#else
            if(args.Length != 2) {
                Console.WriteLine("Requires 2 arguments, the API url to parse and an output file name. Wrap these in double quotes if you have any issues.");
                return;
            }
            LoadJson(args[0], args[1]);
#endif
        }

        public static void LoadJson(string uri, string file)
        {
            using (WebClient wc = new WebClient())
            {

                string json = wc.DownloadString(uri);
                Result SearchResults = JsonConvert.DeserializeObject<Result>(json);
                List<SpellCard> CardList = new List<SpellCard>();
                foreach (Spell s in SearchResults.results) {
                    //24 lines, 55 characters per line
                    CardSizeData csd = new CardSizeData();
                    csd.Size = CardSize.S75x50;
                    SpellCard[] Cards = SpellCard.GenerateSpellCard(s, CardType.Spell, csd);
                    CardList.AddRange(Cards);
                }
                File.WriteAllText(file, JsonConvert.SerializeObject(CardList.ToArray()));
                //Console.WriteLine(JsonConvert.SerializeObject(CardList.ToArray()));
                //Console.ReadKey();
            }
        }
    }
}
