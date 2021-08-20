using System;
using System.Collections.Generic;
using System.Text;

namespace API2Card.JSON
{
    public class Result<T> where T : Result
    {
        public int count;
        public string next;
        public string previous;
        public T[] results;
    }
}
