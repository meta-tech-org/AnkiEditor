using AnkiEditor.Model;
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
            //string deckPath = @"C:\Users\juliu_6las01j\Documents\Anki Exports\Japanisch,_bitte!_Neu\deck.json";
            string deckPath = @"C:\Users\juliu_6las01j\Documents\Anki Exports\Russisch_-_Olga_Schöne_-_Ein_guter_Anfang\deck.json";

            // Import
            Deck root = Deck.LoadFromFile(deckPath);
            var chapterConfig = root.GetDeckConfigurationByTitle("Russisch::Kapitel");
            var vocabConfig = root.GetDeckConfigurationByTitle("Russisch::Vokabeln");
            var grammarConfig = root.GetDeckConfigurationByTitle("Russisch::Grammatik");
            var taskConfig = root.GetDeckConfigurationByTitle("Russisch::Aufgaben");
            var sentencesConfig = root.GetDeckConfigurationByTitle("Russisch::Sätze und Phrasen");
            foreach(var level in root.children)
            {
                foreach (var chapter in level.children)
                {
                    chapter.deck_config_uuid = chapterConfig.crowdanki_uuid;
                    var vocabDeck = chapter.GetSubDeckByTitle("01 Vokabeln");
                    vocabDeck.deck_config_uuid = vocabConfig.crowdanki_uuid;
                    var grammarDeck = chapter.GetSubDeckByTitle("02 Grammatik");
                    grammarDeck.deck_config_uuid = grammarConfig.crowdanki_uuid;
                    var sentencesDeck = chapter.GetSubDeckByTitle("03 Sätze und Phrasen");
                    sentencesDeck.deck_config_uuid = sentencesConfig.crowdanki_uuid;
                    var taskDeck = chapter.GetSubDeckByTitle("04 Aufgaben");
                    taskDeck.deck_config_uuid = taskConfig.crowdanki_uuid;
                    //foreach (var letter in chapter.children)
                    //{
                    //    if (letter.children.Count > 0)
                    //    {
                    //        continue;
                    //    }

                    //    ////TODO: move file list from parent deck to new child if applicable
                    //    ////TODO: Check if there are any decks with note models or so
                    //    //var kana = letter.notes.Where(n => n.note_model_uuid == "24bae622-7ab1-11ec-8f92-0c7a15ee466f" || n.note_model_uuid == "24bd090b-7ab1-11ec-99a2-0c7a15ee466f");
                    //    //var kanaDeck = CreateEmptyDeck("01 Kana", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f");
                    //    //letter.notes = letter.notes.Except(kana).ToList();
                    //    //kanaDeck.notes.AddRange(kana);
                    //    //letter.children.Add(kanaDeck);
                    //    //letter.children.Add(CreateEmptyDeck("02 Vokabeln", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    //    //letter.children.Add(CreateEmptyDeck("03 Grammatik", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    //    //letter.children.Add(CreateEmptyDeck("04 Sätze und Phrasen", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    //    //letter.children.Add(CreateEmptyDeck("05 Trivia", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    //}
                }
            }

            // Export
            root.WriteToFile(deckPath);
        }
    }


}
