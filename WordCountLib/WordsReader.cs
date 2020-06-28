using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WordCountLib
{
    public class WordsReader : IWordsReader
    {
        private readonly Regex _regex;
        private string _fileToRead;

        public WordsReader(string regex, string fileToRead)//Add datasource, in our case file
        {
            _regex = new Regex(regex, RegexOptions.Compiled);
            _fileToRead = fileToRead;
        }

        public IEnumerable<string> GetWords()
        {
            var line = string.Empty;
            using StreamReader reader = new StreamReader(_fileToRead);
            while ((line = reader.ReadLine()) != null)
            {
                var words = _regex.Split(line.ToLowerInvariant());
                foreach (var word in words)
                {

                    if (!string.IsNullOrEmpty(word))
                        yield return word;
                }
            }
        }
    }
}
