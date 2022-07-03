using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdAnkiSchema.Model
{
    public class Lapse
    {
        public List<double> delays { get; set; }
        public int leechAction { get; set; }
        public int leechFails { get; set; }
        public int minInt { get; set; }
        public double mult { get; set; }
    }
}
