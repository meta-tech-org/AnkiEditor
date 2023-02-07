using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    public class _Verb
    {
        public int word_id { get; set; }
        public string aspect { get; set; }
        public string partner { get; set; }
        public string imperative_sg { get; set; }
        public string imperative_pl { get; set; }
        public string past_m { get; set; }
        public string past_f { get; set; }
        public string past_n { get; set; }
        public string past_pl { get; set; }
        public int presfut_conj_id { get; set; }
    }
}
