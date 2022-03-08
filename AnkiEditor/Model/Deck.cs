using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Model
{
    public class Deck
    {
        public string __type__ { get; set; }
        public List<Deck> children { get; set; }
        public string crowdanki_uuid { get; set; }
        public string deck_config_uuid { get; set; }
        public List<DeckConfiguration> deck_configurations { get; set; }
        public string desc { get; set; }
        public int dyn { get; set; }
        public int extendNew { get; set; }
        public int extendRev { get; set; }
        public List<string> media_files { get; set; }
        public long mid { get; set; }
        public string name { get; set; }
        public List<NoteModel> note_models { get; set; }
        public List<Note> notes { get; set; }

        public override string ToString()
        {
            return $"Deck {name}, children: {children.Count}, notes: {notes.Count}";
        }

    }
}
