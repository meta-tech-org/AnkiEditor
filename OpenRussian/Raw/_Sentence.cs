using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Raw
{
    internal class _Sentence
    {
        public int id { get; set; }
        public string ru { get; set; }
        public string lang { get; set; }
        public string tl { get; set; }
        public int tatoeba_key { get; set; }
        public string source_url { get; set; }
        public int disabled { get; set; }
        public int locked { get; set; }
        public string level { get; set; }
        public string audio_url { get; set; }
        public string audio_attribution_url { get; set; }
        public string audio_attribution_name { get; set; }
    }
}
