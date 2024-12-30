using System;

namespace WordleAnalyser
{
    internal class WordleCharacter
    {
        public WordleCharacter(char character)
        {
            Character = character;
        }
        
        public char Character { get; set; }
        
        public bool FoundByWordleOne { get; set; }
        
        public bool FoundByWordleTwo { get; set; }
    }
}