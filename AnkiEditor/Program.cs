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
            // Import
            Deck root = JsonConvert.DeserializeObject<Deck>(File.ReadAllText(@"C:\Users\juliu_6las01j\Documents\Anki Exports\Japanisch,_bitte!_Neu\deck.json"));

            // Export
            File.WriteAllText(@"C:\Users\juliu_6las01j\Documents\Anki Exports\Japanisch,_bitte!_Neu\deck.json", JsonConvert.SerializeObject(root, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            }).Replace("  ","    "));
        }
    }
}
