﻿using CrowdAnkiSchema.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace AnkiEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            AIDBMBSAddExcerciseSubdecks(@"C:\Users\juliu\source\repos\Anki Exports\Architecture_and_Implementation_of_Database_Management_Systems\deck.json");

            //FixJapaneseDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Japanisch,_bitte!_Neu\deck.json");
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
