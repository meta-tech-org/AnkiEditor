using AnkiEditor.Model;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AnkiEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            string deckPath = @"C:\Users\juliu_6las01j\Documents\Anki Exports\Japanisch,_bitte!_Neu\deck.json";

            // Import
            Deck root = JsonConvert.DeserializeObject<Deck>(File.ReadAllText(deckPath));

            // Export
            string[] lines = JsonConvert.SerializeObject(root, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                
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
    }
}
