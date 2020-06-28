using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace WordCountLib
{
    public class WordsProcessor : IWordsProcessor
    {
        private Dictionary<string, int> _dWords;
        private readonly IWordsReader _wordsReader;
        public WordsProcessor(IWordsReader reader)
        {
            _wordsReader = reader;
             BuildCollection();//For simplicity loading data here otherwise another class would handle this 
        }

        private void BuildCollection()
        {
            if (_dWords == null)
            {
                _dWords = new Dictionary<string, int>();
                foreach (var word in _wordsReader.GetWords())
                {
                    if (_dWords.ContainsKey(word))
                        _dWords[word] += 1;
                    else
                        _dWords[word] = 1;
                }
            }
        }

        public IEnumerable<(string word, int count)> GetWordsWithOccurrencesCount()
        {
            foreach (var item in _dWords)
            {
                yield return (item.Key, item.Value);
            }
        }

        public IEnumerable<(string word, int count)> GetTopXWordsWithOccurrencesCount(int top)
        {
            var topWords = _dWords.OrderByDescending(x => x.Value).Take(top);
            foreach (var item in topWords)
            {
                yield return (item.Key, item.Value);
            }

        }

    }
}
