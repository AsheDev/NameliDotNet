using System;
using System.Linq;
using System.Text;
using NameliDotNet.Interfaces;
using System.Collections.Generic;

namespace NameliDotNet.Strategies
{
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

        public void SetSourceText(IList<string> source)
        {
            if (ReferenceEquals(source, null) || !source.Any()) throw new ArgumentNullException("A list of words or names must be provided to generate a proper result.");
            _words = source;
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
