using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountLib;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NSubstitute;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace WordCountLib.Tests
{
    [TestClass()]
    public class WordsProcessorTests
    {
      
        private readonly IWordsProcessor _wordsProcessor;
        public WordsProcessorTests()
        {
            IWordsReader wordsReader = Substitute.For<IWordsReader>();
            var words = new Regex(@"\W+", RegexOptions.Compiled).Split("This is a test string. String which contains some words. String starts with some new words. Words made of words. Words containing words".ToLowerInvariant()).ToList();
            wordsReader.GetWords().Returns(words);
            _wordsProcessor = new WordsProcessor(wordsReader);



        }


   
        [TestMethod()]
        public void GetWordsWithOccurrencesCountTest()
        {
            var wordsWithCouts = _wordsProcessor.GetWordsWithOccurrencesCount();
           var words_word= wordsWithCouts.First(w => w.word == "words");
            words_word.count.Should().Be(6);
            var string_word = wordsWithCouts.First(w => w.word == "string");
            string_word.count.Should().Be(3);
            var is_word = wordsWithCouts.First(w => w.word == "is");
            is_word.count.Should().Be(1);
        }

        [TestMethod()]
        public void GetTopXWordsWithOccurrencesCountTest()
        {
            var wordsWithCouts = _wordsProcessor.GetTopXWordsWithOccurrencesCount(2).ToList();
            var firstItem = wordsWithCouts[0];
            firstItem.count.Should().Be(6);
            firstItem.word.Should().Be("words");
            var secondItem = wordsWithCouts[1];
            secondItem.count.Should().Be(3);
            secondItem.word.Should().Be("string");
            wordsWithCouts.Count.Should().Be(2);
        }
    }
}