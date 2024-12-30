using System.Collections.Generic;
using System.Linq;

namespace WordleAnalyser
{
    internal class Wordle
    {
        public Wordle(string word)
        {
            Word = word;
            OrderedWord = new string(Word.ToCharArray().OrderBy(x => x).ToArray());
        }
        
        public string Word { get; set; }
        
        public string OrderedWord { get; set; }

        public double Score { get; set; }
        
        public double Green { get; set; }
        
        public bool Dupes { get; set; }
    }
}
