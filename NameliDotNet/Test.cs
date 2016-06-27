using System;
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

    public abstract class MarkovBase
    {
        internal Random _random;
        internal IList<Matches> _matches;
        internal IList<string> _words;
        internal int _scope;

        public MarkovBase()
        {
            _random = new Random();
            _matches = new List<Matches>();
            _words = new List<string>();
        }

        public abstract string GenerateText();
    }

    public class MarkovPhrases : MarkovBase
    {
        public MarkovPhrases(IList<string> words, int scope = 1)
        {
            _words = words;
            _scope = scope;
            ConstructMatches();
        }

        public override string GenerateText()
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
                Matches match = _matches.FirstOrDefault(m => m.Text.ToUpper().Equals(starterText.ToString().ToUpper()));
                if (ReferenceEquals(match, null)) throw new ApplicationException("TODO!");

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

    // TODO: I wonder if I could "weight" which first letter gets chosen so we find a great starting spot
    public class MarkovWords : MarkovBase
    {
        private int _minLength; // TODO
        //private IList<string> _used = new List<string>(); // TODO
        private IDictionary<string, List<char>> _chains = new Dictionary<string, List<char>>(); // TODO

        public MarkovWords(IList<string> words, int scope = 1)
        {
            _words = words;
            _scope = (scope < 1) ? 1 : scope;
            ConstructMatches();

            // TODO
            _minLength = 4;
        }

        public override string GenerateText()
        {
            // Try out a stringbuilder
            // Get a random token from somewhere in middle of sample word  
            //StringBuilder name = new StringBuilder();  
            string name = "";
            do
            {
                // could we sort the _chains so we could more popular letters to begin with?
                // this orders _chains by chunks of text that occur most frequently
                //var checkIt = _chains.OrderByDescending(c => c.Value.Count);

                int value = _random.Next(_words.Count); // snag a random word's index
                int nameLength = _words[value].Length;
                if (nameLength < _scope) _scope = nameLength;
                name = _words[value].Substring(_random.Next(0, nameLength - _scope), _scope);
                if (name.Length == _minLength) break;

                while (name.Length < nameLength) // should add a bit of variablity to this
                {
                    string token = name.Substring(name.Length - _scope, _scope);
                    char character = GetLetter(token);
                    if (character != '?')
                    {
                        //name += GetLetter(token);
                        name += character;
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
                    name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
                }
                //Console.WriteLine("\nLength: " + s.Length);
            }
            while (name.Length < _minLength);
            //while (_used.Contains(name) || name.Length < _minLength) ;
            //_used.Add(name);
            return name;
        }

        private char GetLetter(string value)
        {
            if (!_chains.ContainsKey(value)) return '?';
            List<char> letters = _chains[value];
            int randomNumber = _random.Next(letters.Count);
            return letters[randomNumber];
        }

        private void ConstructMatches()
        {
            // fix the parameter values
            //if (_scope < 1) _scope = 1;
            //if (minLength < 1) minLength = 1;

            //_minLength = minLength;
            
            // Build the chains
            foreach (string word in _words)
            {
                for (int letter = 0; letter < (word.Length - _scope); ++letter)
                {
                    string token = word.Substring(letter, _scope);
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
                    entry.Add(word[letter + _scope]);
                }
            }
        }
    }
}
