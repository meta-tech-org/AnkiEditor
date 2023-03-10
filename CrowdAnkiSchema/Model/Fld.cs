using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdAnkiSchema.Model
{
    public class Fld
    {
        public bool? collapsed { get; set; }
        public string description { get; set; }
        public string font { get; set; }
        public string name { get; set; }
        public int ord { get; set; }
        public bool? plainText { get; set; }
        public bool rtl { get; set; }
        public int size { get; set; }
        public bool sticky { get; set; }
    }
}
