using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Model
{
    public class Note
    {
        public string __type__ { get; set; }
        public List<string> fields { get; set; }
        public string guid { get; set; }
        public string note_model_uuid { get; set; }
        public List<string> tags { get; set; }
        public override string ToString()
        {
            return $"Note {fields.ElementAtOrDefault(0)}, Typ: {note_model_uuid}, Tags: {tags.Count}";
        }
    }
}
