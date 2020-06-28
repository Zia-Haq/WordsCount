using System.Collections.Generic;

namespace WordCountLib
{
   public interface IWordsProcessor
    {
        IEnumerable<(string word, int count)> GetTopXWordsWithOccurrencesCount(int top);
        IEnumerable<(string word, int count)> GetWordsWithOccurrencesCount();
    }
}