using CrowdAnkiSchema.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;

namespace AnkiEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            //AIDBMBSAddExcerciseSubdecks(@"C:\Users\juliu\source\repos\Anki Exports\Architecture_and_Implementation_of_Database_Management_Systems\deck.json");

            //FixJapaneseDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Japanisch,_bitte!_Neu\deck.json");
            //FixRussianDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Russisch_-_Olga_Schöne_-_Ein_guter_Anfang\deck.json");
            //FixJapaneseFrequencyDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Japanese_Frequency_6000");
            //FixDeepLearningDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Deep_Learning_(Goodfellow,_Bengio,_Courville)\deck.json");
            //FixDIPDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Digital_Image_Processing_(Gonzalez,_Woods)\deck.json");

            List<(string, string)> testValues = new List<(string, string)>();
            //testValues.Add(("日本語を勉強しています。", "にほんご を べんきょう しています。"));
            //testValues.Add(("お茶を飲みます。", "おちゃ を のみます。"));
            //testValues.Add(("今日は晴れです。", "きょう は はれです。"));
            testValues.Add(("日本語を勉強しています。", "にほんごをべんきょうしています。"));
            testValues.Add(("お茶を飲みます。", "おちゃをのみます。"));
            testValues.Add(("今日は晴れです。", "きょうははれです。"));
            foreach (var testValue in testValues)
            {
                var result = AddFurigana(testValue.Item1, testValue.Item2);
            }
        }

        private static bool IsKanji(string str)
        {
            // Create a lookup table of all hiragana/katakana characters
            string kana = "あいうえおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもやゆよらりるれろわをん";

            // Check if the input string contains any characters that are not in the lookup table
            foreach (char c in str)
            {
                if (!kana.Contains(c))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsKanji(char c)
        {
            return IsKanji(c.ToString());
        }

        public static string AddFurigana(string kanjiSentence, string kanaSentence)
        {
            // Initialize an empty result string
            string result = "";

            // Initialize indices for the kanji sentence and kana sentence
            int kanjiIndex = 0;
            int kanaIndex = 0;

            // Loop until one of the indices reaches the end of the corresponding sentence
            while (kanjiIndex < kanjiSentence.Length && kanaIndex < kanaSentence.Length)
            {
                // If the current character of the kanji sentence is a kanji, add it to the result string
                if (IsKanji(kanjiSentence[kanjiIndex]))
                {
                    result += kanjiSentence[kanjiIndex];

                    // Find the next kanji character in the kanji sentence
                    int nextKanjiIndex = kanjiSentence.IndexOfAny(new char[] { '\u4E00', '\u4E8C', '\u4E09', '\u56DB', '\u4E94', '\u516D', '\u4E03', '\u516B', '\u4E5D', '\u5341' }, kanjiIndex + 1);
                    if (nextKanjiIndex == -1)
                    {
                        nextKanjiIndex = kanjiSentence.Length;
                    }

                    // Find the corresponding kana characters in the kana sentence
                    string kana = kanaSentence.Substring(kanaIndex, nextKanjiIndex - kanjiIndex);

                    // Add the kana characters in brackets after the kanji character
                    result += "[" + kana + "]";

                    // Update the indices
                    kanjiIndex = nextKanjiIndex;
                    kanaIndex += kana.Length;
                }
                // If the current character of the kanji sentence is not a kanji, add it to the result string without any changes
                else
                {
                    result += kanjiSentence[kanjiIndex];
                    kanjiIndex++;
                    kanaIndex++;
                }
            }

            return result;
        }


        private static void FixDIPDeckStructure(string deckPath)
        {
            Deck root = Deck.LoadFromFile(deckPath);
            var chapterConfig = root.GetDeckConfigurationByTitle("DIP::Chapter");
            var chapters = File.ReadAllLines(@"I:\Google Drive\Documents\Lernen\digital image processing chapters.txt");
            int subCounter = 1;
            List<DeepLearningChapter> deepLearningChapters = new List<DeepLearningChapter>();
            DeepLearningChapter currentChapter = null;
            foreach (var line in chapters)
            {
                bool isChapter = int.TryParse(line.Split(' ')[0], out _);
                string clean = line;

                if (isChapter)
                {
                    subCounter = 1;
                    if(line.Split(' ')[0].Length == 1)
                    {
                        clean = "0" + line;
                    }
                    currentChapter = new DeepLearningChapter
                    {
                        Name = clean,
                        SubChapters = new List<DeepLearningChapter>()
                    };
                    deepLearningChapters.Add(currentChapter);
                }
                else
                {
                    currentChapter.SubChapters.Add(new DeepLearningChapter
                    {
                        Name = $"{subCounter.ToString("00")} {clean}"
                    }) ;
                    subCounter++;
                }
            }
            foreach (var chapter in deepLearningChapters)
            {
                var ankiChapter = Deck.CreateEmptyDeck(chapter.Name, chapterConfig.crowdanki_uuid);
                root.children.Add(ankiChapter);
                foreach (var subChapter in chapter.SubChapters)
                {
                    var ankiSubChapter = Deck.CreateEmptyDeck(subChapter.Name, chapterConfig.crowdanki_uuid);
                    ankiChapter.children.Add(ankiSubChapter);
                }
            }
            root.WriteToFile(deckPath);
        }

            private static void FixDeepLearningDeckStructure(string deckPath)
        {
            Deck root = Deck.LoadFromFile(deckPath);
            var chapterConfig = root.GetDeckConfigurationByTitle("DL::Chapter");
            var chapters = File.ReadAllLines(@"I:\Google Drive\Documents\Lernen\deep learning chapters.txt");
            List<DeepLearningChapter> deepLearningChapters = new List<DeepLearningChapter>();
            DeepLearningChapter currentChapter = null;
            DeepLearningChapter currentSubChapter = null;
            foreach (var chapter in chapters)
            {
                var clean = chapter.Trim();
                var number = clean.Split(' ')[0];
                var numberParts = number.Split(".");
                string realNumber = "";
                if (numberParts[1] == "0" && numberParts[2] == "0")
                {
                    currentChapter = new DeepLearningChapter
                    {
                        Name = clean,
                        SubChapters = new List<DeepLearningChapter>()
                    };
                    deepLearningChapters.Add(currentChapter);
                }
                else if (numberParts[1] != "0" && numberParts[2] == "0")
                {
                    currentSubChapter = new DeepLearningChapter
                    {
                        Name = clean,
                        SubChapters = new List<DeepLearningChapter>()
                    };
                    currentChapter.SubChapters.Add(currentSubChapter);
                }
                else
                {
                    currentSubChapter.SubChapters.Add(new DeepLearningChapter
                    {
                        Name = clean
                    });
                }
            }

            foreach(var chapter in deepLearningChapters)
            {
                var ankiChapter = Deck.CreateEmptyDeck(chapter.InferredName, chapterConfig.crowdanki_uuid);
                root.children.Add(ankiChapter);
                foreach(var subChapter in chapter.SubChapters)
                {
                    var ankiSubChapter = Deck.CreateEmptyDeck(subChapter.InferredName, chapterConfig.crowdanki_uuid);
                    ankiChapter.children.Add(ankiSubChapter);
                    foreach (var subSubChapter in subChapter.SubChapters)
                    {
                        var ankiSubSubChapter = Deck.CreateEmptyDeck(subSubChapter.InferredName, chapterConfig.crowdanki_uuid);
                        ankiSubChapter.children.Add(ankiSubSubChapter);
                    }
                }
            }
            root.WriteToFile(deckPath);
        }

        private class DeepLearningChapter
        {
            public string Name { get; set; }
            public string InferredName
            {
                get
                {
                    var title = Name.Substring(Name.IndexOf(' '));
                    var parts = Name.Split(" ")[0].Split(".");
                    string number1 = parts[0].Length == 1 ? "0" + parts[0] : parts[0];
                    string number2 = parts[1].Length == 1 ? "0" + parts[1] : parts[1];
                    string number3 = parts[2].Length == 1 ? "0" + parts[2] : parts[2];
                    return $"{number1}.{number2}.{number3} {title}".Replace(".00","");
                }
            }
            public List<DeepLearningChapter> SubChapters { get; set; }
            public override string ToString()
            {
                return InferredName;
            }
        }

        private static void FixJapaneseFrequencyDeckStructure(string deckPath)
        {
            //var iknow = File.ReadAllLines("C:\\Users\\juliu\\source\\repos\\IKnowJPDownloader\\IKnowJPDownloader\\IKnowJPDownloader\\bin\\Debug\\net6.0\\iknow.csv").Select(x => x.Split(";"));
            //Fix deck structure
            Deck root = Deck.LoadFromFile(Path.Combine(deckPath, "deck.json"));
            var baseConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000");
            var levelConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000::Level");
            var vocabConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000::Vocabulary");
            var kanjiConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000::Kanji");

            root.children = root.children.Where(c => !c.name.Contains("Level")).ToList();


            // Export
            root.WriteToFile(Path.Combine(deckPath, "deck.json"));
        }

        private static void FixRussianDeckStructure(string deckPath)
        {
            Deck root = Deck.LoadFromFile(deckPath);
            var chapterConfig = root.GetDeckConfigurationByTitle("Russisch::Kapitel");
            var vocabConfig = root.GetDeckConfigurationByTitle("Russisch::Vokabeln");
            var grammarConfig = root.GetDeckConfigurationByTitle("Russisch::Grammatik");
            var sentencesConfig = root.GetDeckConfigurationByTitle("Russisch::Sätze und Phrasen");
            var tasksConfig = root.GetDeckConfigurationByTitle("Russisch::Aufgaben");
            foreach (var level in root.children)
            {
                //if(level.name == "B1")
                //{
                //    for (int i = 1; i <= 6; i++)
                //    {
                //        level.children.Add(Deck.CreateEmptyDeck("Kapitel 0" + i, chapterConfig.crowdanki_uuid));
                //    }
                //}
                foreach (var chapter in level.children)
                {
                    chapter.SetDeckConfiguration(chapterConfig);
                    if (!chapter.children.Any(c => c.name == "01 Vokabeln"))
                    {
                        chapter.children.Add(Deck.CreateEmptyDeck("01 Vokabeln", vocabConfig.crowdanki_uuid));
                    }
                    if (!chapter.children.Any(c => c.name == "02 Grammatik"))
                    {
                        chapter.children.Add(Deck.CreateEmptyDeck("02 Grammatik", grammarConfig.crowdanki_uuid));
                    }
                    if (!chapter.children.Any(c => c.name == "03 Sätze und Phrasen"))
                    {
                        chapter.children.Add(Deck.CreateEmptyDeck("03 Sätze und Phrasen", sentencesConfig.crowdanki_uuid));
                    }
                    if (!chapter.children.Any(c => c.name == "04 Aufgaben"))
                    {
                        chapter.children.Add(Deck.CreateEmptyDeck("04 Aufgaben", tasksConfig.crowdanki_uuid));
                    }
                }
            }

            // Export
            root.WriteToFile(deckPath);
        }
        private static void FixJapaneseDeckStructure(string deckPath)
        {
            Deck root = Deck.LoadFromFile(deckPath);
            var chapterConfig = root.GetDeckConfigurationByTitle("Japanisch::Kapitel");
            var kanaConfig = root.GetDeckConfigurationByTitle("Japanisch::Kana");
            var vocabConfig = root.GetDeckConfigurationByTitle("Japanisch::Vokabeln");
            var grammarConfig = root.GetDeckConfigurationByTitle("Japanisch::Grammatik");
            var sentencesConfig = root.GetDeckConfigurationByTitle("Japanisch::Sätze und Phrasen");
            var triviaConfig = root.GetDeckConfigurationByTitle("Japanisch::Trivia");
            var tasksConfig = root.GetDeckConfigurationByTitle("Japanisch::Aufgaben");
            foreach (var level in root.children)
            {
                foreach (var chapter in level.children)
                {
                    chapter.SetDeckConfiguration(chapterConfig);



                    foreach (var letter in chapter.children)
                    {
                        letter.SetDeckConfiguration(chapterConfig);
                        if (!letter.children.Any(c => c.name == "01 Kana"))
                        {
                            letter.children.Add(Deck.CreateEmptyDeck("01 Kana", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                        }
                        if (!letter.children.Any(c => c.name == "02 Vokabeln"))
                        {
                            letter.children.Add(Deck.CreateEmptyDeck("02 Vokabeln", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                        }
                        if (!letter.children.Any(c => c.name == "03 Grammatik"))
                        {
                            letter.children.Add(Deck.CreateEmptyDeck("03 Grammatik", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                        }
                        if (!letter.children.Any(c => c.name == "04 Sätze und Phrasen"))
                        {
                            letter.children.Add(Deck.CreateEmptyDeck("04 Sätze und Phrasen", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                        }
                        if (!letter.children.Any(c => c.name == "05 Trivia"))
                        {
                            letter.children.Add(Deck.CreateEmptyDeck("05 Trivia", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                        }
                        if (!letter.children.Any(c => c.name == "06 Aufgaben"))
                        {
                            letter.children.Add(Deck.CreateEmptyDeck("06 Aufgaben", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                        }
                        letter.GetSubDeckByTitle("01 Kana").SetDeckConfiguration(kanaConfig);
                        letter.GetSubDeckByTitle("02 Vokabeln").SetDeckConfiguration(vocabConfig);
                        letter.GetSubDeckByTitle("03 Grammatik").SetDeckConfiguration(grammarConfig);
                        letter.GetSubDeckByTitle("04 Sätze und Phrasen").SetDeckConfiguration(sentencesConfig);
                        letter.GetSubDeckByTitle("05 Trivia").SetDeckConfiguration(triviaConfig);
                        letter.GetSubDeckByTitle("06 Aufgaben").SetDeckConfiguration(tasksConfig);
                        //TODO: move file list from parent deck to new child if applicable
                        //TODO: Check if there are any decks with note models or so
                        var kana = letter.notes.Where(n => n.note_model_uuid == "24bae622-7ab1-11ec-8f92-0c7a15ee466f" || n.note_model_uuid == "24bd090b-7ab1-11ec-99a2-0c7a15ee466f");
                        letter.notes = letter.notes.Except(kana).ToList();
                        letter.GetSubDeckByTitle("01 Kana").notes.AddRange(kana);

                        var vocabs = letter.notes.Where(n => n.note_model_uuid == "24bb0d42-7ab1-11ec-ad9a-0c7a15ee466f");
                        letter.notes = letter.notes.Except(vocabs).ToList();
                        letter.GetSubDeckByTitle("02 Vokabeln").notes.AddRange(vocabs);

                        var grammars = letter.notes.Where(n => n.note_model_uuid == "24bd0917-7ab1-11ec-b16e-0c7a15ee466f" || n.note_model_uuid == "b37a10fb-8a71-11ec-a896-3c58c2d0fe16");
                        letter.notes = letter.notes.Except(grammars).ToList();
                        letter.GetSubDeckByTitle("03 Grammatik").notes.AddRange(grammars);

                        var sentences = letter.notes.Where(n => n.note_model_uuid == "24bb0d45-7ab1-11ec-83a4-0c7a15ee466f");
                        letter.notes = letter.notes.Except(sentences).ToList();
                        letter.GetSubDeckByTitle("04 Sätze und Phrasen").notes.AddRange(sentences);

                        var trivias = letter.notes.Where(n => n.note_model_uuid == "35d3e520-8515-11ec-a736-3c58c2d0fe16");
                        letter.notes = letter.notes.Except(trivias).ToList();
                        letter.GetSubDeckByTitle("05 Trivia").notes.AddRange(trivias);

                        var tasks = letter.notes.Where(n => n.note_model_uuid == "c7da95f8-89c9-11ec-9683-3c58c2d0fe16");
                        letter.notes = letter.notes.Except(tasks).ToList();
                        letter.GetSubDeckByTitle("06 Aufgaben").notes.AddRange(tasks);
                    }
                }
            }

            // Export
            root.WriteToFile(deckPath);
        }

        public static void AIDBMBSAddExcerciseSubdecks(string path)
        {

            //Add subdecks
            var deck = Deck.LoadFromFile(path);
            var subDeck1 = deck.children.First();
            foreach (var chapter in subDeck1.children)
            {
                var lectureDeck = chapter.GetSubDeckByTitle("01 Lecture");
                if (lectureDeck == null)
                {
                    chapter.children.Add(Deck.CreateEmptyDeck("01 Lecture", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                }
                var exDeck = chapter.GetSubDeckByTitle("02 Exercise");
                if (exDeck == null)
                {
                    chapter.children.Add(Deck.CreateEmptyDeck("02 Exercise", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                }
            }

            //Add study options to all subdecks
            var chapterConfig = deck.GetDeckConfigurationByTitle("AIDBMS::Chapter");
            var lectureConfig = deck.GetDeckConfigurationByTitle("AIDBMS::Lecture");
            var exConfig = deck.GetDeckConfigurationByTitle("AIDBMS::Exercise");
            var subChapterConfig = deck.GetDeckConfigurationByTitle("AIDBMS::Subchapter");

            foreach (var chapter in subDeck1.children)
            {
                chapter.SetDeckConfiguration(chapterConfig);
                var lectures = chapter.children.First(c => c.name == "01 Lecture");
                lectures.SetDeckConfiguration(lectureConfig);
                foreach (var subChapter in lectures.children)
                {
                    subChapter.SetDeckConfiguration(subChapterConfig);
                }
                var exs = chapter.children.First(c => c.name == "02 Exercise");
                exs.SetDeckConfiguration(exConfig);
                foreach (var ex in exs.children)
                {
                    ex.SetDeckConfiguration(exConfig);
                }
            }
            deck.WriteToFile(path);
        }
    }


}
