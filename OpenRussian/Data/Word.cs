using OpenRussian.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Data
{
    public class Word
    {
        public Word(_Word word)
        {
            string? wordType = word.type?[0].ToString().ToUpper() + word.type?.Substring(1);

            Id = word.id;
            Bare = word.bare;
            Accented = word.accented;
            Audio = word.audio;
            Type = (WordType)Enum.Parse(typeof(WordType), wordType);
            Level = word.level;
        }
        public int Id { get; set; }
        public string Bare { get; set; }
        public string Accented { get; set; }
        public string AccentedAnki => "TODO";
        public string? Audio { get; set; }
        public string Usage { get; set; }
        public WordType Type { get; set; }
        public string Level { get; set; }
    }

    public enum WordType
    {
        Other = 0,
        Noun = 1,
        Verb = 2,
        Adjective = 3
    }
}
