using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    internal class _Word
    {
        public int id { get; set; }
        public int? position { get; set; }
        public string bare { get; set; }
        public string accented { get; set; }
        public int? derived_from_word_id { get; set; }
        public int? rank { get; set; }
        public bool? disabled { get; set; }
        public string? audio { get; set; }
        public string? usage_en { get; set; }
        public string? usage_de { get; set; }
        public string? number_value { get; set; }
        public string? type { get; set; }
        public string level { get; set; }
    }
}
