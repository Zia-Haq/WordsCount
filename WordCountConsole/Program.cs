using System;
using WordCountLib;
using WordCountLib.Trie;

namespace WordCountConsole
{
    class Program
    {
        static void Main(string[] args)
        {


            //for simplicity not using appsettings and DI container, otherwise parameters should come from appsettings file 
            //and also instances be addeded to built in DI container like
            //services.AddSingleton<IWordsReader, WordsReader>();

            IWordsReader reader = new WordsReader(@"\W+", "sampleFile.txt");//can use a more powerful regex to ignore numbers etc

            //Dictionary based solution **********************************
            IWordsProcessor wordsProcessor = new WordsProcessor(reader);
            //Iterate and disply all the words and their count
            foreach (var word in wordsProcessor.GetWordsWithOccurrencesCount())
              Console.WriteLine($"{word.word}={word.count} times found");
            //Diplay top 10 words with count
            Console.WriteLine("*******************TOP 10 WORDS***********************");
            foreach (var word in wordsProcessor.GetTopXWordsWithOccurrencesCount(10))
                Console.WriteLine($"{word.word}={word.count} times found");
            //***************************************************************
            Console.WriteLine("*********************************************************");
            //Trie based solution ***************
            IWordsProcessor trieWordsProcessor = new WordTrie(100, reader); //Capacity of Top x words list agains should come from appsettings
            //Iterate and disply all the words and their ,,,,,,,,count
            foreach (var word in trieWordsProcessor.GetWordsWithOccurrencesCount())
               Console.WriteLine($"{word.word}={word.count} times found");
            //Diplay top 10 words with count
            Console.WriteLine("*******************TOP 10 WORDS***********************");
            foreach (var word in trieWordsProcessor.GetTopXWordsWithOccurrencesCount(10))
                Console.WriteLine($"{word.word}={word.count} times found");
            //*************************************************************

            Console.ReadLine();

        }
    }
}
