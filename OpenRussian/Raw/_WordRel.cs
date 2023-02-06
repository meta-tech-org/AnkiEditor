using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    internal class _WordRel
    {
        public int id { get; set; }
        public int word_id { get; set; }
        public int rel_word_id { get; set; }
        public string relation { get; set; }
    }
}
