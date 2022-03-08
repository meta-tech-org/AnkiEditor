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
            string deckPath = @"C:\Users\juliu_6las01j\Documents\Anki Exports\Japanisch,_bitte!_Neu\deck.json";

            // Import
            Deck root = JsonConvert.DeserializeObject<Deck>(File.ReadAllText(deckPath));
            foreach(var chapter in root.children)
            {
                foreach(var letter in chapter.children)
                {
                    if(letter.children.Count > 0)
                    {
                        continue;
                    }
                    letter.children.Add(CreateEmptyDeck("01 Kana", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    letter.children.Add(CreateEmptyDeck("02 Vokabeln", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    letter.children.Add(CreateEmptyDeck("03 Grammatik", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    letter.children.Add(CreateEmptyDeck("04 Sätze und Phrasen", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                    letter.children.Add(CreateEmptyDeck("05 Trivia", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
                }
            }

            // Export
            string[] lines = JsonConvert.SerializeObject(root, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented, //Try to match CrowdAnki export
                NullValueHandling = NullValueHandling.Ignore, //Try to match CrowdAnki export

            }).Split("\r\n");
            for (int i = 0; i < lines.Length; i++)
            {
                var lineSpaces = 0;
                var lineParts = lines[i].Split(" ");
                foreach (var linePart in lineParts)
                {
                    if (linePart == "")
                    {
                        lineSpaces++;
                    }
                    else
                    {
                        break;
                    }
                }
                lines[i] = lines[i].PadLeft(lines[i].Length + lineSpaces, ' ');
            }
            string resultString = string.Join("\r\n", lines);
            File.WriteAllText(deckPath, resultString);
        }

        static Deck CreateEmptyDeck(string name, string configId)
        {
            return new Deck
            {
                children = new System.Collections.Generic.List<Deck>(),
                crowdanki_uuid = Guid.NewGuid().ToString().ToLower(),
                deck_configurations = null,
                deck_config_uuid = configId,
                desc = "",
                dyn = 0,
                extendNew = 0,
                extendRev = 0,
                media_files = new System.Collections.Generic.List<string>(),
                mid = null,
                name = name,
                notes = new System.Collections.Generic.List<Note>(),
                note_models = null,
                __type__ = "Deck"
            };
        }
    }


}
