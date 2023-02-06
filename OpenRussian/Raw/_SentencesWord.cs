using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    internal class _SentencesWord
    {
        public int id { get; set; }
        public int sentence_id { get; set; }
        public int word_id { get; set; }
        public int start { get; set; }
        public int length { get; set; }
    }
}
