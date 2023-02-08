using OpenRussian.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Data
{
    public class Adjective : Word
    {
        public Adjective(_Word word, _Adjective adjective, _Declension? mDeclension, _Declension? fDeclension, _Declension? nDeclension, _Declension? plDeclension) : base(word)
        {
            Incomparable = adjective.incomparable;
            Comparative = adjective.comparative;
            Superlative= adjective.superlative;
            ShortM = adjective.short_f;
            ShortF = adjective?.short_f;
            ShortN= adjective?.short_n;
            ShortPl = adjective?.short_pl;

            NomM = mDeclension?.nom;
            GenM = mDeclension?.gen;
            DatM = mDeclension?.dat;
            AccM = mDeclension?.acc;
            InstM = mDeclension?.inst;
            PrepM = mDeclension?.prep;

            NomF = fDeclension?.nom;
            GenF = fDeclension?.gen;
            DatF = fDeclension?.dat;
            AccF = fDeclension?.acc;
            InstF = fDeclension?.inst;
            PrepF = fDeclension?.prep;

            NomN = nDeclension?.nom;
            GenN = nDeclension?.gen;
            DatN = nDeclension?.dat;
            AccN = nDeclension?.acc;
            InstN = nDeclension?.inst;
            PrepN = nDeclension?.prep;

            NomPl = plDeclension?.nom;
            GenPl = plDeclension?.gen;
            DatPl = plDeclension?.dat;
            AccPl = plDeclension?.acc;
            InstPl = plDeclension?.inst;
            PrepPl = plDeclension?.prep;

        }
        public bool? Incomparable { get; set; }
        public string? Comparative { get; set; }
        public string? Superlative { get; set; }
        public string? ShortM { get; set; }
        public string? ShortF { get; set; }
        public string? ShortN { get; set; }
        public string? ShortPl { get; set; }
        public string? NomM { get; set; }
        public string? GenM { get; set; }
        public string? DatM { get; set; }
        public string? AccM { get; set; }
        public string? InstM { get; set; }
        public string? PrepM { get; set; }
        public string? NomF { get; set; }
        public string? GenF { get; set; }
        public string? DatF { get; set; }
        public string? AccF { get; set; }
        public string? InstF { get; set; }
        public string? PrepF { get; set; }
        public string? NomN { get; set; }
        public string? GenN { get; set; }
        public string? DatN { get; set; }
        public string? AccN { get; set; }
        public string? InstN { get; set; }
        public string? PrepN { get; set; }
        public string? NomPl { get; set; }
        public string? GenPl { get; set; }
        public string? DatPl { get; set; }
        public string? AccPl { get; set; }
        public string? InstPl { get; set; }
        public string? PrepPl { get; set; }
    }
}
