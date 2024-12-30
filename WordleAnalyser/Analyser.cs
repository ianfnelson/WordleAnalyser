using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WordleAnalyser
{
    public static class Analyser
    {
        private static List<Wordle> _wordles;

        private static List<WordlePair> _wordlePairs;

        public static void Analyse()
        {
            var wordList = File.ReadAllText("words.txt");

            _wordles = wordList
                .Split(",")
                .OrderBy(x => x)
                .Select(x => new Wordle(x.Replace("\"", "")))
                .ToList();
            
            Console.WriteLine("Total wordles - " + _wordles.Count);
            
            Console.WriteLine("Scoring single guesses");
            
            Parallel.ForEach(_wordles, Analyse);

            var adieu = _wordles.Find(x => x.Word == "adieu");
            var ghost = _wordles.Find(x => x.Word == "ghost");
            AnalyseGreen(adieu);
            Console.WriteLine(adieu.Word + " " + adieu.Score + " " + adieu.Green);
            
            AnalyseGreen(ghost);
            Console.WriteLine(ghost.Word + " " + ghost.Score + " " + ghost.Green);
            
            Console.WriteLine("Total wordles - " + _wordles.Count);
            Console.WriteLine("Wordles above 1.0 - " + _wordles.Count(x => x.Score > 1.0));
            
            Console.WriteLine("HERE COME THE TOP 20");
            Console.WriteLine("--------------------");
            
            foreach (var topWordle in _wordles.OrderByDescending(x => x.Score).Take(20))
            {
                AnalyseGreen(topWordle);
                Console.WriteLine(topWordle.Word + " " + topWordle.Score + " " + topWordle.Green);
            }
            
            // _wordlePairs = new List<WordlePair>();
            //
            // var goodWordles = _wordles.Where(x => x.Score > 1.153D).ToList();
            //
            // Console.WriteLine("Good wordle count - " + goodWordles.Count);
            //
            // HashSet<string> orderedWordles = new HashSet<string>();
            // for (int i = 0; i < goodWordles.Count - 1; i++)
            // {
            //     for (int j = i + 1; j < goodWordles.Count; j++)
            //     {
            //         if (goodWordles[i].Score + goodWordles[j].Score > 3.06428D)
            //         {
            //             if ((goodWordles[i].Word + goodWordles[j].Word).ToCharArray().Distinct().Count()>=8)
            //             {
            //                 var wordlePair = new WordlePair(goodWordles[i], goodWordles[j]);
            //                 if (!orderedWordles.Contains(wordlePair.Ordered))
            //                 {
            //                     _wordlePairs.Add(wordlePair);
            //                     orderedWordles.Add(wordlePair.Ordered);
            //                 }
            //             }
            //         }
            //     }
            // }
            
            // Console.WriteLine("Wordle pair count " + _wordlePairs.Count);
            //
            // Parallel.ForEach(_wordlePairs, Analyse);
            //
            // Console.WriteLine("best wordle pairs");
            //
            // foreach (var pair in _wordlePairs.OrderByDescending(x => x.Score).Take(10))
            // {
            //     AnalyseGreen(pair);
            //     Console.WriteLine(pair.Guess1.Word + " " + pair.Guess1.Score + " " + pair.Guess2.Word + " " + pair.Guess2.Score + " " + pair.Score + " " + pair.Green);
            // }

            // var until = _wordles.Find(x => x.Word == "until");
            // var unlit = _wordles.Find(x => x.Word == "unlit");
            //
            // AnalyseGreen(until);
            // AnalyseGreen(unlit);
            //
            // Console.WriteLine(until.Word + " " + until.Score + " " + until.Green);
            // Console.WriteLine(unlit.Word + " " + unlit.Score + " " + unlit.Green);
            //
            // Console.WriteLine("Finding super pairs");
            //
            // var top10Str = "aeilnorstu";
            // var top10 = top10Str.ToCharArray();
            //
            // _wordlePairs = new List<WordlePair>();
            // for (int i = 0; i < _wordles.Count - 1; i++)
            // {
            //     if (_wordles[i].Score < 1.0D || _wordles[i].Word.ToCharArray().Any(x => !top10.Contains(x)))
            //     {
            //         continue;
            //     }
            //
            //     for (int j = i + 1; j < _wordles.Count; j++)
            //     {
            //         if (_wordles[j].Score >= 1.0D)
            //         {
            //             var wordlePair = new WordlePair(_wordles[i], _wordles[j]);
            //             if (wordlePair.Ordered == top10Str)
            //             {
            //                 _wordlePairs.Add(wordlePair);
            //             }
            //         }
            //     }
            // }
            //
            // Console.WriteLine("Super pair count " + _wordlePairs.Count);
            //
            // foreach (var pair in _wordlePairs)
            // {
            //     AnalyseGreen(pair.Guess1);
            //     AnalyseGreen(pair.Guess2);
            //
            //     pair.Green = pair.Guess1.Green + pair.Guess2.Green;
            // }
            //
            // Console.WriteLine("HERE COME THE BEST OF THE BEST");
            //
            // foreach (var pair in _wordlePairs.OrderByDescending(x => x.Green).Take(10))
            // {
            //     Console.WriteLine(pair.Guess1.Word + " " + pair.Guess1.Score + " " + pair.Guess2.Word + " " + pair.Guess2.Score + " " + pair.Green);
            // }

            // Console.WriteLine("Total pairs under consideration - " + +_wordlePairs.Count);
            //
            // Console.WriteLine("Analyzing pairs");
            //
            // Parallel.ForEach(_wordlePairs, Analyse);
            //
            // Console.WriteLine("HERE COME THE TOP 10");
            // Console.WriteLine("--------------------");
            //
            // foreach (var topWordlePair in _wordlePairs.OrderByDescending(x => x.Score).Take(10))
            // {
            //     Console.WriteLine(topWordlePair.Guess1.Word + " " + topWordlePair.Guess2.Word + " " + topWordlePair.Score);
            // }
            //
            // Console.WriteLine("HERE COME THE BOTTOM 10");
            // Console.WriteLine("-----------------------");
            //
            // foreach (var bottomWordlePair in _wordlePairs.OrderBy(x => x.Score).Take(10))
            // {
            //     Console.WriteLine(bottomWordlePair.Guess1.Word + " " + bottomWordlePair.Guess2.Word + " " + bottomWordlePair.Score);
            // }

        }

        private static void Analyse(WordlePair wordlePair)
        {
            var matchCounter = new Dictionary<int, int>();
            for (int i = 0; i <= 5; i++)
            {
                matchCounter.Add(i,0);
            }
            
            foreach (var otherWordle in _wordles)
            {
                var score = GetScore(otherWordle, wordlePair.Guess1, wordlePair.Guess2);

                matchCounter[score]++;
            }
            
            foreach (var kvp in matchCounter)
            {
                wordlePair.Score += (kvp.Key * kvp.Value);
            }

            wordlePair.Score /= _wordles.Count;
        }
        
        private static void AnalyseGreen(Wordle wordle)
        {
            var matchCounter = new Dictionary<int, int>();
            for (int i = 0; i <= 5; i++)
            {
                matchCounter.Add(i,0);
            }
            
            foreach (var otherWordle in _wordles)
            {
                var score = GetGreen(wordle, otherWordle);

                matchCounter[score]++;
            }
            
            foreach (var kvp in matchCounter)
            {
                wordle.Green += (kvp.Key * kvp.Value);
            }

            wordle.Green /= _wordles.Count;
        }
        
        private static void AnalyseGreen(WordlePair wordlePair)
        {
            var matchCounter = new Dictionary<int, int>();
            for (int i = 0; i <= 5; i++)
            {
                matchCounter.Add(i,0);
            }
            
            foreach (var otherWordle in _wordles)
            {
                var score = GetGreen(otherWordle, wordlePair.Guess1, wordlePair.Guess2);

                matchCounter[score]++;
            }
            
            foreach (var kvp in matchCounter)
            {
                wordlePair.Green += (kvp.Key * kvp.Value);
            }

            wordlePair.Green /= _wordles.Count;
        }
        
        private static void Analyse(Wordle wordle)
        {
            
            var matchCounter = new Dictionary<int, int>();
            for (int i = 0; i <= 5; i++)
            {
                matchCounter.Add(i,0);
            }
            
            foreach (var otherWordle in _wordles)
            {
                var score = GetScore(wordle, otherWordle);

                matchCounter[score]++;
            }
            
            foreach (var kvp in matchCounter)
            {
                wordle.Score += (kvp.Key * kvp.Value);
            }

            wordle.Score /= _wordles.Count;

            wordle.Dupes = wordle.Word.ToCharArray().GroupBy(x => x).Count() < 5;
        }

        private static int GetScore(Wordle word, Wordle guess)
        {
            List<WordleCharacter> chars = word.Word.ToCharArray().Select(x => new WordleCharacter(x)).ToList();

            foreach (var guessCharacter in guess.Word.ToCharArray())
            {
                var matchChar = chars.FirstOrDefault(x => x.Character == guessCharacter && !x.FoundByWordleOne);

                if (matchChar != null)
                {
                    matchChar.FoundByWordleOne = true;
                }
            }

            return chars.Count(x => x.FoundByWordleOne);
        }

        private static int GetGreen(Wordle word, Wordle guess)
        {
            List<WordleCharacter> chars = word.Word.ToCharArray().Select(x => new WordleCharacter(x)).ToList();

            var guessChars = guess.Word.ToCharArray();
            
            for (int j = 0; j < 5; j++)
            {
                if (guessChars[j] == chars[j].Character)
                {
                    chars[j].FoundByWordleOne = true;
                }
            }

            return chars.Count(x => x.FoundByWordleOne);
        }
        
        private static int GetGreen(Wordle word, Wordle guess1, Wordle guess2)
        {
            List<WordleCharacter> chars = word.Word.ToCharArray().Select(x => new WordleCharacter(x)).ToList();

            var guess1Chars = guess1.Word.ToCharArray();
            
            for (int j = 0; j < 5; j++)
            {
                if (guess1Chars[j] == chars[j].Character)
                {
                    chars[j].FoundByWordleOne = true;
                }
            }
            
            var guess2Chars = guess1.Word.ToCharArray();
            
            for (int j = 0; j < 5; j++)
            {
                if (guess2Chars[j] == chars[j].Character)
                {
                    chars[j].FoundByWordleTwo = true;
                }
            }

            return chars.Count(x => x.FoundByWordleOne || x.FoundByWordleTwo);
        }

        private static int GetScore(Wordle word, Wordle guess1, Wordle guess2)
        {
            List<WordleCharacter> chars = word.Word.ToCharArray().Select(x => new WordleCharacter(x)).ToList();
            
            foreach (var guessCharacter in guess1.Word.ToCharArray())
            {
                var matchChar = chars.FirstOrDefault(x => x.Character == guessCharacter && !x.FoundByWordleOne);

                if (matchChar != null)
                {
                    matchChar.FoundByWordleOne = true;
                }
            }
            
            foreach (var guessCharacter in guess2.Word.ToCharArray())
            {
                var matchChar = chars.FirstOrDefault(x => x.Character == guessCharacter && !x.FoundByWordleTwo);

                if (matchChar != null)
                {
                    matchChar.FoundByWordleTwo = true;
                }
            }

            return chars.Count(x => x.FoundByWordleOne || x.FoundByWordleTwo);
        }
    }
}