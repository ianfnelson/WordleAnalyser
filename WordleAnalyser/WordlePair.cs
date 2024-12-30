using System.Collections.Generic;
using System.Linq;

namespace WordleAnalyser
{
    internal class WordlePair
    {

        public WordlePair(Wordle guess1, Wordle guess2)
        {
           
            Guess1 = guess1;
            Guess2 = guess2;
            
            Ordered = new string((guess1.Word + guess2.Word).ToCharArray().OrderBy(x => x).ToArray());
        }
        
        public string Ordered { get; set; }
        
        public Wordle Guess1 { get; set; }
        
        public Wordle Guess2 { get; set; }
        
        public double Score { get; set; }
        
        public double Green { get; set; }
    }
}