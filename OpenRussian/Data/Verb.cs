using OpenRussian.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian.Data
{
    public class Verb : Word
    {
        public Verb(_Word word, _Verb verb, _Conjugation conjugation) : base(word)
        {
            Aspect = verb.aspect;
            Partner = verb.partner;
            ImperativeSg = verb.imperative_sg;
            ImperativePl = verb.imperative_pl;
            PastM = verb.past_m;
            PastF= verb.past_f;
            PastN= verb.past_n;
            PastPl= verb.past_pl;
            Sg1 = conjugation.sg1;
            Sg2 = conjugation.sg2;
            Sg3 = conjugation.sg3;
            Pl1 = conjugation.pl1;
            Pl2 = conjugation.pl2;
            Pl3 = conjugation.pl3;
        }
        public string Aspect { get; set; }
        public string Partner { get; set; }
        public string ImperativeSg { get; set; }
        public string ImperativePl { get; set; }
        public string PastM { get; set; }
        public string PastF { get; set; }
        public string PastN { get; set; }
        public string PastPl { get; set; }
        public string Sg1 { get; set; }
        public string Sg2 { get; set; }
        public string Sg3 { get; set; }
        public string Pl1 { get; set; }
        public string Pl2 { get; set; }
        public string Pl3 { get; set; }
    }
}
