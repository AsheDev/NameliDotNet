using System.Collections.Generic;

namespace NameliDotNet.Interfaces
{
    public interface IStrategy
    {
        void SetSourceText(IList<string> source);
        //IList<Matches> GenerateMatches();
        string GenerateText();
        bool CompareLists(IList<string> newList);
    }
}
