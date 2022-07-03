﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrowdAnkiSchema.Model
{
    public class Deck
    {
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
        public NoteModel GetNoteModelByTitle(string title)
        {
            return note_models.First(n => n.name == title);
        }
        public List<Note> notes { get; set; }
        /// <summary>
        /// Returns all cards that match the given note_model_uuid.
        /// </summary>
        /// <param name="noteModelUuid"></param>
        /// <returns></returns>
        public List<Note> GetNotesByNoteModel(string noteModelUuid)
        {
            return notes.Where(n => n.note_model_uuid == noteModelUuid).ToList();
        }
        /// <summary>
        /// Returns all cards that match the given note model
        /// </summary>
        /// <param name="noteModelUuid"></param>
        /// <returns></returns>
        public List<Note> GetNotesByNoteModel(NoteModel model)
        {
            return notes.Where(n => n.note_model_uuid == model.crowdanki_uuid).ToList();
        }

        public void FixMediaFiles()
        {
            this.media_files.Clear();
            foreach(var subDeck in children)
            {
                subDeck.FixMediaFiles();
            }
            Regex soundRegex = new Regex(@"(?<=\[sound:).*?(?=\])");
            Regex imgRegex = new Regex(File.ReadAllText("regex.txt"));
            foreach(var note in notes)
            {
                foreach(var field in note.fields)
                {
                    foreach (var match in soundRegex.Matches(field))
                    {
                        if(this.media_files == null)
                        {
                            this.media_files = new List<string>();
                        }
                        this.media_files.Add(match.ToString());
                    }
                    foreach (var match in imgRegex.Matches(field))
                    {
                        if (this.media_files == null)
                        {
                            this.media_files = new List<string>();
                        }
                        this.media_files.Add(match.ToString());
                    }
                    this.media_files = this.media_files.Distinct().OrderBy(f => f).ToList();
                }
            }
        }

        /// <summary>
        /// Sets the deck config (study options) for the current deck.
        /// </summary>
        /// <param name="config"></param>
        public void SetDeckConfiguration(DeckConfiguration config)
        {
            SetDeckConfiguration(config.crowdanki_uuid);
        }
        /// <summary>
        /// Sets the deck config (study options) for the current deck.
        /// </summary>
        /// <param name="deckConfigurationUUID"></param>
        public void SetDeckConfiguration(string deckConfigurationUUID)
        {
            this.deck_config_uuid = deckConfigurationUUID;
        }

        public Deck GetSubDeckByTitle(string title)
        {
            return children?.FirstOrDefault(d => d.name == title);
        }
        /// <summary>
        /// Returns an instance of the deck configuration (study options) that matches the given title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public DeckConfiguration GetDeckConfigurationByTitle(string title)
        {
            return deck_configurations?.FirstOrDefault(d => d.name == title);
        }
        public static Deck CreateEmptyDeck(string name, string deckConfigurationUUID)
        {
            return new Deck
            {
                children = new List<Deck>(),
                crowdanki_uuid = Guid.NewGuid().ToString().ToLower(),
                deck_configurations = null,
                deck_config_uuid = deckConfigurationUUID,
                desc = "",
                dyn = 0,
                extendNew = 0,
                extendRev = 0,
                media_files = new List<string>(),
                mid = null,
                name = name,
                notes = new List<Note>(),
                note_models = null,
                __type__ = "Deck"
            };
        }

        public override string ToString()
        {
            return $"Deck {name}, children: {children.Count}, notes: {notes.Count}";
        }

        #region File Interaction
        public static Deck LoadFromFile(string path)
        {
            return JsonConvert.DeserializeObject<Deck>(File.ReadAllText(path));
        }

        public void WriteToFile(string path)
        {
            FixMediaFiles();
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

        #endregion
    }
}