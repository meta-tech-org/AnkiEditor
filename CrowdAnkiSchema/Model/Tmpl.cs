using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdAnkiSchema.Model
{
    public class Tmpl
    {
        public string afmt { get; set; }
        public string bafmt { get; set; }
        public string bfont { get; set; }
        public string bqfmt { get; set; }
        public int bsize { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public object did { get; set; } 
        public string name { get; set; }
        public int ord { get; set; }
        public string qfmt { get; set; }
        public bool? scratchPad { get; set; }
    }
}
