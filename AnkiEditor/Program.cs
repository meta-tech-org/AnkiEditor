using CrowdAnkiSchema.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace AnkiEditor
{
    static class Program
    {
        const string HIRAGANA = "あいうえおかがきぎくぐけげこごさざしじすずせぜそぞただちぢつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもやゆよらりるれろわをんゃゅょっぁぇぃぉぅー。、「」０１２３４５６７８９";
        const string KATAKANA = "アイウエオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモヤユヨラリルレロワヲンャュョッァェィォゥー。、「」０１２３４５６７８９";
        static void Main(string[] args)
        {
            //AIDBMBSAddExcerciseSubdecks(@"C:\Users\juliu\source\repos\Anki Exports\Architecture_and_Implementation_of_Database_Management_Systems\deck.json");
            //FixJapaneseFrequencyFuriganaDeckStructure(@"C:\Users\juliu\source\Anki Exports\Japanese_Frequency_6000\deck.json");
            //FixRussianDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Russisch_-_Olga_Schöne_-_Ein_guter_Anfang\deck.json");
            //FixJapaneseFrequencyDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Japanese_Frequency_6000");
            //FixDeepLearningDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Deep_Learning_(Goodfellow,_Bengio,_Courville)\deck.json");
            //FixDIPDeckStructure(@"C:\Users\juliu\source\repos\Anki Exports\Digital_Image_Processing_(Gonzalez,_Woods)\deck.json");
            //CreateOpenRussianDeck(@"C:\Users\juliu\source\Anki Exports\OpenRussian\deck.json", @"C:\Users\juliu\source\repos\OpenRussianConverter\OpenRussianExporter\output.json");
            CreateSpanishDeck(@"C:\Users\juliu\source\Anki Exports\UNIVERSO.ele\deck.json", @"C:\Users\juliu\source\repos\meta-tech-org\AnkiEditor\AnkiEditor\SpanischFormatted.csv");
        }

        private static void CreateSpanishDeck(string deckPath, string dataPath)
        {
            string[] subdeckNames = new string[]
            {
                "Intro",
                "A",
                "B",
                "C",
                "Proyecto",
                "Mi gramática",
                "Mi léxico",
                "Cultura",
                "AB"
            };
            Deck root = Deck.LoadFromFile(deckPath);
            if (root.children.Count == 0)
            {
                //Create decks
                var a1Deck = Deck.CreateEmptyDeck("A1", root.deck_config_uuid);
                for (int unidad = 1; unidad <= 7; unidad++)
                {
                    var unidadDeck = Deck.CreateEmptyDeck($"Unidad {unidad.ToString("00")}", root.deck_config_uuid);
                    for (int subdeck = 1; subdeck < subdeckNames.Length + 1; subdeck++)
                    {
                        if(unidad == 1 && new int[] { 5, 6, 7, 8 }.Contains(subdeck))
                        {
                            continue;
                        }
                        var currentName = $"{subdeck.ToString("00")} {subdeckNames[subdeck - 1]}";
                        unidadDeck.children.Add(Deck.CreateEmptyDeck(currentName, root.deck_config_uuid));
                    }
                    a1Deck.children.Add(unidadDeck);
                }
                root.children.Add(a1Deck);
            }

            var check = File.ReadAllLines(dataPath).Select(d => d.Split(";")).Where(d => d.Length != 7).ToList();

            var data = File.ReadAllLines(dataPath).Select(d => d.Split(";")).Select(d =>
            new SpanishWord
            {
                Lesson = d[0],
                Spanish = d[1],
                German = d[2],
                Wordtype = d[3],
                Gender = d[4],
                Number = d[5],
                LA = d[6],
            }).ToList();
            List<SpanishWord> filteredWords = new List<SpanishWord>();
            List<SpanishWord> filteredOutWords = new List<SpanishWord>();
            //Filter out female extra words
            for(int i = 0; i < data.Count-1; i++)
            {
                var word = data[i];
                var nextWord = data[i+1];

                if (word.Spanish.Last() == 'o' && word.Spanish.Substring(0, word.Spanish.Length-1)+"a" == nextWord.Spanish)
                {
                    word.Spanish += "/-a";
                    word.Gender += "/"+nextWord.Gender;
                    filteredWords.Add(word);
                    filteredOutWords.Add(nextWord);
                    i++;
                }
                else
                {
                    filteredWords.Add(word);
                }
            }

            data = filteredWords;

            var noteModelUUID = root.note_models.First().crowdanki_uuid;
            foreach(var unidad in root.children.First().children)
            {
                foreach(var subdeck in unidad.children)
                {
                    subdeck.notes.Clear();
                    var wordTitle = (unidad.name.Replace("Unidad 0","")+subdeck.name.Replace(subdeck.name.Split(" ")[0],"")).Replace(" Intro", "");
                    var words = data.Where(w => w.Lesson == wordTitle).ToList();
                    if(words.Count == 0)
                    {
                        throw new Exception("WTF");
                    }
                    foreach(var word in words)
                    {
                        List<string> fieldValues = new List<string>()
                        {
                            word.Spanish,
                            word.German,
                            "", //audio
                            word.Wordtype,
                            word.Gender,
                            word.Number,
                            word.LA,
                        };
                        List<string> tags = new List<string>();

                        subdeck.AddNote(noteModelUUID, fieldValues, tags);
                    }
                }
            }
            root.WriteToFile(deckPath);
        }

        private static void CreateOpenRussianDeck(string deckPath, string dataPath)
        {
            Deck root = Deck.LoadFromFile(deckPath);
            OpenRussianWord[] words = JsonConvert.DeserializeObject<OpenRussianWord[]>(File.ReadAllText(dataPath));
            var noteType = root.note_models.FirstOrDefault(nm => nm.crowdanki_uuid == "07aac7c1-a960-11ed-8dac-0c7a15ee466b");
            foreach (var level in root.children)
            {
                var levelName = level.name;
                var relatedData = words.Where(w => w.Level == levelName);
                level.notes.Clear();
                foreach (var wordData in relatedData)
                {
                    List<string> fieldValues = new List<string>()
                    {
                        wordData.Index.ToString(),
                        wordData.Level,
                        wordData.RussianAccented,
                        wordData.English,
                        wordData.RussianBare,
                        wordData.Audio ?? "",
                        wordData.DerivedFrom??"",
                        wordData.UsageExample??"",
                        wordData.RelatedTo??"",
                        wordData.SentenceExamplesRussian??"",
                        wordData.SentenceExamplesAudio??"",
                        wordData.SentenceExamplesEnglish??"",
                        wordData.IsNoun?"True":"",
                        wordData.IsAnimate?.ToString()??"",
                        wordData.Gender??"",
                        wordData.IsAdjective?"True":"",
                        wordData.Comparative??"",
                        wordData.Superlative??"",
                        wordData.IsVerb?"True":"",
                        wordData.Partner??"",
                        wordData.Aspect??"",
                    };
                    List<string> tags = new List<string>();
                    if (wordData.IsAdjective)
                    {
                        tags.Add("OpenRussian::Adjective");
                    }
                    if (wordData.IsNoun)
                    {
                        tags.Add("OpenRussian::Noun");
                    }
                    if (wordData.IsVerb)
                    {
                        tags.Add("OpenRussian::Verb");
                    }
                    if (!wordData.IsVerb && !wordData.IsNoun && !wordData.IsAdjective)
                    {
                        tags.Add("OpenRussian::Other");
                    }
                    level.AddNote("07aac7c1-a960-11ed-8dac-0c7a15ee466b", fieldValues, tags);
                }
            }
            root.WriteToFile(deckPath);
        }

        private static bool IsHiragana(this char c)
        {
            return HIRAGANA.Contains(c);
        }

        private static bool IsKatakana(this char c)
        {
            return KATAKANA.Contains(c);
        }

        private static bool IsRomaji(this char c)
        {
            return /*(c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || */(c >= '0' && c <= '9');
        }

        private static bool IsKanji(this char c)
        {
            return !IsHiragana(c) && !IsKatakana(c) && !IsRomaji(c);
        }

        private static char GetLetterType(this char c)
        {
            if (c.IsKanji())
            {
                return 'W';
            }
            if (c.IsHiragana())
            {
                return 'H';
            }
            if (c.IsKatakana())
            {
                return 'K';
            }
            if (c.IsRomaji())
            {
                return 'R';
            }
            return 'E';
        }

        private static string GetWordType(this string word)
        {
            string type = "";
            foreach (var c in word)
            {
                type += c.GetLetterType();
            }
            return type;
        }

        private static string GetCompressedWordType(this string word)
        {
            var wordType = word.GetWordType();
            string compressed = "";
            char current = '_';
            foreach (var c in wordType)
            {
                if (c != current)
                {
                    compressed += c;
                    current = c;
                }
            }

            return compressed;
        }

        public static char ToHiragana(this char katakana)
        {
            int index = katakana.IsKatakana() ? KATAKANA.IndexOf(katakana) : -1;
            return HIRAGANA[index];
        }

        private static List<(char, string)> SeparateIntoCharacterRuns(string sentence)
        {
            List<(char, string)> runs = new List<(char, string)>();
            char previousType = sentence[0].GetLetterType();
            string currentString = sentence[0].ToString();
            for (int i = 1; i < sentence.Length; i++)
            {
                var currentType = sentence[i].GetLetterType();
                if (previousType != currentType)
                {
                    runs.Add((previousType, currentString));
                    currentString = sentence[i].ToString();
                }
                else
                {
                    currentString += sentence[i];
                }
                previousType = currentType;
            }
            if (currentString.Length > 0)
            {
                runs.Add((previousType, currentString));
            }
            return runs;
        }

        public static string AddFurigana(string kanji, string kana)
        {
            var runs = SeparateIntoCharacterRuns(kanji);
            string furiganaString = "";
            for (int r = 0; r < runs.Count; r++)
            {
                var run = runs[r];
                (char, string)? runNext = r + 1 < runs.Count ? runs[r + 1] : null;
                if (run.Item1 == 'H')
                {
                    //Add unaltered hiragana to the string
                    furiganaString += run.Item2;

                    //If the current block consists of hiragana, remove them from the kana list
                    kana = kana.ReplaceFirst(run.Item2, "");
                }
                else if (run.Item1 == 'K')
                {
                    //Add unaltered katakana to the string
                    furiganaString += run.Item2;

                    //Find corresponding hiragana symbol
                    var hiraganaRun = new string(run.Item2.Select(c => c.ToHiragana()).ToArray());

                    //If the current block consists of hiragana or katakana, remove them from the kana list
                    kana = kana.ReplaceFirst(hiraganaRun, "");
                }
                else if (run.Item1 == 'W')
                {
                    string currentFurigana = "";
                    //If the current block consists of kanji, find the index of the next block
                    if (runNext == null)
                    {
                        //If there is no next block, the string ends with a kanji and all remaining kana must be added.
                        currentFurigana = kana;
                    }
                    else
                    {
                        //If there is a next block, use that

                        //If the next run is katakana, retrieve the corresponding hiragana first
                        string hiraganaRunNext = "";
                        if (runNext?.Item1 == 'K')
                        {
                            hiraganaRunNext = new string(runNext?.Item2.Select(c => c.ToHiragana()).ToArray());
                        }
                        else
                        {
                            hiraganaRunNext = runNext?.Item2;
                        }
                        var nextIndex = kana.Substring(1).IndexOf(hiraganaRunNext) + 1; //Source of probems if the next block (Example: "de" is fully contained in the current kanji block, example: "densha" (densha de ...) - the wrong index is selected.
                                                                                        //var nextIndex = kana.Substring(1).IndexOf(hiraganaRunNext)+1; 
                        currentFurigana = kana.Substring(0, nextIndex);
                    }

                    //Add a space before the next kanji block
                    if (furiganaString != "")
                    {
                        furiganaString += " ";
                    }

                    //Add the kanji block
                    furiganaString += run.Item2;
                    //Add the furigana
                    furiganaString += $"[{currentFurigana}]";

                    //Remove the current furigana from the kana list
                    kana = kana.ReplaceFirst(currentFurigana, "");
                }
            }

            return furiganaString;
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
                    if (line.Split(' ')[0].Length == 1)
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
                    });
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

            foreach (var chapter in deepLearningChapters)
            {
                var ankiChapter = Deck.CreateEmptyDeck(chapter.InferredName, chapterConfig.crowdanki_uuid);
                root.children.Add(ankiChapter);
                foreach (var subChapter in chapter.SubChapters)
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
                    return $"{number1}.{number2}.{number3} {title}".Replace(".00", "");
                }
            }
            public List<DeepLearningChapter> SubChapters { get; set; }
            public override string ToString()
            {
                return InferredName;
            }
        }
        private static void FixJapaneseFrequencyFuriganaDeckStructure(string deckPath)
        {
            //var iknow = File.ReadAllLines("C:\\Users\\juliu\\source\\repos\\IKnowJPDownloader\\IKnowJPDownloader\\IKnowJPDownloader\\bin\\Debug\\net6.0\\iknow.csv").Select(x => x.Split(";"));
            //Fix deck structure
            Deck root = Deck.LoadFromFile(deckPath);
            var vocabDeck = root.children.First(c => c.name == "Words").children.First(c => c.name == "02 Kanji");
            foreach (var note in vocabDeck.notes)
            {
                var word = note.fields[4];
                var kanji = word.Split("[")[0];
                var kana = word.Split("[")[1].Replace("]", "");
                var fixedWord = AddFurigana(kanji, kana);
                note.fields[4] = fixedWord;

                //var kanjiSentence = note.fields[10].Replace("<b>", "").Replace("</b>", "").Replace("&nbsp;", "");
                //var kanaSentence = note.fields[11].Replace("<b>", "").Replace("</b>", "").Replace("&nbsp;", "").Replace(" ", "");
                //var fixedSentence = AddFurigana(kanjiSentence, kanaSentence);
                //note.fields[10] = fixedSentence;
            }
            root.WriteToFile(deckPath);
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

    public static class StringExtensionMethods
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
    public class OpenRussianWord
    {
        public int Index { get; set; }
        public string Level { get; set; }
        public string RussianAccented { get; set; }
        public string English { get; set; }
        public string RussianBare { get; set; }
        public string? Audio { get; set; }
        public string? DerivedFrom { get; set; }
        public string? UsageExample { get; set; }
        public string? RelatedTo { get; set; }
        public string? SentenceExamplesRussian { get; set; }
        public string? SentenceExamplesAudio { get; set; }
        public string? SentenceExamplesEnglish { get; set; }
        public bool IsNoun { get; set; }
        public bool? IsAnimate { get; set; }
        public string? Gender { get; set; }
        public bool IsAdjective { get; set; }
        public string? Comparative { get; set; }
        public string? Superlative { get; set; }
        public bool IsVerb { get; set; }
        public string? Partner { get; set; }
        public string? Aspect { get; set; }
    }
    public class SpanishWord
    {
        public string? Lesson { get; set; }
        public string? Spanish { get; set; }
        public string? German { get; set; }
        public string? Wordtype { get; set; }
        public string? Gender { get; set; }
        public string? Number { get; set; }
        public string? LA { get; set; }
    }
}
