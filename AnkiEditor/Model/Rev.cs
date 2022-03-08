using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Model
{
    public class Rev
    {
        public bool bury { get; set; }
        public double ease4 { get; set; }
        public double hardFactor { get; set; }
        public double ivlFct { get; set; }
        public int maxIvl { get; set; }
        public int perDay { get; set; }
    }
}
