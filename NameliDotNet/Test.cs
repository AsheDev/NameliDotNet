﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NameliDotNet
{
    public class Test
    {
        private Dictionary<string, string> _matches;
        private IList<string> _words;
        private Random _random;
        private string _sample = "Sometimes fate is like a small sandstorm that keeps changing directions. You change direction but the sandstorm chases you. You turn again, but the storm adjusts. Over and over you play this out, like some ominous dance with death just before dawn. Why? Because this storm isn’t something that blew in from far away, something that has nothing to do with you. This storm is you. Something inside of you. So all you can do is give in to it, step right inside the storm, closing your eyes and plugging up your ears so the sand doesn’t get in, and walk through it, step by step. There’s no sun there, no moon, no direction, no sense of time. Just fine white sand swirling up into the sky like pulverized bones. That’s the kind of sandstorm you need to imagine. " +
            "And you really will have to make it through that violent, metaphysical, symbolic storm. No matter how metaphysical or symbolic it might be, make no mistake about it: it will cut through flesh like a thousand razor blades. People will bleed there, and you will bleed too. Hot, red blood. You’ll catch that blood in your hands, your own blood and the blood of others. " +
            "And once the storm is over you won’t remember how you made it through, how you managed to survive. You won’t even be sure, in fact, whether the storm is really over. But one thing is certain. When you come out of the storm you won’t be the same person who walked in. That’s what this storm’s all about.";

        public Test()
        {
            _matches = new Dictionary<string, string>();
            _words = _sample.Split(' ');
            _random = new Random();
            Database();
        }

        private void Database()
        {
            for (int n = 0; n < _words.Count - 2; ++n)
            {
                string one = _words[n];
                string two = _words[n + 1];
                string three = _words[n + 2];
                string key = one + "|" + two;
                if (_matches.ContainsKey(key))
                {
                    _matches[key] = _matches[key] + "|" + three;
                }
                else
                {
                    _matches[key] = three;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Generate(int length = 25)
        {
            int seed = _random.Next(0, _words.Count - 3); // this might skip the upper bound?
            seed = 213;
            string initial = _words[seed];
            string subsequent = _words[seed + 1];

            IList<string> generated = new List<string>();
            for (int n = 0; n < length; ++n)
            {
                generated.Add(initial);
                initial = subsequent;
                //subsequent = _matches.ContainsKey("you");
            }
            generated.Add(subsequent);
            return "TEST COMPLETE";
        }
    }

    // this guy is what I need to be working with
    public class MarkovStrategies
    {
        private IStrategy _strategy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope">Scope determines how far back we look to construct the next word</param>
        public MarkovStrategies(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public string GenerateText()
        {
            return _strategy.GenerateText();
        }
    }

    internal class Matches
    {
        internal string Text { get; set; }  // this could be "the" or "the brown fox"
        //internal IDictionary<string, int> FollowedBy { get; set; } // a word that proceeds the Text and its frequency count
        internal IList<string> FollowedBy { get; set; }

        public Matches()
        {
            //FollowedBy = new Dictionary<string, int>();
            FollowedBy = new List<string>();
        }
    }

    public interface IStrategy
    {
        string GenerateText();
    }

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
        public NameStrategy(IList<string> names, int scope = 1)
        {
            if (ReferenceEquals(names, null) || !names.Any()) throw new ArgumentNullException("A list of words or names must be provided to generate a proper result.");
            _scope = (scope < 1) ? 1 : scope;
            _random = new Random();
            _names = names;
            ConstructMatches();
        }

        // TODO: this needs to be letter based and not word based
        private void ConstructMatches()
        {
            _matches = new List<Matches>();
            foreach(string name in _names)
            {
                for(int n = 0; n < (name.Length - _scope); ++n)
                {
                    StringBuilder stringToMatch = new StringBuilder();
                    int loops = _scope - name.Length;

                    for (int x = 0; x < _scope; ++x) // this breaks on the second iteration as x = 1 (_scope = 1)
                    {
                        stringToMatch.Append(name[n + x]);
                    }

                    Matches match = _matches.FirstOrDefault(m => m.Text.ToUpper().Equals(stringToMatch.ToString().ToUpper()));
                    if(!ReferenceEquals(match, null))
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
                if(ReferenceEquals(match, null))
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

    /// <summary>
    /// Construct a few phrases based on some source text
    /// </summary>
    public class PhraseStrategy : IStrategy
    {
        private Random _random;
        private IList<Matches> _matches;
        private IList<string> _words;
        private int _scope;

        /// <summary>
        /// Seed the strategy with a source text. The larger the source text the better the final result. 
        /// Setting the scope with a value higher than one will improve the results with a higher number 
        /// offering a more "realistic" result.
        /// </summary>
        /// <param name="sourceText">Text to base the final result off of</param>
        /// <param name="scope">Defaults to 1. Higher values may return better final results.</param>
        public PhraseStrategy(string sourceText, int scope = 1)
        {
            if (string.IsNullOrWhiteSpace(sourceText)) throw new ArgumentNullException("PhaseStrategy must implement a valid string of text.");
            _scope = (scope < 1) ? 1 : scope;
            _random = new Random();
            // check this a bit more throughly
            _words = sourceText.Split(' ');
            _matches = new List<Matches>();
            ConstructMatches();
        }

        public string GenerateText()
        {
            StringBuilder starterText = new StringBuilder();
            int startIndex = _random.Next(0, _words.Count - _scope);
            for (int x = 0; x < _scope; ++x)
            {
                starterText.Append(_words[startIndex]);
                if (x < (_scope - 1)) starterText.Append(" ");
                ++startIndex;
            }

            StringBuilder finalText = new StringBuilder();
            finalText.Append(starterText + " ");
            int periodCount = 0;
            for (int n = 0; n < 125; ++n)
            {
                Matches match = _matches.First(m => m.Text.ToUpper().Equals(starterText.ToString().ToUpper()));
                // TODO: this could be null and throw

                string nextWord = match.FollowedBy[_random.Next(0, match.FollowedBy.Count)];
                if (nextWord.Contains('.')) ++periodCount;
                finalText.Append(nextWord);
                if (periodCount < 4 || n < 125) finalText.Append(" ");
                if (periodCount >= 4) break;

                starterText.Clear();
                if (_scope > 1)
                {
                    IList<string> temp = match.Text.Split(' ').ToList();
                    temp.RemoveAt(0);
                    starterText.Append(string.Join(" ", temp.ToArray()) + " ");
                    starterText.Append(nextWord);
                }
                else
                {
                    starterText.Append(nextWord);
                }
            }

            return finalText.ToString();
        }

        private void ConstructMatches()
        {
            for (int n = 0; n < (_words.Count - _scope); ++n)
            {
                StringBuilder currentText = new StringBuilder();
                for (int x = n; x < (_scope + n); ++x)
                {
                    currentText.Append(_words[x]);
                    if ((x + 1) < (_scope + n)) currentText.Append(" ");
                }

                Matches match = _matches.FirstOrDefault(m => m.Text.ToUpper().Equals(currentText.ToString().ToUpper()));
                if (!ReferenceEquals(match, null))
                {
                    match.FollowedBy.Add(_words[n + _scope]);
                    _matches.Add(match);
                }
                else
                {
                    match = new Matches();
                    match.Text = currentText.ToString();
                    match.FollowedBy = new List<string> { _words[n + _scope] };
                    _matches.Add(match);
                }
            }
        }
    }
}
