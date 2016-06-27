using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace NameliDotNet
{
    public sealed class NameGenerator
    {
        private int _order;
        private int _minLength;
        private Random _random { get; set; }
        //private IList<string> _used = new List<string>();
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
        public string CreateName(IList<string> names)
        {
            if(!names.Any()) throw new ArgumentNullException("List of names provided to generator is empty!");

            _samples = names;
            int minLength = 4;
            int baseOrder = ((_random.Next(0, 2) == 1)) ? 4 + 1 : 4 - 1; // 4 seems to be a sweet spot so we mix it up by doing plus or minus 1
            ConstructChains(baseOrder, minLength);
            return NewName().FirstCharToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="minLength"></param>
        private void ConstructChains(int order, int minLength)
        {
            // fix the parameter values
            if (order < 1) order = 1;
            if (minLength < 1) minLength = 1;

            _order = order;
            _minLength = minLength;

            // Build the chains
            foreach (string word in _samples)
            {
                for (int letter = 0; letter < (word.Length - order); ++letter)
                {
                    string token = word.Substring(letter, order);
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
                    entry.Add(word[letter + order]);
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
            // Get a random token from somewhere in middle of sample word  
            //StringBuilder name = new StringBuilder();  
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
