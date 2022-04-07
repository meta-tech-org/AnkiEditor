﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiEditor.Model
{
    public class Deck
    {
        public static Deck LoadFromFile(string path)
        {
            return JsonConvert.DeserializeObject<Deck>(File.ReadAllText(path));
        }
        public string __type__ { get; set; }
        public List<Deck> children { get; set; }
        public string crowdanki_uuid { get; set; }
        public string deck_config_uuid { get; set; }
        public List<DeckConfiguration> deck_configurations { get; set; }
        public string desc { get; set; }
        public int dyn { get; set; }
        public int extendNew { get; set; }
        public int extendRev { get; set; }
        public List<string> media_files { get; set; }
        public long? mid { get; set; }
        public string name { get; set; }
        public List<NoteModel> note_models { get; set; }
        public List<Note> notes { get; set; }

        public Deck GetSubDeckByTitle(string title)
        {
            return children?.FirstOrDefault(d => d.name == title);
        }
        public DeckConfiguration GetDeckConfigurationByTitle(string title)
        {
            return deck_configurations?.FirstOrDefault(d => d.name == title);
        }

        public override string ToString()
        {
            return $"Deck {name}, children: {children.Count}, notes: {notes.Count}";
        }

        public void WriteToFile(string path)
        {
            string[] lines = JsonConvert.SerializeObject(this, new JsonSerializerSettings
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
            File.WriteAllText(path, resultString);
        }
    }
}
