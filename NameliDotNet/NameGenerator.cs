using System;
using System.IO;
using System.Linq;
using System.Reflection; // I really need to use the other one :/
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace NameliDotNet
{
    internal class NameGenerator
    {
        private int _order;
        private int _minLength;
        private Random _random { get; set; }
        private IList<string> _used = new List<string>();
        private IList<string> _samples = new List<string>();
        private IDictionary<string, List<char>> _chains = new Dictionary<string, List<char>>();

        internal NameGenerator(Random random)
        {
            _random = random;
        }

        /// <summary>
        /// Generate a single name.
        /// </summary>
        /// <param name="names"></param>
        /// <returns>A name.</returns>
        internal string CreateFirstName(IList<string> names)
        {
            if(!names.Any()) throw new ArgumentNullException("List of names provided to generator is empty!");

            _samples = names;
            int minLength = 4;
            int baseOrder = ((_random.Next(0, 2) == 1)) ? 4 + 1 : 4 - 1; // 4 seems to be a sweet spot so we mix it up by doing plus or minus 1
            Generator(baseOrder, minLength);
            return FirstLetterToUpper(NextName);
        }

        // I think this is the problem area when it comes to threads
        // possibly the parser for reading from a file?
        private void Generator(int order, int minLength)
        {
            // fix the parameter values
            if (order < 1) order = 1;
            if (minLength < 1) minLength = 1;

            _order = order;
            _minLength = minLength;

            // Build the chains
            foreach (string word in _samples)
            {
                for (int letter = 0; letter < word.Length - order; letter++)
                {
                    string token = word.Substring(letter, order);
                    List<char> entry = null;
                    if (_chains.ContainsKey(token))
                        entry = _chains[token];
                    else
                    {
                        entry = new List<char>();
                        _chains[token] = entry;
                    }
                    entry.Add(word[letter + order]);
                }
            }
        }

        /// <summary>
        /// This used to be public. It really should just be a method. Left over from 
        /// when I began testing the original code.
        /// </summary>
        private string NextName
        {
            get
            {
                // TODO: use a StringBuilder. Seriously.
                // Get a random token from somewhere in middle of sample word                
                string name = "";
                do
                {
                    int value = _random.Next(_samples.Count);
                    int nameLength = _samples[value].Length;
                    if (nameLength < _order) _order = nameLength;
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
                while (_used.Contains(name) || name.Length < _minLength);
                _used.Add(name);
                return name;
            }
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

        // TODO
        // move this, it doesn't really belong
        // make extension method
        private string FirstLetterToUpper(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;
            if (str.Length > 1) return char.ToUpper(str[0]) + str.Substring(1);
            return str.ToUpper();
        }
    }
}
