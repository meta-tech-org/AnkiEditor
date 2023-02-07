using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    public class _Noun
    {
        public int word_id { get; set; }
        public string? gender { get; set; }
        public string? partner { get; set; }
        public bool? animate { get; set; }
        public bool? indeclinable { get; set; }
        public bool? sg_only { get; set; }
        public bool? pl_only { get; set; }
        public int? decl_sg_id { get; set; }
        public int? decl_pl_id { get; set; }
    }
}
