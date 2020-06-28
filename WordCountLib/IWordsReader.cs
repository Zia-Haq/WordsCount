using System.Collections.Generic;

namespace WordCountLib
{
    public interface IWordsReader
    {
        IEnumerable<string> GetWords();
    }
}