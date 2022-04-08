using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Model
{
    public class NoteModel
    {
        public string __type__ { get; set; }
        public string crowdanki_uuid { get; set; }
        public string css { get; set; }
        public List<Fld> flds { get; set; }
        public string latexPost { get; set; }
        public string latexPre { get; set; }
        public bool latexsvg { get; set; }
        public string name { get; set; }
        public List<List<object>> req { get; set; }
        public int sortf { get; set; }
        public List<string> tags { get; set; }
        public List<Tmpl> tmpls { get; set; }
        public int type { get; set; }
        public override string ToString()
        {
            return $"{name}";
        }
    }
}
