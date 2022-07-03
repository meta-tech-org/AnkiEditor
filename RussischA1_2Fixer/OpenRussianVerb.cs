using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RussischA1_2Fixer
{
    public class OpenRussianVerb
    {
        public int id { get; set; }
        public string accented { get; set; }
        public string bare => accented.Replace("'", "");
        //public string aspect { get; set; }
        //public string partner { get; set; }
        //public string imperative_sg { get; set; }
        //public string imperative_pl { get; set; }
        //public string past_m { get; set; }
        //public string past_f { get; set; }
        //public string past_n { get; set; }
        //public string past_pl { get; set; }
        public string sg1 { get; set; }
        public string sg2 { get; set; }
        public string sg3 { get; set; }
        public string pl1 { get; set; }
        public string pl2 { get; set; }
        public string pl3 { get; set; }
    }
}
