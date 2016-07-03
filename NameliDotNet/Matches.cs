using System.Collections.Generic;

namespace NameliDotNet
{
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
}
