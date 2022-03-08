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
            File.WriteAllText(deckPath, JsonConvert.SerializeObject(root, Formatting.Indented).Replace("  ","    "));
        }
    }
}
