using CsvHelper;
using CsvHelper.Configuration;
using OpenRussian.Raw;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRussian
{
    /// <summary>
    /// Audio: https://api.openrussian.org/read/ru/%D0%B4%D0%B8%D1%81%D0%BA%D0%BE%D0%B2%D0%BE'%D0%B4 (urldecode to get the word)
    /// </summary>
    public class Loader
    {
        static List<(string, Type)> types = new List<(string, Type)>()
        {
            ("adjectives.csv",typeof(_Adjective)),
            ("categories_words2.csv",typeof(_CategoriesWord2)),
            ("conjugations.csv",typeof(_Conjugation)),
            ("declensions.csv",typeof(_Declension)),
            ("expressions_words.csv",typeof(_ExpressionWord)),
            ("nouns.csv",typeof(_Noun)),
            ("sentences.csv",typeof(_Sentence)),
            ("sentences_words.csv",typeof(_SentencesWord)),
            ("translations.csv",typeof(_Translation)),
            ("verbs.csv",typeof(_Verb)),
            ("words.csv",typeof(_Word)),
            ("words_rels.csv",typeof(_WordRel)),
        };
        public Loader()
        {
            string pathAdjectives = "adjectives.csv";
            List<_Adjective> adjectives = GetList<_Adjective>(pathAdjectives);

            string pathCategoriesWords2 = "categories_words2.csv";
            List<_CategoriesWord2> categoriesWords2 = GetList<_CategoriesWord2>(pathCategoriesWords2);

            string pathConjugations = "conjugations.csv";
            List<_Conjugation> conjugations = GetList<_Conjugation>(pathConjugations);

            string pathDeclensions = "declensions.csv";
            List<_Declension> declensions = GetList<_Declension>(pathDeclensions);

            string pathExpressionsWords = "expressions_words.csv";
            List<_ExpressionWord> expressionsWords = GetList<_ExpressionWord>(pathExpressionsWords);

            string pathNouns = "nouns.csv";
            List<_Noun> nouns = GetList<_Noun>(pathNouns);

            string pathSentences = "sentences.csv";
            List<_Sentence> sentences = GetList<_Sentence>(pathSentences);

            string pathSentencesWords = "sentences_words.csv";
            List<_SentencesWord> sentencesWords = GetList<_SentencesWord>(pathSentencesWords);

            string pathTranslations = "translations.csv";
            List<_Translation> translations = GetList<_Translation>(pathTranslations);

            string pathVerbs = "verbs.csv";
            List<_Verb> verbs = GetList<_Verb>(pathVerbs);

            string pathWords = "words.csv";
            List<_Word> words = GetList<_Word>(pathWords);

            string pathWordsRels = "words_rels.csv";
            List<_WordRel> wordsRels = GetList<_WordRel>(pathWordsRels);
        }

        private static List<T> GetList<T>(string path)
        {
            path = @"openrussian-csv\" + path;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                BadDataFound = null,
                MissingFieldFound = null,
            };

            List<T> list = new List<T>();
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                list = csv.GetRecords<T>().ToList();
            }
            return list;
        }
    }
}
