﻿using System;
using System.Linq;
using NameliDotNet.Interfaces;
using System.Collections.Generic;

namespace NameliDotNet.Strategies
{
    public class OriginalWordStrategy : IStrategy
    {
        private int _order;
        private int _minLength;
        private Random _random { get; set; }
        //private IList<string> _used = new List<string>();
        private IList<string> _samples = new List<string>();
        private IDictionary<string, List<char>> _chains = new Dictionary<string, List<char>>();

        public OriginalWordStrategy()
        {
            // I should probably allow for custom minLength and Order?
            _random = new Random();
            //_samples = names;
        }

        public void SetSourceText(IList<string> source)
        {
            if (ReferenceEquals(source, null) || !source.Any()) throw new ArgumentNullException("A list of words or names must be provided to generate a proper result.");
            _samples = source;
            _chains.Clear();
        }

        public bool CompareLists(IList<string> newList)
        {
            return (newList.SequenceEqual(_samples));
        }

        /// <summary>
        /// Generate a single name.
        /// </summary>
        /// <param name="names"></param>
        /// <returns>A name.</returns>
        public string GenerateText()
        {
            if (!_samples.Any()) throw new ArgumentNullException("List of names provided to generator is empty!");

            _minLength = 4;
            _order = ((_random.Next(0, 2) == 1)) ? 4 + 1 : 4 - 1; // 4 seems to be a sweet spot so we mix it up by doing plus or minus 1
            ConstructChains();

            // this is a weird bug :| I'll deal with it later.
            while(_chains.FirstOrDefault().Key.Length != _order)
            {
                _chains.Clear();
                ConstructChains();
            }
            return NewName().FirstCharToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="minLength"></param>
        private void ConstructChains()
        {
            // Build the chains
            foreach (string word in _samples)
            {
                for (int letter = 0; letter < (word.Length - _order); ++letter)
                {
                    string token = word.Substring(letter, _order);
                    List<char> entry = null;
                    if (_chains.ContainsKey(token))
                    {
                        entry = _chains[token];
                    }
                    else
                    {
                        entry = new List<char>();
                        _chains[token] = entry;
                    }
                    entry.Add(word[letter + _order]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string NewName()
        {
            // Try out a stringbuilder
            string name = "";
            int safety = 0;
            do
            {
                int value = _random.Next(_samples.Count);
                int nameLength = _samples[value].Length;
                //if (nameLength < _order) _order = nameLength; // I think this is the culprit
                if (nameLength < _order) continue;
                name = _samples[value].Substring(_random.Next(0, nameLength - _order), _order);
                while (name.Length < nameLength)
                {
                    string token = name.Substring(name.Length - _order, _order);
                    char character = GetLetter(token);
                    if (character != '?')
                    {
                        name += GetLetter(token);
                    }
                    else
                    {
                        break; // end of the line
                    }
                }

                if (name.Contains(" "))
                {
                    string[] tokens = name.Split(' ');
                    name = "";
                    for (int t = 0; t < tokens.Length; t++)
                    {
                        if (tokens[t] == "") continue;
                        if (tokens[t].Length == 1)
                        {
                            tokens[t] = tokens[t].ToUpper();
                        }
                        else
                        {
                            tokens[t] = tokens[t].Substring(0, 1) + tokens[t].Substring(1).ToLower();
                        }
                        if (name != "") name += " ";
                        name += tokens[t];
                    }
                }
                else
                {
                    name = name.Substring(0, 1) + name.Substring(1).ToLower();
                }
                //Console.WriteLine("\nLength: " + s.Length);
            }
            while (name.Length < _minLength);
            //while (_used.Contains(name) || name.Length < _minLength) ;
            //_used.Add(name);
            return name;
        }

        /// <summary>
        /// Retrieve a random letter from the in-process chain.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private char GetLetter(string value)
        {
            if (!_chains.ContainsKey(value)) return '?';
            List<char> letters = _chains[value];
            int randomNumber = _random.Next(letters.Count);
            return letters[randomNumber];
        }
    }
}
