using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    internal class _Translation
    {
        public int id { get; set; }
        public string lang { get; set; }
        public int word_id { get; set; }
        public int position { get; set; }
        public string tl { get; set; }
        public string example_ru { get; set; }
        public string example_tl { get; set; }
        public string info { get; set; }
    }
}
