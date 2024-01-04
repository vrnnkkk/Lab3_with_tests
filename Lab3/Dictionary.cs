using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab3
{
    public class Dictionary
    {
        //совокупность объектов класса Word 
        private readonly List<Word> dictionary = new List<Word>();

        public IEnumerable<Word> Words
        {
            get
            {
                return dictionary;
            }

        }

        //метод добавления нового слова 
        public void AddWord(Word word)
        {
            dictionary.Add(word);
            dictionary.Sort((x, y) => x.Count.CompareTo(y.Count));
        }

        //метод поиска индекса переданного слова в словаре
        public Word GetWordByText(String word)
        {
            return dictionary.FirstOrDefault(w => w.CompareWord(word));
        }
    }
}
