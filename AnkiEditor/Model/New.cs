using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Model
{
    public class New
    {
        public bool bury { get; set; }
        public List<double> delays { get; set; }
        public int initialFactor { get; set; }
        public List<int> ints { get; set; }
        public int order { get; set; }
        public int perDay { get; set; }
    }
}
