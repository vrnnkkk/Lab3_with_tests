using Lab3;
using Lab3.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class WordTests
    {
        [TestMethod]
        public void HasTheSameRoot_ReturnsTrue()
        {
            var word1 = new Word("pre", "root", "suf", "ending");
            var word2 = new Word("pre", "root", "suffix", "end");

            var result = word1.HasTheSameRoot(word2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasTheSameRoot_ReturnsFalse()
        {
            var word1 = new Word("pre", "root1", "suf", "ending");
            var word2 = new Word("pre", "root2", "suffix", "end");

            var result = word1.HasTheSameRoot(word2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CompareWord_ReturnsTrue()
        {
            var word = new Word("pre", "root", "suf", "ending");

            var result = word.CompareWord("preRootsufending");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CompareWord_ReturnsFalse()
        {
            var word = new Word("pre", "root", "suf", "ending");

            var result = word.CompareWord("invalidWord");

            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class DictionaryTests
    {
        [TestMethod]
        public void AddWord_AddsToDictionaryAndSortsByCount()
        {
            var dictionary = new Dictionary();
            var word1 = new Word("pre", "root1", "suffix", "ending");
            var word2 = new Word("pre", "root2", "suffix", "");

            dictionary.AddWord(word1);
            dictionary.AddWord(word2);

            Assert.AreEqual(2, dictionary.Words.Count());
            Assert.AreEqual("pre-root2-suffix", dictionary.Words.First().SplittedWord);
        }

        [TestMethod]
        public void GetWordByText_ReturnsCorrectWord()
        {
            var dictionary = new Dictionary();
            var word1 = new Word("pre", "root1", "suffix", "ending");
            var word2 = new Word("pre", "root2", "suffix", "");
            dictionary.AddWord(word1);
            dictionary.AddWord(word2);

            var result = dictionary.GetWordByText("pre"+"root2"+"suffix");

            Assert.IsNotNull(result);
            Assert.AreEqual("pre"+"root2"+"suffix", result.FullWord);
        }
    }

    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void SaveToJson_CreatesJsonFile()
        {
            var dictionary = new Dictionary();
            var word1 = new Word("pre", "root1", "suf", "ending");
            var word2 = new Word("pre", "root2", "suffix", "end");
            dictionary.AddWord(word1);
            dictionary.AddWord(word2);

            Serializer.SaveToJson("test.json", dictionary);
            Serializer.FromJson("test.json");
        }

        [TestMethod]
        public void SaveToXml_CreatesXmlFile()
        {
            var dictionary = new Dictionary();
            var word1 = new Word("pre", "root1", "suf", "ending");
            var word2 = new Word("pre", "root2", "suffix", "end");
            dictionary.AddWord(word1);
            dictionary.AddWord(word2);

            Serializer.SaveToXml("test.xml", dictionary);
            Serializer.FromXml("test.xml", new Dictionary());
        }

        [TestMethod]
        public void SaveToBD_SavesToDatabase()
        {
            var dictionary = new Dictionary();
            var word1 = new Word("pre", "root1", "suf", "ending");
            var word2 = new Word("pre", "root2", "suffix", "end");
            dictionary.AddWord(word1);
            dictionary.AddWord(word2);

            Serializer.SaveToBD(dictionary);
            Serializer.FromBD();
        }
    }
}