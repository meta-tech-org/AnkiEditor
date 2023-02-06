using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    internal class _Adjective
    {
        public int word_id { get; set; }
        public int? incomparable { get; set; }
        public string? comparative { get; set; }
        public string? superlative { get; set; }
        public string? short_m { get; set; }
        public string? short_f { get; set; }
        public string? short_n { get; set; }
        public string? short_pl { get; set; }
        public int decl_m_id { get; set; }
        public int decl_f_id { get; set; }
        public int decl_n_id { get; set; }
        public int decl_pl_id { get; set; }
    }
}
