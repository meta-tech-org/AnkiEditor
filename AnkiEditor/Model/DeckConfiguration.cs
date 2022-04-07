using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Model
{
    public class DeckConfiguration
    {
        public string __type__ { get; set; }
        public bool autoplay { get; set; }
        public string crowdanki_uuid { get; set; }
        public bool dyn { get; set; }
        public int interdayLearningMix { get; set; }
        public Lapse lapse { get; set; }
        public int maxTaken { get; set; }
        public string name { get; set; }
        public New @new { get; set; }
        public int newGatherPriority { get; set; }
        public int newMix { get; set; }
        public int newPerDayMinimum { get; set; }
        public int newSortOrder { get; set; }
        public bool replayq { get; set; }
        public Rev rev { get; set; }
        public int reviewOrder { get; set; }
        public int timer { get; set; }
        public override string ToString()
        {
            return $"{name} - {crowdanki_uuid} [{@new.perDay}, {@rev.perDay}]";
        }
    }
}
