using OpenRussian.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Data
{
    public class Noun : Word
    {
        public Noun(_Word word, _Noun noun, _Declension? singularDeclension, _Declension? pluralDeclension) : base(word)
        {
            Gender = noun.gender;
            Partner = noun.partner;
            Animate = noun.animate;
            Indeclinable = noun.indeclinable;
            SingularOnly = noun.sg_only;
            PluralOnly = noun.pl_only;
            NomSg = singularDeclension?.nom;
            GenSg = singularDeclension?.gen;
            DatSg = singularDeclension?.dat;
            AccSg= singularDeclension?.acc;
            InstSg= singularDeclension?.inst;
            PrepSg = singularDeclension?.prep;
            NomPl = pluralDeclension?.nom;
            GenPl= pluralDeclension?.gen;
            DatPl = pluralDeclension?.dat;
            AccPl = pluralDeclension?.acc;
            InstPl = pluralDeclension?.inst;
            PrepPl = pluralDeclension?.prep;
        }

        public string? Gender { get; set; }
        public string? Partner { get; set; }
        public bool? Animate { get; set; }
        public bool? Indeclinable { get; set; }
        public bool? SingularOnly { get; set; }
        public bool? PluralOnly { get; set; }
        public string? NomSg { get; set; }
        public string? GenSg { get; set; }
        public string? DatSg { get; set; }
        public string? AccSg { get; set; }
        public string? InstSg { get; set; }
        public string? PrepSg { get; set; }
        public string? NomPl { get; set; }
        public string? GenPl { get; set; }
        public string? DatPl { get; set; }
        public string? AccPl { get; set; }
        public string? InstPl { get; set; }
        public string? PrepPl { get; set; }
    }
}
