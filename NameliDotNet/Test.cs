using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            for(int n = 0; n < _words.Count - 2; ++n)
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
            for(int n = 0; n < length; ++n)
            {
                generated.Add(initial);
                initial = subsequent;
                //subsequent = _matches.ContainsKey("you");
            }
            generated.Add(subsequent);
            return "TEST COMPLETE";
        }
    }
}
