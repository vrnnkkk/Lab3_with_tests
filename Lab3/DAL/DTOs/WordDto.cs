using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.DAL.DTOs
{
    public class WordDto
    {
        public string Prefix { get; set; }
        public string Root { get; set; }
        public string Suffix { get; set; }
        public string Ending { get; set; }

        public string FullWord
        {
            get; set;
        }

        public string SplittedWord { get; set; }
        [NonSerialized]
        public int Count;
    }
}
