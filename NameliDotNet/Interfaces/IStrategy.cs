using System.Collections.Generic;

namespace NameliDotNet.Interfaces
{
    public interface IStrategy
    {
        void SetSourceText(IList<string> source);
        string GenerateText();
    }
}
