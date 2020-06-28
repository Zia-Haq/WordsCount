using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace WordCountLib.Trie
{

    public class WordTrieNode : IComparable<WordTrieNode>
    {
        private WordTrieNode _parent = null;
        private char _char;

        internal int WordCount { get; set; }
        internal Dictionary<char, WordTrieNode> Children { get; } = null;
        public WordTrieNode()
        {
            Children = new Dictionary<char, WordTrieNode>();
        }
        public void AddChild(char ch)
        {
            if (!Children.ContainsKey(ch))
                Children.Add(ch, new WordTrieNode() { _parent = this, _char = ch });
        }

        public WordTrieNode GetChild(char ch)
        {
            return Children[ch];
        }

        public int CompareTo([AllowNull] WordTrieNode other)
        {
            return this.WordCount.CompareTo(other.WordCount);
        }

        public override int GetHashCode()
        {
            return this.WordCount.GetHashCode();
        }

        public override string ToString()
        {
            if (_parent == null)
                return "";
            else
                return _parent.ToString() + _char;
        }
    }

    public class WordTrie : IWordsProcessor
    {

        private readonly WordTrieNode _root = null;
        private List<WordTrieNode> _topXWordsList;
        private readonly int _maxTopXWords;
        private readonly IWordsReader _wordsReader;
        private void UpdateTopXWords(WordTrieNode node)
        {
            if (!_topXWordsList.Contains(node))
            {
                if (_topXWordsList.Count == _maxTopXWords)
                {
                    _topXWordsList.Sort();
                    var first = _topXWordsList.First();
                    if (first.WordCount < node.WordCount)
                        _topXWordsList[0] = node;
                }
                else
                    _topXWordsList.Add(node);
            }
        }


        public IEnumerable<(string word, int count)> TraverseTrie(Dictionary<char, WordTrieNode> trie)
        {
            foreach (var node in trie)
            {
                if (node.Value.WordCount > 0)
                    yield return (node.Value.ToString(), node.Value.WordCount);
                if (node.Value.Children != null && node.Value.Children.Count > 0)
                {
                    foreach (var x in TraverseTrie(node.Value.Children))
                        yield return x;
                }

            }
        }

        public WordTrie(int maxTopWords, IWordsReader reader)
        {
            _maxTopXWords = maxTopWords;
            _wordsReader = reader;
            _root = new WordTrieNode();

            BuildCollection();//For simplicity loading data here otherwise another class would handle this 
        }

        private void BuildCollection()
        {
            if (_topXWordsList == null)
            {
                _topXWordsList = new List<WordTrieNode>();
                foreach (var word in _wordsReader.GetWords())
                    AddWord(word);

            }
        }

        public void AddWord(string word)
        {
            var currentNode = _root;
            foreach (var ch in word)
            {
                currentNode.AddChild(ch);
                currentNode = currentNode.GetChild(ch);
            }

            currentNode.WordCount += 1;
            UpdateTopXWords(currentNode);
        }

        public IEnumerable<(string word, int count)> GetTopXWordsWithOccurrencesCount(int top)
        {

            if (_topXWordsList.Count > 0)
            {
                for (int i = _topXWordsList.Count - 1; i >= _topXWordsList.Count - top; i--)
                {
                    var item = _topXWordsList[i];
                    yield return (item.ToString(), item.WordCount);
                }
            }
        }

        public IEnumerable<(string word, int count)> GetWordsWithOccurrencesCount()
        {
            return TraverseTrie(_root.Children);
        }
    }
}
