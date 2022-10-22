using CrowdAnkiSchema.Model;
using Newtonsoft.Json;
using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;

namespace AnkiEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            //AIDBMBSAddExcerciseSubdecks(@"C:\Users\juliu\source\repos\Anki Exports\Architecture_and_Implementation_of_Database_Management_Systems\deck.json");

            //FixJapaneseDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Japanisch,_bitte!_Neu\deck.json");
            //FixRussianDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Russisch_-_Olga_Schöne_-_Ein_guter_Anfang\deck.json");
            FixJapaneseFrequencyDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Japanese_Frequency_6000\deck.json");
        }

        private static void FixJapaneseFrequencyDeckStructure(string deckPath)
        {
            //Fix deck structure
            Deck root = Deck.LoadFromFile(deckPath);
            var baseConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000");
            var levelConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000::Level");
            var vocabConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000::Vocabulary");
            var kanjiConfig = root.GetDeckConfigurationByTitle("Japanese Frequency 6000::Kanji");

            var noteModelKanji = root.GetNoteModelByTitle("Japanese::Kanji");
            for (int i = 0; i <= 6000; i += 100)
            {
                string deckName = $"Level {i.ToString("0000")}";
                var level = root.children.FirstOrDefault(l => l.name == deckName);
                if (level == null)
                {
                    root.children.Add(Deck.CreateEmptyDeck(deckName, levelConfig.crowdanki_uuid));
                }
                else
                {
                    level.SetDeckConfiguration(levelConfig);
                }
            }

            foreach (var level in root.children)
            {
                var vocabDeck = level.children.FirstOrDefault(d => d.name == "01 Vocabulary");
                var kanjiDeck = level.children.FirstOrDefault(d => d.name == "02 Kanji");
                if (vocabDeck == null)
                {
                    level.children.Add(Deck.CreateEmptyDeck("01 Vocabulary", vocabConfig.crowdanki_uuid));
                }
                else
                {
                    vocabDeck.SetDeckConfiguration(vocabConfig);
                }
                if (kanjiDeck == null)
                {
                    level.children.Add(Deck.CreateEmptyDeck("02 Kanji", kanjiConfig.crowdanki_uuid));
                }
                else
                {
                    kanjiDeck.SetDeckConfiguration(kanjiConfig);
                }
            }

            //Move notes and create new cards
            foreach (var level in root.children)
            {
                var vocabDeck = level.children.FirstOrDefault(d => d.name == "01 Vocabulary");
                var levelMin = int.Parse(level.name.Replace("Level ", ""));
                var levelMax = levelMin + 100;
                var relevantNotes = root.notes.Where(n => int.Parse(n.ValueMain) < levelMax && int.Parse(n.ValueMain) >= levelMin).ToList();

                var kanjiDeck = level.children.FirstOrDefault(d => d.name == "02 Kanji");
                foreach (var relevantNote in relevantNotes)
                {
                    root.notes.Remove(relevantNote);
                    vocabDeck.notes.Add(relevantNote);
                    var kanjiFields = relevantNote.fields.Take(8).ToList();
                    kanjiFields.Add("");
                    kanjiDeck.AddNote(noteModelKanji.crowdanki_uuid, kanjiFields, new System.Collections.Generic.List<string>());
                }

            }

            //Add kanji information
            foreach (var level in root.children)
            {
                foreach (var subDeck in level.children)
                {
                    foreach (var note in subDeck.notes.Where(n => n.fields[5] == ""))
                    {
                        var word = note.fields.ElementAt(4);
                        string wordKanji = word.Replace("[", "").Replace("]", "");
                        var wordKana = note.fields.ElementAt(6);
                        foreach (var letterKana in wordKana)
                        {
                            wordKanji = wordKanji.Replace(letterKana.ToString(), "");
                        }
                        note.fields[5] = wordKanji;
                    }
                }
            }
            // Export
            root.WriteToFile(deckPath);
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
