using System;
using System.Collections.Generic;
using System.Text;

namespace API2Card.JSON.Spells
{
    public class Result
    {
        public int count;
        public string next;
        public string previous;
        public Spell[] results;
    }
}
