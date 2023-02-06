using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    internal class _Declension
    {
        public int id { get; set; }
        public int word_id { get; set; }
        public string nom { get; set; }
        public string gen { get; set; }
        public string dat { get; set; }
        public string acc { get; set; }
        public string inst { get; set; }
        public string prep { get; set; }
    }
}
