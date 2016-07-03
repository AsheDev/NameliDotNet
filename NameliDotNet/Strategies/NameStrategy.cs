using System;
using System.Linq;
using System.Text;
using NameliDotNet.Interfaces;
using System.Collections.Generic;

namespace NameliDotNet.Strategies
{
    /// <summary>
    /// Construct a name based on a Markov chain implementation
    /// </summary>
    public class NameStrategy : IStrategy
    {
        private Random _random;
        private IList<Matches> _matches;
        private IList<string> _names;
        private int _scope;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="names">A list of seed words to construct name</param>
        /// <param name="scope">Defaults to 1. Refers to how many previous letters the algorithm will include 
        /// while constructing similar words. The higher the value the more letters will be accounted for.</param>
        public NameStrategy(int scope = 1)
        {
            //if (ReferenceEquals(names, null) || !names.Any()) throw new ArgumentNullException("A list of words or names must be provided to generate a proper result.");
            _scope = (scope < 1) ? 1 : scope;
            _random = new Random();
            //_names = names;
            _names = new List<string>();
            ConstructMatches();
        }

        public void SetSourceText(IList<string> source)
        {
            if (ReferenceEquals(source, null) || !source.Any()) throw new ArgumentNullException("A list of words or names must be provided to generate a proper result.");
            _names = source;
            _matches.Clear();
        }

        public bool CompareLists(IList<string> newList)
        {
            return (newList.SequenceEqual(_names));
        }

        // TODO: this needs to be letter based and not word based
        private void ConstructMatches()
        {
            _matches = new List<Matches>();
            foreach (string name in _names)
            {
                for (int n = 0; n < (name.Length - _scope); ++n)
                {
                    StringBuilder stringToMatch = new StringBuilder();
                    int loops = _scope - name.Length;

                    for (int x = 0; x < _scope; ++x) // this breaks on the second iteration as x = 1 (_scope = 1)
                    {
                        stringToMatch.Append(name[n + x]);
                    }

                    Matches match = _matches.FirstOrDefault(m => m.Text.ToUpper().Equals(stringToMatch.ToString().ToUpper()));
                    if (!ReferenceEquals(match, null))
                    {
                        match.FollowedBy.Add(name[n + _scope].ToString());
                        _matches.Add(match);
                    }
                    else
                    {
                        match = new Matches();
                        match.Text = stringToMatch.ToString();
                        match.FollowedBy = new List<string> { name[n + _scope].ToString() };
                        _matches.Add(match);
                    }
                }
            }
        }

        public string GenerateText()
        {
            if (!_matches.Any()) ConstructMatches();
            StringBuilder starterText = new StringBuilder();
            int startIndex = _random.Next(0, _matches.Count - _scope);
            for (int x = 0; x < 1; ++x) // 1 used to be _scope
            {
                starterText.Append(_matches[startIndex].Text);
                //if (x < (_scope - 1)) starterText.Append(" ");
                ++startIndex;
            }

            StringBuilder finalText = new StringBuilder();
            finalText.Append(starterText);
            for (int n = 0; n < 4; ++n)
            {
                Matches match = _matches.FirstOrDefault(m => m.Text.ToUpper().Equals(starterText.ToString().ToUpper()));
                if (ReferenceEquals(match, null))
                {
                    break;
                }
                // TODO: this could be null and throw

                string nextLetter = match.FollowedBy[_random.Next(0, match.FollowedBy.Count)];
                finalText.Append(nextLetter);

                if (_scope > 1)
                {
                    string next = starterText.ToString().Substring(1, starterText.Length - 1);
                    starterText.Clear();
                    starterText.Append(next + nextLetter);
                }
                else
                {
                    starterText.Clear();
                    starterText.Append(nextLetter);
                }
            }
            
            return finalText.ToString();
        }
    }
}
