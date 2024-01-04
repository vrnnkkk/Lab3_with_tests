using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.DAL.DTOs
{
    public class DictionaryDto
    {
        public List<WordDto> Words
        {
            get; set;
        }

        public void AddWord(WordDto word)
        {
            Words.Add(word);
            Words.Sort((x, y) => x.Count.CompareTo(y.Count));
        }
    }
}
